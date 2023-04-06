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
// NOTE: Below hopefully "value-added" wrapper methods for some commonly used AgSCPI99 commands are conveniences, not necessities.
// NOTE: Will never fully implement wrapper methods for the complete set of AgSCPI99 commands, just some of the most commonly used ones.
// - In general, TestLibrary's InterfaceAdapters, Logging, SCPI_VISA_Instruments & Switching namespaces exist partly to eliminate
//   the need to reference TestLibrary's various DLLs directly from TestProgram client apps.
// - As long as suitable wrapper methods exists in PI_SCPI99, needn't directly reference AgSCPI99_1_0
//   from TestProgram client apps, as referencing TestLibrary suffices.
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

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            SCPI_VISA.Initialize(SVI);
        }

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