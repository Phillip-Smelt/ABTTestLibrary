using System;
using System.Collections.Generic;
using System.Threading;
using Agilent.CommandExpert.ScpiNet.AgE36200_1_0_0_1_0_2_1_00;
using TestLibrary.AppConfig;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new SVIs in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new SVIs are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
namespace TestLibrary.SCPI_VISA {
    public static class PS_E36234A {
        private static Int32 ConvertChannel(SCPI_VISA_Instrument SVI, String sChannel) {
            if (String.Equals(sChannel, SCPI99.CHANNEL_1)) return 0;
            else if (String.Equals(sChannel, SCPI99.CHANNEL_2)) return 1;
            else throw new InvalidOperationException(SCPI99.GetMessage(SVI, $"Invalid Channel '{sChannel}'"));
        }

        public static Boolean IsPS_E36234A(SCPI_VISA_Instrument SVI) { return (SVI.Instance.GetType() == typeof(AgE36200)); }

        public static void Clear(SCPI_VISA_Instrument SVI) { ((AgE36200)SVI.Instance).SCPI.CLS.Command(); }

        public static void ClearAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsPS_E36234A(SVI.Value)) Clear(SVI.Value); }

        public static void Local(SCPI_VISA_Instrument SVI) { ((AgE36200)SVI.Instance).SCPI.SYSTem.LOCal.Command(); }

        public static void LocalAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsPS_E36234A(SVI.Value)) Local(SVI.Value); }

        public static void Remote(SCPI_VISA_Instrument SVI) { ((AgE36200)SVI.Instance).SCPI.SYSTem.REMote.Command(); }

        public static void RemoteAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsPS_E36234A(SVI.Value)) Remote(SVI.Value); }

        public static void RemoteLock(SCPI_VISA_Instrument SVI) { ((AgE36200)SVI.Instance).SCPI.SYSTem.RWLock.Command(); }

        public static void RemoteLockAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsPS_E36234A(SVI.Value)) RemoteLock(SVI.Value); }

        public static void Reset(SCPI_VISA_Instrument SVI) { ((AgE36200)SVI.Instance).SCPI.RST.Command(); }

        public static void ResetAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsPS_E36234A(SVI.Value)) Reset(SVI.Value); }

        public static void SelfTest(SCPI_VISA_Instrument SVI) {
            ((AgE36200)SVI.Instance).SCPI.TST.Query(out Int32 selfTestResult);
            if (selfTestResult != 0) {
                ((AgE36200)SVI.Instance).SCPI.SYSTem.ERRor.NEXT.Query(out String nextError);
                throw new InvalidOperationException(SCPI99.GetErrorMessage(SVI, nextError));
            }
        }

        public static void SelfTestAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsPS_E36234A(SVI.Value)) SelfTest(SVI.Value); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            Reset(SVI); // Reset SVI to default power-on states.
            Clear(SVI); // Clear all event registers & the Status Byte register.
            SelfTest(SVI);
            ((AgE36200)SVI.Instance).SCPI.OUTPut.PROTection.CLEar.Command(SCPI99.CHANNEL_1_2);
            ((AgE36200)SVI.Instance).SCPI.DISPlay.WINDow.TEXT.CLEar.Command();
            RemoteLock(SVI);
        }

        public static void InitializeAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsPS_E36234A(SVI.Value)) Initialize(SVI.Value); }

        public static Boolean IsOff(SCPI_VISA_Instrument SVI, String sChannel) { return !IsOn(SVI, sChannel); }

        public static Boolean AreOnAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) {
            Boolean AreOn = true;
            foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsPS_E36234A(SVI.Value)) AreOn = AreOn && IsOn(SVI.Value, SCPI99.CHANNEL_1) && IsOn(SVI.Value, SCPI99.CHANNEL_2);
            return AreOn;
        }

        public static Boolean IsOn(SCPI_VISA_Instrument SVI, String sChannel) {
            ((AgE36200)SVI.Instance).SCPI.OUTPut.STATe.Query(sChannel, out Boolean[] States);
            return States[ConvertChannel(SVI, sChannel)];
        }

        public static Boolean AreOffAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { return !AreOnAll(SVIs); }

        public static void Off(SCPI_VISA_Instrument SVI, String sChannel) { ((AgE36200)SVI.Instance).SCPI.OUTPut.STATe.Command(false, sChannel); }

        public static void OffAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsPS_E36234A(SVI.Value)) Off(SVI.Value, SCPI99.CHANNEL_1_2); }

        public static void On(SCPI_VISA_Instrument SVI, Double voltsDC, Double ampsDC, String sChannel, Double secondsDelayCurrentProtection = 0, Double secondsDelayMeasurement = 0) {
            Int32 iChannel = ConvertChannel(SVI, sChannel);
            try {
                String s;
                ((AgE36200)SVI.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MINimum", sChannel, out Double[] min);
                ((AgE36200)SVI.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MAXimum", sChannel, out Double[] max);
                if ((voltsDC < min[iChannel]) || (voltsDC > max[iChannel])) {
                    s = $"< MINimum/MAXimum Voltage.{Environment.NewLine}"
                    + $" - MINimum   :  Voltage={min[iChannel]} VDC.{Environment.NewLine}"
                    + $" - Programmed:  Voltage={voltsDC} VDC.{Environment.NewLine}"
                    + $" - MAXimum   :  Voltage={max[iChannel]} VDC.";
                    throw new InvalidOperationException(SCPI99.GetMessage(SVI, s));
                }
                ((AgE36200)SVI.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MINimum", sChannel, out min);
                ((AgE36200)SVI.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MAXimum", sChannel, out max);
                if ((ampsDC < min[iChannel]) || (ampsDC > max[iChannel])) {
                    s = $"> MINimum/MAXimum Current.{Environment.NewLine}"
                    + $" - MINimum   :  Current={min[iChannel]} ADC.{Environment.NewLine}"
                    + $" - Programmed:  Current={ampsDC} ADC.{Environment.NewLine}"
                    + $" - MAXimum   :  Current={max[iChannel]} ADC.";
                    throw new InvalidOperationException(SCPI99.GetMessage(SVI, s));
                }
                ((AgE36200)SVI.Instance).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query("MINimum", sChannel, out min);
                ((AgE36200)SVI.Instance).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query("MAXimum", sChannel, out max);
                if ((secondsDelayCurrentProtection < min[iChannel]) || (secondsDelayCurrentProtection > max[iChannel])) {
                    s = $"> MINimum/MAXimum Current Protection Delay.{Environment.NewLine}"
                    + $" - MINimum   :  Delay={min[iChannel]} seconds.{Environment.NewLine}"
                    + $" - Programmed:  Delay={secondsDelayCurrentProtection} seconds.{Environment.NewLine}"
                    + $" - MAXimum   :  Delay={max[iChannel]} seconds.";
                    throw new InvalidOperationException(SCPI99.GetMessage(SVI, s));
                }
                ((AgE36200)SVI.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal", sChannel);
                ((AgE36200)SVI.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(voltsDC, sChannel);
                ((AgE36200)SVI.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(ampsDC, sChannel);
                ((AgE36200)SVI.Instance).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Command(secondsDelayCurrentProtection, sChannel);
                ((AgE36200)SVI.Instance).SCPI.SOURce.CURRent.PROTection.STATe.Command(true, sChannel);
                ((AgE36200)SVI.Instance).SCPI.SOURce.VOLTage.PROTection.STATe.Command(false, sChannel);
                ((AgE36200)SVI.Instance).SCPI.OUTPut.STATe.Command(true, sChannel);
                if (secondsDelayMeasurement > 0) Thread.Sleep((Int32)(secondsDelayMeasurement * 1000));
            } catch (InvalidOperationException) {
                throw;
            } catch (Exception e) {
                throw new InvalidOperationException(SCPI99.GetMessage(SVI), e);
            }
        }

        public static (Double VoltsDC, Double AmpsDC) MeasureVA(SCPI_VISA_Instrument SVI, String sChannel) {
            Int32 iChannel = ConvertChannel(SVI, sChannel);
            ((AgE36200)SVI.Instance).SCPI.MEASure.SCALar.VOLTage.DC.Query(sChannel, out Double[] voltsDC);
            ((AgE36200)SVI.Instance).SCPI.MEASure.SCALar.CURRent.DC.Query(sChannel, out Double[] ampsDC);
            return (voltsDC[iChannel], ampsDC[iChannel]);
        }
    }
}