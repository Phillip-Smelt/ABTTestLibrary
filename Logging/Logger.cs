using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Serilog; // Install Serilog via NuGet Package Manager.  Site is https://serilog.net/.
using ABT.TestSpace.AppConfig;
using System.Drawing;

// TODO: SQL Server Express: Persist test data into Microsoft SQL Server Express.
// TODO: SQL Server Express: Create a Microsoft C# front-end exporting/report app for persisted SQL Server Express test data.  Export in CSV, report in RTF.
// TODO: SQL Server Express: Use Logger & RTFSinks to write SQL Server Express' data to RTF then File/Save As RTF.
//
// NOTE: Below hopefully "value-added" wrapper methods for some commonly used Serilog commands are conveniences, not necessities.
// NOTE: Will never fully implement wrapper methods for the complete set of Serilog commands, just some of the most commonly used ones.
// - In general, TestExecutive's InterfaceAdapters, Logging, SCPI_VISA_Instruments & Switching namespaces exist partly to eliminate
//   the need to reference TestExecutive's various DLLs directly from TestExecutor client apps.
// - As long as suitable wrapper methods exists in Logger, needn't directly reference Serilog from TestExecutor client apps,
//   as referencing TestExecutive suffices.
namespace ABT.TestSpace.Logging {
    public static class Logger {
        public const String LOGGER_TEMPLATE = "{Message}{NewLine}";
        public const String SPACES_16 = "                ";
        private const String MESSAGE_STOP = "STOP              : ";
        private const String MESSAGE_UUT_RESULT = "Result            : ";

        public static void Start(TestExecutive testExecutive, ref RichTextBox rtfResults) {
            if (!testExecutive.ConfigTest.IsOperation) {
                // When TestGroups are executed, test data is never saved to configLogger.FilePath as Rich Text.  Never.
                // RichTextBox only. 
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Sink(new RichTextBoxSink(richTextBox: ref rtfResults, outputTemplate: LOGGER_TEMPLATE))
                    .CreateLogger();
                Log.Information($"Note: following test results invalid for UUT production testing, only troubleshooting.");
                Log.Information($"START             : {DateTime.Now}");
                Log.Information($"UUT Revision      : {testExecutive.ConfigUUT.Revision}");
                Log.Information($"UUT Number        : {testExecutive.ConfigUUT.Number}");
                Log.Information($"UUT Serial Number : {testExecutive.ConfigUUT.SerialNumber}");
                Log.Information($"Test ID           : {testExecutive.ConfigTest.TestElementID}");
                Log.Information($"Test Description  : {testExecutive.ConfigTest.TestElementDescription}\n");
                return;
                // Log Header isn't written to Console when TestGroups are executed, further emphasizing test results are invalid for pass verdict/$hip disposition, only troubleshooting failures.
            }

            if (testExecutive.ConfigLogger.FileEnabled && !testExecutive.ConfigLogger.SQLEnabled) {
                // When TestOperations are executed, test data is always & automatically saved to config.Logger.FilePath as Rich Text.  Always.
                // RichTextBox + File.
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Sink(new RichTextBoxSink(richTextBox: ref rtfResults, outputTemplate: LOGGER_TEMPLATE))
                    .CreateLogger();
            } else if (!testExecutive.ConfigLogger.FileEnabled && testExecutive.ConfigLogger.SQLEnabled) {
                // TODO: RichTextBox + SQL.
                SQLStart(testExecutive);
            } else if (testExecutive.ConfigLogger.FileEnabled && testExecutive.ConfigLogger.SQLEnabled) {
                // TODO: RichTextBox + File + SQL.
                SQLStart(testExecutive);
            } else {
                // RichTextBox only; customer doesn't require saved test data, unusual for Functional testing, but common for other testing methodologies.
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Sink(new RichTextBoxSink(richTextBox: ref rtfResults, outputTemplate: LOGGER_TEMPLATE))
                    .CreateLogger();
            }
            Log.Information($"UUT:");
            Log.Information($"\t{MESSAGE_UUT_RESULT}");
            Log.Information($"\tSerial Number     : {testExecutive.ConfigUUT.SerialNumber}");
            Log.Information($"\tNumber            : {testExecutive.ConfigUUT.Number}");
            Log.Information($"\tRevision          : {testExecutive.ConfigUUT.Revision}");
            Log.Information($"\tDescription       : {testExecutive.ConfigUUT.Description}");
            Log.Information($"\tType              : {testExecutive.ConfigUUT.Type}");
            Log.Information($"\tCustomer          : {testExecutive.ConfigUUT.Customer}\n");

            Log.Information($"Test:");
            Log.Information($"\tSTART             : {DateTime.Now}");
            Log.Information($"\t{MESSAGE_STOP}");
            Log.Information($"\tOperator          : {UserPrincipal.Current.DisplayName}");
            // NOTE: UserPrincipal.Current.DisplayName requires a connected/active Domain session for Active Directory PCs.
            Log.Information($"\tExecutive Version : {testExecutive._libraryAssemblyVersion}");
            Log.Information($"\tExecutor Version  : {testExecutive._appAssemblyVersion}");
            Log.Information($"\tSpecification     : {testExecutive.ConfigUUT.TestSpecification}");
            Log.Information($"\tID                : {testExecutive.ConfigTest.TestElementID}");
            Log.Information($"\tRevision          : {testExecutive.ConfigTest.TestElementRevision}");
            Log.Information($"\tDescription       : {testExecutive.ConfigTest.TestElementDescription}\n");

            StringBuilder s = new StringBuilder();
            Operation operation = Operation.Get(testExecutive.ConfigTest.TestElementID);
            List<String> testGroupIDs = operation.TestGroupIDs.Split(Test.SPLIT_ARGUMENTS_CHAR).Select(id => id.Trim()).ToList();
            foreach (String testGroupID in testGroupIDs) {
                Group group = Group.Get(testGroupID);
                s.Append(String.Format("\t{0,-" + testExecutive.ConfigTest.FormattingLengthTestGroup + "} : {1}\n", group.ID, group.Description));
                List<String> testMeasurementIDs = group.TestMeasurementIDs.Split(Test.SPLIT_ARGUMENTS_CHAR).Select(id => id.Trim()).ToList();
                foreach (String testMeasurementID in testMeasurementIDs) {
                    s.Append(String.Format("\t\t{0,-" + testExecutive.ConfigTest.FormattingLengthTestMeasurement + "} : {1}\n", testExecutive.ConfigTest.Tests[testMeasurementID].ID, testExecutive.ConfigTest.Tests[testMeasurementID].Description));
                }
            }
            Log.Information($"Measurements:\n{s}");
        }

