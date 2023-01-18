using System;
using System.IO;
using System.DirectoryServices.AccountManagement;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using TestLibrary.Config;
using TestLibrary.TestSupport;
using Serilog;
using System.Runtime.Remoting.Channels;
using System.Collections.Generic;

namespace TestLibrary.Logging {
    public static class LogTasks {
        public const String LOGGER_TEMPLATE = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        public static void Start(ConfigLib configLib, String appAssemblyVersion, String libraryAssemblyVersion, Group group, ref RichTextBox rtfResults) {
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
                Log.Information($"UUT Group ID           : {group.ID}");
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
            Log.Information($"TestProgram Version    : {appAssemblyVersion}");
            Log.Information($"TestLibrary Version    : {libraryAssemblyVersion}");
            Log.Information($"UUT Customer           : {configLib.UUT.Customer}");
            Log.Information($"UUT Test Specification : {configLib.UUT.TestSpecification}");
            Log.Information($"UUT Description        : {configLib.UUT.Description}");
            Log.Information($"UUT Type               : {configLib.UUT.Type}");
            Log.Information($"UUT Number             : {configLib.UUT.Number}");
            Log.Information($"UUT Revision           : {configLib.UUT.Revision}");
            Log.Information($"UUT Group ID           : {group.ID}");
            Log.Information($"UUT Group Revision     : {group.Revision}");
            Log.Information($"UUT Group Name         : {group.Name}");
            Log.Information($"UUT Group Description  :{Environment.NewLine}{Environment.NewLine}" +
                $"{group.Description}{Environment.NewLine}");
            Log.Information($"Test Operator          : {UserPrincipal.Current.DisplayName}");
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
            message =  $"Test ID '{test.ID}'{Environment.NewLine}";
            message += $"  Revision    : {test.Revision}{Environment.NewLine}";
            message += $"  Description : {test.Description}{Environment.NewLine}";
            message += $"  ClassName   : {test.ClassName}{Environment.NewLine}";
            switch (test.ClassName) {
                case TestCustom.ClassName:
                    TestCustom tc = (TestCustom)test.ClassObject;
                    foreach (KeyValuePair<String, String> kvp in tc.Arguments) message += $"  Key=Value   : {kvp.Key}={kvp.Value}{Environment.NewLine}";
                    break;
                case TestISP.ClassName:
                    TestISP tp = (TestISP)test.ClassObject;
                    message += $"  ISPResult   : {tp.ISPResult}{Environment.NewLine}";
                    message += $"  Measurement : {test.Measurement}{Environment.NewLine}";
                    break;
                case TestRanged.ClassName:
                    TestRanged tr = (TestRanged)test.ClassObject;
                    message += $"  High Limit  : {tr.High}{Environment.NewLine}";
                    message += $"  Measurement : {test.Measurement}{Environment.NewLine}";
                    message += $"  Low Limit   : {tr.Low}{Environment.NewLine}";
                    message += $"  Units       : {tr.Unit}{Environment.NewLine}";
                    message += $"  UnitType    : {tr.UnitType}{Environment.NewLine}";
                    break;
                case TestTextual.ClassName:
                    TestTextual tt = (TestTextual)test.ClassObject;
                    message += $"  Text        : {tt.Text}{Environment.NewLine}";
                    message += $"  Measurement : {test.Measurement}{Environment.NewLine}";
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
            // Will fail if invalid this.configLib.Logger.FilePath.  Don't catch resulting Exception though; this has to be fixed in App.config.
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