using System;
using System.Collections.Generic;
using Agilent.CommandExpert.ScpiNet.AgEL30000_1_2_5_1_0_6_17_114;
using TestLibrary.AppConfig;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new SVIs in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new SVIs are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
namespace TestLibrary.SCPI_VISA {
    public static class EL_34143A {

        public static Boolean IsEL_34143A(SCPI_VISA_Instrument SVI) { return (SVI.Instance.GetType() == typeof(AgEL30000)); }

        public static void Clear(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instance).SCPI.CLS.Command(); }

        public static void ClearAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsEL_34143A(SVI.Value)) Clear(SVI.Value); }

        public static void Local(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instance).SCPI.SYSTem.LOCal.Command(); }

        public static void LocalAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsEL_34143A(SVI.Value)) Local(SVI.Value); }

        public static void Remote(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instance).SCPI.SYSTem.REMote.Command(); }

        public static void RemoteAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsEL_34143A(SVI.Value)) Remote(SVI.Value); }

        public static void RemoteLock(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instance).SCPI.SYSTem.RWLock.Command(); }

        public static void RemoteLockAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsEL_34143A(SVI.Value)) RemoteLock(SVI.Value); }

        public static void Reset(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instance).SCPI.RST.Command(); }

        public static void ResetAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsEL_34143A(SVI.Value)) Reset(SVI.Value); }

        public static void SelfTest(SCPI_VISA_Instrument SVI) {
            ((AgEL30000)SVI.Instance).SCPI.TST.Query(out Int32 selfTestResult);
            if (selfTestResult != 0) {
                ((AgEL30000)SVI.Instance).SCPI.SYSTem.ERRor.NEXT.Query(out Int32 errorNumber, out String errorMessage);
                throw new InvalidOperationException(PI_SCPI99.GetErrorMessage(SVI, errorMessage, errorNumber));
            }
        }

        public static void SelfTestAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsEL_34143A(SVI.Value)) SelfTest(SVI.Value); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            Reset(SVI); // Reset SVI to default power-on states.
            Clear(SVI); // Clear all event registers & the Status Byte register.
            SelfTest(SVI);
            ((AgEL30000)SVI.Instance).SCPI.OUTPut.PROTection.CLEar.Command();
            ((AgEL30000)SVI.Instance).SCPI.DISPlay.WINDow.TEXT.CLEar.Command();
            RemoteLock(SVI);
        }

        public static void InitializeAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsEL_34143A(SVI.Value)) Initialize(SVI.Value); }

        public static Boolean IsOff(SCPI_VISA_Instrument SVI) { return !IsOn(SVI); }

        public static Boolean AreOnAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) {
            Boolean AreOn = true;
            foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsEL_34143A(SVI.Value)) AreOn = AreOn && IsOn(SVI.Value);
            return AreOn;
        }

        public static Boolean IsOn(SCPI_VISA_Instrument SVI) {
            ((AgEL30000)SVI.Instance).SCPI.OUTPut.STATe.Query(out Boolean State);
            return State;
        }

        public static Boolean AreOffAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { return !AreOnAll(SVIs); }

        public static String Mode(SCPI_VISA_Instrument SVI) {
            ((AgEL30000)SVI.Instance).SCPI.SOURce.MODE.Query(null, out String Mode);
            return Mode;
        }

        public static void Off(SCPI_VISA_Instrument SVI) { ((AgEL30000)SVI.Instance).SCPI.OUTPut.STATe.Command(false, null); }

        public static void OffAll(Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs) { foreach (KeyValuePair<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVI in SVIs) if (IsEL_34143A(SVI.Value)) Off(SVI.Value); }

        public static void OnConstantCurrent(SCPI_VISA_Instrument SVI, Double amps) {
            ((AgEL30000)SVI.Instance).SCPI.SOURce.MODE.Command("CURRent", null);
            ((AgEL30000)SVI.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(amps, null);
            ((AgEL30000)SVI.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MINimum", null, out Double min);
            ((AgEL30000)SVI.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MAXimum", null, out Double max);
            if ((amps < min) || (amps > max)) {
                String s = $"> MINimum/MAXimum Current.{Environment.NewLine}"
                    + $" - MINimum   :  Current={min} A.{Environment.NewLine}"
                    + $" - Programmed:  Current={amps} A.{Environment.NewLine}"
                    + $" - MAXimum   :  Current={max} A.";
                throw new InvalidOperationException(PI_SCPI99.GetMessage(SVI, s));
            }
            // TODO: ((AgEL30000)SVI.Instance).SCPI.SOURce.CURRent.PROTection.STATe.Command(false, null);
            ((AgEL30000)SVI.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgEL30000)SVI.Instance).SCPI.OUTPut.STATe.Command(true, null);
        }

        public static void OnConstantVoltage(SCPI_VISA_Instrument SVI, Double volts) {
            ((AgEL30000)SVI.Instance).SCPI.SOURce.MODE.Command("VOLTage", null);
            ((AgEL30000)SVI.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(volts, null);
            ((AgEL30000)SVI.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MINimum", null, out Double[] min);
            ((AgEL30000)SVI.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MAXimum", null, out Double[] max);
            if ((volts < min[0]) || (volts > max[1])) {
                String s = $"< MINimum/MAXimum Voltage.{Environment.NewLine}"
                    + $" - MINimum   :  Voltage={min[0]} V.{Environment.NewLine}"
                    + $" - Programmed:  Voltage={volts} V.{Environment.NewLine}"
                    + $" - MAXimum   :  Voltage={max[1]} V.";
                throw new InvalidOperationException(PI_SCPI99.GetMessage(SVI, s));
            }
            ((AgEL30000)SVI.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgEL30000)SVI.Instance).SCPI.OUTPut.STATe.Command(true, null);
        }

        public static void OnConstantPower(SCPI_VISA_Instrument SVI, Double watts) {
            ((AgEL30000)SVI.Instance).SCPI.SOURce.MODE.Command("POWer", null);
            ((AgEL30000)SVI.Instance).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Command(watts, null);
            ((AgEL30000)SVI.Instance).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Query("MINimum", null, out Double[] min);
            ((AgEL30000)SVI.Instance).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Query("MAXimum", null, out Double[] max);
            if ((watts < min[0]) || (watts > max[1])) {
                String s = $"< MINimum/MAXimum Wattage.{Environment.NewLine}"
                    + $" - MINimum   :  Wattage={min[0]} W.{Environment.NewLine}"
                    + $" - Programmed:  Wattage={watts} W.{Environment.NewLine}"
                    + $" - MAXimum   :  Wattage={max[1]} W.";
                throw new InvalidOperationException(PI_SCPI99.GetMessage(SVI, s));
            }
            ((AgEL30000)SVI.Instance).SCPI.SOURce.POWer.PROTection.STATe.Command(false, null);
            ((AgEL30000)SVI.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgEL30000)SVI.Instance).SCPI.OUTPut.STATe.Command(true, null);
        }

        public static void OnConstantResistance(SCPI_VISA_Instrument SVI, Double ohms) {
            ((AgEL30000)SVI.Instance).SCPI.SOURce.MODE.Command("RESistance", null);
            ((AgEL30000)SVI.Instance).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Command(ohms, null);
            ((AgEL30000)SVI.Instance).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Query("MINimum", null, out Double[] min);
            ((AgEL30000)SVI.Instance).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Query("MAXimum", null, out Double[] max);
            if ((ohms < min[0]) || (ohms > max[0])) {
                String s = $"< MINimum/MAXimum Resistance.{Environment.NewLine}"
                    + $" - MINimum   :  Resistance={min[0]} Ω.{Environment.NewLine}"
                    + $" - Programmed:  Resistance={ohms} Ω.{Environment.NewLine}"
                    + $" - MAXimum   :  Resistance={max[0]} Ω.";
                throw new InvalidOperationException(PI_SCPI99.GetMessage(SVI, s));
            }
            ((AgEL30000)SVI.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgEL30000)SVI.Instance).SCPI.OUTPut.STATe.Command(true, null);
        }
    }
}