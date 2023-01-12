using System;
using Agilent.CommandExpert.ScpiNet.Ag33500B_33600A_2_09;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new instruments are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI & IVI drivers commands, which are directly exportable as .Net statements.
//
namespace TestLibrary.Instruments.Keysight {
    // TODO: KS33509B Class.
    public static class KS33509B {
        // NOTE: Consider using IVI driver instead of wrapping SCPI driver's calls.

        public static void ApplyDC(Instrument Instrument, Double Volts) {
            ((Ag33500B_33600A)Instrument.Instance).SCPI.SOURce.APPLy.DC.Command(1u, "DEFAult", "DEFAult", Volts);
        }

        public static void SetOutputOn(Instrument Instrument) {
            ((Ag33500B_33600A)Instrument.Instance).SCPI.OUTPut.Command(null, true);
        }

        public static void SetOutputOff(Instrument Instrument) {
            ((Ag33500B_33600A)Instrument.Instance).SCPI.OUTPut.Command(null, false);
        }
    }
}