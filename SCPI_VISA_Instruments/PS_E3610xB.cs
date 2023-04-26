using System;
using System.Threading;
using Agilent.CommandExpert.ScpiNet.AgE3610XB_1_0_0_1_00;
using ABT.TestSpace.AppConfig;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new SCPI VISA Instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new SVIs are added.
//  - The Agilent.CommandExpert.ScpiNet drivers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
// NOTE: Below hopefully "value-added" wrapper methods for some commonly used SCPI commands are conveniences, not necessities.
namespace ABT.TestSpace.SCPI_VISA_Instruments {
    // TODO: Convert the PS_E3610xB class to a Singleton, like the USB_TO_GPIO class?
    //  - Realize Singletons often considered "anti-patterns", but handy for objects that can only have 1 instance.
    //  - If there are more than one PS_E3610xB in the test system, make the PS_E3610xB Singleton class a Dictionary of PS_E3610xBs, rather than just one PS_E3610xB.
    //  - Each PS_E3610xB in the Singleton's Dictionary can be accessed by its enum; PS.S01, PS.S02...PS.Snn, for Power Supply Singletons 01, 02...nn.
    // NOTE: SCPI99's IDN Identity Query won't return "E3610xB" in it's Identity string.  It only returns "E36103B", "E36105B" etc.
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

        public static void Set(SCPI_VISA_Instrument SVI, OUTPUT State, Double VoltsDC, Double AmpsDC, Double DelayCurrentProtectionSeconds = 0, Double DelayMeasurementSeconds = 0) {
            SetVDC(SVI, VoltsDC);
            SetADC(SVI, AmpsDC);
            SetCurrentProtectionDelay(SVI, DelayCurrentProtectionSeconds);
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.VOLTage.PROTection.STATe.Command(false);
            SCPI.SetOutputState(SVI, State);
            Thread.Sleep((Int32)(DelayMeasurementSeconds * 1000));
        }

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
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MINimum", out Double min);
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MAXimum", out Double max);
            if ((AmpsDC < min) || (max < AmpsDC)) {
                s=$"MINimum/MAXimum Current.{Environment.NewLine}"
                + $" - MINimum   :  Current={min} ADC.{Environment.NewLine}"
                + $" - Programmed:  Current={AmpsDC} ADC.{Environment.NewLine}"
                + $" - MAXimum   :  Current={max} ADC.";
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
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query("MINimum", out Double min);
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Query("MAXimum", out Double max);
            if ((DelaySeconds < min) || max < (DelaySeconds)) {
                s=$"MINimum/MAXimum Current Protection Delay.{Environment.NewLine}"
                + $" - MINimum   :  Delay={min} seconds.{Environment.NewLine}"
                + $" - Programmed:  Delay={DelaySeconds} seconds.{Environment.NewLine}"
                + $" - MAXimum   :  Delay={max} seconds.";
                throw new InvalidOperationException(SCPI.GetErrorMessage(SVI, s));
            }
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.DELay.TIME.Command(DelaySeconds);
        }

        public static Boolean GetCurrentProtectionState(SCPI_VISA_Instrument SVI) {
            ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.STATe.Query(out Boolean state);
            return state;
        }

        public static void SetCurrentProtectionState(SCPI_VISA_Instrument SVI, OUTPUT State) { ((AgE3610XB)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.STATe.Command((State is OUTPUT.ON)); }
    }
}