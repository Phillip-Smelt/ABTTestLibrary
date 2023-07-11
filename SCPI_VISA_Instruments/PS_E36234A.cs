using System;
using System.Threading;
using Agilent.CommandExpert.ScpiNet.AgE36200_1_0_0_1_0_2_1_00;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new SCPI VISA Instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new SVIs are added.
//  - The Agilent.CommandExpert.ScpiNet drivers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
namespace ABT.TestSpace.SCPI_VISA_Instruments {
    public static class PS_E36234A {
        public const String MODEL = "E36234A";

        public static Boolean IsPS_E36234A(SCPI_VISA_Instrument SVI) { return (SVI.Instrument.GetType() == typeof(AgE36200)); }

        public static void Local(SCPI_VISA_Instrument SVI) { ((AgE36200)SVI.Instrument).SCPI.SYSTem.LOCal.Command(); }

        public static void Remote(SCPI_VISA_Instrument SVI) { ((AgE36200)SVI.Instrument).SCPI.SYSTem.REMote.Command(); }

        public static void RemoteLock(SCPI_VISA_Instrument SVI) { ((AgE36200)SVI.Instrument).SCPI.SYSTem.RWLock.Command(); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            SCPI99.Initialize(SVI);
            ((AgE36200)SVI.Instrument).SCPI.OUTPut.PROTection.CLEar.Command(SCPI.Channels[CHANNELS.C1]);
            ((AgE36200)SVI.Instrument).SCPI.OUTPut.PROTection.CLEar.Command(SCPI.Channels[CHANNELS.C2]);
            ((AgE36200)SVI.Instrument).SCPI.DISPlay.WINDow.TEXT.CLEar.Command();
        }

        public static Double MeasureVDC(SCPI_VISA_Instrument SVI, CHANNELS Channel) {
            ((AgE36200)SVI.Instrument).SCPI.MEASure.SCALar.VOLTage.DC.Query(SCPI.Channels[Channel], out Double[] voltsDC);
            return voltsDC[(Int32)(Channel)];
        }

        public static Double MeasureADC(SCPI_VISA_Instrument SVI, CHANNELS Channel) {
            ((AgE36200)SVI.Instrument).SCPI.MEASure.SCALar.CURRent.DC.Query(SCPI.Channels[Channel], out Double[] ampsDC);
            return ampsDC[(Int32)(Channel)];
        }

        public static OUTPUT GetOutputState(SCPI_VISA_Instrument SVI, CHANNELS Channel) {
            ((AgE36200)SVI.Instrument).SCPI.OUTPut.STATe.Query(SCPI.Channels[Channel], out Boolean[] States);
            return States[(Int32)(Channel)] ? OUTPUT.ON : OUTPUT.off;
        }

        public static Boolean AreSlewRates(SCPI_VISA_Instrument SVI, CHANNELS Channel, Double SlewRateRising, Double SlewRateFalling) { return ((SlewRateRising, SlewRateFalling) == GetSlewRates(SVI, Channel)); }

        public static void SetSlewRates(SCPI_VISA_Instrument SVI, CHANNELS Channel, Double SlewRateRising, Double SlewRateFalling) {
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.SLEW.RISing.IMMediate.Command(SlewRateRising, SCPI.Channels[Channel]);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.SLEW.FALLing.IMMediate.Command(SlewRateFalling, SCPI.Channels[Channel]);
        }

        public static (Double SlewRateRising, Double SlewRateFalling) GetSlewRates(SCPI_VISA_Instrument SVI, CHANNELS Channel) {
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.SLEW.RISing.IMMediate.Query(null, SCPI.Channels[Channel], out Double[] srr);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.SLEW.FALLing.IMMediate.Query(null, SCPI.Channels[Channel], out Double[] srf);
            return (srr[(Int32)Channel], srf[(Int32)Channel]);
        }

