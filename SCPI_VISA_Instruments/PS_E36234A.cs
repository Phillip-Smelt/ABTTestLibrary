using System;
using System.Collections.Generic;
using System.Threading;
using Agilent.CommandExpert.ScpiNet.AgE36200_1_0_0_1_0_2_1_00;
using TestLibrary.AppConfig;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new SCPI VISA Instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new SVIs are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
// NOTE: Below hopefully "value-added" wrapper methods for some commonly used AgE36200 commands are conveniences, not necessities.
// NOTE: Will never fully implement wrapper methods for the complete set of AgE36200 commands, just some of the most commonly used ones.
// - In general, TestLibrary's InterfaceAdapters, Logging, SCPI_VISA_Instruments & Switching namespaces exist partly to eliminate
//   the need to reference TestLibrary's various DLLs directly from TestProgram client apps.
// - As long as suitable wrapper methods exists in PS_E36234A, needn't directly reference AgE36200_1_0_0_1_0_2_1_00
//   from TestProgram client apps, as referencing TestLibrary suffices.
namespace TestLibrary.SCPI_VISA_Instruments {
    public static class PS_E36234A {
        public const String MODEL = "E36234A";

        public static Boolean IsPS_E36234A(SCPI_VISA_Instrument SVI) { return (SVI.Instrument.GetType() == typeof(AgE36200)); }

        public static void Local(SCPI_VISA_Instrument SVI) { ((AgE36200)SVI.Instrument).SCPI.SYSTem.LOCal.Command(); }

        public static void Remote(SCPI_VISA_Instrument SVI) { ((AgE36200)SVI.Instrument).SCPI.SYSTem.REMote.Command(); }

        public static void RemoteLock(SCPI_VISA_Instrument SVI) { ((AgE36200)SVI.Instrument).SCPI.SYSTem.RWLock.Command(); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            SCPI_VISA.Initialize(SVI);
            ((AgE36200)SVI.Instrument).SCPI.OUTPut.PROTection.CLEar.Command(SCPI_VISA.CHANNEL_1_2);
            ((AgE36200)SVI.Instrument).SCPI.DISPlay.WINDow.TEXT.CLEar.Command();
            RemoteLock(SVI);
        }

        public static void On(SCPI_VISA_Instrument SVI, Double VoltsDC, Double AmpsDC, String Channel, Double DelayCurrentProtectionSeconds = 0, Double DelayMeasurementSeconds = 0) {
            SetVDC(SVI, VoltsDC, Channel);
            SetADC(SVI, AmpsDC, Channel);
            SetCurrentProtectionDelay(SVI, DelayCurrentProtectionSeconds, Channel);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal", Channel);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.STATe.Command(false, Channel);
            ((AgE36200)SVI.Instrument).SCPI.OUTPut.STATe.Command(true, Channel);
            Thread.Sleep((Int32)(DelayMeasurementSeconds * 1000));
        }

        public static Double MeasureVDC(SCPI_VISA_Instrument SVI, String Channel) {
            Int32 iChannel = ConvertChannel(SVI, Channel);
            ((AgE36200)SVI.Instrument).SCPI.MEASure.SCALar.VOLTage.DC.Query(Channel, out Double[] voltsDC);
            return voltsDC[iChannel];
        }

        public static Double MeasureADC(SCPI_VISA_Instrument SVI, String Channel) {
            Int32 iChannel = ConvertChannel(SVI, Channel);
            ((AgE36200)SVI.Instrument).SCPI.MEASure.SCALar.CURRent.DC.Query(Channel, out Double[] ampsDC);
            return ampsDC[iChannel];
        }

        public static BINARY GetOutputState(SCPI_VISA_Instrument SVI, String Channel) {
            ((AgE36200)SVI.Instrument).SCPI.OUTPut.STATe.Query(Channel, out Boolean[] States);
            if (States[ConvertChannel(SVI, Channel)]) return BINARY.On;
            else return BINARY.Off;
        }

        public static void SetOutputState(SCPI_VISA_Instrument SVI, BINARY State, String Channel) { ((AgE36200)SVI.Instrument).SCPI.OUTPut.STATe.Command((State is BINARY.On), Channel); }

        public static Boolean IsOutputState(SCPI_VISA_Instrument SVI, BINARY State, String Channel) { return (State == GetOutputState(SVI, Channel)); }

        public static Double GetVDC(SCPI_VISA_Instrument SVI, String Channel) {
            Int32 iChannel = ConvertChannel(SVI, Channel);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query(null, Channel, out Double[] voltsDC);
            return voltsDC[iChannel];
        }

