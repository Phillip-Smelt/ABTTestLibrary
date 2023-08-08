using System;
using System.Threading;
using Agilent.CommandExpert.ScpiNet.AgE3610XB_1_0_0_1_00;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new SCPI VISA Instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new SVIs are added.
//  - The Agilent.CommandExpert.ScpiNet drivers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
// NOTE: Below hopefully "value-added" wrapper methods for some commonly used SCPI commands are conveniences, not necessities.
namespace ABT.TestSpace.TestExec.SCPI_VISA_Instruments {
    public static class PS_E36103B { public const String MODEL = "E36103B"; } // PS_E36103B needed only in class TestExecutive.AppConfig.SCPI_VISA_Instrument.

    public static class PS_E36105B { public const String MODEL = "E36105B"; } // PS_E36105B needed only in class TestExecutive.AppConfig.SCPI_VISA_Instrument.

    public static class PS_E3610xB {
        public static Boolean IsPS_E3610xB(SCPI_VISA_Instrument SVI) { return (SVI.Instrument.GetType() == typeof(AgE3610XB)); }

        public static void Local(SCPI_VISA_Instrument SVI) { ((AgE3610XB)SVI.Instrument).SCPI.SYSTem.LOCal.Command(); }

        public static void Remote(SCPI_VISA_Instrument SVI) { ((AgE3610XB)SVI.Instrument).SCPI.SYSTem.REMote.Command(); }

        public static void RemoteLock(SCPI_VISA_Instrument SVI) { ((AgE3610XB)SVI.Instrument).SCPI.SYSTem.RWLock.Command(); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            SCPI99.Initialize(SVI);
            ((AgE3610XB)SVI.Instrument).SCPI.OUTPut.PROTection.CLEar.Command();
            ((AgE3610XB)SVI.Instrument).SCPI.DISPlay.WINDow.TEXT.CLEar.Command();
        }

        public static Double MeasureVDC(SCPI_VISA_Instrument SVI) {
            ((AgE3610XB)SVI.Instrument).SCPI.MEASure.VOLTage.DC.Query(out Double voltsDC);
            return voltsDC;
        }

        public static Double MeasureADC(SCPI_VISA_Instrument SVI) {
            ((AgE3610XB)SVI.Instrument).SCPI.MEASure.CURRent.DC.Query(out Double ampsDC);
            return ampsDC;
        }

        public static void Set(SCPI_VISA_Instrument SVI, OUTPUT State, Double VoltsDC, Double AmpsDC, SENSE_MODE KelvinSense = SENSE_MODE.INTernal, Double DelaySecondsCurrentProtection = 0, Double DelaySecondsSettling = 0) {
            SetVDC(SVI, VoltsDC);
            SetVoltageProtection(SVI, VoltsDC * 1.10);
            SetADC(SVI, AmpsDC);
            SetCurrentProtectionDelay(SVI, DelaySecondsCurrentProtection);
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.SENSe.SOURce.Command(Enum.GetName(typeof(SENSE_MODE), KelvinSense));
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.STATe.Command(false);
            SCPI.SetOutputState(SVI, State);
            Thread.Sleep((Int32)(DelaySecondsSettling * 1000));
        }

        public static Double GetVDC(SCPI_VISA_Instrument SVI) {
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query(null, out Double voltsDC);
            return voltsDC;
        }

        public static void SetVDC(SCPI_VISA_Instrument SVI, Double VoltsDC) {
            String s;
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query(SCPI.MINimum, out Double min);
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query(SCPI.MAXimum, out Double max);
            if((VoltsDC < min) || (max < VoltsDC)) {
                s=$"{SCPI.MINimum}/{SCPI.MAXimum} Voltage.{Environment.NewLine}"
                + $" - {SCPI.MINimum}   :  Voltage={min} VDC.{Environment.NewLine}"
                + $" - Programmed:  Voltage={VoltsDC} VDC.{Environment.NewLine}"
                + $" - {SCPI.MAXimum}   :  Voltage={max} VDC.";
                throw new InvalidOperationException(SCPI.GetErrorMessage(SVI, s));
            }
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(VoltsDC);
        }

        public static Boolean IsVDC(SCPI_VISA_Instrument SVI, Double VoltsDC, Double Delta) { return SCPI.IsCloseEnough(GetVDC(SVI), VoltsDC, Delta); }