        public static void Set(SCPI_VISA_Instrument SVI, OUTPUT State, Double VoltsDC, Double AmpsDC, CHANNELS Channel, SENSE_MODE KelvinSense = SENSE_MODE.INTernal, Double DelaySecondsCurrentProtection = 0, Double DelaySecondsSettling = 0) {
            SetVDC(SVI, VoltsDC, Channel);
            SetVoltageProtection(SVI, VoltsDC * 1.10, Channel);
            SetADC(SVI, AmpsDC, Channel);
            SetCurrentProtectionDelay(SVI, DelaySecondsCurrentProtection, Channel);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.SENSe.SOURce.Command(Enum.GetName(typeof(SENSE_MODE), KelvinSense), SCPI.Channels[Channel]);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.STATe.Command(false, SCPI.Channels[Channel]);
            ((AgE36200)SVI.Instrument).SCPI.OUTPut.STATe.Command(true, SCPI.Channels[Channel]);
            SetOutputState(SVI, State, Channel);
            Thread.Sleep((Int32)(DelaySecondsSettling * 1000));
        }

        public static void SetOutputState(SCPI_VISA_Instrument SVI, OUTPUT State, CHANNELS Channel) { ((AgE36200)SVI.Instrument).SCPI.OUTPut.STATe.Command((State is OUTPUT.ON), SCPI.Channels[Channel]); }

        public static Boolean IsOutputState(SCPI_VISA_Instrument SVI, OUTPUT State, CHANNELS Channel) { return (State == GetOutputState(SVI, Channel)); }

        public static Double GetVDC(SCPI_VISA_Instrument SVI, CHANNELS Channel) {
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query(null, SCPI.Channels[Channel], out Double[] voltsDC);
            return voltsDC[(Int32)Channel];
        }

        public static void SetVDC(SCPI_VISA_Instrument SVI, Double VoltsDC, CHANNELS Channel) {
            String s;
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MINimum", SCPI.Channels[Channel], out Double[] min);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MAXimum", SCPI.Channels[Channel], out Double[] max);
            if ((VoltsDC < min[(Int32)Channel]) || (max[(Int32)Channel] < VoltsDC)) {
                s = $"MINimum/MAXimum Voltage.{Environment.NewLine}"
                + $" - MINimum   :  Voltage={min[(Int32)Channel]} VDC.{Environment.NewLine}"
                + $" - Programmed:  Voltage={VoltsDC} VDC.{Environment.NewLine}"
                + $" - MAXimum   :  Voltage={max[(Int32)Channel]} VDC.";
                throw new InvalidOperationException(SCPI.GetErrorMessage(SVI, s));
            }
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(VoltsDC, SCPI.Channels[Channel]);
        }

        public static Boolean IsVDC(SCPI_VISA_Instrument SVI, Double VoltsDC, CHANNELS Channel, Double Delta) { return SCPI.IsCloseEnough(GetVDC(SVI, Channel), VoltsDC, Delta); }

        public static Double GetADC(SCPI_VISA_Instrument SVI, CHANNELS Channel) {
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query(null, SCPI.Channels[Channel], out Double[] ampsDC);
            return ampsDC[(Int32)Channel];
        }

