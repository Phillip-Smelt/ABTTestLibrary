using System;
using System.Collections.Generic;
using Agilent.CommandExpert.ScpiNet.AgEL30000_1_2_5_1_0_6_17_114;
using TestLibrary.AppConfig;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new SCPI VISA Instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new SVIs are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
// NOTE: Below hopefully "value-added" wrapper methods for some commonly used AgEL30000 commands are conveniences, not necessities.
// NOTE: Will never fully implement wrapper methods for the complete set of AgEL30000 commands, just some of the most commonly used ones.
// - In general, TestLibrary's InterfaceAdapters, Logging, SCPI_VISA_Instruments & Switching namespaces exist partly to eliminate
//   the need to reference TestLibrary's various DLLs directly from TestProgram client apps.
// - As long as suitable wrapper methods exists in EL_34143A, needn't directly reference AgEL30000_1_2_5_1_0_6_17_114
//   from TestProgram client apps, as referencing TestLibrary suffices.
namespace TestLibrary.SCPI_VISA_Instruments {
    public enum LOAD_MODE { CURR, POW, RES, VOLT }

    public enum LOAD_UNITS { AMPS_DC, WATTS, OHMS, VOLTS_DC }

    public static class EL_34143A {
        public const String MODEL = "EL34143A";

        public static Boolean IsEL_34143A(SCPI_VISA_Instrument SVI) { return (SVI.Instrument.GetType() == typeof(AgEL30000)); }

        public static void Clear(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instrument).SCPI.CLS.Command(); }

