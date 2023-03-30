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
        // SCPI-99 Commands/Queries are supposedly standard across all SCPI-99 compliant instruments, which allows shared functionality.
        // TODO: Add wrapper methods for remaining SCPI-99 commands.  Definitely want to fully implement SCPI99's commands.
        public static Boolean IsPI_SCPI99B(SCPI_VISA_Instrument SVI) { return (SVI.Instance.GetType() == typeof(AgSCPI99)); }

        public static void Reset(SCPI_VISA_Instrument SVI) { ((AgSCPI99)SVI.Instance).SCPI.RST.Command(); }

        public static void ResetAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> kvp in SVIs) if (IsPI_SCPI99B(kvp.Value)) Reset(kvp.Value); }

        public static void Clear(SCPI_VISA_Instrument SVI) { ((AgSCPI99)SVI.Instance).SCPI.CLS.Command(); }

        public static void ClearAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> kvp in SVIs) if (IsPI_SCPI99B(kvp.Value)) Clear(kvp.Value); }

        public static void SelfTest(SCPI_VISA_Instrument SVI) {
            Clear(SVI);
            ((AgSCPI99)SVI.Instance).SCPI.TST.Query(out Int32 selfTestResult);
            if (selfTestResult != 0) throw new InvalidOperationException(SCPI_VISA.GetErrorMessage(SVI));
        }

        public static void SelfTestAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> kvp in SVIs) if (IsPI_SCPI99B(kvp.Value)) SelfTest(kvp.Value); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            Reset(SVI); // Reset SVI to default power-on states.
            Clear(SVI); // Clear all event registers & the Status Byte register.
            SelfTest(SVI);
        }

        public static void InitializeAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> kvp in SVIs) if (IsPI_SCPI99B(kvp.Value)) Initialize(kvp.Value); }

        public static Int32 QuestionCondition(SCPI_VISA_Instrument SVI) {
            ((AgSCPI99)SVI.Instance).SCPI.STATus.QUEStionable.CONDition.Query(out Int32 ConditionRegister);
            return ConditionRegister;
        }

        public static String GetIdentity(SCPI_VISA_Instrument SVI) {
            ((AgSCPI99)SVI.Instance).SCPI.IDN.Query(out String Identity);
            return Identity;
        }

        public static String GetManufacturer(SCPI_VISA_Instrument SVI) {
            String[] s = GetIdentity(SVI).Split(SCPI_VISA.IDENTITY_SEPERATOR);
            return s[0] ?? SCPI_VISA.UNKNOWN;
        }

        public static String GetModel(SCPI_VISA_Instrument SVI) {
            String[] s = GetIdentity(SVI).Split(SCPI_VISA.IDENTITY_SEPERATOR);
            return s[1] ?? SCPI_VISA.UNKNOWN;
        }

        public static void Command(String command, SCPI_VISA_Instrument SVI) { ((AgSCPI99)SVI.Instance).Transport.Command.Invoke(command); }

        public static String Query(String query, SCPI_VISA_Instrument SVI) {
            ((AgSCPI99)SVI.Instance).Transport.Query.Invoke(query, out String ReturnString);
            return ReturnString;
        }
    }
}