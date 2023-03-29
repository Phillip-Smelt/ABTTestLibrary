using System;
using System.Collections.Generic;
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
    public static class SCPI99 {
        // SCPI-99 Commands/Queries are supposedly standard across all SCPI-99 instruments, which allows shared functionality.
        // Can Reset, Self-Test, Question Condition, issue Commands & Queries to all SCPI-99 conforming instruments with below methods.
        // TODO: Add wrapper methods for remaining SCPI-99 commands.  Definitely want to fully implement these core commands.
        private const Char IDNSepChar = ',';

        public static void Reset(Instrument instrument) {
            AgSCPI99 SCPI99 = new AgSCPI99(instrument.Address);
            SCPI99.SCPI.RST.Command();
        }

        public static void ResetAll(Dictionary<IDs, Instrument> instruments) { foreach (KeyValuePair<IDs, Instrument> i in instruments) Reset(i.Value); }

        public static void Clear(Instrument instrument) {
            AgSCPI99 SCPI99 = new AgSCPI99(instrument.Address);
            SCPI99.SCPI.CLS.Command();
        }

        public static void ClearAll(Dictionary<IDs, Instrument> instruments) { foreach (KeyValuePair<IDs, Instrument> i in instruments) Clear(i.Value); }

        public static void SelfTest(Instrument instrument) {
            Clear(instrument);
            AgSCPI99 SCPI99 = new AgSCPI99(instrument.Address);
            SCPI99.SCPI.TST.Query(out Int32 selfTestResult);
            if (selfTestResult != 0) {
                throw new InvalidOperationException(GetErrorMessage(instrument));
            }
        }

        public static void SelfTestAll(Dictionary<IDs, Instrument> instruments) { foreach (KeyValuePair<IDs, Instrument> i in instruments) SelfTest(i.Value); }

        public static void Initialize(Instrument instrument) {
            Reset(instrument); // Reset instrument to default power-on states.
            Clear(instrument); // Clear all event registers & the Status Byte register.
            SelfTest(instrument);
        }

        public static void InitializeAll(Dictionary<IDs, Instrument> instruments) { foreach (KeyValuePair<IDs, Instrument> i in instruments) Initialize(i.Value); }

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

        public static Boolean PowerSuppliesAreOff(Dictionary<Instrument.IDs, Instrument> instruments) {
            Boolean powerSuppliesAreOff = true;
            String returnString;
            foreach (KeyValuePair<Instrument.IDs, Instrument> kvp in instruments) {
                if (kvp.Value.Category == Instrument.CATEGORIES.PowerSupply) {
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
    }
}