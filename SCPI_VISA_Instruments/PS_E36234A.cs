using System;
using System.Threading;
using Agilent.CommandExpert.ScpiNet.AgE36200_1_0_0_1_0_2_1_00;
using ABT.TestSpace.AppConfig;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new SCPI VISA Instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new SVIs are added.
//  - The Agilent.CommandExpert.ScpiNet drivers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
// TODO: Convert the PS_E36234A class to a Singleton, like the USB_TO_GPIO class?
//  - Realize Singletons often considered "anti-patterns", but handy for objects that can only have 1 instance.
//  - If there are more than one PS_E36234A in the test system, make the PS_E36234A Singleton class a Dictionary of PS_E36234As, rather than just one PS_E36234A.
//  - Each PS_E36234A in the Singleton's Dictionary can be accessed by its enum; PS.S01, PS.S02...PS.Snn, for Power Supply Singletons 01, 02...nn.
// NOTE: Below hopefully "value-added" wrapper methods for some commonly used SCPI commands are conveniences, not necessities.
namespace ABT.TestSpace.SCPI_VISA_Instruments {
    public static class PS_E36234A {
        public const String MODEL = "E36234A";

        public static Boolean IsPS_E36234A(SCPI_VISA_Instrument SVI) { return (SVI.Instrument.GetType() == typeof(AgE36200)); }

        public static void Local(SCPI_VISA_Instrument SVI) { ((AgE36200)SVI.Instrument).SCPI.SYSTem.LOCal.Command(); }

        public static void Remote(SCPI_VISA_Instrument SVI) { ((AgE36200)SVI.Instrument).SCPI.SYSTem.REMote.Command(); }

        public static void RemoteLock(SCPI_VISA_Instrument SVI) { ((AgE36200)SVI.Instrument).SCPI.SYSTem.RWLock.Command(); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            SCPI99.Initialize(SVI);
            ((AgE36200)SVI.Instrument).SCPI.OUTPut.PROTection.CLEar.Command(SCPI.CHANNEL_1ε2);
            ((AgE36200)SVI.Instrument).SCPI.DISPlay.WINDow.TEXT.CLEar.Command();
            RemoteLock(SVI);
        }

        public static Double MeasureVDC(SCPI_VISA_Instrument SVI, CHANNELS Channel) {
            ((AgE36200)SVI.Instrument).SCPI.MEASure.SCALar.VOLTage.DC.Query(ConvertChannel(Channel), out Double[] voltsDC);
            return voltsDC[(Int32)(Channel)];
        }

        public static Double MeasureADC(SCPI_VISA_Instrument SVI, CHANNELS Channel) {
            ((AgE36200)SVI.Instrument).SCPI.MEASure.SCALar.CURRent.DC.Query(ConvertChannel(Channel), out Double[] ampsDC);
            return ampsDC[(Int32)(Channel)];
        }

        public static OUTPUT GetOutputState(SCPI_VISA_Instrument SVI, CHANNELS Channel) {
            ((AgE36200)SVI.Instrument).SCPI.OUTPut.STATe.Query(ConvertChannel(Channel), out Boolean[] States);
            return States[(Int32)(Channel)] ? OUTPUT.ON : OUTPUT.off;
        }

        public static Boolean AreSlewRates(SCPI_VISA_Instrument SVI, CHANNELS Channel, Double SlewRateRising, Double SlewRateFalling) {
            return ((SlewRateRising, SlewRateFalling) == GetSlewRates(SVI, Channel));
        }

        public static void SetSlewRates(SCPI_VISA_Instrument SVI, CHANNELS Channel, Double SlewRateRising, Double SlewRateFalling) {
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.SLEW.RISing.IMMediate.Command(SlewRateRising, ConvertChannel(Channel));
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.SLEW.FALLing.IMMediate.Command(SlewRateFalling, ConvertChannel(Channel));
        }

        public static (Double SlewRateRising, Double SlewRateFalling) GetSlewRates(SCPI_VISA_Instrument SVI, CHANNELS Channel) {
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.SLEW.RISing.IMMediate.Query(null, ConvertChannel(Channel), out Double[] srr);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.SLEW.FALLing.IMMediate.Query(null, ConvertChannel(Channel), out Double[] srf);
            return (srr[(Int32)Channel], srf[(Int32)Channel]);
        }

        public static void Set(SCPI_VISA_Instrument SVI, OUTPUT State, Double VoltsDC, Double AmpsDC, CHANNELS Channel, Double DelayCurrentProtectionSeconds = 0, Double DelayMeasurementSeconds = 0) {
            SetVDC(SVI, VoltsDC, Channel);
            SetADC(SVI, AmpsDC, Channel);
            SetCurrentProtectionDelay(SVI, DelayCurrentProtectionSeconds, Channel);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal", ConvertChannel(Channel));
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.STATe.Command(false, ConvertChannel(Channel));
            ((AgE36200)SVI.Instrument).SCPI.OUTPut.STATe.Command(true, ConvertChannel(Channel));
            SetOutputState(SVI, State, Channel);
            Thread.Sleep((Int32)(DelayMeasurementSeconds * 1000));
        }

