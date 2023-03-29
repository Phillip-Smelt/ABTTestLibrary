using System;
using System.Collections.Generic;
using System.Reflection;
using Agilent.CommandExpert.ScpiNet.AgSCPI99_1_0;
using static TestLibrary.SCPI_VISA.Instrument;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new instruments are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
namespace TestLibrary.SCPI_VISA {
    public enum SCPI_VISA_CATEGORIES {// Abbreviations:
        CounterTimer,       // CT
        ElectronicLoad,     // EL
        LogicAnalyzer,      // LA
        MultiMeter,         // MM
        OscilloScope,       // OS
        PowerSupply,        // PS
        SCPI,               // Unidentified SCPI Instrument
        WaveformGenerator   // WG
    }

    public enum SCPI_VISA_IDs {
        // NOTE: Not all SCPI_VISA_IDs are necessarily present/installed; actual configuration defined in file App.config.
        //  - SCPI_VISA_IDs has *vastly* more capacity than needed, but doing so costs little.
        CT1, CT2, CT3, CT4, CT5, CT6, CT7, CT8, CT9,    // Counter Timers 1 - 9.
        EL1, EL2, EL3, EL4, EL5, EL6, EL7, EL8, EL9,    // Electronic Loads 1 - 9.
        LA1, LA2, LA3, LA4, LA5, LA6, LA7, LA8, LA9,    // Logic Analyzers 1 - 9.
        MM1, MM2, MM3, MM4, MM5, MM6, MM7, MM8, MM9,    // Multi-Meters 1 - 9.
        OS1, OS2, OS3, OS4, OS5, OS6, OS7, OS8, OS9,    // OscilloScopes 1 - 9.
        PS1, PS2, PS3, PS4, PS5, PS6, PS7, PS8, PS9,    // Power Supplies 1 - 9.
        WG1, WG2, WG3, WG4, WG5, WG6, WG7, WG8, WG9     // Waveform Generators 1 - 9.
    }

    public static class SCPI99 {
        // SCPI-99 Commands/Queries are supposedly standard across all SCPI-99 instruments, which allows shared functionality.
        // Can Reset, Self-Test, Question Condition, issue Commands & Queries to all SCPI-99 conforming instruments with below methods.
        // TODO: Add wrapper methods for remaining SCPI-99 commands.  Definitely want to fully implement these core commands.
        public static String CHANNEL_1 = "(@1)";
        public static String CHANNEL_2 = "(@2)";
        public static String CHANNEL_1_2 = "(@1:2)";
        private const Int32 WIDTH = -16;
        private const Char IDNSepChar = ',';

        public static void Reset(Instrument instrument) {
            AgSCPI99 SCPI99 = new AgSCPI99(instrument.Address);
            SCPI99.SCPI.RST.Command();
        }

        public static void ResetAll(Dictionary<SCPI_VISA_IDs, Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, Instrument> i in instruments) Reset(i.Value); }

        public static void Clear(Instrument instrument) {
            AgSCPI99 SCPI99 = new AgSCPI99(instrument.Address);
            SCPI99.SCPI.CLS.Command();
        }

        public static void ClearAll(Dictionary<SCPI_VISA_IDs, Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, Instrument> i in instruments) Clear(i.Value); }

        public static void SelfTest(Instrument instrument) {
            Clear(instrument);
            AgSCPI99 SCPI99 = new AgSCPI99(instrument.Address);
            SCPI99.SCPI.TST.Query(out Int32 selfTestResult);
            if (selfTestResult != 0) throw new InvalidOperationException(GetErrorMessage(instrument));
        }

        public static void SelfTestAll(Dictionary<SCPI_VISA_IDs, Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, Instrument> i in instruments) SelfTest(i.Value); }

        public static void Initialize(Instrument instrument) {
            Reset(instrument); // Reset instrument to default power-on states.
            Clear(instrument); // Clear all event registers & the Status Byte register.
            SelfTest(instrument);
        }

        public static void InitializeAll(Dictionary<SCPI_VISA_IDs, Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, Instrument> i in instruments) Initialize(i.Value); }

        public static Int32 QuestionCondition(String address) {
            AgSCPI99 SCPI99 = new AgSCPI99(address);
            SCPI99.SCPI.STATus.QUEStionable.CONDition.Query(out Int32 ConditionRegister);
            return ConditionRegister;
        }

        public static String GetManufacturer(String address) {
            AgSCPI99 SCPI99 = new AgSCPI99(address);
            SCPI99.SCPI.IDN.Query(out String Identity);
            String[] s = Identity.Split(IDNSepChar);
            return s[0] ?? "Unknown";
        }

        public static String GetModel(String address) {
            AgSCPI99 SCPI99 = new AgSCPI99(address);
            SCPI99.SCPI.IDN.Query(out String Identity);
            String[] s = Identity.Split(IDNSepChar);
            return s[1] ?? "Unknown";
        }

        public static void Command(String command, String address) {
            AgSCPI99 SCPI99 = new AgSCPI99(address);
            SCPI99.Transport.Command.Invoke(command);
        }

        public static String Query(String query, String address) {
            AgSCPI99 SCPI99 = new AgSCPI99(address);
            SCPI99.Transport.Query.Invoke(query, out String ReturnString);
            return ReturnString;
        }

        public static Boolean PowerSuppliesAreOff(Dictionary<SCPI_VISA_IDs, Instrument> instruments) {
            Boolean powerSuppliesAreOff = true;
            String returnString;
            foreach (KeyValuePair<SCPI_VISA_IDs, Instrument> kvp in instruments) {
                if (kvp.Value.Category == SCPI_VISA_CATEGORIES.PowerSupply) {
                    if (PS_E36234A.IsPS_E36234A(kvp.Value)) {
                        returnString = Query(":OUTPut:STATe? (@1:2)", kvp.Value.Address);
                        powerSuppliesAreOff = powerSuppliesAreOff && (String.Equals(returnString, "0,0")); // "0,0" = both channels 1 & 2 are off.
                    } else {
                        returnString = Query(":OUTPut:STATe?", kvp.Value.Address);
                        powerSuppliesAreOff = powerSuppliesAreOff && (String.Equals(returnString, "0")); // "0" = off.
                    }
                }
            }
            return powerSuppliesAreOff;
        }

        public static String GetMessage(Instrument instrument, String optionalHeader = "") {
            String SCPI_VISA_Message = (optionalHeader == "") ? "" : optionalHeader += Environment.NewLine;
            foreach (PropertyInfo pi in instrument.GetType().GetProperties()) SCPI_VISA_Message += $"{pi.Name,WIDTH}: '{pi.GetValue(instrument)}'{Environment.NewLine}";
            return SCPI_VISA_Message;
        }

        internal static String GetErrorMessage(Instrument instrument) { return GetMessage(instrument, $"SCPI-VISA Instrument failed self-test:"); }

        internal static String GetErrorMessage(Instrument instrument, String errorMessage) { return $"{GetErrorMessage(instrument)}{"Error Message",WIDTH}: '{errorMessage}'.{Environment.NewLine}"; }

        internal static String GetErrorMessage(Instrument instrument, String errorMessage, Int32 errorNumber) { return $"{GetErrorMessage(instrument, errorMessage)}{"Error Number",WIDTH}: '{errorNumber}'.{Environment.NewLine}"; }

    }
}