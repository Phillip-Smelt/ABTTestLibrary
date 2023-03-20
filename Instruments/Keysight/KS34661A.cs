using System;
using Agilent.CommandExpert.ScpiNet.Ag3466x_2_08;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new instruments are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI & IVI drivers commands, which are directly exportable as .Net statements.
//
namespace TestLibrary.Instruments.Keysight {
    // TODO: KS34661A Class.
    public static class KS34661A {
        // No good SCPI device driver, only IVI.

        public static void Local(Instrument instrument) { ((Ag3466x)instrument.Instance).SCPI.SYSTem.LOCal.Command(); }

        public static void Reset(Instrument instrument) { ((Ag3466x)instrument.Instance).SCPI.RST.Command(); }

        public static void ResetClear(Instrument instrument) {
            ((Ag3466x)instrument.Instance).SCPI.RST.Command();
            ((Ag3466x)instrument.Instance).SCPI.CLS.Command();
            ((Ag3466x)instrument.Instance).SCPI.DISPlay.TEXT.CLEar.Command();
        }

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