        public static void LogTest(Test test, ref RichTextBox rtfResults) {
            StringBuilder message = new StringBuilder();
            message.AppendLine($"ID '{test.ID}'");
            message.AppendLine($"  Revision    : {test.Revision}");
            message.AppendLine($"  Description : {test.Description}");
            message.AppendLine($"  Test Type   : {test.ClassName}");
            switch (test.ClassName) {
                case TestCustomizable.ClassName:
                    TestCustomizable testCustomizable = (TestCustomizable)test.ClassObject;
                    if (testCustomizable.Arguments != null) foreach (KeyValuePair<String, String> kvp in testCustomizable.Arguments) message.AppendLine($"  Key=Value   : {kvp.Key}={kvp.Value}");
                    message.AppendLine($"  Actual      : {test.Measurement}");
                    break;
                case TestISP.ClassName:
                    TestISP testISP = (TestISP)test.ClassObject;
                    message.AppendLine($"  Expected    : {testISP.ISPExpected}");
                    message.AppendLine($"  Actual      : {test.Measurement}");
                    break;
                case TestNumerical.ClassName:
                    TestNumerical testNumerical = (TestNumerical)test.ClassObject;
                    message.AppendLine($"  High Limit  : {testNumerical.High:G}");
                    message.AppendLine($"  Measurement : {Double.Parse(test.Measurement, NumberStyles.Float, CultureInfo.CurrentCulture):G}");
                    message.AppendLine($"  Low Limit   : {testNumerical.Low:G}");
                    message.Append($"  SI Units    : {Enum.GetName(typeof(SI_UNITS), testNumerical.SI_Units)}");
                    if (testNumerical.SI_Units_Modifier != SI_UNITS_MODIFIERS.NotApplicable) message.Append($" {Enum.GetName(typeof(SI_UNITS_MODIFIERS), testNumerical.SI_Units_Modifier)}");
                    message.AppendLine("");
                    break;
                case TestTextual.ClassName:
                    TestTextual testTextual = (TestTextual)test.ClassObject;
                    message.AppendLine($"  Expected    : {testTextual.Text}");
                    message.AppendLine($"  Actual      : {test.Measurement}");
                    break;
                default:
                    throw new NotImplementedException($"TestElement ID '{test.ID}' with ClassName '{test.ClassName}' not implemented.");
            }
            message.AppendLine($"  Result      : {test.Result}");
            SetBackColor(ref rtfResults, 0, test.ID, EventCodes.GetColor(test.Result));
#if DEBUG
            message.AppendLine(test.DebugMessage);
#endif
            Log.Information(message.ToString());
        }

