using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using ABTTestLibrary.AppConfig;
using ABTTestLibrary.TestSupport;
using Serilog;

namespace ABTTestLibrary.Logging {
    public static class LogTasks {
        public static readonly String LOGGER_FILE = $"{Path.GetTempPath()}ABTTestLibraryLog.txt";
        public const String LOGGER_TEMPLATE = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        public static void Start(Config config, ref RichTextBox rtfResults) {
            if (!config.Group.Required) {
                // When non-Required Groups are executed, test data is never saved to config.Logger.FilePath as UTF-8 text.  Never.
                // RichTextBox only. 
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Sink(new RichTextBoxSink(richTextBox: ref rtfResults, outputTemplate: LOGGER_TEMPLATE))
                    .CreateLogger();
                Log.Information($"Note: following test results invalid for UUT production testing, only troubleshooting.");
                Log.Information($"UUT Number             : {config.UUT.Number}");
                Log.Information($"UUT Revision           : {config.UUT.Revision}");
                Log.Information($"UUT Serial Number      : {config.UUT.SerialNumber}");
                Log.Information($"UUT Group ID           : {config.Group.ID}");
                return;
                // Log Header isn't written to Console when Group not Required, futher emphasizing test results are invalid for pass verdict/$hip disposition, only troubleshooting failures.
            }

            if (config.Logger.FileEnabled && !config.Logger.SQLEnabled) {
                // When Required Groups are executed, test data is always & automatically saved to config.Logger.FilePath as UTF-8 text.  Always.
                // RichTextBox + File.
                FileStart(config);
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Sink(new RichTextBoxSink(richTextBox: ref rtfResults, outputTemplate: LOGGER_TEMPLATE))
                    .WriteTo.File(LOGGER_FILE, outputTemplate: LOGGER_TEMPLATE, fileSizeLimitBytes: null, retainedFileCountLimit: null)
                    .CreateLogger();
            } else if (!config.Logger.FileEnabled && config.Logger.SQLEnabled) {
                // TODO: Logger - RichTextBox + SQL.
                SQLStart(config);
            } else if (config.Logger.FileEnabled && config.Logger.SQLEnabled) {
                // TODO: Logger - RichTextBox + File + SQL.
                FileStart(config);
                SQLStart(config);
            } else {
                // RichTextBox only; customer doesn't require saved test data, unusual for Functional testing, but common for other testing methodologies.
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Sink(new RichTextBoxSink(richTextBox: ref rtfResults, outputTemplate: LOGGER_TEMPLATE))
                    .CreateLogger();
            }
            Log.Information($"START                  : {DateTime.Now}");
            Log.Information($"UUT Customer           : {config.UUT.Customer}");
            Log.Information($"UUT Test Specification : {config.UUT.TestSpecification}");
            Log.Information($"UUT Description        : {config.UUT.Description}");
            Log.Information($"UUT Type               : {config.UUT.Type}");
            Log.Information($"UUT Number             : {config.UUT.Number}");
            Log.Information($"UUT Revision           : {config.UUT.Revision}");
            Log.Information($"UUT Group ID           : {config.Group.ID}");
            Log.Information($"UUT Group Summary      : {config.Group.Summary}");
            Log.Information($"UUT Group Detail       \n{config.Group.Detail}");
            Log.Information($"Environment.UserName   : {Environment.UserName}");
            Log.Information($"UUT Serial Number      : {config.UUT.SerialNumber}\n");
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
            message += $"  Summary     : {test.Summary}{Environment.NewLine}";
            message += $"  Detail      : {test.Detail}{Environment.NewLine}";
            message += $"  Limit Low   : {test.LimitLow}{Environment.NewLine}";
            message += $"  Measurement : {test.Measurement}{Environment.NewLine}";
            message += $"  Limit High  : {test.LimitHigh}{Environment.NewLine}";
            message += $"  Units       : {test.Units}{Environment.NewLine}";
            message += $"  Result      : {test.Result}{Environment.NewLine}";
            Log.Information(message);
        }

        public static void Stop(Config config) {
            if (!config.Group.Required) Log.CloseAndFlush();
            // Log Trailer isn't written when Group isn't Required, futher emphasizing test results
            // aren't valid for pass verdict/$hip disposition, only troubleshooting failures.
            else {
                Log.Information($"Final Result: {config.UUT.EventCode}");
                Log.Information($"STOP:  {DateTime.Now}");
                Log.CloseAndFlush();
                if (config.Logger.FileEnabled) FileStop(config);
                if (config.Logger.SQLEnabled) SQLStop(config);
            }
        }

        private static void FileStart(Config config) {
            if (File.Exists(LOGGER_FILE)) File.Delete(LOGGER_FILE);
            // A previous run likely failed to complete; delete it and begin anew.
        }

        private static void FileStop(Config config) {
            String fileName = $"{config.UUT.Number}_{config.UUT.SerialNumber}_{config.Group.ID}";
            String[] files = Directory.GetFiles(config.Logger.FilePath, $"{fileName}_*.txt", SearchOption.TopDirectoryOnly);
            // files is the set of all files in config.Logger.FilePath like config.UUT.Number_Config.UUT.SerialNumber_Config.Group.ID_*.txt.
            Int32 maxNumber = 0; String s;
            foreach (String f in files) {
                s = f;
                s = s.Replace($"{config.Logger.FilePath}{fileName}", String.Empty);
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
            fileName += $"_{++maxNumber}_{config.UUT.EventCode}.txt";
            File.Move(LOGGER_FILE, $"{config.Logger.FilePath}{fileName}");
        }

        private static void SQLStart(Config config) {
            // TODO: Logger - SQLStart.
        }

        private static void SQLStop(Config config) {
            // TODO: Logger - SQLStop.
        }

        public static void TestEvents(Config config) {
            String eventCode = String.Empty;
            switch (config.UUT.EventCode) {
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
                    throw new Exception($"Unrecognized EventCode '{config.UUT.EventCode}'.");
                // TODO: Logger - Invoke TestEvents with $"{config.UUT.Number} {config.UUT.SerialNumber} {eventCode}";
            }
        }
    }
}