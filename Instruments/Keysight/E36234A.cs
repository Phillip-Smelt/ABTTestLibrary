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
    // NOTE: Channel lists aren't allowed in any methods though many, perhaps most, E36234A SCPI commands do permit them.
    public static class E36234A {
        // NOTE: Consider using IVI driver instead of wrapping SCPI driver's calls.
        private static void ConvertChannel(Instrument instrument, String sChannel, out Int32 iChannel) {
            iChannel = -1;
            if (Int32.TryParse(sChannel, out Int32 ic)) iChannel = --ic;
            // E36234A Channels are indexed 1 to 2, but C# arrays are indexed 0 to 1.
            // Decrement E36234A's iChannel to align to C# arrays.
            if ((iChannel != 0) && (iChannel != 1)) throw new InvalidOperationException(InstrumentTasks.GetMessage(instrument, $"Invalid Channel '{sChannel}'"));
            return;
        }

        public static void Local(Instrument instrument) { ((AgE36200)instrument.Instance).SCPI.SYSTem.LOCal.Command(); }

        public static void Remote(Instrument instrument) { ((AgE36200)instrument.Instance).SCPI.SYSTem.REMote.Command(); }

        public static void RemoteLock(Instrument instrument) { ((AgE36200)instrument.Instance).SCPI.SYSTem.RWLock.Command(); }

        public static void Reset(Instrument instrument) { ((AgE36200)instrument.Instance).SCPI.RST.Command(); }

        public static void ResetClear(Instrument instrument) {
            ((AgE36200)instrument.Instance).SCPI.RST.Command();
            ((AgE36200)instrument.Instance).SCPI.CLS.Command();
            ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.PROTection.CLEar.Command("(@1:2)");
            ((AgE36200)instrument.Instance).SCPI.SOURce.VOLTage.PROTection.CLEar.Command("(@1:2)");
            ((AgE36200)instrument.Instance).SCPI.OUTPut.PROTection.CLEar.Command("(@1:2)");
            ((AgE36200)instrument.Instance).SCPI.DISPlay.WINDow.TEXT.CLEar.Command();
        }

        public static Boolean IsOn(Instrument instrument, String sChannel) {
            ConvertChannel(instrument, sChannel, out Int32 iChannel);
            ((AgE36200)instrument.Instance).SCPI.OUTPut.STATe.Query(sChannel, out Boolean[] States);
            return States[iChannel];
        }

        public static Boolean IsOff(Instrument instrument, String sChannel) { return !IsOn(instrument, sChannel); }

        public static void Off(Instrument instrument, String sChannel) {
            ConvertChannel(instrument, sChannel, out Int32 _);
            ((AgE36200)instrument.Instance).SCPI.OUTPut.STATe.Command(false, sChannel);
        }

        public static void ON(Instrument instrument, Double VoltsDC, Double AmpsDC, String sChannel, Double CurrentProtectionDelaySeconds = 0, Double MeasureDelaySeconds = 0) {
            ConvertChannel(instrument, sChannel, out Int32 iChannel);
            try {
                String s;
                ((AgE36200)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MINimum", sChannel, out Double[] min);
                ((AgE36200)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MAXimum", sChannel, out Double[] max);
                if ((VoltsDC < min[iChannel]) || (VoltsDC > max[iChannel])) {
                    s = $"< MINimum/MAXimum Voltage.{Environment.NewLine}";
                    s += $" - MINimum   :  Voltage={min[iChannel]} VDC.{Environment.NewLine}";
                    s += $" - Programmed:  Voltage={VoltsDC} VDC.{Environment.NewLine}";
                    s += $" - MAXimum   :  Voltage={max[iChannel]} VDC.";
                    throw new InvalidOperationException(InstrumentTasks.GetMessage(instrument, s));
                }
                ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MINimum", sChannel, out min);
                ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MAXimum", sChannel, out max);
                if ((AmpsDC < min[iChannel]) || (AmpsDC > max[iChannel])) {
                    s = $"> MINimum/MAXimum Current.{Environment.NewLine}";
                    s += $" - MINimum   :  Current={min[iChannel]} ADC.{Environment.NewLine}";
                    s += $" - Programmed:  Current={AmpsDC} ADC.{Environment.NewLine}";
                    s += $" - MAXimum   :  Current={max[iChannel]} ADC.";
                    throw new InvalidOperationException(InstrumentTasks.GetMessage(instrument, s));
                }
                ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query("MINimum", sChannel, out min);
                ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query("MAXimum", sChannel, out max);
                if ((CurrentProtectionDelaySeconds < min[iChannel]) || (CurrentProtectionDelaySeconds > max[iChannel])) {
                    s = $"> MINimum/MAXimum Current Protection Delay.{Environment.NewLine}";
                    s += $" - MINimum   :  Delay={min[iChannel]} seconds.{Environment.NewLine}";
                    s += $" - Programmed:  Delay={CurrentProtectionDelaySeconds} seconds.{Environment.NewLine}";
                    s += $" - MAXimum   :  Delay={max[iChannel]} seconds.";
                    throw new InvalidOperationException(InstrumentTasks.GetMessage(instrument, s));
                }
                ((AgE36200)instrument.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal", sChannel);
                ((AgE36200)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(VoltsDC, sChannel);
                ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(AmpsDC, sChannel);
                ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Command(CurrentProtectionDelaySeconds, sChannel);
                ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.PROTection.STATe.Command(true, sChannel);
                ((AgE36200)instrument.Instance).SCPI.SOURce.VOLTage.PROTection.STATe.Command(false, sChannel);
                ((AgE36200)instrument.Instance).SCPI.OUTPut.STATe.Command(true, sChannel);
                if (MeasureDelaySeconds > 0) Thread.Sleep((Int32)(MeasureDelaySeconds * 1000));
            } catch (InvalidOperationException) {
                throw;
            } catch (Exception e) {
                throw new InvalidOperationException(InstrumentTasks.GetMessage(instrument), e);
            }
        }

        public static (Double VoltsDC, Double AmpsDC) MeasureVA(Instrument instrument, String sChannel) {
            ConvertChannel(instrument, sChannel, out Int32 iChannel);
            ((AgE36200)instrument.Instance).SCPI.MEASure.SCALar.VOLTage.DC.Query(sChannel, out Double[] VDC);
            ((AgE36200)instrument.Instance).SCPI.MEASure.SCALar.CURRent.DC.Query(sChannel, out Double[] ADC);
            return (VDC[iChannel], ADC[iChannel]);
        }
    }
}