        public static void Stop(TestExecutive testExecutive, ref RichTextBox rtfResults) {
            if (!testExecutive.ConfigTest.IsOperation) Log.CloseAndFlush();
            // Log Trailer isn't written when not a TestOperation, further emphasizing test results aren't valid for passing & $hipping, only troubleshooting failures.
            else {
                ReplaceText(ref rtfResults, 0, MESSAGE_UUT_RESULT, MESSAGE_UUT_RESULT + testExecutive.ConfigUUT.EventCode);
                SetBackColor(ref rtfResults, 0, testExecutive.ConfigUUT.EventCode, EventCodes.GetColor(testExecutive.ConfigUUT.EventCode));
                ReplaceText(ref rtfResults, 0, MESSAGE_STOP, MESSAGE_STOP + DateTime.Now);               
                Log.CloseAndFlush();
                if (testExecutive.ConfigLogger.FileEnabled) FileStop(testExecutive, ref rtfResults);
                if (testExecutive.ConfigLogger.SQLEnabled) SQLStop(testExecutive);
                if (testExecutive.ConfigLogger.TestEventsEnabled) TestEvents(testExecutive.ConfigUUT);
            }
        }

        public static void UnexpectedErrorHandler(String logMessage) {
            Log.Error(logMessage);
            MessageBox.Show(Form.ActiveForm, $"Unexpected error.  Details logged for analysis & resolution.{Environment.NewLine}{Environment.NewLine}" +
                            $"If reoccurs, please contact Test Engineering.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void ReplaceText(ref RichTextBox richTextBox, Int32 startFind, String originalText, String replacementText) {
            richTextBox.SelectionStart = richTextBox.Find(originalText, startFind, RichTextBoxFinds.MatchCase & RichTextBoxFinds.WholeWord); ;
            richTextBox.SelectionLength = originalText.Length;
            richTextBox.SelectedText = replacementText;
        }

        private static void SetBackColor(ref RichTextBox richTextBox, Int32 startFind, String findText, Color backColor) {
            richTextBox.SelectionStart = richTextBox.Find(findText, startFind, RichTextBoxFinds.MatchCase & RichTextBoxFinds.WholeWord); ;
            richTextBox.SelectionLength = findText.Length;
            richTextBox.SelectionBackColor = backColor;
        }

        private static void FileStop(TestExecutive testExecutive, ref RichTextBox rtfResults) {
            String fileName = $"{testExecutive.ConfigUUT.Number}_{testExecutive.ConfigUUT.SerialNumber}_{testExecutive.ConfigTest.TestElementID}";
            String[] files = Directory.GetFiles(testExecutive.ConfigLogger.FilePath, $"{fileName}_*.rtf", SearchOption.TopDirectoryOnly);
            // Will fail if invalid this.ConfigLogger.FilePath.  Don't catch resulting Exception though; this has to be fixed in App.config.
            // Otherwise, files is the set of all files in config.Logger.FilePath like
            // config.configUUT.Number_Config.configUUT.SerialNumber_configTest.TestElementID_*.rtf.
            Int32 maxNumber = 0; String s;
            foreach (String f in files) {
                s = f;
                s = s.Replace($"{testExecutive.ConfigLogger.FilePath}{fileName}", String.Empty);
                s = s.Replace(".rtf", String.Empty);
                s = s.Replace("_", String.Empty);
                foreach (FieldInfo fi in typeof(EventCodes).GetFields()) s = s.Replace((String)fi.GetValue(null), String.Empty);
                if (Int32.Parse(s) > maxNumber) maxNumber = Int32.Parse(s);
                // Example for final (3rd) iteration of foreach (String f in files):
                //   FileName            : 'UUTNumber_TestElementID_SerialNumber'
                //   Initially           : 'P:\Test\TDR\D4522137\Functional\UUTNumber_TestElementID_SerialNumber_3_PASS.rtf'
                //   FilePath + fileName : '_3_PASS.rtf'
                //   .txt                : '_3_PASS'
                //   _                   : '3PASS'
                //   foreach (FieldInfo  : '3'
                //   maxNumber           : '3'
            }
            fileName += $"_{++maxNumber}_{testExecutive.ConfigUUT.EventCode}.rtf";
            rtfResults.SaveFile($"{testExecutive.ConfigLogger.FilePath}{fileName}");
        }

        private static void SQLStart(TestExecutive testExecutive) {
            // TODO: SQL Server Express: SQLStart.
        }

        private static void SQLStop(TestExecutive testExecutive) {
            // TODO: SQL Server Express: SQLStop.
        }

        private static void TestEvents(AppConfigUUT uut) {
            String eventCode;
            switch (uut.EventCode) {
                case EventCodes.CANCEL:
                    eventCode = "A";
                    break;
                case EventCodes.ERROR:
                    eventCode = "E";
                    break;
                case EventCodes.FAIL:
                    eventCode = "F";
                    break;
                case EventCodes.PASS:
                    eventCode = "P";
                    break;
                case EventCodes.UNSET:
                    eventCode = "U";
                    break;
                default:
                    throw new NotImplementedException($"Unrecognized EventCode '{uut.EventCode}'.");
            }
            // TODO: Invoke TestEvents with $"{uut.Number} {uut.SerialNumber} {uut.eventCode}";
        }
    }
}