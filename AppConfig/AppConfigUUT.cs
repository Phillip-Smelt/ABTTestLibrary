using System;
using System.Configuration;

namespace TestLibrary.AppConfig {
    public class AppConfigUUT {
        public readonly String Customer = ConfigurationManager.AppSettings["UUT_Customer"].Trim();
        public readonly String Type = ConfigurationManager.AppSettings["UUT_Type"].Trim();
        public readonly String Number = ConfigurationManager.AppSettings["UUT_Number"].Trim();
        public readonly String Revision = ConfigurationManager.AppSettings["UUT_Revision"].Trim();
        public readonly String Description = ConfigurationManager.AppSettings["UUT_Description"].Trim();
        public readonly String TestSpecification = ConfigurationManager.AppSettings["UUT_TestSpecification"].Trim();
        public readonly String DocumentationFolder = ConfigurationManager.AppSettings["UUT_DocumentationFolder"].Trim();
        public String SerialNumber { get; set; } = String.Empty; // Input during testing.
        public String EventCode { get; set; } = EventCodes.UNSET; // Determined post-test.

        private AppConfigUUT() { }

        public static AppConfigUUT Get() { return new AppConfigUUT();  }
    }
}
