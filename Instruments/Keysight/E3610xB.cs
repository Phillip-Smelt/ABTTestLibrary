using System;
using System.Threading;
using Agilent.CommandExpert.ScpiNet.AgE3610XB_1_0_0_1_00;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new instruments are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html

namespace TestLibrary.Instruments.Keysight {
    public static class E3610xB {
        // NOTE: Consider using IVI driver instead of wrapping SCPI driver's calls.
        public static void RemoteOn(Instrument Instrument) {
            ((AgE3610XB)Instrument.Instance).SCPI.SYSTem.REMote.Command();
        }

        public static void RemoteLockOn(Instrument Instrument) {
            ((AgE3610XB)Instrument.Instance).SCPI.SYSTem.RWLock.Command();
        }

        public static Boolean IsOff(Instrument Instrument) {
            return !IsOn(Instrument);
        }

        public static Boolean IsOn(Instrument Instrument) {
            ((AgE3610XB)Instrument.Instance).SCPI.OUTPut.STATe.Query(out Boolean State);
            return State;
        }

        public static void Off(Instrument Instrument) {
            ((AgE3610XB)Instrument.Instance).SCPI.OUTPut.STATe.Command(false);
        }

        public static void ON(Instrument Instrument, Double Volts, Double Amps, Int32 SettlingDelayMS = 0) {
            try {
                String s;
                ((AgE3610XB)Instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MINimum", out Double V);
                ((AgE3610XB)Instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MINimum", out Double A);
                if ((Volts < V) || (Amps < A)) {
                    s = $"< MINimum Voltage/Current.{Environment.NewLine}";
                    s += $" - Programmed:  Voltage={Volts}/Current={Amps}.{Environment.NewLine}";
                    s += $" - Minimal   :  Voltage={V}/Current={A}.";
                    throw new InvalidOperationException(InstrumentTasks.GetMessage(Instrument, s));
                }
                ((AgE3610XB)Instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MAXimum", out V);
                ((AgE3610XB)Instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MAXimum", out A);
                if ((Volts > V) || (Amps > A)) {
                    s = $"> MAXimum Voltage/Current.{Environment.NewLine}";
                    s += $" - Programmed:  Voltage={Volts}/Current={Amps}.{2}";
                    s += $" - Maximal   :  Voltage={V}/Current={A}.";
                    throw new InvalidOperationException(InstrumentTasks.GetMessage(Instrument, s));
                }
                // TODO: E3610xB.SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
                ((AgE3610XB)Instrument.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("INTernal");
                ((AgE3610XB)Instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(Volts);
                ((AgE3610XB)Instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(Amps);
                ((AgE3610XB)Instrument.Instance).SCPI.SOURce.CURRent.PROTection.STATe.Command(true);
                ((AgE3610XB)Instrument.Instance).SCPI.OUTPut.STATe.Command(true);
                if (SettlingDelayMS != 0) Thread.Sleep(SettlingDelayMS);
            } catch (InvalidOperationException) {
                throw;
            } catch (Exception e) {
                throw new InvalidOperationException(InstrumentTasks.GetMessage(Instrument), e);
            }
        }

        public static (Double V, Double A) MeasureVA(Instrument Instrument) {
            ((AgE3610XB)Instrument.Instance).SCPI.MEASure.VOLTage.DC.Query(out Double V);
            ((AgE3610XB)Instrument.Instance).SCPI.MEASure.CURRent.DC.Query(out Double A);
            return (V, A);
        }
    }
}