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
    // LOAD_MODE represents the canonical set of EL34143A electrical loads; current, power, resistance & voltage, in alphabetical & numerical order.
    public enum LOAD_UNITS { AMPS_DC=0, WATTS=1, OHMS=2, VOLTS_DC=3 }
    // LOAD_UNITS is a syntactic sugar representation of LOAD_MODE substituting Système International Units for their equivalent modes; AMPS_DC≡CURR, WATTS≡POW, OHMS≡RES, VOLTS_DC≡VOLT.
    // The explicit integer values correlate LOAD_MODE to LOAD_UNITS; their actual values are irrelevant.

    public static class EL_34143A {
        public const String MODEL = "EL34143A";
 
        public static Boolean IsEL_34143A(SCPI_VISA_Instrument SVI) { return (SVI.Instrument.GetType() == typeof(AgEL30000)); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            SCPI99.Initialize(SVI);
            ((AgEL30000)SVI.Instrument).SCPI.OUTPut.PROTection.CLEar.Command();
            ((AgEL30000)SVI.Instrument).SCPI.DISPlay.WINDow.TEXT.CLEar.Command();
        }

        public static void Local(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instrument).SCPI.SYSTem.LOCal.Command(); }

        public static Boolean LoadIs(SCPI_VISA_Instrument SVI, Double LoadValue, LOAD_MODE LoadMode) {
            if (!LoadModeIs(SVI, LoadMode)) return false;
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

        public static Boolean LoadIs(SCPI_VISA_Instrument SVI, Double LoadValue, LOAD_UNITS LoadUnits) { return LoadIs(SVI, LoadValue, (LOAD_MODE)(Int32)LoadUnits); }

        public static Boolean LoadModeIs(SCPI_VISA_Instrument SVI, LOAD_MODE LoadMode) { return LoadMode == LoadModeGet(SVI); }

        public static LOAD_MODE LoadModeGet(SCPI_VISA_Instrument SVI) {
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Query(null, out String LoadMode);
            return (LOAD_MODE)Enum.Parse(typeof(LOAD_MODE), LoadMode); 
        }

        public static Boolean LoadUnitsAre(SCPI_VISA_Instrument SVI, LOAD_UNITS LoadUnits) { return LoadUnits == LoadUnitsGet(SVI); }

        public static LOAD_UNITS LoadUnitsGet(SCPI_VISA_Instrument SVI) { return (LOAD_UNITS)(Int32)LoadModeGet(SVI); }

        public static Double MeasureADC(SCPI_VISA_Instrument SVI) {
            ((AgEL30000)SVI.Instrument).SCPI.MEASure.SCALar.CURRent.DC.Query(null, out Double[] ampsDC);
            return ampsDC[0];
        }

        public static Double MeasureVDC(SCPI_VISA_Instrument SVI) {
            ((AgEL30000)SVI.Instrument).SCPI.MEASure.SCALar.VOLTage.DC.Query(null, out Double[] voltsDC);
            return voltsDC[0];
        }

        public static void OutputStateSet(SCPI_VISA_Instrument SVI, OUTPUT State) { ((AgEL30000)SVI.Instrument).SCPI.OUTPut.STATe.Command(State is OUTPUT.ON, null); }

        public static void Remote(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instrument).SCPI.SYSTem.REMote.Command(); }

        public static void RemoteLock(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instrument).SCPI.SYSTem.RWLock.Command(); }

        public static void Set(SCPI_VISA_Instrument SVI, OUTPUT State, Double LoadValue, LOAD_UNITS LoadUnits, SENSE_MODE KelvinSense = SENSE_MODE.INTernal) { Set(SVI, State, LoadValue, (LOAD_MODE)(Int32)LoadUnits, KelvinSense); }

        public static void Set(SCPI_VISA_Instrument SVI, OUTPUT State, Double LoadValue, LOAD_MODE LoadMode, SENSE_MODE KelvinSense = SENSE_MODE.INTernal) {
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.SENSe.SOURce.Command(Enum.GetName(typeof(SENSE_MODE), KelvinSense));
            // Despite being part of VOLTage sub-system, SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal"/"INTernal") enables/disables 4-wire Kelvin sensing for all Kelvin capable loads.
            switch (LoadMode) {
                case LOAD_MODE.CURR:
                    SetCURRent(SVI, LoadValue);
                    break;
                case LOAD_MODE.POW:
                    SetPOWer(SVI, LoadValue);
                    break;
                case LOAD_MODE.RES:
                    SetRESistance(SVI, LoadValue);
                    break;
                case LOAD_MODE.VOLT:
                    SetVOLtage(SVI, LoadValue);
                    break;
                default:
                    throw new NotImplementedException(TestExecutive.NotImplementedMessageEnum(typeof(LOAD_MODE)));
            }
            OutputStateSet(SVI, State);
        }

        public static void SetCURRent(SCPI_VISA_Instrument SVI, Double AmpsDC) {
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Command("CURRent", null);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(AmpsDC, null);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.RANGe.Command(AmpsDC, null);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query(MINimum, null, out Double min);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query(MAXimum, null, out Double max);
            if ((AmpsDC < min) || (max < AmpsDC)) {
                String s = $"> MINimum/MAXimum Current.{Environment.NewLine}"
                    + $" - MINimum   :  Current={min} A.{Environment.NewLine}"
                    + $" - Programmed:  Current={AmpsDC} A.{Environment.NewLine}"
                    + $" - MAXimum   :  Current={max} A.";
                throw new InvalidOperationException(SCPI99.ErrorMessageGet(SVI, s));
            }
            // TODO: ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.STATe.Command(false, null);
        }

        public static void SetPOWer(SCPI_VISA_Instrument SVI, Double Watts) {
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Command("POWer", null);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Command(Watts, null);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.RANGe.Command(Watts, null);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Query(MINimum, null, out Double[] min);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Query(MAXimum, null, out Double[] max);
            if ((Watts < min[0]) || (max[0] < Watts)) {
                String s = $"< MINimum/MAXimum Wattage.{Environment.NewLine}"
                    + $" - MINimum   :  Wattage={min[0]} W.{Environment.NewLine}"
                    + $" - Programmed:  Wattage={Watts} W.{Environment.NewLine}"
                    + $" - MAXimum   :  Wattage={max[0]} W.";
                throw new InvalidOperationException(SCPI99.ErrorMessageGet(SVI, s));
            }
            // TODO: ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.PROTection.STATe.Command(false, null);
        }

        public static void SetRESistance(SCPI_VISA_Instrument SVI, Double Ohms) {
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Command("RESistance", null);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Command(Ohms, null);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.RESistance.RANGe.Command(Ohms, null);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Query(MINimum, null, out Double[] min);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Query(MAXimum, null, out Double[] max);
            if ((Ohms < min[0]) || (max[0] < Ohms)) {
                String s = $"< MINimum/MAXimum Resistance.{Environment.NewLine}"
                    + $" - MINimum   :  Resistance={min[0]} Ω.{Environment.NewLine}"
                    + $" - Programmed:  Resistance={Ohms} Ω.{Environment.NewLine}"
                    + $" - MAXimum   :  Resistance={max[0]} Ω.";
                throw new InvalidOperationException(SCPI99.ErrorMessageGet(SVI, s));
            }
      }

        public static void SetVOLtage(SCPI_VISA_Instrument SVI, Double VoltsDC) {
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Command("VOLTage", null);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(VoltsDC, null);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.RANGe.Command(VoltsDC, null);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query(MINimum, null, out Double[] min);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query(MAXimum, null, out Double[] max);
            if ((VoltsDC < min[0]) || (max[0] < VoltsDC)) {
                String s = $"< MINimum/MAXimum Voltage.{Environment.NewLine}"
                    + $" - MINimum   :  Voltage={min[0]} V.{Environment.NewLine}"
                    + $" - Programmed:  Voltage={VoltsDC} V.{Environment.NewLine}"
                    + $" - MAXimum   :  Voltage={max[0]} V.";
                throw new InvalidOperationException(SCPI99.ErrorMessageGet(SVI, s));
            }
        }
    }
}