using System;
using System.Threading;
using Agilent.CommandExpert.ScpiNet.AgE3610XB_1_0_0_1_00;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new instruments are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI & IVI drivers commands, which are directly exportable as .Net statements.
//
namespace TestLibrary.TestInstruments.Keysight {
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

        public static void ON(Instrument instrument, Double VoltsDC, Double AmpsDC, Double CurrentProtectionDelaySeconds = 0, Double MeasureDelaySeconds = 0) {
            try {
                String s;
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MINimum", out Double min);
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MAXimum", out Double max);
                if ((VoltsDC < min) || (VoltsDC > max)) {
                    s = $"< MINimum/MAXimum Voltage.{Environment.NewLine}";
                    s += $" - MINimum   :  Voltage={min} VDC.{Environment.NewLine}";
                    s += $" - Programmed:  Voltage={VoltsDC} VDC.{Environment.NewLine}";
                    s += $" - MAXimum   :  Voltage={max} VDC.";
                    throw new InvalidOperationException(InstrumentTasks.GetMessage(instrument, s));
                }
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MINimum", out min);
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MAXimum", out max);
                if ((AmpsDC < min) || (AmpsDC > max)) {
                    s = $"> MINimum/MAXimum Current.{Environment.NewLine}";
                    s += $" - MINimum   :  Current={min} ADC.{Environment.NewLine}";
                    s += $" - Programmed:  Current={AmpsDC} ADC.{Environment.NewLine}";
                    s += $" - MAXimum   :  Current={max} ADC.";
                    throw new InvalidOperationException(InstrumentTasks.GetMessage(instrument, s));
                }
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query("MINimum", out min);
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query("MAXimum", out max);
                // NOTE: Keysight Command Expert's AgE3610XB driver documentation states:
                //  - [SOURce:]CURRent:PROTection:DELay[:TIME] <time> | MINimum | MAXimum
                //  - [SOURce:] CURRent: PROTection: DELay[:TIME]?[MINimum | MAXimum]
                //  - Sets the time (in milliseconds) that the overcurrent protection is temporarily disabled after a current level change.
                //  - The query returns a number of the form +#.########E+##.
                //
                // However, the MIN/MAX query returned values are 0/32.767 respectively, which I assume are 0 to 32.767 *seconds* instead of *milli-seconds*.
                // Keysight's E36100B Series Operating and Service Guide states the same thing.
                //  - https://www.keysight.com/us/en/assets/9018-04288/user-manuals/9018-04288.pdf
                //
                if ((CurrentProtectionDelaySeconds < min) || (CurrentProtectionDelaySeconds > max)) {
                    s = $"> MINimum/MAXimum Current Protection Delay.{Environment.NewLine}";
                    s += $" - MINimum   :  Delay={min} seconds.{Environment.NewLine}";
                    s += $" - Programmed:  Delay={CurrentProtectionDelaySeconds} seconds.{Environment.NewLine}";
                    s += $" - MAXimum   :  Delay={max} seconds.";
                    throw new InvalidOperationException(InstrumentTasks.GetMessage(instrument, s));
                }
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(VoltsDC);
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(AmpsDC);
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Command(CurrentProtectionDelaySeconds);
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.CURRent.PROTection.STATe.Command(true);
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.VOLTage.PROTection.STATe.Command(false);
                ((AgE3610XB)instrument.Instance).SCPI.OUTPut.STATe.Command(true);
                if (MeasureDelaySeconds > 0) Thread.Sleep((Int32)(MeasureDelaySeconds * 1000));
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