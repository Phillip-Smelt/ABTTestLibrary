using System;
using Agilent.CommandExpert.ScpiNet.AgEL30000_1_2_5_1_0_6_17_114;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new instruments are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI & IVI drivers commands, which are directly exportable as .Net statements.
//
namespace TestLibrary.Instruments.Keysight {
    // TODO: EL34143A Class.
    public static class EL34143A {
        // NOTE: Consider using IVI driver instead of wrapping SCPI driver's calls.
        public static void Local(Instrument instrument) { ((AgEL30000)instrument.Instance).SCPI.SYSTem.LOCal.Command(); }

        public static void Remote(Instrument instrument) { ((AgEL30000)instrument.Instance).SCPI.SYSTem.REMote.Command(); }

        public static void RemoteLock(Instrument instrument) { ((AgEL30000)instrument.Instance).SCPI.SYSTem.RWLock.Command(); }

        public static void Reset(Instrument instrument) { ((AgEL30000)instrument.Instance).SCPI.RST.Command(); }

        public static void ResetClear(Instrument instrument) {
            ((AgEL30000)instrument.Instance).SCPI.RST.Command();
            ((AgEL30000)instrument.Instance).SCPI.CLS.Command();
            ((AgEL30000)instrument.Instance).SCPI.OUTPut.PROTection.CLEar.Command();
            ((AgEL30000)instrument.Instance).SCPI.DISPlay.WINDow.TEXT.CLEar.Command();
        }

        public static void On(Instrument instrument, Double ampsDC) {
            ((AgEL30000)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(ampsDC, null);
        }

        public static void Off(Instrument instrument) {
            ((AgEL30000)instrument.Instance).SCPI.OUTPut.STATe.Command(false, null);
        }
    }
}