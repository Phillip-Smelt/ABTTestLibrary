using System;
using System.Collections.Generic;
using System.Configuration;
using ABTTestLibrary.TestSupport;

namespace ABTTestLibrary.AppConfig {
    public class App {
        public String Revision { get; private set; }
        public Boolean TestEventsEnabled { get; private set; }

        private App(String Revision, Boolean TestEventsEnabled) {
            this.Revision = Revision;
            this.TestEventsEnabled = TestEventsEnabled;
        }

        public static App Get() {
            return new App(ConfigurationManager.AppSettings["APP_Revision"], 
                Boolean.Parse(ConfigurationManager.AppSettings["APP_TestEventsEnabled"]));
        }
    }

    public class Logger {
        public Boolean FileEnabled { get; private set; }
        public String FilePath { get; private set; }
        public Boolean SQLEnabled { get; private set; }
        public String SQLConnectionString { get; private set; }

        private Logger(Boolean FileEnabled, String FilePath, Boolean SQLEnabled, String SQLConnectionString) {
            this.FileEnabled = FileEnabled;
            this.FilePath = FilePath;
            this.SQLEnabled = SQLEnabled;
            this.SQLConnectionString = SQLConnectionString;
        }

        public static Logger Get() {
            return new Logger(
                Boolean.Parse(ConfigurationManager.AppSettings["LOGGER_FileEnabled"]),
                ConfigurationManager.AppSettings["LOGGER_FilePath"],
                Boolean.Parse(ConfigurationManager.AppSettings["LOGGER_SQLEnabled"]),
                ConfigurationManager.AppSettings["LOGGER_SQLConnectionString"]
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
        public String SerialNumber { get; set; }
        public String EventCode { get; set; }

        private UUT(String Customer, String Type, String Number, String Revision, String Description, String TestSpecification, String SerialNumber, String EventCode) {
            this.Customer = Customer;
            this.Type = Type;
            this.Number = Number;
            this.Revision = Revision;
            this.Description = Description;
            this.TestSpecification = TestSpecification;
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
                String.Empty, // Input during testing.
                EventCodes.UNSET // Determined post-test.
            );
        }
    }

    public class Config {
        public App App { get; private set; }
        public Logger Logger { get; private set; }
        public UUT UUT { get; private set; }
        public Group Group { get; private set; }
        public Dictionary<String, Test> Tests { get; private set; }

        private Config() {
            this.App = App.Get();
            this.Logger = Logger.Get();
            this.UUT = UUT.Get();

            Dictionary<String, Group> Groups = Group.Get();
            String GroupSelected = GroupSelect.Get(Groups);
            this.Group = Groups[GroupSelected];
            // Operator selects the Group they want to test, from the Dictionary of all Groups.
            // GroupSelected is Dictionary Groups' Key.

            Dictionary<String, Test> tests = Test.Get();
            this.Tests = new Dictionary<String, Test>();
            String[] g = this.Group.TestIDs.Split('|');
            foreach (String s in g) {
                if (!Tests.ContainsKey(s)) throw new InvalidOperationException($"Group '{Group.ID}' includes IDTest '{s}', which isn't present in TestElements in App.config.");
                this.Tests.Add(s, tests[s]);
                // Add only Tests correlated to the Group previously selected by operator.
            }
        }

        public static Config Get() {
            return new Config();
        }
    }
}
