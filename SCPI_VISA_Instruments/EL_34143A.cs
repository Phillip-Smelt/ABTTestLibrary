using System;
using Agilent.CommandExpert.ScpiNet.AgEL30000_1_2_5_1_0_6_17_114;
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
    public enum LOAD_MODE { CURR, POW, RES, VOLT } // Musn't re-order LOAD_MODE, as sequence directly correlates to LOAD_UNITS for conversion.

    public enum LOAD_UNITS { AMPS_DC, WATTS, OHMS, VOLTS_DC } // Mustn't re-order LOAD_UNITS, as sequence directly correlates to LOAD_MODE for conversion.

    public static class EL_34143A {
        public const String MODEL = "EL34143A";

        private const String INVALID_MODE = "Invalid EL_34143A Load Mode, must be in enum '{ CURR, POW, RES, VOLT }'.";

        public static Boolean IsEL_34143A(SCPI_VISA_Instrument SVI) { return (SVI.Instrument.GetType() == typeof(AgEL30000)); }

        public static void Local(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instrument).SCPI.SYSTem.LOCal.Command(); }

        public static void Remote(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instrument).SCPI.SYSTem.REMote.Command(); }

        public static void RemoteLock(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instrument).SCPI.SYSTem.RWLock.Command(); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            SCPI_VISA.Initialize(SVI);
            ((AgEL30000)SVI.Instrument).SCPI.OUTPut.PROTection.CLEar.Command();
            ((AgEL30000)SVI.Instrument).SCPI.DISPlay.WINDow.TEXT.CLEar.Command();
            RemoteLock(SVI);
        }

        public static Boolean IsState(SCPI_VISA_Instrument SVI, STATE State) {
            if (State is STATE.ON) return IsOn(SVI);
            else return IsOff(SVI);
        }

        public static Boolean IsOff(SCPI_VISA_Instrument SVI) { return !IsOn(SVI); }

        public static Boolean IsOn(SCPI_VISA_Instrument SVI) {
            ((AgEL30000)SVI.Instrument).SCPI.OUTPut.STATe.Query(out Boolean State);
            return State;
        }

        public static LOAD_MODE GetLoadMode(SCPI_VISA_Instrument SVI) {
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Query(SCPI_VISA.CHANNEL_1, out String LoadMode);
            return (LOAD_MODE)Enum.Parse(typeof(LOAD_MODE), LoadMode); 
        }

        public static LOAD_UNITS GetLoadUnits(SCPI_VISA_Instrument SVI) { return (LOAD_UNITS)(Int32)GetLoadMode(SVI); }

        public static Boolean IsLoadMode(SCPI_VISA_Instrument SVI, LOAD_MODE LoadMode) { return LoadMode == GetLoadMode(SVI); }

        public static Boolean IsLoadUnits(SCPI_VISA_Instrument SVI, LOAD_UNITS LoadUnits) { return LoadUnits == GetLoadUnits(SVI); }

        public static Boolean IsValueAndMode(SCPI_VISA_Instrument SVI, Double LoadValue, LOAD_MODE LoadMode) {
            Boolean stateIs = IsLoadMode(SVI, LoadMode);
            Double delta = 0.01;
            switch (LoadMode) {
                case LOAD_MODE.CURR:
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query(null, SCPI_VISA.CHANNEL_1, out Double ampsDC);
                    return stateIs && SCPI_VISA.IsCloseEnough(LoadValue, ampsDC, delta);
                case LOAD_MODE.POW:
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Query(null, SCPI_VISA.CHANNEL_1, out Double[] watts);
                    return stateIs && SCPI_VISA.IsCloseEnough(LoadValue, watts[0], delta);
                case LOAD_MODE.RES:
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Query(null, SCPI_VISA.CHANNEL_1, out Double[] ohms);
                    return stateIs && SCPI_VISA.IsCloseEnough(LoadValue, ohms[0], delta);
                case LOAD_MODE.VOLT:
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query(null, SCPI_VISA.CHANNEL_1, out Double[] voltsDC);
                    return stateIs && SCPI_VISA.IsCloseEnough(LoadValue, voltsDC[0], delta);
                default:
                    throw new ArgumentException(INVALID_MODE);
            }
        }

        public static Boolean IsValueAndUnits(SCPI_VISA_Instrument SVI, Double LoadValue, LOAD_UNITS LoadUnits) { return IsValueAndMode(SVI, LoadValue, (LOAD_MODE)(Int32)LoadUnits); }

        public static void Off(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instrument).SCPI.OUTPut.STATe.Command(false, SCPI_VISA.CHANNEL_1); }

        public static void On(SCPI_VISA_Instrument SVI, Double LoadValue, LOAD_MODE LoadMode) {
            switch (LoadMode) {
                case LOAD_MODE.CURR:
                    OnCURRent(SVI, LoadValue);
                    break;
                case LOAD_MODE.POW:
                    OnPOWer(SVI, LoadValue);
                    break;
                case LOAD_MODE.RES:
                    OnRESistance(SVI, LoadValue);
                    break;
                case LOAD_MODE.VOLT:
                    OnVOLtage(SVI, LoadValue);
                    break;
                default:
                    throw new ArgumentException(INVALID_MODE);
            }
        }

        public static void On(SCPI_VISA_Instrument SVI, Double LoadValue, LOAD_UNITS LoadUnits) { On(SVI, LoadValue, (LOAD_MODE)(Int32)LoadUnits); }

        public static void OnCURRent(SCPI_VISA_Instrument SVI, Double AmpsDC) {
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Command("CURRent", SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(AmpsDC, SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MINimum", SCPI_VISA.CHANNEL_1, out Double min);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MAXimum", SCPI_VISA.CHANNEL_1, out Double max);
            if ((AmpsDC < min) || (AmpsDC > max)) {
                String s = $"> MINimum/MAXimum Current.{Environment.NewLine}"
                    + $" - MINimum   :  Current={min} A.{Environment.NewLine}"
                    + $" - Programmed:  Current={AmpsDC} A.{Environment.NewLine}"
                    + $" - MAXimum   :  Current={max} A.";
                throw new InvalidOperationException(SCPI_VISA.GetErrorMessage(SVI, s));
            }
            // TODO: ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.STATe.Command(false, SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgEL30000)SVI.Instrument).SCPI.OUTPut.STATe.Command(true, SCPI_VISA.CHANNEL_1);
        }

        public static void OnVOLtage(SCPI_VISA_Instrument SVI, Double VoltsDC) {
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Command("VOLTage", SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(VoltsDC, SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MINimum", SCPI_VISA.CHANNEL_1, out Double[] min);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MAXimum", SCPI_VISA.CHANNEL_1, out Double[] max);
            if ((VoltsDC < min[0]) || (VoltsDC > max[1])) {
                String s = $"< MINimum/MAXimum Voltage.{Environment.NewLine}"
                    + $" - MINimum   :  Voltage={min[0]} V.{Environment.NewLine}"
                    + $" - Programmed:  Voltage={VoltsDC} V.{Environment.NewLine}"
                    + $" - MAXimum   :  Voltage={max[1]} V.";
                throw new InvalidOperationException(SCPI_VISA.GetErrorMessage(SVI, s));
            }
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgEL30000)SVI.Instrument).SCPI.OUTPut.STATe.Command(true, SCPI_VISA.CHANNEL_1);
        }

        public static void OnPOWer(SCPI_VISA_Instrument SVI, Double Watts) {
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Command("POWer", SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Command(Watts, SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Query("MINimum", SCPI_VISA.CHANNEL_1, out Double[] min);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Query("MAXimum", SCPI_VISA.CHANNEL_1, out Double[] max);
            if ((Watts < min[0]) || (Watts > max[1])) {
                String s = $"< MINimum/MAXimum Wattage.{Environment.NewLine}"
                    + $" - MINimum   :  Wattage={min[0]} W.{Environment.NewLine}"
                    + $" - Programmed:  Wattage={Watts} W.{Environment.NewLine}"
                    + $" - MAXimum   :  Wattage={max[1]} W.";
                throw new InvalidOperationException(SCPI_VISA.GetErrorMessage(SVI, s));
            }
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.PROTection.STATe.Command(false, SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgEL30000)SVI.Instrument).SCPI.OUTPut.STATe.Command(true, SCPI_VISA.CHANNEL_1);
        }

        public static void OnRESistance(SCPI_VISA_Instrument SVI, Double Ohms) {
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Command("RESistance", SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Command(Ohms, SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Query("MINimum", SCPI_VISA.CHANNEL_1, out Double[] min);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Query("MAXimum", SCPI_VISA.CHANNEL_1, out Double[] max);
            if ((Ohms < min[0]) || (Ohms > max[0])) {
                String s = $"< MINimum/MAXimum Resistance.{Environment.NewLine}"
                    + $" - MINimum   :  Resistance={min[0]} Ω.{Environment.NewLine}"
                    + $" - Programmed:  Resistance={Ohms} Ω.{Environment.NewLine}"
                    + $" - MAXimum   :  Resistance={max[0]} Ω.";
                throw new InvalidOperationException(SCPI_VISA.GetErrorMessage(SVI, s));
            }
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgEL30000)SVI.Instrument).SCPI.OUTPut.STATe.Command(true, SCPI_VISA.CHANNEL_1);
        }

        public static Double MeasureVDC(SCPI_VISA_Instrument SVI) {
            ((AgEL30000)SVI.Instrument).SCPI.MEASure.SCALar.VOLTage.DC.Query(SCPI_VISA.CHANNEL_1, out Double[] voltsDC);
            return voltsDC[0];
        }

        public static Double MeasureADC(SCPI_VISA_Instrument SVI) {
            ((AgEL30000)SVI.Instrument).SCPI.MEASure.SCALar.CURRent.DC.Query(SCPI_VISA.CHANNEL_1, out Double[] ampsDC);
            return ampsDC[0];
        }
    }
}