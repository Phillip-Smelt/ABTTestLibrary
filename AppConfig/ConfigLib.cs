using System;
using System.Configuration;
using TestLibrary.TestSupport;

namespace TestLibrary.AppConfig {
    public class Logger {
        public Boolean FileEnabled { get; private set; }
        public String FilePath { get; private set; }
        public Boolean SQLEnabled { get; private set; }
        public String SQLConnectionString { get; private set; }
        public Boolean TestEventsEnabled { get; private set; }

        private Logger(Boolean fileEnabled, String filePath, Boolean sqlEnabled, String sqlConnectionString, Boolean testEventsEnabled) {
            this.FileEnabled = fileEnabled;
            if (!filePath.EndsWith(@"\")) filePath += @"\"; // Logging.FileStop() requires terminating "\" character.
            this.FilePath = filePath;
            this.SQLEnabled = sqlEnabled;
            this.SQLConnectionString = sqlConnectionString;
            this.TestEventsEnabled = testEventsEnabled;
        }

        public static Logger Get() {
            return new Logger(
                Boolean.Parse(ConfigurationManager.AppSettings["LOGGER_FileEnabled"].Trim()),
                ConfigurationManager.AppSettings["LOGGER_FilePath"].Trim(),
                Boolean.Parse(ConfigurationManager.AppSettings["LOGGER_SQLEnabled"].Trim()),
                ConfigurationManager.AppSettings["LOGGER_SQLConnectionString"].Trim(),
                Boolean.Parse(ConfigurationManager.AppSettings["LOGGER_TestEventsEnabled"].Trim())
            );
        }
    }

    public class UUT {
        public String Customer { get; private set; }
        public String Type { get; private set; }
        public String Number { get; private set; }
        public String Revision { get; private set; }
        public String Description { get; private set; }
        public String TestSpecification { get; private set; }
        public String DocumentationFolder { get; private set; }
        public String SerialNumber { get; set; }
        public String EventCode { get; set; }

        private UUT(String customer, String type, String number, String revision, String description, String testSpecification, String documentationFolder, String serialNumber, String eventCode) {
            this.Customer = customer;
            this.Type = type;
            this.Number = number;
            this.Revision = revision;
            this.Description = description;
            this.TestSpecification = testSpecification;
            this.DocumentationFolder = documentationFolder;
            this.SerialNumber = serialNumber;
            this.EventCode = eventCode;
        }

        public static UUT Get() {
            return new UUT(
                ConfigurationManager.AppSettings["UUT_Customer"].Trim(),
                ConfigurationManager.AppSettings["UUT_Type"].Trim(),
                ConfigurationManager.AppSettings["UUT_Number"].Trim(),
                ConfigurationManager.AppSettings["UUT_Revision"].Trim(),
                ConfigurationManager.AppSettings["UUT_Description"].Trim(),
                ConfigurationManager.AppSettings["UUT_TestSpecification"].Trim(),
                ConfigurationManager.AppSettings["UUT_DocumentationFolder"].Trim(),
                String.Empty, // Input during testing.
                EventCodes.UNSET // Determined post-test.
            );
        }
    }

    public class ConfigLib {
        public Logger Logger { get; private set; }
        public UUT UUT { get; private set; }

        private ConfigLib() {
            this.Logger = Logger.Get();
            this.UUT = UUT.Get();
        }

        public static ConfigLib Get() {
            return new ConfigLib();
        }
    }
}

