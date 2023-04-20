using System;
using System.Configuration;

namespace ABT.TestSpace.AppConfig {
    public class AppConfigLogger {
        public readonly Boolean FileEnabled = Boolean.Parse(ConfigurationManager.AppSettings["LOGGER_FileEnabled"].Trim());
        public readonly String FilePath = ConfigurationManager.AppSettings["LOGGER_FilePath"].Trim();
        public readonly Boolean SQLEnabled = Boolean.Parse(ConfigurationManager.AppSettings["LOGGER_SQLEnabled"].Trim());
        public readonly String SQLConnectionString = ConfigurationManager.AppSettings["LOGGER_SQLConnectionString"].Trim();
        public readonly Boolean TestEventsEnabled = Boolean.Parse(ConfigurationManager.AppSettings["LOGGER_TestEventsEnabled"].Trim());

        private AppConfigLogger() { if (!this.FilePath.EndsWith(@"\")) this.FilePath += @"\"; }
        // Logging.FileStop() requires terminating "\" character.

        public static AppConfigLogger Get() { return new AppConfigLogger(); }
    }
}
