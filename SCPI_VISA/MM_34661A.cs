using System;
using System.Collections.Generic;
using Agilent.CommandExpert.ScpiNet.Ag3466x_2_08;
using TestLibrary.AppConfig;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new SVIs in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new SVIs are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
namespace TestLibrary.SCPI_VISA {
    public static class MM_34661A {
        // TODO: Could generics eliminate all these similar local methods, replacing with a universal set, into which are passed in the specific SCPI_VISA class?

        public static Boolean IsMM_34661A(SCPI_VISA_Instrument SVI) { return (SVI.Instance.GetType() == typeof(Ag3466x)); }

        public static void Clear(SCPI_VISA_Instrument SVI) { ((Ag3466x)SVI.Instance).SCPI.CLS.Command(); }

        public static void ClearAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsMM_34661A(SVI.Value)) Clear(SVI.Value); }

        public static void Local(SCPI_VISA_Instrument SVI) { ((Ag3466x)SVI.Instance).SCPI.SYSTem.LOCal.Command(); }

        public static void LocalAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsMM_34661A(SVI.Value)) Local(SVI.Value); }

        public static void Reset(SCPI_VISA_Instrument SVI) { ((Ag3466x)SVI.Instance).SCPI.RST.Command(); }

        public static void ResetAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsMM_34661A(SVI.Value)) Reset(SVI.Value); }

        public static void SelfTest(SCPI_VISA_Instrument SVI) {
            ((Ag3466x)SVI.Instance).SCPI.TST.Query(out Int32 selfTestResult);
            if (selfTestResult != 0) {
                ((Ag3466x)SVI.Instance).SCPI.SYSTem.ERRor.NEXT.Query(out Int32 errorNumber, out String errorMessage);
                throw new InvalidOperationException(PI_SCPI99.GetErrorMessage(SVI, errorMessage, errorNumber));
            }
        }

        public static void SelfTestAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsMM_34661A(SVI.Value)) SelfTest(SVI.Value); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            Reset(SVI); // Reset SVI to default power-on states.
            Clear(SVI); // Clear all event registers & the Status Byte register.
            SelfTest(SVI);
        }

        public static void InitializeAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsMM_34661A(SVI.Value)) Initialize(SVI.Value); }

        public static Double MeasureVDC(SCPI_VISA_Instrument SVI) {
            ((Ag3466x)SVI.Instance).SCPI.MEASure.VOLTage.DC.QueryAsciiRealClone("AUTO", "MAXimum", out Double voltsDC);
            return voltsDC;
        }

        public static Double MeasureADC(SCPI_VISA_Instrument SVI) {
            ((Ag3466x)SVI.Instance).SCPI.MEASure.CURRent.DC.QueryAsciiReal("AUTO", "MAXimum", out Double ampsDC);
            return ampsDC;
        }
    }
}