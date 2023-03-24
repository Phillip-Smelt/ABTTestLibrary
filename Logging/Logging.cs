using System;
using System.IO;
using System.DirectoryServices.AccountManagement;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Serilog; // Install Serilog via NuGet Package Manager.  Site is https://serilog.net/.
using TestLibrary.AppConfig;
using TestLibrary.TestSupport;

// TODO: Persist test data into Microsoft SQL Server Express.
// TODO: Create a Microsoft Access front-end exporting/report app for persisted SQL Server Express test data.  Export in CSV, report in PDF.
// TODO: After test data persisted in MS SQL Server Express & exported/reported from MS Access, eliminate Serilog RTF output, replaced by Access PDF report.
namespace TestLibrary.Logging {
    public static class LogTasks {
        public const String LOGGER_TEMPLATE = "{Message}{NewLine}";

        public static void Start(ConfigLib configLib, ConfigTest configTest, String _appAssemblyVersion, String _libraryAssemblyVersion, Group group, ref RichTextBox rtfResults) {
            if (!group.Required) {
                // When non-Required Groups are executed, test data is never saved to config.Logger.FilePath as Rich Text.  Never.
                // RichTextBox only. 
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Sink(new RichTextBoxSink(richTextBox: ref rtfResults, outputTemplate: LOGGER_TEMPLATE))
                    .CreateLogger();
                Log.Information($"Note: following test results invalid for UUT production testing, only troubleshooting.");
                Log.Information($"UUT Number             : {configLib.UUT.Number}");
                Log.Information($"UUT Revision           : {configLib.UUT.Revision}");
                Log.Information($"UUT Serial Number      : {configLib.UUT.SerialNumber}");
                Log.Information($"UUT Group ID           : {group.ID}\n");
                return;
                // Log Header isn't written to Console when Group not Required, futher emphasizing test results are invalid for pass verdict/$hip disposition, only troubleshooting failures.
            }

            if (configLib.Logger.FileEnabled && !configLib.Logger.SQLEnabled) {
                // When Required Groups are executed, test data is always & automatically saved to config.Logger.FilePath as Rich Text.  Always.
                // RichTextBox + File.
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Sink(new RichTextBoxSink(richTextBox: ref rtfResults, outputTemplate: LOGGER_TEMPLATE))
                    .CreateLogger();
            } else if (!configLib.Logger.FileEnabled && configLib.Logger.SQLEnabled) {
                // TODO: RichTextBox + SQL.
                SQLStart(configLib, group);
            } else if (configLib.Logger.FileEnabled && configLib.Logger.SQLEnabled) {
                // TODO: RichTextBox + File + SQL.
                SQLStart(configLib, group);
            } else {
                // RichTextBox only; customer doesn't require saved test data, unusual for Functional testing, but common for other testing methodologies.
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Sink(new RichTextBoxSink(richTextBox: ref rtfResults, outputTemplate: LOGGER_TEMPLATE))
                    .CreateLogger();
            }
            Log.Information($"START                  : {DateTime.Now}");
            Log.Information($"TestProgram Version    : {_appAssemblyVersion}");
            Log.Information($"TestLibrary Version    : {_libraryAssemblyVersion}");
            Log.Information($"UUT Customer           : {configLib.UUT.Customer}");
            Log.Information($"UUT Test Specification : {configLib.UUT.TestSpecification}");
            Log.Information($"UUT Description        : {configLib.UUT.Description}");
            Log.Information($"UUT Type               : {configLib.UUT.Type}");
            Log.Information($"UUT Number             : {configLib.UUT.Number}");
            Log.Information($"UUT Revision           : {configLib.UUT.Revision}");
            Log.Information($"UUT Group ID           : {group.ID}");
            Log.Information($"UUT Group Revision     : {group.Revision}");
            Log.Information($"UUT Group Description  : \n{group.Description}\n");
            StringBuilder s = new StringBuilder();
            foreach (KeyValuePair<String, Test> test in configTest.Tests) s.Append(String.Format("\t{0:" + configTest.LogFormattingLength + "} : {1}\n", test.Value.ID, test.Value.Description));
            Log.Information($"UUT Group Tests        : \n{s}");
            Log.Information($"Test Operator          : {UserPrincipal.Current.DisplayName}");
            // NOTE: UserPrincipal.Current.DisplayName requires a connected/active Domain session for Active Directory PCs.
            // Haven't used it on non-Active Directory PCs.
            Log.Information($"UUT Serial Number      : {configLib.UUT.SerialNumber}\n");
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
                    message += $"  Expected    : {testISP.ISPResult}\n";
                    message += $"  Actual      : {test.Measurement}\n";
                    break;
                case TestNumerical.ClassName:
                    TestNumerical testNumerical = (TestNumerical)test.ClassObject;
                    message += $"  High Limit  : {testNumerical.High:G}\n";
                    message += $"  Measurement : {Double.Parse(test.Measurement, NumberStyles.Float, CultureInfo.CurrentCulture):G}\n";
                    message += $"  Low Limit   : {testNumerical.Low:G}\n";
                    message += $"  Units       : {testNumerical.Unit}{testNumerical.UnitType}\n";
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
            Log.Information(message);
        }

        public static void Stop(ConfigLib configLib, Group group, ref RichTextBox rtfResults) {
            if (!group.Required) Log.CloseAndFlush();
            // Log Trailer isn't written when Group isn't Required, futher emphasizing test results
            // aren't valid for pass verdict/$hip disposition, only troubleshooting failures.
            else {
                Log.Information($"Final Result: {configLib.UUT.EventCode}");
                Log.Information($"STOP:  {DateTime.Now}");
                Log.CloseAndFlush();
                if (configLib.Logger.FileEnabled) FileStop(configLib, group, ref rtfResults);
                if (configLib.Logger.SQLEnabled) SQLStop(configLib, group);
                if (configLib.Logger.TestEventsEnabled) TestEvents(configLib.UUT);
            }
        }

        private static void FileStop(ConfigLib configLib, Group group, ref RichTextBox rtfResults) {
            String fileName = $"{configLib.UUT.Number}_{configLib.UUT.SerialNumber}_{group.ID}";
            String[] files = Directory.GetFiles(configLib.Logger.FilePath, $"{fileName}_*.rtf", SearchOption.TopDirectoryOnly);
            // Will fail if invalid this.ConfigLib.Logger.FilePath.  Don't catch resulting Exception though; this has to be fixed in App.config.
            // Otherwise, files is the set of all files in config.Logger.FilePath like
            // config.UUT.Number_Config.UUT.SerialNumber_Config.Group.ID_*.rtf.
            Int32 maxNumber = 0; String s;
            foreach (String f in files) {
                s = f;
                s = s.Replace($"{configLib.Logger.FilePath}{fileName}", String.Empty);
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
            fileName += $"_{++maxNumber}_{configLib.UUT.EventCode}.rtf";
            rtfResults.SaveFile($"{configLib.Logger.FilePath}{fileName}");
        }

        private static void SQLStart(ConfigLib configLib, Group group) {
            // TODO: SQLStart.
        }

        private static void SQLStop(ConfigLib configLib, Group group) {
            // TODO: SQLStop.
        }

        public static void TestEvents(UUT uut) {
            String eventCode = String.Empty;
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
            // TODO: Invoke TestEvents with $"{uut.Number} {uut.SerialNumber} {eventCode}";
        }
    }
}