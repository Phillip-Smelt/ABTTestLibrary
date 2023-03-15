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
        //public static void Local(Instrument instrument) { ((Ag33500B_33600A)instrument.Instance).SCPI.SYSTem.LOCal.Command(); }

        //public static void Remote(Instrument instrument) { ((Ag33500B_33600A)instrument.Instance).SCPI.SYSTem.REMote.Command(); }

        //public static void RemoteLock(Instrument instrument) { ((Ag33500B_33600A)instrument.Instance).SCPI.SYSTem.RWLock.Command(); }

        //public static void Reset(Instrument instrument) { ((Ag33500B_33600A)instrument.Instance).SCPI.RST.Command(); }

        //public static void ResetClear(Instrument instrument) {
        //    ((Ag33500B_33600A)instrument.Instance).SCPI.RST.Command();
        //    ((Ag33500B_33600A)instrument.Instance).SCPI.CLS.Command();
        //    ((Ag33500B_33600A)instrument.Instance).SCPI.OUTPut.PROTection.CLEar.Command();
        //    ((Ag33500B_33600A)instrument.Instance).SCPI.DISPlay.WINDow.TEXT.CLEar.Command();
        //    ((Ag33500B_33600A)instrument.Instance).SCPI.SOURce.CURRent.PROTection.STATe.Command(false, null);
        //    ((Ag33500B_33600A)instrument.Instance).SCPI.SOURce.POWer.PROTection.STATe.Command(false, null);
        //}

        //public static Boolean IsOff(Instrument instrument) { return !IsOn(instrument); }

        //public static Boolean IsOn(Instrument instrument) {
        //    ((Ag33500B_33600A)instrument.Instance).SCPI.OUTPut.Query(out Boolean State);
        //    return State;
        //}

        //public static void Off(Instrument instrument) {
        //    ((Ag33500B_33600A)instrument.Instance).SCPI.OUTPut.STATe.Command(false);
        //}

        //public static void ApplyDC(Instrument instrument, Double voltsDC) {
        //    ((Ag33500B_33600A)instrument.Instance).SCPI.SOURce.APPLy.DC.Command(1u, "DEFAult", "DEFAult", voltsDC);
        //}

        //public static void SetOutputOn(Instrument instrument) {
        //    ((Ag33500B_33600A)instrument.Instance).SCPI.OUTPut.Command(null, true);
        //}

        //public static void SetOutputOff(Instrument instrument) {
        //    ((Ag33500B_33600A)instrument.Instance).SCPI.OUTPut.Command(null, false);
        //}
    }
}