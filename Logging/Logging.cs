using System;
using System.IO;
using System.DirectoryServices.AccountManagement;
using System.Reflection;
using System.Windows.Forms;
using ABTTestLibrary.Config;
using ABTTestLibrary.TestSupport;
using Serilog;

namespace ABTTestLibrary.Logging {
    public static class LogTasks {
        public static readonly String LOGGER_FILE = $"{Path.GetTempPath()}ABTTestLibraryLog.txt";
        public const String LOGGER_TEMPLATE = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        public static void Start(ConfigLib configLib, String appAssemblyVersion, String libraryAssemblyVersion, Group group, ref RichTextBox rtfResults) {
            if (!group.Required) {
                // When non-Required Groups are executed, test data is never saved to config.Logger.FilePath as UTF-8 text.  Never.
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
                // When Required Groups are executed, test data is always & automatically saved to config.Logger.FilePath as UTF-8 text.  Always.
                // RichTextBox + File.
                FileStart();
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Sink(new RichTextBoxSink(richTextBox: ref rtfResults, outputTemplate: LOGGER_TEMPLATE))
                    .WriteTo.File(LOGGER_FILE, outputTemplate: LOGGER_TEMPLATE, fileSizeLimitBytes: null, retainedFileCountLimit: null)
                    .CreateLogger();
            } else if (!configLib.Logger.FileEnabled && configLib.Logger.SQLEnabled) {
                // TODO: RichTextBox + SQL.
                SQLStart(configLib, group);
            } else if (configLib.Logger.FileEnabled && configLib.Logger.SQLEnabled) {
                // TODO: RichTextBox + File + SQL.
                FileStart();
                SQLStart(configLib, group);
            } else {
                // RichTextBox only; customer doesn't require saved test data, unusual for Functional testing, but common for other testing methodologies.
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Sink(new RichTextBoxSink(richTextBox: ref rtfResults, outputTemplate: LOGGER_TEMPLATE))
                    .CreateLogger();
            }
            Log.Information($"START                  : {DateTime.Now}");
            Log.Information($"ABT Program Version    : {appAssemblyVersion}");
            Log.Information($"ABT Library Version    : {libraryAssemblyVersion}");
            Log.Information($"UUT Customer           : {configLib.UUT.Customer}");
            Log.Information($"UUT Test Specification : {configLib.UUT.TestSpecification}");
            Log.Information($"UUT Description        : {configLib.UUT.Description}");
            Log.Information($"UUT Type               : {configLib.UUT.Type}");
            Log.Information($"UUT Number             : {configLib.UUT.Number}");
            Log.Information($"UUT Revision           : {configLib.UUT.Revision}");
            Log.Information($"UUT Group ID           : {group.ID}");
            Log.Information($"UUT Group Revision     : {group.Revision}");
            Log.Information($"UUT Group Summary      : {group.Summary}");
            Log.Information($"UUT Group Detail{Environment.NewLine}{Environment.NewLine}" +
                $"{group.Detail}{Environment.NewLine}");
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
            message += $"  Summary     : {test.Summary}{Environment.NewLine}";
            message += $"  Detail      : {test.Detail}{Environment.NewLine}";
            message += $"  Limit High  : {test.LimitHigh}{Environment.NewLine}";
            message += $"  Measurement : {test.Measurement}{Environment.NewLine}";
            message += $"  Limit Low   : {test.LimitLow}{Environment.NewLine}";
            message += $"  Units       : {test.Units}{Environment.NewLine}";
            message += $"  UnitType    : {test.UnitType}{Environment.NewLine}";
            message += $"  Result      : {test.Result}{Environment.NewLine}";
            Log.Information(message);
        }

        public static void Stop(ConfigLib configLib, Group group) {
            if (!group.Required) Log.CloseAndFlush();
            // Log Trailer isn't written when Group isn't Required, futher emphasizing test results
            // aren't valid for pass verdict/$hip disposition, only troubleshooting failures.
            else {
                Log.Information($"Final Result: {configLib.UUT.EventCode}");
                Log.Information($"STOP:  {DateTime.Now}");
                Log.CloseAndFlush();
                if (configLib.Logger.FileEnabled) FileStop(configLib, group);
                if (configLib.Logger.SQLEnabled) SQLStop(configLib, group);
            }
        }

        private static void FileStart() {
            if (File.Exists(LOGGER_FILE)) File.Delete(LOGGER_FILE);
            // A previous run likely failed to complete; delete it and begin anew.
        }

        private static void FileStop(ConfigLib configLib, Group group) {
            String fileName = $"{configLib.UUT.Number}_{configLib.UUT.SerialNumber}_{group.ID}";
            String[] files = Directory.GetFiles(configLib.Logger.FilePath, $"{fileName}_*.txt", SearchOption.TopDirectoryOnly);
            // files is the set of all files in config.Logger.FilePath like config.UUT.Number_Config.UUT.SerialNumber_Config.Group.ID_*.txt.
            Int32 maxNumber = 0; String s;
            foreach (String f in files) {
                s = f;
                s = s.Replace($"{configLib.Logger.FilePath}{fileName}", String.Empty);
                s = s.Replace(".txt", String.Empty);
                s = s.Replace("_", String.Empty);
                foreach (FieldInfo fi in typeof(EventCodes).GetFields()) s = s.Replace((String)fi.GetValue(null), String.Empty);
                if (Int32.Parse(s) > maxNumber) maxNumber = Int32.Parse(s);
                // Example for final (3rd) iteration of foreach (String f in files):
                //   FileName            : 'UUTNumber_GroupID_SerialNumber'
                //   Initially           : 'P:\Test\TDR\D4522137\Functional\UUTNumber_GroupID_SerialNumber_3_PASS.txt'
                //   FilePath + fileName : '_3_PASS.txt'
                //   .txt                : '_3_PASS'
                //   _                   : '3PASS'
                //   foreach (FieldInfo  : '3'
                //   maxNumber           : '3'
            }
            fileName += $"_{++maxNumber}_{configLib.UUT.EventCode}.txt";
            File.Move(LOGGER_FILE, $"{configLib.Logger.FilePath}{fileName}");
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
                case EventCodes.ABORT:
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