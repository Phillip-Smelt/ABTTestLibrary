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

        public static void Start(Config Config, ref RichTextBox rtfResults) {
            if (!Config.Group.Required) {
                // When non-Required Groups are executed, test data is never saved to Config.Logger.FilePath as UTF-8 text.  Never.
                // RichTextBox only. 
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Sink(new RichTextBoxSink(richTextBox: ref rtfResults, outputTemplate: LOGGER_TEMPLATE))
                    .CreateLogger();
                Log.Information($"Note: following test results invalid for UUT production testing, only troubleshooting.");
                Log.Information($"UUT Number             : {Config.UUT.Number}");
                Log.Information($"UUT Revision           : {Config.UUT.Revision}");
                Log.Information($"UUT Serial Number      : {Config.UUT.SerialNumber}");
                Log.Information($"UUT Group ID           : {Config.Group.ID}");
                return;
                // Log Header isn't written to Console when Group not Required, futher emphasizing test results are invalid for pass verdict/$hip disposition, only troubleshooting failures.
            }

            if (Config.Logger.FileEnabled && !Config.Logger.SQLEnabled) {
                // When Required Groups are executed, test data is always & automatically saved to Config.Logger.FilePath as UTF-8 text.  Always.
                // RichTextBox + File.
                FileStart(Config);
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Sink(new RichTextBoxSink(richTextBox: ref rtfResults, outputTemplate: LOGGER_TEMPLATE))
                    .WriteTo.File(LOGGER_FILE, outputTemplate: LOGGER_TEMPLATE, fileSizeLimitBytes: null, retainedFileCountLimit: null)
                    .CreateLogger();
            } else if (!Config.Logger.FileEnabled && Config.Logger.SQLEnabled) {
                // TODO: Logger - RichTextBox + SQL.
                SQLStart(Config);
            } else if (Config.Logger.FileEnabled && Config.Logger.SQLEnabled) {
                // TODO: Logger - RichTextBox + File + SQL.
                FileStart(Config);
                SQLStart(Config);
            } else {
                // RichTextBox only; customer doesn't require saved test data, unusual for Functional testing, but common for other testing methodologies.
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Sink(new RichTextBoxSink(richTextBox: ref rtfResults, outputTemplate: LOGGER_TEMPLATE))
                    .CreateLogger();
            }
            Log.Information($"START                  : {DateTime.Now}");
            Log.Information($"UUT Customer           : {Config.UUT.Customer}");
            Log.Information($"UUT Test Specification : {Config.UUT.TestSpecification}");
            Log.Information($"UUT Description        : {Config.UUT.Description}");
            Log.Information($"UUT Type               : {Config.UUT.Type}");
            Log.Information($"UUT Number             : {Config.UUT.Number}");
            Log.Information($"UUT Revision           : {Config.UUT.Revision}");
            Log.Information($"UUT Group ID           : {Config.Group.ID}");
            Log.Information($"UUT Group Summary      : {Config.Group.Summary}");
            Log.Information($"UUT Group Detail       \n{Config.Group.Detail}");
            Log.Information($"Environment.UserName   : {Environment.UserName}");
            Log.Information($"UUT Serial Number      : {Config.UUT.SerialNumber}\n");
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

        public static void Stop(Config Config) {
            if (!Config.Group.Required) Log.CloseAndFlush();
            // Log Trailer isn't written when Group isn't Required, futher emphasizing test results
            // aren't valid for pass verdict/$hip disposition, only troubleshooting failures.
            else {
                Log.Information($"Final Result: {Config.UUT.EventCode}");
                Log.Information($"STOP:  {DateTime.Now}");
                Log.CloseAndFlush();
                if (Config.Logger.FileEnabled) FileStop(Config);
                if (Config.Logger.SQLEnabled) SQLStop(Config);
            }
        }

        private static void FileStart(Config Config) {
            if (File.Exists(LOGGER_FILE)) File.Delete(LOGGER_FILE);
            // A previous run likely failed to complete; delete it and begin anew.
        }

        private static void FileStop(Config Config) {
            String fileName = $"{Config.UUT.Number}_{Config.UUT.SerialNumber}_{Config.Group.ID}";
            String[] files = Directory.GetFiles(Config.Logger.FilePath, $"{fileName}_*.txt", SearchOption.TopDirectoryOnly);
            // files is the set of all files in Config.Logger.FilePath like Config.UUT.Number_Config.UUT.SerialNumber_Config.Group.ID_*.txt.
            Int32 maxNumber = 0; String s;
            foreach (String f in files) {
                s = f;
                s = s.Replace($"{Config.Logger.FilePath}{fileName}", String.Empty);
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
            fileName += $"_{++maxNumber}_{Config.UUT.EventCode}.txt";
            File.Move(LOGGER_FILE, $"{Config.Logger.FilePath}{fileName}");
        }

        private static void SQLStart(Config Config) {
            // TODO: Logger - SQLStart.
        }

        private static void SQLStop(Config Config) {
            // TODO: Logger - SQLStop.
        }

        public static void TestEvents(Config Config) {
            String eventCode = String.Empty;
            switch (Config.UUT.EventCode) {
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
                    throw new Exception($"Unrecognized EventCode '{Config.UUT.EventCode}'.");
                // TODO: Logger - Invoke TestEvents with $"{Config.UUT.Number} {Config.UUT.SerialNumber} {eventCode}";
            }
        }
    }
}