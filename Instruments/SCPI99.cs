using System;
using Agilent.CommandExpert.ScpiNet.AgSCPI99_1_0;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new instruments are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI & IVI driver commands, which are directly exportable as .Net statements.
//
namespace TestLibrary.Instruments {
    public static class SCPI99 {
        // SCPI-99 Commands/Queries are supposedly standard across all SCPI-99 instruments, which allows shared functionality.
        // Can Reset, Self-Test, Question Condition, issue Commands & Queries to all SCPI-99 conforming instruments with below methods.
        // TODO: Add wrapper methods for remaining SCPI-99 commands, particularly Command Batching.
        private const Char IDNSepChar = ',';

        public static void Reset(String address) {
            AgSCPI99 SCPI99 = new AgSCPI99(address);
            SCPI99.SCPI.RST.Command();
        }

        public static void Clear(String address) {
            AgSCPI99 SCPI99 = new AgSCPI99(address);
            SCPI99.SCPI.CLS.Command();
        }

        public static Int32 SelfTest(String address) {
            AgSCPI99 SCPI99 = new AgSCPI99(address);
            SCPI99.SCPI.TST.Query(out Int32 selfTestResult);
            if (selfTestResult != 0) throw new InvalidOperationException($"VISA address '{address}' failed it's Self-Test with result '{selfTestResult}'.");
            // SCPI99 command *TST issues a Factory Reset (*RST) command after *TST completes.
            return selfTestResult;
        }

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
    }
}