using System;
using System.Collections.Generic;
using System.Threading;
using Agilent.CommandExpert.ScpiNet.AgE3610XB_1_0_0_1_00;
using TestLibrary.AppConfig;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new instruments are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
namespace TestLibrary.SCPI_VISA {
    public static class PS_E3610xB {
        public static Boolean IsPS_E3610xB(SCPI_VISA_Instrument instrument) { return (instrument.Instance.GetType() == typeof(AgE3610XB)); }

        public static void Clear(SCPI_VISA_Instrument instrument) { ((AgE3610XB)instrument.Instance).SCPI.CLS.Command(); }

        public static void ClearAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> i in instruments) if (IsPS_E3610xB(i.Value)) Clear(i.Value); }

        public static void Local(SCPI_VISA_Instrument instrument) { ((AgE3610XB)instrument.Instance).SCPI.SYSTem.LOCal.Command(); }

        public static void LocalAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> i in instruments) if (IsPS_E3610xB(i.Value)) Local(i.Value); }

        public static void Remote(SCPI_VISA_Instrument instrument) { ((AgE3610XB)instrument.Instance).SCPI.SYSTem.REMote.Command(); }

        public static void RemoteAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> i in instruments) if (IsPS_E3610xB(i.Value)) Remote(i.Value); }

        public static void RemoteLock(SCPI_VISA_Instrument instrument) { ((AgE3610XB)instrument.Instance).SCPI.SYSTem.RWLock.Command(); }

        public static void RemoteLockAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> i in instruments) if (IsPS_E3610xB(i.Value)) RemoteLock(i.Value); }

        public static void Reset(SCPI_VISA_Instrument instrument) { ((AgE3610XB)instrument.Instance).SCPI.RST.Command(); }

        public static void ResetAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> i in instruments) if (IsPS_E3610xB(i.Value)) Reset(i.Value); }

        public static void SelfTest(SCPI_VISA_Instrument instrument) {
            ((AgE3610XB)instrument.Instance).SCPI.TST.Query(out Int32 selfTestResult);
            if (selfTestResult != 0) {
                ((AgE3610XB)instrument.Instance).SCPI.SYSTem.ERRor.NEXT.Query(out Double errorNumber, out String errorMessage);
                throw new InvalidOperationException(SCPI99.GetErrorMessage(instrument, errorMessage, Convert.ToInt32(errorNumber)));
            }
        }

        public static void SelfTestAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> i in instruments) if (IsPS_E3610xB(i.Value)) SelfTest(i.Value); }

        public static void Initialize(SCPI_VISA_Instrument instrument) {
            Reset(instrument); // Reset instrument to default power-on states.
            Clear(instrument); // Clear all event registers & the Status Byte register.
            SelfTest(instrument);
            ((AgE3610XB)instrument.Instance).SCPI.OUTPut.PROTection.CLEar.Command();
            ((AgE3610XB)instrument.Instance).SCPI.DISPlay.WINDow.TEXT.CLEar.Command();
            RemoteLock(instrument);
        }

        public static void InitializeAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> i in instruments) if (IsPS_E3610xB(i.Value)) Initialize(i.Value); }

        public static Boolean IsOff(SCPI_VISA_Instrument instrument) { return !IsOn(instrument); }

        public static Boolean AreOnAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> instruments) {
            Boolean AreOn = true;
            foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> i in instruments) if (IsPS_E3610xB(i.Value)) AreOn = AreOn && IsOn(i.Value);
            return AreOn;
        }

        public static Boolean IsOn(SCPI_VISA_Instrument instrument) {
            ((AgE3610XB)instrument.Instance).SCPI.OUTPut.STATe.Query(out Boolean State);
            return State;
        }

        public static Boolean AreOffAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> instruments) { return !AreOnAll(instruments); }

        public static void Off(SCPI_VISA_Instrument instrument) { ((AgE3610XB)instrument.Instance).SCPI.OUTPut.STATe.Command(false); }

        public static void OffAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> i in instruments) if (IsPS_E3610xB(i.Value)) Off(i.Value); }

        public static void On(SCPI_VISA_Instrument instrument, Double voltsDC, Double ampsDC, Double secondsDelayCurrentProtection = 0, Double secondsDelayMeasurement = 0) {
            try {
                String s;
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MINimum", out Double min);
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MAXimum", out Double max);
                if ((voltsDC < min) || (voltsDC > max)) {
                    s = $"< MINimum/MAXimum Voltage.{Environment.NewLine}"
                    +   $" - MINimum   :  Voltage={min} VDC.{Environment.NewLine}"
                    +   $" - Programmed:  Voltage={voltsDC} VDC.{Environment.NewLine}"
                    +   $" - MAXimum   :  Voltage={max} VDC.";
                    throw new InvalidOperationException(SCPI99.GetMessage(instrument, s));
                }
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MINimum", out min);
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MAXimum", out max);
                if ((ampsDC < min) || (ampsDC > max)) {
                    s = $"> MINimum/MAXimum Current.{Environment.NewLine}"
                    +   $" - MINimum   :  Current={min} ADC.{Environment.NewLine}"
                    +   $" - Programmed:  Current={ampsDC} ADC.{Environment.NewLine}"
                    +   $" - MAXimum   :  Current={max} ADC.";
                    throw new InvalidOperationException(SCPI99.GetMessage(instrument, s));
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
                if ((secondsDelayCurrentProtection < min) || (secondsDelayCurrentProtection > max)) {
                    s = $"> MINimum/MAXimum Current Protection Delay.{Environment.NewLine}"
                    +   $" - MINimum   :  Delay={min} seconds.{Environment.NewLine}"
                    +   $" - Programmed:  Delay={secondsDelayCurrentProtection} seconds.{Environment.NewLine}"
                    +   $" - MAXimum   :  Delay={max} seconds.";
                    throw new InvalidOperationException(SCPI99.GetMessage(instrument, s));
                }
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(voltsDC);
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(ampsDC);
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Command(secondsDelayCurrentProtection);
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.CURRent.PROTection.STATe.Command(true);
                ((AgE3610XB)instrument.Instance).SCPI.SOURce.VOLTage.PROTection.STATe.Command(false);
                ((AgE3610XB)instrument.Instance).SCPI.OUTPut.STATe.Command(true);
                if (secondsDelayMeasurement > 0) Thread.Sleep((Int32)(secondsDelayMeasurement * 1000));
            } catch (InvalidOperationException) {
                throw;
            } catch (Exception e) {
                throw new InvalidOperationException(SCPI99.GetMessage(instrument), e);
            }
        }

        public static (Double VoltsDC, Double AmpsDC) MeasureVA(SCPI_VISA_Instrument instrument) {
            ((AgE3610XB)instrument.Instance).SCPI.MEASure.VOLTage.DC.Query(out Double voltsDC);
            ((AgE3610XB)instrument.Instance).SCPI.MEASure.CURRent.DC.Query(out Double ampsDC);
            return (voltsDC, ampsDC);
        }
    }
}