        public static void SetOutputState(SCPI_VISA_Instrument SVI, OUTPUT State, CHANNELS Channel) { ((AgE36200)SVI.Instrument).SCPI.OUTPut.STATe.Command((State is OUTPUT.ON), ConvertChannel(Channel)); }

        public static Boolean IsOutputState(SCPI_VISA_Instrument SVI, OUTPUT State, CHANNELS Channel) { return (State == GetOutputState(SVI, Channel)); }

        public static Double GetVDC(SCPI_VISA_Instrument SVI, CHANNELS Channel) {
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query(null, ConvertChannel(Channel), out Double[] voltsDC);
            return voltsDC[(Int32)Channel];
        }

        public static void SetVDC(SCPI_VISA_Instrument SVI, Double VoltsDC, CHANNELS Channel) {
            String s;
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MINimum", ConvertChannel(Channel), out Double[] min);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MAXimum", ConvertChannel(Channel), out Double[] max);
            if ((VoltsDC < min[(Int32)Channel]) || (max[(Int32)Channel] < VoltsDC)) {
                s=$"MINimum/MAXimum Voltage.{Environment.NewLine}"
                + $" - MINimum   :  Voltage={min[(Int32)Channel]} VDC.{Environment.NewLine}"
                + $" - Programmed:  Voltage={VoltsDC} VDC.{Environment.NewLine}"
                + $" - MAXimum   :  Voltage={max[(Int32)Channel]} VDC.";
                throw new InvalidOperationException(SCPI.GetErrorMessage(SVI, s));
            }
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(VoltsDC, ConvertChannel(Channel));
        }

        public static Boolean IsVDC(SCPI_VISA_Instrument SVI, Double VoltsDC, CHANNELS Channel, Double Delta) { return SCPI.IsCloseEnough(GetVDC(SVI, Channel), VoltsDC, Delta); }

        public static Double GetADC(SCPI_VISA_Instrument SVI, CHANNELS Channel) {
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query(null, ConvertChannel(Channel), out Double[] ampsDC);
            return ampsDC[(Int32)Channel];
        }

        public static void SetADC(SCPI_VISA_Instrument SVI, Double AmpsDC, CHANNELS Channel) {
            String s;
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MINimum", ConvertChannel(Channel), out Double[] min);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MAXimum", ConvertChannel(Channel), out Double[] max);
            if ((AmpsDC < min[(Int32)Channel]) || (max[(Int32)Channel] < AmpsDC)) {
                s=$"MINimum/MAXimum Current.{Environment.NewLine}"
                + $" - MINimum   :  Current={min[(Int32)Channel]} ADC.{Environment.NewLine}"
                + $" - Programmed:  Current={AmpsDC} ADC.{Environment.NewLine}"
                + $" - MAXimum   :  Current={max[(Int32)Channel]} ADC.";
                throw new InvalidOperationException(SCPI.GetErrorMessage(SVI, s));
            }
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(AmpsDC, ConvertChannel(Channel));
        }

        public static Boolean IsADC(SCPI_VISA_Instrument SVI, Double AmpsDC, CHANNELS Channel, Double Delta) { return SCPI.IsCloseEnough(GetADC(SVI, Channel), AmpsDC, Delta); }

        public static Double GetCurrentProtectionDelay(SCPI_VISA_Instrument SVI, CHANNELS Channel) {
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query(null, ConvertChannel(Channel), out Double[] seconds);
            return seconds[(Int32)Channel];
        }

        public static void SetCurrentProtectionDelay(SCPI_VISA_Instrument SVI, Double DelaySeconds, CHANNELS Channel) {
            String s;
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query("MINimum", ConvertChannel(Channel), out Double[] min);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query("MAXimum", ConvertChannel(Channel), out Double[] max);
            if ((DelaySeconds < min[(Int32)Channel]) || (max[(Int32)Channel] < DelaySeconds)) {
                s=$"MINimum/MAXimum Current Protection Delay.{Environment.NewLine}"
                + $" - MINimum   :  Delay={min[(Int32)Channel]} seconds.{Environment.NewLine}"
                + $" - Programmed:  Delay={DelaySeconds} seconds.{Environment.NewLine}"
                + $" - MAXimum   :  Delay={max[(Int32)Channel]} seconds.";
                throw new InvalidOperationException(SCPI.GetErrorMessage(SVI, s));
            }
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Command(DelaySeconds, ConvertChannel(Channel));
        }

        public static Boolean GetCurrentProtectionState(SCPI_VISA_Instrument SVI, CHANNELS Channel) {
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.STATe.Query(ConvertChannel(Channel), out Boolean[] state);
            return state[(Int32)Channel];
        }

        public static void SetCurrentProtectionState(SCPI_VISA_Instrument SVI, OUTPUT State, String Channel) { ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.STATe.Command((State is OUTPUT.ON), Channel); }

        private static String ConvertChannel(CHANNELS Channel) { return Channel == CHANNELS.C1 ? SCPI.CHANNEL_1 : SCPI.CHANNEL_2; }
    }
}