        public static void SetADC(SCPI_VISA_Instrument SVI, Double AmpsDC, CHANNELS Channel) {
            String s;
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MINimum", SCPI.Channels[Channel], out Double[] min);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MAXimum", SCPI.Channels[Channel], out Double[] max);
            if ((AmpsDC < min[(Int32)Channel]) || (max[(Int32)Channel] < AmpsDC)) {
                s = $"MINimum/MAXimum Current.{Environment.NewLine}"
                + $" - MINimum   :  Current={min[(Int32)Channel]} ADC.{Environment.NewLine}"
                + $" - Programmed:  Current={AmpsDC} ADC.{Environment.NewLine}"
                + $" - MAXimum   :  Current={max[(Int32)Channel]} ADC.";
                throw new InvalidOperationException(SCPI.GetErrorMessage(SVI, s));
            }
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(AmpsDC, SCPI.Channels[Channel]);
        }

        public static Boolean IsADC(SCPI_VISA_Instrument SVI, Double AmpsDC, CHANNELS Channel, Double Delta) { return SCPI.IsCloseEnough(GetADC(SVI, Channel), AmpsDC, Delta); }

        public static Double[] GetCurrentProtectionDelay(SCPI_VISA_Instrument SVI, CHANNELS Channel) {
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query(null, SCPI.Channels[Channel], out Double[] seconds);
            return seconds;
        }

        public static void SetCurrentProtectionDelay(SCPI_VISA_Instrument SVI, Double DelaySeconds, CHANNELS Channel) {
            String s;
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query("MINimum", SCPI.Channels[Channel], out Double[] min);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query("MAXimum", SCPI.Channels[Channel], out Double[] max);
            if ((DelaySeconds < min[(Int32)Channel]) || (max[(Int32)Channel] < DelaySeconds)) {
                s=$"MINimum/MAXimum Current Protection Delay.{Environment.NewLine}"
                + $" - MINimum   :  Delay={min[(Int32)Channel]} seconds.{Environment.NewLine}"
                + $" - Programmed:  Delay={DelaySeconds} seconds.{Environment.NewLine}"
                + $" - MAXimum   :  Delay={max[(Int32)Channel]} seconds.";
                throw new InvalidOperationException(SCPI.GetErrorMessage(SVI, s));
            }
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Command(DelaySeconds, SCPI.Channels[Channel]);
            SetCurrentProtectionState(SVI, OUTPUT.ON, Channel);
        }

        public static Boolean GetCurrentProtectionState(SCPI_VISA_Instrument SVI, CHANNELS Channel) {
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.STATe.Query(SCPI.Channels[Channel], out Boolean[] state);
            return state[(Int32)Channel];
        }

        public static void SetCurrentProtectionState(SCPI_VISA_Instrument SVI, OUTPUT State, CHANNELS Channel) { ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.STATe.Command((State is OUTPUT.ON), SCPI.Channels[Channel]); }

        public static Double GetVoltageProtection(SCPI_VISA_Instrument SVI, CHANNELS Channel) {
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.LEVel.AMPLitude.Query(null, SCPI.Channels[Channel], out Double[] amplitude);
            return amplitude[(Int32)Channel];
        }

        public static void SetVoltageProtection(SCPI_VISA_Instrument SVI, Double VoltsDC, CHANNELS Channel) {
            String s;
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.LEVel.AMPLitude.Query("MINimum", SCPI.Channels[Channel], out Double[] min);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.LEVel.AMPLitude.Query("MAXimum", SCPI.Channels[Channel], out Double[] max);
            if ((VoltsDC < min[(Int32)Channel]) || (max[(Int32)Channel] < VoltsDC)) {
                s = $"MINimum/MAXimum Voltage Protection.{Environment.NewLine}"
                + $" - MINimum   :  Voltage={min[(Int32)Channel]} VDC.{Environment.NewLine}"
                + $" - Programmed:  Voltage={VoltsDC} VDC.{Environment.NewLine}"
                + $" - MAXimum   :  Voltage={max[(Int32)Channel]} VDC.";
                throw new InvalidOperationException(SCPI.GetErrorMessage(SVI, s));
            }
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.LEVel.AMPLitude.Command(VoltsDC, SCPI.Channels[Channel]);
            SetVoltageProtectionState(SVI, OUTPUT.ON, Channel);
        }

        public static void ClearVoltageProtectionTripped(SCPI_VISA_Instrument SVI, CHANNELS Channel) { ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.CLEar.Command(SCPI.Channels[Channel]); }

        public static Boolean GetVoltageProtectionTripped(SCPI_VISA_Instrument SVI, CHANNELS Channel) {
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.TRIPped.Query(SCPI.Channels[Channel], out Int32[] tripped);
            return (tripped[(Int32)Channel] == 1);
        }

        public static Boolean GetVoltageProtectionState(SCPI_VISA_Instrument SVI, CHANNELS Channel) {
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.STATe.Query(SCPI.Channels[Channel], out Boolean state);
            return state;
        }

        public static void SetVoltageProtectionState(SCPI_VISA_Instrument SVI, OUTPUT State, CHANNELS Channel) { ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.STATe.Command((State is OUTPUT.ON), SCPI.Channels[Channel]); }
    }
}