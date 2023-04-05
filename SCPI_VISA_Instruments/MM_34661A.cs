using System;
using System.Collections.Generic;
using Agilent.CommandExpert.ScpiNet.Ag3466x_2_08;
using TestLibrary.AppConfig;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new SCPI VISA Instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new SVIs are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
// NOTE: Below hopefully "value-added" wrapper methods for some commonly used Ag3466x commands are conveniences, not necessities.
// NOTE: Will never fully implement wrapper methods for the complete set of Ag3466x commands, just some of the most commonly used ones.
// - In general, TestLibrary's InterfaceAdapters, Logging, SCPI_VISA_Instruments & Switching namespaces exist partly to eliminate
//   the need to reference TestLibrary's various DLLs directly from TestProgram client apps.
// - As long as suitable wrapper methods exists in MM_34661A, needn't directly reference Ag3466x_2_08
//   from TestProgram client apps, as referencing TestLibrary suffices.
namespace TestLibrary.SCPI_VISA_Instruments {
    public static class MM_34661A {
        // TODO: Could generics eliminate all these similar local methods, replacing with a universal set, into which are passed in the specific SCPI_VISA class?
        public const String MODEL = "34461A";

        public static Boolean IsMM_34661A(SCPI_VISA_Instrument SVI) { return (SVI.Instrument.GetType() == typeof(Ag3466x)); }

        public static void Clear(SCPI_VISA_Instrument SVI) { ((Ag3466x)SVI.Instrument).SCPI.CLS.Command(); }

        public static void ClearAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) if (IsMM_34661A(kvp.Value)) Clear(kvp.Value); }

        public static void Local(SCPI_VISA_Instrument SVI) { ((Ag3466x)SVI.Instrument).SCPI.SYSTem.LOCal.Command(); }

        public static void LocalAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) if (IsMM_34661A(kvp.Value)) Local(kvp.Value); }

        public static void Reset(SCPI_VISA_Instrument SVI) { ((Ag3466x)SVI.Instrument).SCPI.RST.Command(); }

        public static void ResetAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) if (IsMM_34661A(kvp.Value)) Reset(kvp.Value); }

        public static void SelfTest(SCPI_VISA_Instrument SVI) {
            ((Ag3466x)SVI.Instrument).SCPI.TST.Query(out Int32 selfTestResult);
            if (selfTestResult != 0) {
                ((Ag3466x)SVI.Instrument).SCPI.SYSTem.ERRor.NEXT.Query(out Int32 errorNumber, out String errorMessage);
                throw new InvalidOperationException(SCPI_VISA.GetErrorMessage(SVI, errorMessage, errorNumber));
            }
        }

        public static void SelfTestAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) if (IsMM_34661A(kvp.Value)) SelfTest(kvp.Value); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            Reset(SVI); // Reset SVI to default power-on states.
            Clear(SVI); // Clear all event registers & the Status Byte register.
            SelfTest(SVI);
        }

        public static void InitializeAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) if (IsMM_34661A(kvp.Value)) Initialize(kvp.Value); }

        public static Double MeasureVDC(SCPI_VISA_Instrument SVI) {
            ((Ag3466x)SVI.Instrument).SCPI.MEASure.VOLTage.DC.QueryAsciiRealClone("AUTO", "MAXimum", out Double voltsDC);
            return voltsDC;
        }

        public static Double MeasureADC(SCPI_VISA_Instrument SVI) {
            ((Ag3466x)SVI.Instrument).SCPI.MEASure.CURRent.DC.QueryAsciiReal("AUTO", "MAXimum", out Double ampsDC);
            return ampsDC;
        }
    }
}