        public static void SetVDC(SCPI_VISA_Instrument SVI, Double VoltsDC, String Channel) {
            Int32 iChannel = ConvertChannel(SVI, Channel);
            String s;
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MINimum", Channel, out Double[] min);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MAXimum", Channel, out Double[] max);
            if ((VoltsDC < min[iChannel]) || (VoltsDC > max[iChannel])) {
                s = $"< MINimum/MAXimum Voltage.{Environment.NewLine}"
                + $" - MINimum   :  Voltage={min[iChannel]} VDC.{Environment.NewLine}"
                + $" - Programmed:  Voltage={VoltsDC} VDC.{Environment.NewLine}"
                + $" - MAXimum   :  Voltage={max[iChannel]} VDC.";
                throw new InvalidOperationException(SCPI_VISA.GetErrorMessage(SVI, s));
            }
            ((AgE36200)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(VoltsDC, Channel);
        }

        public static Boolean IsVDC(SCPI_VISA_Instrument SVI, Double VoltsDC, String Channel, Double Delta) { return SCPI_VISA.IsCloseEnough(GetVDC(SVI, Channel), VoltsDC, Delta); }

        public static Double GetADC(SCPI_VISA_Instrument SVI, String Channel) {
            Int32 iChannel = ConvertChannel(SVI, Channel);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query(null, Channel, out Double[] ampsDC);
            return ampsDC[iChannel];
        }

        public static void SetADC(SCPI_VISA_Instrument SVI, Double AmpsDC, String Channel) {
            Int32 iChannel = ConvertChannel(SVI, Channel);
            String s;
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MINimum", Channel, out Double[] min);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MAXimum", Channel, out Double[] max);
            if ((AmpsDC < min[iChannel]) || (AmpsDC > max[iChannel])) {
                s = $"> MINimum/MAXimum Current.{Environment.NewLine}"
                + $" - MINimum   :  Current={min[iChannel]} ADC.{Environment.NewLine}"
                + $" - Programmed:  Current={AmpsDC} ADC.{Environment.NewLine}"
                + $" - MAXimum   :  Current={max[iChannel]} ADC.";
                throw new InvalidOperationException(SCPI_VISA.GetErrorMessage(SVI, s));
            }
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(AmpsDC, Channel);
        }

        public static Boolean IsADC(SCPI_VISA_Instrument SVI, Double AmpsDC, String Channel, Double Delta) { return SCPI_VISA.IsCloseEnough(GetADC(SVI, Channel), AmpsDC, Delta); }

        public static Double GetCurrentProtectionDelay(SCPI_VISA_Instrument SVI, String Channel) {
            Int32 iChannel = ConvertChannel(SVI, Channel);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query(null, Channel, out Double[] seconds);
            return seconds[iChannel];
        }

        public static void SetCurrentProtectionDelay(SCPI_VISA_Instrument SVI, Double DelaySeconds, String Channel) {
            Int32 iChannel = ConvertChannel(SVI, Channel);
            String s;
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query("MINimum", Channel, out Double[] min);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query("MAXimum", Channel, out Double[] max);
            if ((DelaySeconds < min[iChannel]) || (DelaySeconds > max[iChannel])) {
                s = $"> MINimum/MAXimum Current Protection Delay.{Environment.NewLine}"
                + $" - MINimum   :  Delay={min[iChannel]} seconds.{Environment.NewLine}"
                + $" - Programmed:  Delay={DelaySeconds} seconds.{Environment.NewLine}"
                + $" - MAXimum   :  Delay={max[iChannel]} seconds.";
                throw new InvalidOperationException(SCPI_VISA.GetErrorMessage(SVI, s));
            }
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Command(DelaySeconds, Channel);
        }

        public static Boolean GetCurrentProtectionState(SCPI_VISA_Instrument SVI, String Channel) {
            Int32 iChannel = ConvertChannel(SVI, Channel);
            ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.STATe.Query(Channel, out Boolean[] state);
            return state[iChannel];
        }

        public static void SetCurrentProtectionState(SCPI_VISA_Instrument SVI, BINARY State, String Channel) { ((AgE36200)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.STATe.Command((State is BINARY.On), Channel); }

        private static Int32 ConvertChannel(SCPI_VISA_Instrument SVI, String Channel) {
            if (String.Equals(Channel, SCPI_VISA.CHANNEL_1)) return 0;
            else if (String.Equals(Channel, SCPI_VISA.CHANNEL_2)) return 1;
            else throw new InvalidOperationException(SCPI_VISA.GetErrorMessage(SVI, $"Invalid Channel '{Channel}'"));
        }
    }
}