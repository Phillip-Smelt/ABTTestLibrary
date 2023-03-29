using System;
using System.Collections.Generic;
using Agilent.CommandExpert.ScpiNet.Ag33500B_33600A_2_09;
using static TestLibrary.SCPI_VISA.Instrument;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new instruments are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
namespace TestLibrary.SCPI_VISA {
    public static class WG_33509B {
        public static Boolean IsWG_33509(Instrument instrument) { return (instrument.Instance.GetType() == typeof(Ag33500B_33600A)); }

        public static void Clear(Instrument instrument) { ((Ag33500B_33600A)instrument.Instance).SCPI.CLS.Command(); }

        public static void ClearAll(Dictionary<IDs, Instrument> instruments) { foreach (KeyValuePair<IDs, Instrument> i in instruments) if (IsWG_33509(i.Value)) Clear(i.Value); }

        public static void Reset(Instrument instrument) { ((Ag33500B_33600A)instrument.Instance).SCPI.RST.Command(); }

        public static void ResetAll(Dictionary<IDs, Instrument> instruments) { foreach (KeyValuePair<IDs, Instrument> i in instruments) if (IsWG_33509(i.Value)) Reset(i.Value); }

        public static void SelfTest(Instrument instrument) {
            ((Ag33500B_33600A)instrument.Instance).SCPI.TST.Query(out Int32 selfTestResult);
            if (selfTestResult != 0) {
                ((Ag33500B_33600A)instrument.Instance).SCPI.SYSTem.ERRor.Query(out Int32 errorNumber, out String errorMessage);
                throw new InvalidOperationException(GetSCPI_VISA_ErrorMessage(instrument, errorMessage, errorNumber));
            }
        }

        public static void SelfTestAll(Dictionary<IDs, Instrument> instruments) { foreach (KeyValuePair<IDs, Instrument> i in instruments) if (IsWG_33509(i.Value)) SelfTest(i.Value); }

        public static void Initialize(Instrument instrument) {
            Reset(instrument); // Reset instrument to default power-on states.
            Clear(instrument); // Clear all event registers & the Status Byte register.
            SelfTest(instrument);
            ((Ag33500B_33600A)instrument.Instance).SCPI.DISPlay.TEXT.CLEar.Command();
        }

        public static void InitializeAll(Dictionary<IDs, Instrument> instruments) { foreach (KeyValuePair<IDs, Instrument> i in instruments) if (IsWG_33509(i.Value)) Initialize(i.Value); }
    }
}