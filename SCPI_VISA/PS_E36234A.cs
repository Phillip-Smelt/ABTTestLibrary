using System;
using System.Collections.Generic;
using System.Threading;
using Agilent.CommandExpert.ScpiNet.AgE36200_1_0_0_1_0_2_1_00;
using static TestLibrary.SCPI_VISA.Instrument;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new instruments are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
namespace TestLibrary.SCPI_VISA {
    public static class PS_E36234A {
        private static Int32 ConvertChannel(Instrument instrument, String sChannel) {
            if (String.Equals(sChannel, Instrument.CHANNEL_1)) return 0;
            else if (String.Equals(sChannel, Instrument.CHANNEL_2)) return 1;
            else throw new InvalidOperationException(Instrument.GetSCPI_VISA_Message(instrument, $"Invalid Channel '{sChannel}'"));
        }

        public static Boolean IsPS_E36234A(Instrument instrument) { return (instrument.Instance.GetType() == typeof(AgE36200)); }

        public static void Clear(Instrument instrument) { ((AgE36200)instrument.Instance).SCPI.CLS.Command(); }

        public static void ClearAll(Dictionary<IDs, Instrument> instruments) { foreach (KeyValuePair<IDs, Instrument> i in instruments) if (IsPS_E36234A(i.Value)) Clear(i.Value); }

        public static void Local(Instrument instrument) { ((AgE36200)instrument.Instance).SCPI.SYSTem.LOCal.Command(); }

        public static void LocalAll(Dictionary<IDs, Instrument> instruments) { foreach (KeyValuePair<IDs, Instrument> i in instruments) if (IsPS_E36234A(i.Value)) Local(i.Value); }

        public static void Remote(Instrument instrument) { ((AgE36200)instrument.Instance).SCPI.SYSTem.REMote.Command(); }

        public static void RemoteAll(Dictionary<IDs, Instrument> instruments) { foreach (KeyValuePair<IDs, Instrument> i in instruments) if (IsPS_E36234A(i.Value)) Remote(i.Value); }

        public static void RemoteLock(Instrument instrument) { ((AgE36200)instrument.Instance).SCPI.SYSTem.RWLock.Command(); }

        public static void RemoteLockAll(Dictionary<IDs, Instrument> instruments) { foreach (KeyValuePair<IDs, Instrument> i in instruments) if (IsPS_E36234A(i.Value)) RemoteLock(i.Value); }

        public static void Reset(Instrument instrument) { ((AgE36200)instrument.Instance).SCPI.RST.Command(); }

        public static void ResetAll(Dictionary<IDs, Instrument> instruments) { foreach (KeyValuePair<IDs, Instrument> i in instruments) if (IsPS_E36234A(i.Value)) Reset(i.Value); }

        public static void SelfTest(Instrument instrument) {
            ((AgE36200)instrument.Instance).SCPI.TST.Query(out Int32 selfTestResult);
            if (selfTestResult != 0) {
                ((AgE36200)instrument.Instance).SCPI.SYSTem.ERRor.NEXT.Query(out String nextError);
                throw new InvalidOperationException(GetSCPI_VISA_ErrorMessage(instrument, nextError));
            }
        }

        public static void SelfTestAll(Dictionary<IDs, Instrument> instruments) { foreach (KeyValuePair<IDs, Instrument> i in instruments) if (IsPS_E36234A(i.Value)) SelfTest(i.Value); }

        public static void Initialize(Instrument instrument) {
            Reset(instrument); // Reset instrument to default power-on states.
            Clear(instrument); // Clear all event registers & the Status Byte register.
            SelfTest(instrument);
            ((AgE36200)instrument.Instance).SCPI.OUTPut.PROTection.CLEar.Command(CHANNEL_1_2);
            ((AgE36200)instrument.Instance).SCPI.DISPlay.WINDow.TEXT.CLEar.Command();
            RemoteLock(instrument);
        }

        public static void InitializeAll(Dictionary<IDs, Instrument> instruments) { foreach (KeyValuePair<IDs, Instrument> i in instruments) if (IsPS_E36234A(i.Value)) Initialize(i.Value); }

        public static Boolean IsOff(Instrument instrument, String sChannel) { return !IsOn(instrument, sChannel); }

        public static Boolean AreOnAll(Dictionary<IDs, Instrument> instruments) {
            Boolean AreOn = true;
            foreach (KeyValuePair<IDs, Instrument> i in instruments) if (IsPS_E36234A(i.Value)) AreOn = AreOn && IsOn(i.Value, CHANNEL_1) && IsOn(i.Value, CHANNEL_2);
            return AreOn;
        }

