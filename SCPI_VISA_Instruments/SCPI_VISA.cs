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
    public enum BINARY { On, Off }

    public static class SCPI_VISA {
        // NOTE: Unlike all other classes in namespace TestLibrary.SCPI_VISA_Instruments, SCPI_VISA utilizes only VISA addresses, not Instrument objects contained in their SCPI_VISA_Instrument objects.
        //  - Thus SCPI_VISA has to inefficiently create temporary AgSCPI99 objects for each method, disposed immediately after use.
        public static String CHANNEL_1 = "(@1)";
        public static String CHANNEL_2 = "(@2)";
        public static String CHANNEL_1_2 = "(@1:2)";
        public static String SELF_TEST_ERROR_MESSAGE = $"SCPI VISA Instrument Address '{0}' failed SelfTest.";
        public const Char IDENTITY_SEPARATOR = ',';

        public static void Reset(SCPI_VISA_Instrument SVI) { Reset(SVI.Address); }
        public static void Reset(String address) { new AgSCPI99(address).SCPI.RST.Command(); }

        public static void ResetAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) Reset(kvp.Value); }
        public static void ResetAll(List<String> addresses) { foreach (String address in addresses) Reset(address); }

        public static void Clear(SCPI_VISA_Instrument SVI) { Clear(SVI.Address); }
        public static void Clear(String address) { new AgSCPI99(address).SCPI.CLS.Command(); }
        
        public static void ClearAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) Clear(kvp.Value); }
        public static void ClearAll(List<String> addresses) { foreach (String address in addresses) Clear(address); }

        public static void SelfTest(SCPI_VISA_Instrument SVI) { try { SelfTest(SVI.Address); } catch (InvalidOperationException) { throw new InvalidOperationException(GetErrorMessage(SVI, String.Format(SELF_TEST_ERROR_MESSAGE, SVI.Address))); } }
        public static void SelfTest(String address) {
            Clear(address);
            new AgSCPI99(address).SCPI.TST.Query(out Int32 selfTestResult);
            if (selfTestResult != 0) throw new InvalidOperationException(String.Format(SELF_TEST_ERROR_MESSAGE, address));
        }

        public static void SelfTestAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) SelfTest(kvp.Value); }
        public static void SelfTestAll(List<String> addresses) { foreach (String address in addresses) SelfTest(address); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            Reset(SVI); // Reset SVI to default power-on states.
            SelfTest(SVI);
        }
        public static void Initialize(String address) {
            Reset(address);
            SelfTest(address);
        }

        public static void InitializeAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) Initialize(kvp.Value); }
        public static void InitializeAll(List<String> addresses) { foreach (String address in addresses) Initialize(address); }

        public static Int32 QuestionCondition(SCPI_VISA_Instrument SVI) { return QuestionCondition(SVI.Address); }
        public static Int32 QuestionCondition(String address) {
            new AgSCPI99(address).SCPI.STATus.QUEStionable.CONDition.Query(out Int32 ConditionRegister);
            return ConditionRegister;
        }

        public static String GetIdentity(SCPI_VISA_Instrument SVI) { return GetIdentity(SVI.Address); }
        public static String GetIdentity(String address) {
            new AgSCPI99(address).SCPI.IDN.Query(out String Identity);
            return Identity;
        }

        public static String GetIdentity(SCPI_VISA_Instrument SVI, SCPI_IDENTITY property) { return GetIdentity(SVI.Address, property); }
        public static String GetIdentity(String address, SCPI_IDENTITY property) { return GetIdentity(address).Split(SCPI_VISA.IDENTITY_SEPARATOR)[(Int32)property]; }

        public static void Command(String command, SCPI_VISA_Instrument SVI) { Command(command, SVI.Address); }
        public static void Command(String command, String address) { new AgSCPI99(address).Transport.Command.Invoke(command); }

        public static String Query(String query, SCPI_VISA_Instrument SVI) { return Query(query, SVI.Address); }
        public static String Query(String query, String address) {
            new AgSCPI99(address).Transport.Query.Invoke(query, out String ReturnString);
            return ReturnString;
        }

        internal static String GetErrorMessage(SCPI_VISA_Instrument SVI) { return SCPI_VISA_Instrument.GetInfo(SVI, "SCPI-VISA SCPI_VISA_Instrument failed:"); }

        internal static String GetErrorMessage(SCPI_VISA_Instrument SVI, String errorMessage) { return $"{GetErrorMessage(SVI)}{"Error Message",SCPI_VISA_Instrument.FORMAT_WIDTH}: '{errorMessage}'.{Environment.NewLine}"; }

        internal static String GetErrorMessage(SCPI_VISA_Instrument SVI, String errorMessage, Int32 errorNumber) { return $"{GetErrorMessage(SVI, errorMessage)}{"Error Number",SCPI_VISA_Instrument.FORMAT_WIDTH}: '{errorNumber}'.{Environment.NewLine}"; }

        internal static Boolean IsCloseEnough(Double D1, Double D2, Double Delta) { return Math.Abs(D1 - D2) <= Delta; }
        // Close is good enough for horseshoes, hand grenades, nuclear weapons, and Doubles!
    }
}