        public static Double GetADC(SCPI_VISA_Instrument SVI) {
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query(null, out Double ampsDC);
            return ampsDC;
        }

        public static void SetADC(SCPI_VISA_Instrument SVI, Double AmpsDC) {
            String s;
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query(SCPI.MINimum, out Double min);
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query(SCPI.MAXimum, out Double max);
            if ((AmpsDC < min) || (max < AmpsDC)) {
                s=$"{SCPI.MINimum}/{SCPI.MAXimum} Current.{Environment.NewLine}"
                + $" - {SCPI.MINimum}   :  Current={min} ADC.{Environment.NewLine}"
                + $" - Programmed:  Current={AmpsDC} ADC.{Environment.NewLine}"
                + $" - {SCPI.MAXimum}   :  Current={max} ADC.";
                throw new InvalidOperationException(SCPI.GetErrorMessage(SVI, s));
            }
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(AmpsDC);
        }

        public static Boolean IsADC(SCPI_VISA_Instrument SVI, Double AmpsDC, Double Delta) { return SCPI.IsCloseEnough(GetADC(SVI), AmpsDC, Delta); }

        public static Double GetCurrentProtectionDelay(SCPI_VISA_Instrument SVI) {
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query(null, out Double seconds);
            return seconds;
        }

        public static void SetCurrentProtectionDelay(SCPI_VISA_Instrument SVI, Double DelaySeconds) {
            String s;
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query(SCPI.MINimum, out Double min);
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query(SCPI.MAXimum, out Double max);
            if ((DelaySeconds < min) || max < (DelaySeconds)) {
                s=$"{SCPI.MINimum}/{SCPI.MAXimum} Current Protection Delay.{Environment.NewLine}"
                + $" - {SCPI.MINimum}   :  Delay={min} seconds.{Environment.NewLine}"
                + $" - Programmed:  Delay={DelaySeconds} seconds.{Environment.NewLine}"
                + $" - {SCPI.MAXimum}    :  Delay={max} seconds.";
                throw new InvalidOperationException(SCPI.GetErrorMessage(SVI, s));
            }
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Command(DelaySeconds);
            SetCurrentProtectionState(SVI, OUTPUT.ON);
        }

        public static Boolean GetCurrentProtectionState(SCPI_VISA_Instrument SVI) {
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.STATe.Query(out Boolean state);
            return state;
        }

        public static void SetCurrentProtectionState(SCPI_VISA_Instrument SVI, OUTPUT State) { ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.STATe.Command((State is OUTPUT.ON)); }

        public static Double GetVoltageProtection(SCPI_VISA_Instrument SVI) {
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.LEVel.Query(null, out Double amplitude);
            return amplitude;
        }

        public static void SetVoltageProtection(SCPI_VISA_Instrument SVI, Double VoltsDC) {
            String s;
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.LEVel.Query(SCPI.MINimum, out Double min);
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.LEVel.Query(SCPI.MAXimum, out Double max);
            if (VoltsDC < min || max < VoltsDC) {
                s=$"{SCPI.MINimum}/{SCPI.MAXimum} Voltage Protection.{Environment.NewLine}"
                + $" - {SCPI.MINimum}    :  Voltage={min} VDC.{Environment.NewLine}"
                + $" - Programmed:  Voltage={VoltsDC} VDC.{Environment.NewLine}"
                + $" - {SCPI.MAXimum}    :  Voltage={max} VDC.";
                throw new InvalidOperationException(SCPI.GetErrorMessage(SVI, s));
            }
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.LEVel.Command(VoltsDC);
            SetVoltageProtectionState(SVI, OUTPUT.ON);
        }

        public static void ClearVoltageProtectionTripped(SCPI_VISA_Instrument SVI) { ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.CLEar.Command(); }

        public static Boolean GetVoltageProtectionTripped(SCPI_VISA_Instrument SVI) {
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.TRIPped.Query(out Boolean tripped);
            return tripped;
        }

        public static Boolean GetVoltageProtectionState(SCPI_VISA_Instrument SVI) {
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.STATe.Query(out Boolean state);
            return state;
        }

        public static void SetVoltageProtectionState(SCPI_VISA_Instrument SVI, OUTPUT State) { ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.STATe.Command(State is OUTPUT.ON); }
    }
}