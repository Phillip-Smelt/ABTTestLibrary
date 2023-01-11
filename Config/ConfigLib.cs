using System;
using System.Configuration;
using TestLibrary.TestSupport;

namespace TestLibrary.Config {
    public class Logger {
        public Boolean FileEnabled { get; private set; }
        public String FilePath { get; private set; }
        public Boolean SQLEnabled { get; private set; }
        public String SQLConnectionString { get; private set; }
        public Boolean TestEventsEnabled { get; private set; }

        private Logger(Boolean FileEnabled, String FilePath, Boolean SQLEnabled, String SQLConnectionString, Boolean TestEventsEnabled) {
            this.FileEnabled = FileEnabled;
            if (!FilePath.EndsWith(@"\")) FilePath += @"\"; // Logging.FileStop() requires terminating "\" character.
            this.FilePath = FilePath;
            this.SQLEnabled = SQLEnabled;
            this.SQLConnectionString = SQLConnectionString;
            this.TestEventsEnabled = TestEventsEnabled;
        }

        public static Logger Get() {
            return new Logger(
                Boolean.Parse(ConfigurationManager.AppSettings["LOGGER_FileEnabled"]),
                ConfigurationManager.AppSettings["LOGGER_FilePath"],
                Boolean.Parse(ConfigurationManager.AppSettings["LOGGER_SQLEnabled"]),
                ConfigurationManager.AppSettings["LOGGER_SQLConnectionString"],
                Boolean.Parse(ConfigurationManager.AppSettings["LOGGER_TestEventsEnabled"])
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

        private UUT(String Customer, String Type, String Number, String Revision, String Description, String TestSpecification, String DocumentationFolder, String SerialNumber, String EventCode) {
            this.Customer = Customer;
            this.Type = Type;
            this.Number = Number;
            this.Revision = Revision;
            this.Description = Description;
            this.TestSpecification = TestSpecification;
            this.DocumentationFolder = DocumentationFolder;
            this.SerialNumber = SerialNumber;
            this.EventCode = EventCode;
        }

        public static UUT Get() {
            return new UUT(
                ConfigurationManager.AppSettings["UUT_Customer"],
                ConfigurationManager.AppSettings["UUT_Type"],
                ConfigurationManager.AppSettings["UUT_Number"],
                ConfigurationManager.AppSettings["UUT_Revision"],
                ConfigurationManager.AppSettings["UUT_Description"],
                ConfigurationManager.AppSettings["UUT_TestSpecification"],
                ConfigurationManager.AppSettings["UUT_DocumentationFolder"],
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

