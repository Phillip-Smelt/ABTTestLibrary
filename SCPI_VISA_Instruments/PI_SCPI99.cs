using System;
using System.Collections.Generic;
using Agilent.CommandExpert.ScpiNet.AgSCPI99_1_0;
using TestLibrary.AppConfig;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new SCPI VISA Instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new SVIs are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
namespace TestLibrary.SCPI_VISA_Instruments {
    public static class PI_SCPI99 {
        // TODO: Add wrapper methods for remaining SCPI-99 commands.  Definitely want to fully implement SCPI99's commands.
        //
        // NOTE: SCPI-99 Commands/Queries are supposedly standard across all SCPI-99 compliant instruments, which allows common functionality.
        // NOTE: Using this PI_SCPI99 class is sub-optimal when a compatible .Net VISA instrument driver is available:
        //  - The SCPI99 standard is a *small* subset of any modern SCPI VISA instrument's functionality:
        //	- In order to easily access full modern instrument capabilities, an instrument specific driver is optimal.
        //	- SCPI99 supports Command & Query statements, so any valid SCPI statements can be executed, but not as conveniently as with instrument specific drivers.
        //		- SCPI Command strings must be perfectly phrased, without syntax errors, as C#'s compiler simply passes then into the SCPI instrument's interpreter.
        //		- SCPI Query return strings must be painstakingly parsed & interpreted to extract results.
        //  - Also, the SCPI99 standard isn't always implemented consistently by instrument manufacturers:
        //	    - Assuming the SCPI99 VISA driver utilized by TestLibrary is perfectly SCPI99 compliant & bug-free.
        //	    - Assuming all manufacturer SCPI99 VISA instruments utilized by TestLibrary are perfectly SCPI99 compliant & their interpreters bug-free.
        //  - Then SCPI VISA instruments utilizing this PI_SCPI99 class should work, albeit inconveniently.
        public static Boolean IsPI_SCPI99B(SCPI_VISA_Instrument SVI) { return (SVI.Instrument.GetType() == typeof(AgSCPI99)); }

        public static void Reset(SCPI_VISA_Instrument SVI) { ((AgSCPI99)SVI.Instrument).SCPI.RST.Command(); }

        public static void ResetAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) if (IsPI_SCPI99B(kvp.Value)) Reset(kvp.Value); }

        public static void Clear(SCPI_VISA_Instrument SVI) { ((AgSCPI99)SVI.Instrument).SCPI.CLS.Command(); }

        public static void ClearAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) if (IsPI_SCPI99B(kvp.Value)) Clear(kvp.Value); }

        public static void SelfTest(SCPI_VISA_Instrument SVI) {
            Clear(SVI);
            ((AgSCPI99)SVI.Instrument).SCPI.TST.Query(out Int32 selfTestResult);
            if (selfTestResult != 0) throw new InvalidOperationException(SCPI_VISA.GetErrorMessage(SVI, String.Format(SCPI_VISA.SELF_TEST_ERROR_MESSAGE, SVI.Address)));
        }

        public static void SelfTestAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) if (IsPI_SCPI99B(kvp.Value)) SelfTest(kvp.Value); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            Reset(SVI); // Reset SVI to default power-on states.
            Clear(SVI); // Clear all event registers & the Status Byte register.
            SelfTest(SVI);
        }

        public static void InitializeAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) if (IsPI_SCPI99B(kvp.Value)) Initialize(kvp.Value); }

        public static Int32 QuestionCondition(SCPI_VISA_Instrument SVI) {
            ((AgSCPI99)SVI.Instrument).SCPI.STATus.QUEStionable.CONDition.Query(out Int32 ConditionRegister);
            return ConditionRegister;
        }

        public static String GetIdentity(SCPI_VISA_Instrument SVI) {
            ((AgSCPI99)SVI.Instrument).SCPI.IDN.Query(out String Identity);
            return Identity;
        }

        public static String GetIdentity(SCPI_VISA_Instrument SVI, SCPI_IDENTITY property) { return GetIdentity(SVI).Split(SCPI_VISA.IDENTITY_SEPARATOR)[(Int32)property]; }

        public static void Command(String command, SCPI_VISA_Instrument SVI) { ((AgSCPI99)SVI.Instrument).Transport.Command.Invoke(command); }

        public static String Query(String query, SCPI_VISA_Instrument SVI) {
            ((AgSCPI99)SVI.Instrument).Transport.Query.Invoke(query, out String ReturnString);
            return ReturnString;
        }
    }
}