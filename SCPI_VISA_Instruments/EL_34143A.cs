using System;
using Agilent.CommandExpert.ScpiNet.AgEL30000_1_2_5_1_0_6_17_114;
using static ABT.TestSpace.TestExec.SCPI_VISA_Instruments.Keysight;
// All Agilent.CommandExpert.ScpiNet drivers are procured by adding new SCPI VISA Instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new SVIs are added.
//  - The Agilent.CommandExpert.ScpiNet drivers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Strenuously recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
// https://www.keysight.com/us/en/search.html/command+expert
//
namespace ABT.TestSpace.TestExec.SCPI_VISA_Instruments {
    public enum LOAD_MODE { CURR=0, POW=1, RES=2, VOLT=3 }
    public enum LOAD_MEASURE { CURR=0, POW=1, VOLT=3 }

    public static class EL_34143A {
        public const String MODEL = "EL34143A";
 
        public static Boolean IsEL_34143A(SCPI_VISA_Instrument SVI) { return (SVI.Instrument.GetType() == typeof(AgEL30000)); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            SCPI99.Initialize(SVI);
            ((AgEL30000)SVI.Instrument).SCPI.OUTPut.PROTection.CLEar.Command();
            ((AgEL30000)SVI.Instrument).SCPI.DISPlay.WINDow.TEXT.CLEar.Command();
        }

        public static void Local(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instrument).SCPI.SYSTem.LOCal.Command(); }

        public static Boolean Is(SCPI_VISA_Instrument SVI, Double LoadValue, LOAD_MODE LoadMode) {
            if (!Is(SVI, LoadMode)) return false;
            Double delta = 0.01;
            switch (LoadMode) {
                case LOAD_MODE.CURR:
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query(null, null, out Double ampsDC);
                    return SCPI99.IsCloseEnough(LoadValue, ampsDC, delta);
                case LOAD_MODE.POW:
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Query(null, null, out Double[] watts);
                    return SCPI99.IsCloseEnough(LoadValue, watts[0], delta);
                case LOAD_MODE.RES:
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Query(null, null, out Double[] ohms);
                    return SCPI99.IsCloseEnough(LoadValue, ohms[0], delta);
                case LOAD_MODE.VOLT:
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query(null, null, out Double[] voltsDC);
                    return SCPI99.IsCloseEnough(LoadValue, voltsDC[0], delta);
                default:
                    throw new NotImplementedException(TestExecutive.NotImplementedMessageEnum(typeof(LOAD_MODE)));
            }
        }

        public static Boolean Is(SCPI_VISA_Instrument SVI, LOAD_MODE LoadMode) { return LoadMode == Get(SVI); }

        public static LOAD_MODE Get(SCPI_VISA_Instrument SVI) {
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Query(null, out String LoadMode);
            return (LOAD_MODE)Enum.Parse(typeof(LOAD_MODE), LoadMode); 
        }

        public static Double Get(SCPI_VISA_Instrument SVI, LOAD_MEASURE LoadMeasure) {
            switch(LoadMeasure) {
                case LOAD_MEASURE.CURR:
                    ((AgEL30000)SVI.Instrument).SCPI.MEASure.SCALar.CURRent.DC.Query(null, out Double[] ampsDC);
                    return ampsDC[0];
                case LOAD_MEASURE.POW:
                    ((AgEL30000)SVI.Instrument).SCPI.MEASure.SCALar.POWer.DC.Query(null, out Double[] watts);
                    return watts[0];
                case LOAD_MEASURE.VOLT:
                    ((AgEL30000)SVI.Instrument).SCPI.MEASure.SCALar.VOLTage.DC.Query(null, out Double[] voltsDC);
                    return voltsDC[0];
                default:
                    throw new NotImplementedException(TestExecutive.NotImplementedMessageEnum(typeof(LOAD_MEASURE)));
            }
        }

        public static void OutputStateSet(SCPI_VISA_Instrument SVI, OUTPUT State) { ((AgEL30000)SVI.Instrument).SCPI.OUTPut.STATe.Command(State is OUTPUT.ON, null); }

        public static void Remote(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instrument).SCPI.SYSTem.REMote.Command(); }

        public static void RemoteLock(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instrument).SCPI.SYSTem.RWLock.Command(); }

        public static void Set(SCPI_VISA_Instrument SVI, OUTPUT State, Double LoadValue, LOAD_MODE LoadMode, SENSE_MODE KelvinSense = SENSE_MODE.INTernal) {
            Set(SVI, LoadValue, LoadMode, KelvinSense);
            OutputStateSet(SVI, State);
        }

        public static void Set(SCPI_VISA_Instrument SVI, Double LoadValue, LOAD_MODE LoadMode, SENSE_MODE KelvinSense = SENSE_MODE.INTernal) {
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.SENSe.SOURce.Command(Enum.GetName(typeof(SENSE_MODE), KelvinSense));
            // Despite being part of VOLTage sub-system, SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal"/"INTernal") enables/disables 4-wire Kelvin sensing for all Kelvin capable loads.
            Double min, max;
            Double[] min2, max2;
            String LoadType = Enum.GetName(typeof(LOAD_MODE), LoadMode);
            switch (LoadMode) {
                case LOAD_MODE.CURR:
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Command("CURRent", null);
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(LoadValue, null);
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.RANGe.Command(LoadValue, null);
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query(MINimum, null, out min);
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query(MAXimum, null, out max);
                    SCPI99.ValueValidate(SVI, min, LoadValue, max, LoadType);
                    // TODO: ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.STATe.Command(false, null);
                    break;
                case LOAD_MODE.POW:
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Command("POWer", null);
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Command(LoadValue, null);
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.RANGe.Command(LoadValue, null);
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Query(MINimum, null, out min2);
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Query(MAXimum, null, out max2);
                    SCPI99.ValueValidate(SVI, min2[0], LoadValue, max2[0], LoadType);
                    // TODO: ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.PROTection.STATe.Command(false, null);
                    break;
                case LOAD_MODE.RES:
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Command("RESistance", null);
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Command(LoadValue, null);
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.RESistance.RANGe.Command(LoadValue, null);
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Query(MINimum, null, out min2);
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Query(MAXimum, null, out max2);
                    SCPI99.ValueValidate(SVI, min2[0], LoadValue, max2[0], LoadType);
                    break;
                case LOAD_MODE.VOLT:
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Command("VOLTage", null);
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(LoadValue, null);
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.RANGe.Command(LoadValue, null);
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query(MINimum, null, out min2);
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query(MAXimum, null, out max2);
                    SCPI99.ValueValidate(SVI, min2[0], LoadValue, max2[0], LoadType);
                    break;
                default:
                    throw new NotImplementedException(TestExecutive.NotImplementedMessageEnum(typeof(LOAD_MODE)));
            }
        }
    }
}