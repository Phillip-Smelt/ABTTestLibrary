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

        public static void ON(Instrument instrument, Double Volts, Double Amps, String sChannel, Double SettlingDelaySeconds = 0.5) {
            ConvertChannel(instrument, sChannel, out Int32 iChannel);
            try {
                String s;
                ((AgE36200)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MINimum", sChannel, out Double[] V);
                ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MINimum", sChannel, out Double[] A);
                if ((Volts < V[iChannel]) || (Amps < A[iChannel])) {
                    s = $"< MINimum Voltage/Current with Channel '{sChannel}'.{Environment.NewLine}";
                    s += $" - Programmed:  Voltage={Volts}/Current={Amps}.{Environment.NewLine}";
                    s += $" - Minimal   :  Voltage={V[iChannel]}/Current={A[iChannel]}.";
                    throw new InvalidOperationException(InstrumentTasks.GetMessage(instrument, s));
                }
                ((AgE36200)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MAXimum", sChannel, out V);
                ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MAXimum", sChannel, out A);
                if ((Volts < V[iChannel]) || (Amps < A[iChannel])) {
                    s = $"> MAXimum Voltage/Current with Channel '{sChannel}'.{Environment.NewLine}";
                    s += $" - Programmed:  Voltage={Volts}/Current={Amps}.{Environment.NewLine}";
                    s += $" - Maximal   :  Voltage={V[iChannel]}/Current={A[iChannel]}.";
                    throw new InvalidOperationException(InstrumentTasks.GetMessage(instrument, s));
                }
                ((AgE36200)instrument.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal", sChannel);
                ((AgE36200)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(Volts, sChannel);
                ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(Amps, sChannel);
                ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Command(SettlingDelaySeconds, sChannel);
                ((AgE36200)instrument.Instance).SCPI.OUTPut.STATe.Command(true, sChannel);
                ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.PROTection.STATe.Command(true, sChannel);
                ((AgE36200)instrument.Instance).SCPI.SOURce.VOLTage.PROTection.STATe.Command(false, sChannel);
            } catch (InvalidOperationException) {
                throw;
            } catch (Exception e) {
                throw new InvalidOperationException(InstrumentTasks.GetMessage(instrument), e);
            }
        }

        public static (Double V, Double A) MeasureVA(Instrument instrument, String sChannel) {
            ConvertChannel(instrument, sChannel, out Int32 iChannel);
            ((AgE36200)instrument.Instance).SCPI.MEASure.SCALar.VOLTage.DC.Query(sChannel, out Double[] V);
            ((AgE36200)instrument.Instance).SCPI.MEASure.SCALar.CURRent.DC.Query(sChannel, out Double[] A);
            return (V[iChannel], A[iChannel]);
        }
    }
}