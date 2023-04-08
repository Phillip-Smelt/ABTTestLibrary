using System;
using System.Configuration;

namespace TestLibrary.AppConfig {
    public class AppConfigLogger {
        public Boolean FileEnabled { get; private set; } = Boolean.Parse(ConfigurationManager.AppSettings["LOGGER_FileEnabled"].Trim());
        public String FilePath { get; private set; } = ConfigurationManager.AppSettings["LOGGER_FilePath"].Trim();
        public Boolean SQLEnabled { get; private set; } = Boolean.Parse(ConfigurationManager.AppSettings["LOGGER_SQLEnabled"].Trim());
        public String SQLConnectionString { get; private set; } = ConfigurationManager.AppSettings["LOGGER_SQLConnectionString"].Trim();
        public Boolean TestEventsEnabled { get; private set; } = Boolean.Parse(ConfigurationManager.AppSettings["LOGGER_TestEventsEnabled"].Trim());

        public AppConfigLogger() { if (this.FilePath.EndsWith(@"\")) this.FilePath += @"\"; }
        // Logging.FileStop() requires terminating "\" character.

        public static AppConfigLogger Get() { return new AppConfigLogger(); }
    }
}
