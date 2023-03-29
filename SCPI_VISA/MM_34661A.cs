using System;
using System.Collections.Generic;
using Agilent.CommandExpert.ScpiNet.Ag3466x_2_08;
using static TestLibrary.SCPI_VISA.Instrument;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new instruments are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
namespace TestLibrary.SCPI_VISA {
    public static class MM_34661A {
        // TODO: Could generics eliminate all these similar local methods, replacing with a universal set, into which are passed in the specific SCPI_VISA class?

        public static Boolean IsMM_34661A(Instrument instrument) { return (instrument.Instance.GetType() == typeof(Ag3466x)); }

        public static void Clear(Instrument instrument) { ((Ag3466x)instrument.Instance).SCPI.CLS.Command(); }

        public static void ClearAll(Dictionary<SCPI_VISA_IDs, Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, Instrument> i in instruments) if (IsMM_34661A(i.Value)) Clear(i.Value); }

        public static void Local(Instrument instrument) { ((Ag3466x)instrument.Instance).SCPI.SYSTem.LOCal.Command(); }

        public static void LocalAll(Dictionary<SCPI_VISA_IDs, Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, Instrument> i in instruments) if (IsMM_34661A(i.Value)) Local(i.Value); }

        public static void Reset(Instrument instrument) { ((Ag3466x)instrument.Instance).SCPI.RST.Command(); }

        public static void ResetAll(Dictionary<SCPI_VISA_IDs, Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, Instrument> i in instruments) if (IsMM_34661A(i.Value)) Reset(i.Value); }

        public static void SelfTest(Instrument instrument) {
            ((Ag3466x)instrument.Instance).SCPI.TST.Query(out Int32 selfTestResult);
            if (selfTestResult != 0) {
                ((Ag3466x)instrument.Instance).SCPI.SYSTem.ERRor.NEXT.Query(out Int32 errorNumber, out String errorMessage);
                throw new InvalidOperationException(SCPI99.GetErrorMessage(instrument, errorMessage, errorNumber));
            }
        }

        public static void SelfTestAll(Dictionary<SCPI_VISA_IDs, Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, Instrument> i in instruments) if (IsMM_34661A(i.Value)) SelfTest(i.Value); }

        public static void Initialize(Instrument instrument) {
            Reset(instrument); // Reset instrument to default power-on states.
            Clear(instrument); // Clear all event registers & the Status Byte register.
            SelfTest(instrument);
        }

        public static void InitializeAll(Dictionary<SCPI_VISA_IDs, Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, Instrument> i in instruments) if (IsMM_34661A(i.Value)) Initialize(i.Value); }

        public static Double MeasureVDC(Instrument instrument) {
            ((Ag3466x)instrument.Instance).SCPI.MEASure.VOLTage.DC.QueryAsciiRealClone("AUTO", "MAXimum", out Double voltsDC);
            return voltsDC;
        }

        public static Double MeasureADC(Instrument instrument) {
            ((Ag3466x)instrument.Instance).SCPI.MEASure.CURRent.DC.QueryAsciiReal("AUTO", "MAXimum", out Double ampsDC);
            return ampsDC;
        }
    }
}