        public static Boolean IsOn(Instrument instrument, String sChannel) {
            ((AgE36200)instrument.Instance).SCPI.OUTPut.STATe.Query(sChannel, out Boolean[] States);
            return States[ConvertChannel(instrument, sChannel)];
        }

        public static Boolean AreOffAll(Dictionary<IDs, Instrument> instruments) { return !AreOnAll(instruments); }

        public static void Off(Instrument instrument, String sChannel) { ((AgE36200)instrument.Instance).SCPI.OUTPut.STATe.Command(false, sChannel); }

        public static void OffAll(Dictionary<IDs, Instrument> instruments) { foreach (KeyValuePair<IDs, Instrument> i in instruments) if (IsPS_E36234A(i.Value)) Off(i.Value, CHANNEL_1_2); }

        public static void On(Instrument instrument, Double voltsDC, Double ampsDC, String sChannel, Double secondsDelayCurrentProtection = 0, Double secondsDelayMeasurement = 0) {
            Int32 iChannel = ConvertChannel(instrument, sChannel);
            try {
                String s;
                ((AgE36200)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MINimum", sChannel, out Double[] min);
                ((AgE36200)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MAXimum", sChannel, out Double[] max);
                if ((voltsDC < min[iChannel]) || (voltsDC > max[iChannel])) {
                    s = $"< MINimum/MAXimum Voltage.{Environment.NewLine}"
                    + $" - MINimum   :  Voltage={min[iChannel]} VDC.{Environment.NewLine}"
                    + $" - Programmed:  Voltage={voltsDC} VDC.{Environment.NewLine}"
                    + $" - MAXimum   :  Voltage={max[iChannel]} VDC.";
                    throw new InvalidOperationException(Instrument.GetSCPI_VISA_Message(instrument, s));
                }
                ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MINimum", sChannel, out min);
                ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MAXimum", sChannel, out max);
                if ((ampsDC < min[iChannel]) || (ampsDC > max[iChannel])) {
                    s = $"> MINimum/MAXimum Current.{Environment.NewLine}"
                    + $" - MINimum   :  Current={min[iChannel]} ADC.{Environment.NewLine}"
                    + $" - Programmed:  Current={ampsDC} ADC.{Environment.NewLine}"
                    + $" - MAXimum   :  Current={max[iChannel]} ADC.";
                    throw new InvalidOperationException(Instrument.GetSCPI_VISA_Message(instrument, s));
                }
                ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query("MINimum", sChannel, out min);
                ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query("MAXimum", sChannel, out max);
                if ((secondsDelayCurrentProtection < min[iChannel]) || (secondsDelayCurrentProtection > max[iChannel])) {
                    s = $"> MINimum/MAXimum Current Protection Delay.{Environment.NewLine}"
                    + $" - MINimum   :  Delay={min[iChannel]} seconds.{Environment.NewLine}"
                    + $" - Programmed:  Delay={secondsDelayCurrentProtection} seconds.{Environment.NewLine}"
                    + $" - MAXimum   :  Delay={max[iChannel]} seconds.";
                    throw new InvalidOperationException(Instrument.GetSCPI_VISA_Message(instrument, s));
                }
                ((AgE36200)instrument.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal", sChannel);
                ((AgE36200)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(voltsDC, sChannel);
                ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(ampsDC, sChannel);
                ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Command(secondsDelayCurrentProtection, sChannel);
                ((AgE36200)instrument.Instance).SCPI.SOURce.CURRent.PROTection.STATe.Command(true, sChannel);
                ((AgE36200)instrument.Instance).SCPI.SOURce.VOLTage.PROTection.STATe.Command(false, sChannel);
                ((AgE36200)instrument.Instance).SCPI.OUTPut.STATe.Command(true, sChannel);
                if (secondsDelayMeasurement > 0) Thread.Sleep((Int32)(secondsDelayMeasurement * 1000));
            } catch (InvalidOperationException) {
                throw;
            } catch (Exception e) {
                throw new InvalidOperationException(Instrument.GetSCPI_VISA_Message(instrument), e);
            }
        }

        public static (Double VoltsDC, Double AmpsDC) MeasureVA(Instrument instrument, String sChannel) {
            Int32 iChannel = ConvertChannel(instrument, sChannel);
            ((AgE36200)instrument.Instance).SCPI.MEASure.SCALar.VOLTage.DC.Query(sChannel, out Double[] voltsDC);
            ((AgE36200)instrument.Instance).SCPI.MEASure.SCALar.CURRent.DC.Query(sChannel, out Double[] ampsDC);
            return (voltsDC[iChannel], ampsDC[iChannel]);
        }
    }
}