using System;
using System.IO;
using System.DirectoryServices.AccountManagement;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Serilog; // Install Serilog via NuGet Package Manager.  Site is https://serilog.net/.
using ABT.TestSpace.AppConfig;

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

        public static void Start(AppConfigUUT configUUT, AppConfigLogger configLogger, AppConfigTest configTest, String _appAssemblyVersion, String _libraryAssemblyVersion, ref RichTextBox rtfResults) {
            if (!configTest.Group.Required) {
                // When non-Required Groups are executed, test data is never saved to configLogger.FilePath as Rich Text.  Never.
                // RichTextBox only. 
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Sink(new RichTextBoxSink(richTextBox: ref rtfResults, outputTemplate: LOGGER_TEMPLATE))
                    .CreateLogger();
                Log.Information($"Note: following test results invalid for UUT production testing, only troubleshooting.");
                Log.Information($"START                  : {DateTime.Now}");
                Log.Information($"UUT Number             : {configUUT.Number}");
                Log.Information($"UUT Revision           : {configUUT.Revision}");
                Log.Information($"UUT Serial Number      : {configUUT.SerialNumber}");
                Log.Information($"UUT Group ID           : {configTest.Group.ID}\n");
                return;
                // Log Header isn't written to Console when Group not Required, further emphasizing test results are invalid for pass verdict/$hip disposition, only troubleshooting failures.
            }

            if (configLogger.FileEnabled && !configLogger.SQLEnabled) {
                // When Required Groups are executed, test data is always & automatically saved to config.Logger.FilePath as Rich Text.  Always.
                // RichTextBox + File.
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Sink(new RichTextBoxSink(richTextBox: ref rtfResults, outputTemplate: LOGGER_TEMPLATE))
                    .CreateLogger();
            } else if (!configLogger.FileEnabled && configLogger.SQLEnabled) {
                // TODO: RichTextBox + SQL.
                SQLStart(configUUT, configTest.Group);
            } else if (configLogger.FileEnabled && configLogger.SQLEnabled) {
                // TODO: RichTextBox + File + SQL.
                SQLStart(configUUT, configTest.Group);
            } else {
                // RichTextBox only; customer doesn't require saved test data, unusual for Functional testing, but common for other testing methodologies.
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Sink(new RichTextBoxSink(richTextBox: ref rtfResults, outputTemplate: LOGGER_TEMPLATE))
                    .CreateLogger();
            }
            Log.Information($"START                  : {DateTime.Now}");
            Log.Information($"TestExecutor Version   : {_appAssemblyVersion}");
            Log.Information($"TestExecutive Version  : {_libraryAssemblyVersion}");
            Log.Information($"UUT Customer           : {configUUT.Customer}");
            Log.Information($"UUT Test Specification : {configUUT.TestSpecification}");
            Log.Information($"UUT Description        : {configUUT.Description}");
            Log.Information($"UUT Type               : {configUUT.Type}");
            Log.Information($"UUT Number             : {configUUT.Number}");
            Log.Information($"UUT Revision           : {configUUT.Revision}");
            Log.Information($"UUT Group ID           : {configTest.Group.ID}");
            Log.Information($"UUT Group Revision     : {configTest.Group.Revision}");
            Log.Information($"UUT Group Description  : \n{configTest.Group.Description}\n");
            StringBuilder s = new StringBuilder();
            foreach (KeyValuePair<String, Test> kvp in configTest.Tests) s.Append(String.Format("\t{0:" + configTest.LogFormattingLength + "} : {1}\n", kvp.Value.ID, kvp.Value.Description));
            Log.Information($"UUT Group Tests        : \n{s}");
            Log.Information($"Test Operator          : {UserPrincipal.Current.DisplayName}");
            // NOTE: UserPrincipal.Current.DisplayName requires a connected/active Domain session for Active Directory PCs.
            // Haven't used it on non-Active Directory PCs.
            Log.Information($"UUT Serial Number      : {configUUT.SerialNumber}\n");
            Log.Debug($"Environment.UserDomainName         : {Environment.UserDomainName}");
            Log.Debug($"Environment.MachineName            : {Environment.MachineName}");
            Log.Debug($"Environment.OSVersion              : {Environment.OSVersion}");
            Log.Debug($"Environment.Is64BitOperatingSystem : {Environment.Is64BitOperatingSystem}");
            Log.Debug($"Environment.Is64BitProcess         : {Environment.Is64BitProcess}");
            Log.Debug($"Environment.Version                : {Environment.Version}\n");
        }

        public static void LogTest(Test test) {
            String message;
            message =  $"Test ID '{test.ID}'\n";
            message += $"  Revision    : {test.Revision}\n";
            message += $"  Description : {test.Description}\n";
            message += $"  Test Type   : {test.ClassName}\n";
            switch (test.ClassName) {
                case TestCustomizable.ClassName:
                    TestCustomizable testCustomizable = (TestCustomizable)test.ClassObject;
                    foreach (KeyValuePair<String, String> kvp in testCustomizable.Arguments) message += $"  Key=Value   : {kvp.Key}={kvp.Value}\n";
                    message += $"  Actual      : {test.Measurement}\n";
                    break;
                case TestISP.ClassName:
                    TestISP testISP = (TestISP)test.ClassObject;
                    message += $"  Expected    : {testISP.ISPExpected}\n";
                    message += $"  Actual      : {test.Measurement}\n";
                    break;
                case TestNumerical.ClassName:
                    TestNumerical testNumerical = (TestNumerical)test.ClassObject;
                    message += $"  High Limit  : {testNumerical.High:G}\n";
                    message += $"  Measurement : {Double.Parse(test.Measurement, NumberStyles.Float, CultureInfo.CurrentCulture):G}\n";
                    message += $"  Low Limit   : {testNumerical.Low:G}\n";
                    message += $"  SI Units    : {Enum.GetName(typeof(SI_UNITS), testNumerical.SI_Units)}";
                    if (testNumerical.SI_Units_Modifier != SI_UNITS_MODIFIERS.NotApplicable) message += $" {Enum.GetName(typeof(SI_UNITS_MODIFIERS), testNumerical.SI_Units_Modifier)}";
                    message += "\n";
                    break;
                case TestTextual.ClassName:
                    TestTextual testTextual = (TestTextual)test.ClassObject;
                    message += $"  Expected    : {testTextual.Text}\n";
                    message += $"  Actual      : {test.Measurement}\n";
                    break;
                default:
                    throw new NotImplementedException($"TestElement ID '{test.ID}' with ClassName '{test.ClassName}' not implemented.");
            }
            message += $"  Result      : {test.Result}{Environment.NewLine}";
#if DEBUG
            message += test.DebugMessage;
#endif
            Log.Information(message);
        }

        public static void Stop(AppConfigUUT configUUT, AppConfigLogger configLogger, Group group, ref RichTextBox rtfResults) {
            if (!group.Required) Log.CloseAndFlush();
            // Log Trailer isn't written when Group isn't Required, further emphasizing test results
            // aren't valid for pass verdict/$hip disposition, only troubleshooting failures.
            else {
                Log.Information($"Final Result: {configUUT.EventCode}");
                Log.Information($"STOP:  {DateTime.Now}");
                Log.CloseAndFlush();
                if (configLogger.FileEnabled) FileStop(configUUT, configLogger, group, ref rtfResults);
                if (configLogger.SQLEnabled) SQLStop(configUUT, group);
                if (configLogger.TestEventsEnabled) TestEvents(configUUT);
            }
        }

        private static void FileStop(AppConfigUUT configUUT, AppConfigLogger configLogger, Group group, ref RichTextBox rtfResults) {
            String fileName = $"{configUUT.Number}_{configUUT.SerialNumber}_{group.ID}";
            String[] files = Directory.GetFiles(configLogger.FilePath, $"{fileName}_*.rtf", SearchOption.TopDirectoryOnly);
            // Will fail if invalid this.ConfigLogger.FilePath.  Don't catch resulting Exception though; this has to be fixed in App.config.
            // Otherwise, files is the set of all files in config.Logger.FilePath like
            // config.configUUT.Number_Config.configUUT.SerialNumber_Config.Group.ID_*.rtf.
            Int32 maxNumber = 0; String s;
            foreach (String f in files) {
                s = f;
                s = s.Replace($"{configLogger.FilePath}{fileName}", String.Empty);
                s = s.Replace(".rtf", String.Empty);
                s = s.Replace("_", String.Empty);
                foreach (FieldInfo fi in typeof(EventCodes).GetFields()) s = s.Replace((String)fi.GetValue(null), String.Empty);
                if (Int32.Parse(s) > maxNumber) maxNumber = Int32.Parse(s);
                // Example for final (3rd) iteration of foreach (String f in files):
                //   FileName            : 'UUTNumber_GroupID_SerialNumber'
                //   Initially           : 'P:\Test\TDR\D4522137\Functional\UUTNumber_GroupID_SerialNumber_3_PASS.rtf'
                //   FilePath + fileName : '_3_PASS.rtf'
                //   .txt                : '_3_PASS'
                //   _                   : '3PASS'
                //   foreach (FieldInfo  : '3'
                //   maxNumber           : '3'
            }
            fileName += $"_{++maxNumber}_{configUUT.EventCode}.rtf";
            rtfResults.SaveFile($"{configLogger.FilePath}{fileName}");
        }

        private static void SQLStart(AppConfigUUT UUT, Group group) {
            // TODO: SQL Server Express: SQLStart.
        }

        private static void SQLStop(AppConfigUUT UUT, Group group) {
            // TODO: SQL Server Express: SQLStop.
        }

        public static void UnexpectedErrorHandler(String logMessage) {
            Log.Error(logMessage);
            MessageBox.Show(Form.ActiveForm, $"Unexpected error.  Details logged for analysis & resolution.{Environment.NewLine}{Environment.NewLine}" +
                            $"If reoccurs, please contact Test Engineering.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void TestEvents(AppConfigUUT uut) {
            String eventCode;
            switch (uut.EventCode) {
                case EventCodes.CANCEL:
                    eventCode = "A";
                    break;
                case EventCodes.FAIL:
                    eventCode = "F";
                    break;
                case EventCodes.PASS:
                    eventCode = "P";
                    break;
                case EventCodes.ERROR:
                case EventCodes.UNSET:
                    return;
                    // Don't record TestEvents for ERROR or UNSET.
                default:
                    throw new NotImplementedException($"Unrecognized EventCode '{uut.EventCode}'.");
            }
            // TODO: Invoke TestEvents with $"{uut.Number} {uut.SerialNumber} {uut.eventCode}";
        }
    }
}