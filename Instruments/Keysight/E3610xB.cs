using System;
using System.Threading;
using Agilent.CommandExpert.ScpiNet.AgE3610XB_1_0_0_1_00;
using Agilent.CommandExpert.ScpiNet.AgE36200_1_0_0_1_0_2_1_00;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new instruments are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI & IVI drivers commands, which are directly exportable as .Net statements.
//
namespace TestLibrary.Instruments.Keysight {
    public static class E3610xB {
        // NOTE: Consider using IVI driver instead of wrapping SCPI driver's calls.
        public static void Local(Instrument instrument) { ((AgE3610XB)instrument.Instance).SCPI.SYSTem.LOCal.Command(); }

        public static void Remote(Instrument instrument) { ((AgE3610XB)instrument.Instance).SCPI.SYSTem.REMote.Command(); }

        public static void RemoteLock(Instrument instrument) { ((AgE3610XB)instrument.Instance).SCPI.SYSTem.RWLock.Command(); }

        public static void Reset(Instrument instrument) { ((AgE3610XB)instrument.Instance).SCPI.RST.Command(); }

        public static void ResetClear(Instrument instrument) {
            ((AgE3610XB)instrument.Instance).SCPI.RST.Command();
            ((AgE3610XB)instrument.Instance).SCPI.CLS.Command();
            ((AgE3610XB)instrument.Instance).SCPI.SOURce.CURRent.PROTection.CLEar.Command();
            ((AgE3610XB)instrument.Instance).SCPI.SOURce.VOLTage.PROTection.CLEar.Command();
            ((AgE3610XB)instrument.Instance).SCPI.OUTPut.PROTection.CLEar.Command();
            ((AgE3610XB)instrument.Instance).SCPI.DISPlay.WINDow.TEXT.CLEar.Command();
        }

        public static Boolean IsOff(Instrument instrument) { return !IsOn(instrument); }

        public static Boolean IsOn(Instrument instrument) {
            ((AgE3610XB)instrument.Instance).SCPI.OUTPut.STATe.Query(out Boolean State);
            return State;
        }

        public static void Off(Instrument instrument) { ((AgE3610XB)instrument.Instance).SCPI.OUTPut.STATe.Command(false); }

        public static void ON(Instrument instrument, Double VoltsDC, Double AmpsDC, Double SettlingDelaySeconds = 0.5) {
            try {
                String s;
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MINimum", out Double VDC);
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MINimum", out Double ADC);
                if ((VoltsDC < VDC) || (AmpsDC < ADC)) {
                    s = $"< MINimum Voltage/Current.{Environment.NewLine}";
                    s += $" - Programmed:  Voltage={VoltsDC}/Current={AmpsDC}.{Environment.NewLine}";
                    s += $" - Minimal   :  Voltage={VDC}/Current={ADC}.";
                    throw new InvalidOperationException(InstrumentTasks.GetMessage(instrument, s));
                }
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MAXimum", out VDC);
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MAXimum", out ADC);
                if ((VoltsDC > VDC) || (AmpsDC > ADC)) {
                    s = $"> MAXimum Voltage/Current.{Environment.NewLine}";
                    s += $" - Programmed:  Voltage={VoltsDC}/Current={AmpsDC}.{Environment.NewLine}";
                    s += $" - Maximal   :  Voltage={VDC}/Current={ADC}.";
                    throw new InvalidOperationException(InstrumentTasks.GetMessage(instrument, s));
                }
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(VoltsDC);
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(AmpsDC);
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Command(SettlingDelaySeconds);
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.CURRent.PROTection.STATe.Command(true);
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.VOLTage.PROTection.STATe.Command(false);
                ((AgE3610XB)instrument.Instance).SCPI.OUTPut.STATe.Command(true);
            } catch (InvalidOperationException) {
                throw;
            } catch (Exception e) {
                throw new InvalidOperationException(InstrumentTasks.GetMessage(instrument), e);
            }
        }

        public static (Double VoltsDC, Double AmpsDC) MeasureVA(Instrument instrument) {
            ((AgE3610XB)instrument.Instance).SCPI.MEASure.VOLTage.DC.Query(out Double VDC);
            ((AgE3610XB)instrument.Instance).SCPI.MEASure.CURRent.DC.Query(out Double ADC);
            return (VDC, ADC);
        }
    }
}