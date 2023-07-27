using System;
using System.Collections.Generic;
using Agilent.CommandExpert.ScpiNet.AgSCPI99_1_0;
using ABT.TestSpace.TestExec.SCPI_VISA_Instruments;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new SCPI VISA Instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new SVIs are added.
//  - The Agilent.CommandExpert.ScpiNet drivers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
// NOTE: Unlike all other classes in namespace ABT.TestSpace.TestExec.SCPI_VISA_Instruments, classes in SCPI_VISA utilize only VISA Addresses,
// not Instrument objects contained in their SCPI_VISA_Instrument objects.
namespace ABT.TestSpace.TestExec.SCPI_VISA_Instruments {
    public static class SCPI99 {
        // NOTE: SCPI-99 Commands/Queries are supposedly standard across all SCPI-99 compliant instruments, which allows common functionality.
        // NOTE: Using this SCPI99 class is sub-optimal when a compatible .Net VISA instrument driver is available:
        //  - The SCPI99 standard is a *small* subset of any modern SCPI VISA instrument's functionality:
        //	- In order to easily access full modern instrument capabilities, an instrument specific driver is optimal.
        //	- SCPI99 supports Command & Query statements, so any valid SCPI statements can be executed, but not as conveniently as with instrument specific drivers.
        //		- SCPI Command strings must be perfectly phrased, without syntax errors, as C#'s compiler simply passes them into the SCPI instrument's interpreter.
        //		- SCPI Query return strings must be painstakingly parsed & interpreted to extract results.
        //  - Also, the SCPI99 standard isn't always implemented consistently by instrument manufacturers:
        //	    - Assuming the SCPI99 VISA driver utilized by TestExecutive is perfectly SCPI99 compliant & bug-free.
        //	    - Assuming all manufacturer SCPI99 VISA instruments utilized by TestExecutive are perfectly SCPI99 compliant & their interpreters bug-free.
        //  - Then SCPI VISA instruments utilizing this SCPI99 class should work, albeit inconveniently.
        private const Char IDENTITY_SEPARATOR = ',';

        public static void Reset(SCPI_VISA_Instrument SVI) { new AgSCPI99(SVI.Address).SCPI.RST.Command(); }

        public static void ResetAll(Dictionary<SCPI_VISA_Instrument.Alias, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_Instrument.Alias, SCPI_VISA_Instrument> kvp in SVIs) Reset(kvp.Value); }

        public static void Clear(SCPI_VISA_Instrument SVI) { new AgSCPI99(SVI.Address).SCPI.CLS.Command(); }

        public static void ClearAll(Dictionary<SCPI_VISA_Instrument.Alias, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_Instrument.Alias, SCPI_VISA_Instrument> kvp in SVIs) Clear(kvp.Value); }

        public static void SelfTest(SCPI_VISA_Instrument SVI) {
            try {
                Initialize(SVI);
                new AgSCPI99(SVI.Address).SCPI.TST.Query(out Int32 selfTestResult);
                if (selfTestResult != 0) throw new InvalidOperationException($"Self Test returned result '{selfTestResult}'.");
            } catch (Exception e) {
                throw new InvalidOperationException(SCPI.GetErrorMessage(SVI, e.ToString()));
                // If unpowered, throws a Keysight.CommandExpert.InstrumentAbstraction.CommunicationException exception,
                // which requires a seemingly unavailable Keysight library to explicitly Assert for in MS Unit Test.
                // Instead catch Exception and re-throw as InvalidOperationException, which MS Test can explicitly Assert for.
            }
        }

        public static void SelfTestAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) SelfTest(kvp.Value); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
#if !DEBUG
            SelfTest(SVI); // SelfTest() is painfully slow to run, re-run, re-run, re-run... while debugging, but only executes once during test system initialization after release.
#endif
            Reset(SVI); // Reset SVI to default power-on states.
            Clear(SVI); // Clear all event registers & the Status Byte register.
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
}