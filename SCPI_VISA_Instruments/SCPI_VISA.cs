using System;
using System.Collections.Generic;
using Agilent.CommandExpert.ScpiNet.AgSCPI99_1_0;
using TestLibrary.AppConfig;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new SCPI VISA Instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new SVIs are added.
//  - The Agilent.CommandExpert.ScpiNet drivers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
// NOTE: Unlike all other classes in namespace TestLibrary.SCPI_VISA_Instruments, classes in SCPI_VISA utilize only VISA Addresses,
// not Instrument objects contained in their SCPI_VISA_Instrument objects.
namespace TestLibrary.SCPI_VISA_Instruments {
    public enum STATE { ON, off }

    public static class SCPI99 {
        // NOTE: SCPI-99 Commands/Queries are supposedly standard across all SCPI-99 compliant instruments, which allows common functionality.
        // NOTE: Using this SCPI99 class is sub-optimal when a compatible .Net VISA instrument driver is available:
        //  - The SCPI99 standard is a *small* subset of any modern SCPI VISA instrument's functionality:
        //	- In order to easily access full modern instrument capabilities, an instrument specific driver is optimal.
        //	- SCPI99 supports Command & Query statements, so any valid SCPI statements can be executed, but not as conveniently as with instrument specific drivers.
        //		- SCPI Command strings must be perfectly phrased, without syntax errors, as C#'s compiler simply passes them into the SCPI instrument's interpreter.
        //		- SCPI Query return strings must be painstakingly parsed & interpreted to extract results.
        //  - Also, the SCPI99 standard isn't always implemented consistently by instrument manufacturers:
        //	    - Assuming the SCPI99 VISA driver utilized by TestLibrary is perfectly SCPI99 compliant & bug-free.
        //	    - Assuming all manufacturer SCPI99 VISA instruments utilized by TestLibrary are perfectly SCPI99 compliant & their interpreters bug-free.
        //  - Then SCPI VISA instruments utilizing this SCPI99 class should work, albeit inconveniently.
        private static readonly Char IDENTITY_SEPARATOR = ',';

        public static void Reset(SCPI_VISA_Instrument SVI) { new AgSCPI99(SVI.Address).SCPI.RST.Command(); }

        public static void ResetAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) Reset(kvp.Value); }

        public static void Clear(SCPI_VISA_Instrument SVI) { new AgSCPI99(SVI.Address).SCPI.CLS.Command(); }

        public static void ClearAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) Clear(kvp.Value); }

        public static void SelfTest(SCPI_VISA_Instrument SVI) {
            Reset(SVI);
            Clear(SVI);
            new AgSCPI99(SVI.Address).SCPI.TST.Query(out Int32 selfTestResult);
            if (selfTestResult != 0) throw new InvalidOperationException(SCPI.GetErrorMessage(SVI, String.Format(SCPI.SELF_TEST_ERROR_MESSAGE, SVI.Address)));
        }

        public static void SelfTestAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) SelfTest(kvp.Value); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            Reset(SVI); // Reset SVI to default power-on states.
            Clear(SVI); // Clear all event registers & the Status Byte register.
            SelfTest(SVI);
        }

        public static void InitializeAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) Initialize(kvp.Value); }

        public static Int32 QuestionCondition(SCPI_VISA_Instrument SVI) {
            new AgSCPI99(SVI.Address).SCPI.STATus.QUEStionable.CONDition.Query(out Int32 ConditionRegister);
            return ConditionRegister;
        }

        public static String GetIdentity(SCPI_VISA_Instrument SVI) { return GetIdentity(SVI.Address); }
        public static String GetIdentity(String Address) {
            new AgSCPI99(Address).SCPI.IDN.Query(out String Identity);
            return Identity;
        }

        public static String GetIdentity(SCPI_VISA_Instrument SVI, SCPI_IDENTITY Property) { return GetIdentity(SVI).Split(IDENTITY_SEPARATOR)[(Int32)Property]; }
        public static String GetIdentity(String Address, SCPI_IDENTITY Property) { return GetIdentity(Address).Split(IDENTITY_SEPARATOR)[(Int32)Property]; }

        public static void Command(SCPI_VISA_Instrument SVI, String SCPI_Command) { new AgSCPI99(SVI.Address).Transport.Command.Invoke(SCPI_Command); }

        public static String Query(SCPI_VISA_Instrument SVI, String SCPI_Query) {
            new AgSCPI99(SVI.Address).Transport.Query.Invoke(SCPI_Query, out String response);
            return response;
        }
    }

    public static class SCPI {
        // This class contains properties & methods relevant to some SCPI VISA instruments, but which may not be SCPI99 specific or compliant.
        //  - Example; Keysight's 33509B Multi-Meter & 34661A Waveform Generator don't support output states, but their E3610xB & E36234A Power Supplies & EL34143A Electronic Load do.
        // TODO: Create methods for simple Commands/Queries applicable to multiples SCPI VISA instruments, invoked as SCPI99 Commands/Queries.
        //  - Thus passed directly to SCPI instrument interpreters with Keysight's AgSCPI99 SCPI-99 driver.
        //  - Eliminates instrument specific methods for each SCPI VISA instrument.
        //  - That is, don't need Get/Set/Is_OutputState() methods for the E3610xB & E36234A Power Supplies & EL34143A Electronic Load.
        // NOTE: Could also have declared class SCPI_VISA_Instrument's Instrument property as type dynamic, instead of Object.
        //  - This would permit invoking any applicable SCPI command directly on a SCPI_VISA_Instrument.Instrument property.
        //      - Given: public dynamic Instrument { get; private set; }
        //      - Then:  Instrument.SCPI.OUTPut.STATe.Query(out Boolean state); should work for E3610xB & E36234A Power Supplies & EL34143A Electronic Load.
        // - May ultimately implement this, obviating below methods and need to cast public Object Instrument to specific instrument, ala ((AgE3610XB)SVI.Instrument).

        public static readonly String CHANNEL_1 = "(@1)";
        public static readonly String CHANNEL_2 = "(@2)";
        public static readonly String CHANNEL_1ε2 = "(@1:2)";
        public static readonly String SELF_TEST_ERROR_MESSAGE = $"SCPI VISA Instrument Address '{0}' failed SelfTest.";

        public static STATE GetOutputState(SCPI_VISA_Instrument SVI) {
            if (String.Equals(SCPI99.Query(SVI, ":OUTPUT?"), "0")) return STATE.off;
            else return STATE.ON;
        }

        public static void SetOutputState(SCPI_VISA_Instrument SVI, STATE OutputState) { SCPI99.Command(SVI, (OutputState is STATE.off) ? ":OUTPUT 0" : ":OUTPUT 1"); }

        public static Boolean IsOutputState(SCPI_VISA_Instrument SVI, STATE State) {
            if (GetOutputState(SVI) == State) return true;
            else return false;
        }

        internal static String GetErrorMessage(SCPI_VISA_Instrument SVI) { return SCPI_VISA_Instrument.GetInfo(SVI, "SCPI-VISA SCPI_VISA_Instrument failed:"); }

        internal static String GetErrorMessage(SCPI_VISA_Instrument SVI, String errorMessage) { return $"{GetErrorMessage(SVI)}{"Error Message",SCPI_VISA_Instrument.FORMAT_WIDTH}: '{errorMessage}'.{Environment.NewLine}"; }

        internal static Boolean IsCloseEnough(Double D1, Double D2, Double Delta) { return Math.Abs(D1 - D2) <= Delta; }
        // Close is good enough for horseshoes, hand grenades, nuclear weapons, and Doubles!  Shamelessly plagiarized from the Internet!
    }
}