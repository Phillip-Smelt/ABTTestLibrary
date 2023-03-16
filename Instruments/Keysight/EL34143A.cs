using System;
using Agilent.CommandExpert.ScpiNet.AgE36200_1_0_0_1_0_2_1_00;
using Agilent.CommandExpert.ScpiNet.AgEL30000_1_2_5_1_0_6_17_114;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new instruments are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI & IVI drivers commands, which are directly exportable as .Net statements.
//
namespace TestLibrary.Instruments.Keysight {
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

        public static Boolean IsOff(Instrument instrument) { return !IsOn(instrument); }

        public static Boolean IsOn(Instrument instrument) {
            ((AgEL30000)instrument.Instance).SCPI.OUTPut.STATe.Query(out Boolean State);
            return State;
        }

        public static String Mode(Instrument instrument) {
            ((AgEL30000)instrument.Instance).SCPI.SOURce.MODE.Query("@1", out String Mode);
            return Mode;
        }

        public static void Off(Instrument instrument) {
            ((AgEL30000)instrument.Instance).SCPI.OUTPut.STATe.Command(false, "@1");
        }

        public static void OnConstantCurrent(Instrument instrument, Double amps) {
            ((AgEL30000)instrument.Instance).SCPI.SOURce.MODE.Command("CURRent", "@1");
            ((AgEL30000)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(amps, "@1");
            ((AgEL30000)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MINimum", "@1", out Double min);
            ((AgEL30000)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MAXimum", "@1", out Double max);
            if ((amps < min) || (amps > max)) {
                String s = $"> MINimum/MAXimum Current.{Environment.NewLine}"
                    + $" - MINimum   :  Current={min} A.{Environment.NewLine}"
                    + $" - Programmed:  Current={amps} A.{Environment.NewLine}"
                    + $" - MAXimum   :  Current={max} A.";
                throw new InvalidOperationException(InstrumentTasks.GetMessage(instrument, s));
            }
            ((AgEL30000)instrument.Instance).SCPI.SOURce.CURRent.PROTection.STATe.Command(false, "@1");
            ((AgEL30000)instrument.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgEL30000)instrument.Instance).SCPI.OUTPut.STATe.Command(true, "@1");
        }

        public static void OnConstantVoltage(Instrument instrument, Double volts) {
            ((AgEL30000)instrument.Instance).SCPI.SOURce.MODE.Command("VOLTage", "@1");
            ((AgEL30000)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(volts, "@1");
            ((AgEL30000)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MINimum", "@1", out Double[] min);
            ((AgEL30000)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MAXimum", "@1", out Double[] max);
            if ((volts < min[0]) || (volts > max[1])) {
                String s = $"< MINimum/MAXimum Voltage.{Environment.NewLine}"
                    + $" - MINimum   :  Voltage={min[0]} V.{Environment.NewLine}"
                    + $" - Programmed:  Voltage={volts} V.{Environment.NewLine}"
                    + $" - MAXimum   :  Voltage={max[1]} V.";
                throw new InvalidOperationException(InstrumentTasks.GetMessage(instrument, s));
            }
            ((AgEL30000)instrument.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgEL30000)instrument.Instance).SCPI.OUTPut.STATe.Command(true, "@1");
        }

        public static void OnConstantPower(Instrument instrument, Double watts) {
            ((AgEL30000)instrument.Instance).SCPI.SOURce.MODE.Command("POWer", "@1");
            ((AgEL30000)instrument.Instance).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Command(watts, "@1");
            ((AgEL30000)instrument.Instance).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Query("MINimum", "@1", out Double[] min);
            ((AgEL30000)instrument.Instance).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Query("MAXimum", "@1", out Double[] max);
            if ((watts < min[0]) || (watts > max[1])) {
                String s = $"< MINimum/MAXimum Wattage.{Environment.NewLine}"
                    + $" - MINimum   :  Wattage={min[0]} W.{Environment.NewLine}"
                    + $" - Programmed:  Wattage={watts} W.{Environment.NewLine}"
                    + $" - MAXimum   :  Wattage={max[1]} W.";
                throw new InvalidOperationException(InstrumentTasks.GetMessage(instrument, s));
            }
            ((AgEL30000)instrument.Instance).SCPI.SOURce.POWer.PROTection.STATe.Command(false, "@1");
            ((AgEL30000)instrument.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgEL30000)instrument.Instance).SCPI.OUTPut.STATe.Command(true, "@1");
        }

        public static void OnConstantResistance(Instrument instrument, Double ohms) {
            ((AgEL30000)instrument.Instance).SCPI.SOURce.MODE.Command("RESistance", "@1");
            ((AgEL30000)instrument.Instance).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Command(ohms, "@1");
            ((AgEL30000)instrument.Instance).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Query("MINimum", "@1", out Double[] min);
            ((AgEL30000)instrument.Instance).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Query("MAXimum", "@1", out Double[] max);
            if ((ohms < min[0]) || (ohms > max[0])) {
                String s = $"< MINimum/MAXimum Resistance.{Environment.NewLine}"
                    + $" - MINimum   :  Resistance={min[0]} Ω.{Environment.NewLine}"
                    + $" - Programmed:  Resistance={ohms} Ω.{Environment.NewLine}"
                    + $" - MAXimum   :  Resistance={max[0]} Ω.";
                throw new InvalidOperationException(InstrumentTasks.GetMessage(instrument, s));
            }
            ((AgEL30000)instrument.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgEL30000)instrument.Instance).SCPI.OUTPut.STATe.Command(true, "@1");
        }
    }
}