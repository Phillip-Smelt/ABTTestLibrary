using System;
using System.Configuration;

namespace TestLibrary.AppConfig {
    public class ConfigLogger {
        public Boolean FileEnabled { get; private set; }
        public String FilePath { get; private set; }
        public Boolean SQLEnabled { get; private set; }
        public String SQLConnectionString { get; private set; }
        public Boolean TestEventsEnabled { get; private set; }

        private ConfigLogger(Boolean fileEnabled, String filePath, Boolean sqlEnabled, String sqlConnectionString, Boolean testEventsEnabled) {
            this.FileEnabled = fileEnabled;
            if (!filePath.EndsWith(@"\")) filePath += @"\"; // Logging.FileStop() requires terminating "\" character.
            this.FilePath = filePath;
            this.SQLEnabled = sqlEnabled;
            this.SQLConnectionString = sqlConnectionString;
            this.TestEventsEnabled = testEventsEnabled;
        }

        public static ConfigLogger Get() {
            return new ConfigLogger(
                Boolean.Parse(ConfigurationManager.AppSettings["LOGGER_FileEnabled"].Trim()),
                ConfigurationManager.AppSettings["LOGGER_FilePath"].Trim(),
                Boolean.Parse(ConfigurationManager.AppSettings["LOGGER_SQLEnabled"].Trim()),
                ConfigurationManager.AppSettings["LOGGER_SQLConnectionString"].Trim(),
                Boolean.Parse(ConfigurationManager.AppSettings["LOGGER_TestEventsEnabled"].Trim())
            );
        }
    }
}

