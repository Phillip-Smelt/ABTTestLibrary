using System;
using System.Configuration;

namespace TestLibrary.AppConfig {
    public class AppConfigUUT {
        public String Customer { get; private set; } = ConfigurationManager.AppSettings["UUT_Customer"].Trim();
        public String Type { get; private set; } = ConfigurationManager.AppSettings["UUT_Type"].Trim();
        public String Number { get; private set; } = ConfigurationManager.AppSettings["UUT_Number"].Trim();
        public String Revision { get; private set; } = ConfigurationManager.AppSettings["UUT_Revision"].Trim();
        public String Description { get; private set; } = ConfigurationManager.AppSettings["UUT_Description"].Trim();
        public String TestSpecification { get; private set; } = ConfigurationManager.AppSettings["UUT_TestSpecification"].Trim();
        public String DocumentationFolder { get; private set; } = ConfigurationManager.AppSettings["UUT_DocumentationFolder"].Trim();
        public String SerialNumber { get; set; } = String.Empty; // Input during testing.
        public String EventCode { get; set; } = EventCodes.UNSET; // Determined post-test.

        public AppConfigUUT() { }
    }
}
