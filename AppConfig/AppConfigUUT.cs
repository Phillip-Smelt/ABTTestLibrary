﻿using System;
using System.Configuration;

namespace ABT.TestSpace.TestExec.AppConfig {
    public class AppConfigUUT {
        public readonly String Customer = ConfigurationManager.AppSettings["UUT_Customer"].Trim();
        public readonly String Type = ConfigurationManager.AppSettings["UUT_Type"].Trim();
        public String Number = ConfigurationManager.AppSettings["UUT_Number"].Trim();
        public readonly String Revision = ConfigurationManager.AppSettings["UUT_Revision"].Trim();
        public readonly String Description = ConfigurationManager.AppSettings["UUT_Description"].Trim();
        public readonly String TestSpecification = ConfigurationManager.AppSettings["UUT_TestSpecification"].Trim();
        public readonly String DocumentationFolder = ConfigurationManager.AppSettings["UUT_DocumentationFolder"].Trim();
        public readonly String ManualsFolder = ConfigurationManager.AppSettings["UUT_ManualsFolder"].Trim();
        public readonly String EMailTestEngineer = ConfigurationManager.AppSettings["UUT_TestEngineerEmail"].Trim();
        public readonly String SerialNumberRegExCustom = ConfigurationManager.AppSettings["UUT_SerialNumberRegExCustom"].Trim();
        public readonly Boolean Simulate = Boolean.Parse(ConfigurationManager.AppSettings["UUT_Simulate"].Trim());
        public String SerialNumber { get; set; } = String.Empty; // Input during testing.
        public String TestEvent { get; set; } = TestEvents.UNSET; // Determined post-test.

        private AppConfigUUT() { }

        public static AppConfigUUT Get() { return new AppConfigUUT(); }
    }
}
