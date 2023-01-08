using System;
using Agilent.CommandExpert.ScpiNet.Ag33500B_33600A_2_09; // https://www.keysight.com/us/en/search.html/command+expert

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