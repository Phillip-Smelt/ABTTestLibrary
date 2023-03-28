using System;
using Agilent.CommandExpert.ScpiNet.Ag3466x_2_08;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new instruments are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
namespace TestLibrary.SCPI_VISA {
    public static class MM_34661A {
        public static void Local(Instrument instrument) { ((Ag3466x)instrument.Instance).SCPI.SYSTem.LOCal.Command(); }

        public static void SpecificInitialization(Instrument instrument) {
            SCPI99.SelfTest(instrument.Address); // SCPI99.SelfTest() issues a Factory Reset (*RST) command after its *TST completes.
            SCPI99.Clear(instrument.Address);    // SCPI99.Clear() issues SCPI *CLS.
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