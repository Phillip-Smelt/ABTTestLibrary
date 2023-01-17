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