        public static void ClearAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) if (IsEL_34143A(kvp.Value)) Clear(kvp.Value); }

        public static void Local(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instrument).SCPI.SYSTem.LOCal.Command(); }

        public static void LocalAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) if (IsEL_34143A(kvp.Value)) Local(kvp.Value); }

        public static void Remote(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instrument).SCPI.SYSTem.REMote.Command(); }

        public static void RemoteAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) if (IsEL_34143A(kvp.Value)) Remote(kvp.Value); }

        public static void RemoteLock(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instrument).SCPI.SYSTem.RWLock.Command(); }

        public static void RemoteLockAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) if (IsEL_34143A(kvp.Value)) RemoteLock(kvp.Value); }

        public static void Reset(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instrument).SCPI.RST.Command(); }

        public static void ResetAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) if (IsEL_34143A(kvp.Value)) Reset(kvp.Value); }

        public static void SelfTest(SCPI_VISA_Instrument SVI) {
            ((AgEL30000)SVI.Instrument).SCPI.TST.Query(out Int32 selfTestResult);
            if (selfTestResult != 0) {
                ((AgEL30000)SVI.Instrument).SCPI.SYSTem.ERRor.NEXT.Query(out Int32 errorNumber, out String errorMessage);
                throw new InvalidOperationException(SCPI_VISA.GetErrorMessage(SVI, errorMessage, errorNumber));
            }
        }

        public static void SelfTestAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) if (IsEL_34143A(kvp.Value)) SelfTest(kvp.Value); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            Reset(SVI); // Reset SVI to default power-on states.
            Clear(SVI); // Clear all event registers & the Status Byte register.
            SelfTest(SVI);
            ((AgEL30000)SVI.Instrument).SCPI.OUTPut.PROTection.CLEar.Command();
            ((AgEL30000)SVI.Instrument).SCPI.DISPlay.WINDow.TEXT.CLEar.Command();
            RemoteLock(SVI);
        }

        public static void InitializeAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) if (IsEL_34143A(kvp.Value)) Initialize(kvp.Value); }
        
        public static Boolean IsState(SCPI_VISA_Instrument SVI, BINARY State) {
            if (State is BINARY.On) return IsOn(SVI);
            else return IsOff(SVI);
        }

        public static Boolean IsOff(SCPI_VISA_Instrument SVI) { return !IsOn(SVI); }

        public static Boolean AreOnAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) {
            Boolean AreOn = true;
            foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) if (IsEL_34143A(kvp.Value)) AreOn = AreOn && IsOn(kvp.Value);
            return AreOn;
        }

        public static Boolean IsOn(SCPI_VISA_Instrument SVI) {
            ((AgEL30000)SVI.Instrument).SCPI.OUTPut.STATe.Query(out Boolean State);
            return State;
        }

        public static Boolean AreOffAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { return !AreOnAll(SVIs); }

        public static String Mode(SCPI_VISA_Instrument SVI) {
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Query(SCPI_VISA.CHANNEL_1, out String Mode);
            return Mode;
        }

        public static Boolean IsMode(SCPI_VISA_Instrument SVI, LOAD_MODE loadMode) { return String.Equals(Enum.GetName(typeof(LOAD_MODE), loadMode), Mode(SVI)); }

        public static Boolean IsValueAndUnits(SCPI_VISA_Instrument SVI, Double Value, LOAD_UNITS LoadUnits) {
            LOAD_MODE mode = (LOAD_MODE)(Int32)LoadUnits;
            Boolean stateIs = IsMode(SVI, mode);
            Double delta = 0.01;
            switch (mode) {
                case LOAD_MODE.CURR:
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query(null, SCPI_VISA.CHANNEL_1, out Double ampsDC);
                    return stateIs && SCPI_VISA.IsCloseEnough(Value, ampsDC, delta);
                case LOAD_MODE.POW:
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Query(null, SCPI_VISA.CHANNEL_1, out Double[] watts);
                    return stateIs && SCPI_VISA.IsCloseEnough(Value, watts[0], delta);
                case LOAD_MODE.RES:
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Query(null, SCPI_VISA.CHANNEL_1, out Double[] ohms);
                    return stateIs && SCPI_VISA.IsCloseEnough(Value, ohms[0], delta);
                case LOAD_MODE.VOLT:
                    ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query(null, SCPI_VISA.CHANNEL_1, out Double[] voltsDC);
                    return stateIs && SCPI_VISA.IsCloseEnough(Value, voltsDC[0], delta);
                default:
                    throw new ArgumentException($"Invalid EL_34143A Load Unit, must be in enum '{{CURR, POW, RES, VOLT }}'.");
            }
        }

        public static void Off(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instrument).SCPI.OUTPut.STATe.Command(false, SCPI_VISA.CHANNEL_1); }

        public static void OffAll(Dictionary<String, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<String, SCPI_VISA_Instrument> kvp in SVIs) if (IsEL_34143A(kvp.Value)) Off(kvp.Value); }

        public static void OnCURRent(SCPI_VISA_Instrument SVI, Double amps) {
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Command("CURRent", SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(amps, SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MINimum", SCPI_VISA.CHANNEL_1, out Double min);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MAXimum", SCPI_VISA.CHANNEL_1, out Double max);
            if ((amps < min) || (amps > max)) {
                String s = $"> MINimum/MAXimum Current.{Environment.NewLine}"
                    + $" - MINimum   :  Current={min} A.{Environment.NewLine}"
                    + $" - Programmed:  Current={amps} A.{Environment.NewLine}"
                    + $" - MAXimum   :  Current={max} A.";
                throw new InvalidOperationException(SCPI_VISA.GetErrorMessage(SVI, s));
            }
            // TODO: ((AgEL30000)SVI.Instrument).SCPI.SOURce.CURRent.PROTection.STATe.Command(false, SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgEL30000)SVI.Instrument).SCPI.OUTPut.STATe.Command(true, SCPI_VISA.CHANNEL_1);
        }

        public static void OnVOLtage(SCPI_VISA_Instrument SVI, Double volts) {
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Command("VOLTage", SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(volts, SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MINimum", SCPI_VISA.CHANNEL_1, out Double[] min);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MAXimum", SCPI_VISA.CHANNEL_1, out Double[] max);
            if ((volts < min[0]) || (volts > max[1])) {
                String s = $"< MINimum/MAXimum Voltage.{Environment.NewLine}"
                    + $" - MINimum   :  Voltage={min[0]} V.{Environment.NewLine}"
                    + $" - Programmed:  Voltage={volts} V.{Environment.NewLine}"
                    + $" - MAXimum   :  Voltage={max[1]} V.";
                throw new InvalidOperationException(SCPI_VISA.GetErrorMessage(SVI, s));
            }
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgEL30000)SVI.Instrument).SCPI.OUTPut.STATe.Command(true, SCPI_VISA.CHANNEL_1);
        }

        public static void OnPOWer(SCPI_VISA_Instrument SVI, Double watts) {
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Command("POWer", SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Command(watts, SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Query("MINimum", SCPI_VISA.CHANNEL_1, out Double[] min);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Query("MAXimum", SCPI_VISA.CHANNEL_1, out Double[] max);
            if ((watts < min[0]) || (watts > max[1])) {
                String s = $"< MINimum/MAXimum Wattage.{Environment.NewLine}"
                    + $" - MINimum   :  Wattage={min[0]} W.{Environment.NewLine}"
                    + $" - Programmed:  Wattage={watts} W.{Environment.NewLine}"
                    + $" - MAXimum   :  Wattage={max[1]} W.";
                throw new InvalidOperationException(SCPI_VISA.GetErrorMessage(SVI, s));
            }
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.POWer.PROTection.STATe.Command(false, SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgEL30000)SVI.Instrument).SCPI.OUTPut.STATe.Command(true, SCPI_VISA.CHANNEL_1);
        }

        public static void OnRESistance(SCPI_VISA_Instrument SVI, Double ohms) {
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.MODE.Command("RESistance", SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Command(ohms, SCPI_VISA.CHANNEL_1);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Query("MINimum", SCPI_VISA.CHANNEL_1, out Double[] min);
            ((AgEL30000)SVI.Instrument).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Query("MAXimum", SCPI_VISA.CHANNEL_1, out Double[] max);
            if ((ohms < min[0]) || (ohms > max[0])) {
                String s = $"< MINimum/MAXimum Resistance.{Environment.NewLine}"
                    + $" - MINimum   :  Resistance={min[0]} Ω.{Environment.NewLine}"
                    + $" - Programmed:  Resistance={ohms} Ω.{Environment.NewLine}"
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