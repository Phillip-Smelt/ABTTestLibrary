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
        // SCPI-99 Commands/Queries are supposedly standard across all SCPI-99 Instruments, which allows shared functionality.
        // Can Reset, Self-Test & Question Condition all SCPI-99 conforming Instruments with below methods; handy.
        private const Char IDNSepChar = ',';
        public static void Reset(String Address) {
            AgSCPI99 SCPI99 = new AgSCPI99(Address);
            SCPI99.SCPI.RST.Command();
            SCPI99.SCPI.CLS.Command();
        }

        public static Int32 SelfTest(String Address) {
            AgSCPI99 SCPI99 = new AgSCPI99(Address);
            SCPI99.SCPI.TST.Query(out Int32 SelfTestResult);
            if (SelfTestResult != 0) throw new InvalidOperationException($"VISA Address '{Address}' failed it's Self-Test with result '{SelfTestResult}'.");
            // SCPI99 command *TST issues a Factory Reset (*RST) command after *TST completes.
            return SelfTestResult;
        }

        public static Int32 QuestionCondition(String Address) {
            AgSCPI99 SCPI99 = new AgSCPI99(Address);
            SCPI99.SCPI.STATus.QUEStionable.CONDition.Query(out Int32 ConditionRegister);
            return ConditionRegister;
        }

        public static String GetManufacturer(String Address) {
            AgSCPI99 SCPI99 = new AgSCPI99(Address);
            SCPI99.SCPI.IDN.Query(out String Identity);
            String[] s = Identity.Split(IDNSepChar);
            return s[0] ?? "Unknown";
        }

        public static String GetModel(String Address) {
            AgSCPI99 SCPI99 = new AgSCPI99(Address);
            SCPI99.SCPI.IDN.Query(out String Identity);
            String[] s = Identity.Split(IDNSepChar);
            return s[1] ?? "Unknown";
        }
    }
}