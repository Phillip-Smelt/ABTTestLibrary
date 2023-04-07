using System;
using System.Threading;
using Agilent.CommandExpert.ScpiNet.AgE3610XB_1_0_0_1_00;
using TestLibrary.AppConfig;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new SCPI VISA Instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new SVIs are added.
//  - The Agilent.CommandExpert.ScpiNet drivers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
// NOTE: Below hopefully "value-added" wrapper methods for some commonly used SCPI commands are conveniences, not necessities.
namespace TestLibrary.SCPI_VISA_Instruments {
    // NOTE: SCPI99's IDN Identity Query won't return "E3610xB" in it's Identity string.  It only returns "E36103B", "E36105B" etc.
    public static class PS_E36103B { public const String MODEL = "E36103B"; } // PS_E36103B needed only in class TestLibrary.AppConfig.SCPI_VISA_Instrument.

    public static class PS_E36105B { public const String MODEL = "E36105B"; } // PS_E36105B needed only in class TestLibrary.AppConfig.SCPI_VISA_Instrument.

    public static class PS_E3610xB {

        public static Boolean IsPS_E3610xB(SCPI_VISA_Instrument SVI) { return (SVI.Instrument.GetType() == typeof(AgE3610XB)); }

        public static void Local(SCPI_VISA_Instrument SVI) { ((AgE3610XB)SVI.Instrument).SCPI.SYSTem.LOCal.Command(); }

        public static void Remote(SCPI_VISA_Instrument SVI) { ((AgE3610XB)SVI.Instrument).SCPI.SYSTem.REMote.Command(); }

        public static void RemoteLock(SCPI_VISA_Instrument SVI) { ((AgE3610XB)SVI.Instrument).SCPI.SYSTem.RWLock.Command(); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            SCPI_VISA.Initialize(SVI);
            ((AgE3610XB)SVI.Instrument).SCPI.OUTPut.PROTection.CLEar.Command();
            ((AgE3610XB)SVI.Instrument).SCPI.DISPlay.WINDow.TEXT.CLEar.Command();
            RemoteLock(SVI);
        }

        public static Double MeasureVDC(SCPI_VISA_Instrument SVI) {
            ((AgE3610XB)SVI.Instrument).SCPI.MEASure.VOLTage.DC.Query(out Double voltsDC);
            return voltsDC;
        }

        public static Double MeasureADC(SCPI_VISA_Instrument SVI) {
            ((AgE3610XB)SVI.Instrument).SCPI.MEASure.CURRent.DC.Query(out Double ampsDC);
            return ampsDC;
        }

        public static STATE GetOutputState(SCPI_VISA_Instrument SVI) {
            ((AgE3610XB)SVI.Instrument).SCPI.OUTPut.STATe.Query(out Boolean State);
            if (State) return STATE.ON;
            else return STATE.off;
        }

        public static void SetOutputState(SCPI_VISA_Instrument SVI, STATE State) { ((AgE3610XB)SVI.Instrument).SCPI.OUTPut.STATe.Command((State is STATE.ON)); }

        // TODO:
        public static void SetOutputState(SCPI_VISA_Instrument SVI, STATE State, Double VoltsDC, Double AmpsDC, Double DelayCurrentProtectionSeconds = 0, Double DelayMeasurementSeconds = 0) {
            SetVDC(SVI, VoltsDC);
            SetADC(SVI, AmpsDC);
            SetCurrentProtectionDelay(SVI, DelayCurrentProtectionSeconds);
            // TODO: ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("INTernal");
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.STATe.Command(false);
            SetOutputState(SVI, State);
            Thread.Sleep((Int32)(DelayMeasurementSeconds * 1000));
        }

        public static Boolean IsOutputState(SCPI_VISA_Instrument SVI, STATE State) { return (State == GetOutputState(SVI)); }

        public static Double GetVDC(SCPI_VISA_Instrument SVI) {
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query(null, out Double voltsDC);
            return voltsDC;
        }

        public static void SetVDC(SCPI_VISA_Instrument SVI, Double VoltsDC) {
            String s;
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MINimum", out Double min);
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MAXimum", out Double max);
            if((VoltsDC < min) || (max < VoltsDC)) {
                s=$"MINimum/MAXimum Voltage.{Environment.NewLine}"
                + $" - MINimum   :  Voltage={min} VDC.{Environment.NewLine}"
                + $" - Programmed:  Voltage={VoltsDC} VDC.{Environment.NewLine}"
                + $" - MAXimum   :  Voltage={max} VDC.";
                throw new InvalidOperationException(SCPI_VISA.GetErrorMessage(SVI, s));
            }
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(VoltsDC);
        }

        public static Boolean IsVDC(SCPI_VISA_Instrument SVI, Double VoltsDC, Double Delta) { return SCPI_VISA.IsCloseEnough(GetVDC(SVI), VoltsDC, Delta); }

        public static Double GetADC(SCPI_VISA_Instrument SVI) {
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query(null, out Double ampsDC);
            return ampsDC;
        }

        public static void SetADC(SCPI_VISA_Instrument SVI, Double AmpsDC) {
            String s;
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MINimum", out Double min);
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MAXimum", out Double max);
            if ((AmpsDC < min) || (max < AmpsDC)) {
                s=$"MINimum/MAXimum Current.{Environment.NewLine}"
                + $" - MINimum   :  Current={min} ADC.{Environment.NewLine}"
                + $" - Programmed:  Current={AmpsDC} ADC.{Environment.NewLine}"
                + $" - MAXimum   :  Current={max} ADC.";
                throw new InvalidOperationException(SCPI_VISA.GetErrorMessage(SVI, s));
            }
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(AmpsDC);
        }

        public static Boolean IsADC(SCPI_VISA_Instrument SVI, Double AmpsDC, Double Delta) { return SCPI_VISA.IsCloseEnough(GetADC(SVI), AmpsDC, Delta); }

        public static Double GetCurrentProtectionDelay(SCPI_VISA_Instrument SVI) {
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query(null, out Double seconds);
            return seconds;
        }

        public static void SetCurrentProtectionDelay(SCPI_VISA_Instrument SVI, Double DelaySeconds) {
            String s;
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query("MINimum", out Double min);
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query("MAXimum", out Double max);
            if ((DelaySeconds < min) || max < (DelaySeconds)) {
                s=$"MINimum/MAXimum Current Protection Delay.{Environment.NewLine}"
                + $" - MINimum   :  Delay={min} seconds.{Environment.NewLine}"
                + $" - Programmed:  Delay={DelaySeconds} seconds.{Environment.NewLine}"
                + $" - MAXimum   :  Delay={max} seconds.";
                throw new InvalidOperationException(SCPI_VISA.GetErrorMessage(SVI, s));
            }
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Command(DelaySeconds);
        }

        public static Boolean GetCurrentProtectionState(SCPI_VISA_Instrument SVI) {
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.STATe.Query(out Boolean state);
            return state;
        }

        public static void SetCurrentProtectionState(SCPI_VISA_Instrument SVI, STATE State) { ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.STATe.Command((State is STATE.ON)); }
    }
}