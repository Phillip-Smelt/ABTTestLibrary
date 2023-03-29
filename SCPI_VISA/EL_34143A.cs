using System;
using System.Collections.Generic;
using Agilent.CommandExpert.ScpiNet.AgEL30000_1_2_5_1_0_6_17_114;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new instruments are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
namespace TestLibrary.SCPI_VISA {
    public static class EL_34143A {

        public static Boolean IsEL_34143A(Instrument instrument) { return (instrument.Instance.GetType() == typeof(AgEL30000)); }

        public static void Clear(Instrument instrument) { ((AgEL30000)instrument.Instance).SCPI.CLS.Command(); }

        public static void ClearAll(Dictionary<SCPI_VISA_IDs, Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, Instrument> i in instruments) if (IsEL_34143A(i.Value)) Clear(i.Value); }

        public static void Local(Instrument instrument) { ((AgEL30000)instrument.Instance).SCPI.SYSTem.LOCal.Command(); }

        public static void LocalAll(Dictionary<SCPI_VISA_IDs, Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, Instrument> i in instruments) if (IsEL_34143A(i.Value)) Local(i.Value); }

        public static void Remote(Instrument instrument) { ((AgEL30000)instrument.Instance).SCPI.SYSTem.REMote.Command(); }

        public static void RemoteAll(Dictionary<SCPI_VISA_IDs, Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, Instrument> i in instruments) if (IsEL_34143A(i.Value)) Remote(i.Value); }

        public static void RemoteLock(Instrument instrument) { ((AgEL30000)instrument.Instance).SCPI.SYSTem.RWLock.Command(); }

        public static void RemoteLockAll(Dictionary<SCPI_VISA_IDs, Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, Instrument> i in instruments) if (IsEL_34143A(i.Value)) RemoteLock(i.Value); }

        public static void Reset(Instrument instrument) { ((AgEL30000)instrument.Instance).SCPI.RST.Command(); }

        public static void ResetAll(Dictionary<SCPI_VISA_IDs, Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, Instrument> i in instruments) if (IsEL_34143A(i.Value)) Reset(i.Value); }

        public static void SelfTest(Instrument instrument) {
            ((AgEL30000)instrument.Instance).SCPI.TST.Query(out Int32 selfTestResult);
            if (selfTestResult != 0) {
                ((AgEL30000)instrument.Instance).SCPI.SYSTem.ERRor.NEXT.Query(out Int32 errorNumber, out String errorMessage);
                throw new InvalidOperationException(SCPI99.GetErrorMessage(instrument, errorMessage, errorNumber));
            }
        }

        public static void SelfTestAll(Dictionary<SCPI_VISA_IDs, Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, Instrument> i in instruments) if (IsEL_34143A(i.Value)) SelfTest(i.Value); }

        public static void Initialize(Instrument instrument) {
            Reset(instrument); // Reset instrument to default power-on states.
            Clear(instrument); // Clear all event registers & the Status Byte register.
            SelfTest(instrument);
            ((AgEL30000)instrument.Instance).SCPI.OUTPut.PROTection.CLEar.Command();
            ((AgEL30000)instrument.Instance).SCPI.DISPlay.WINDow.TEXT.CLEar.Command();
            RemoteLock(instrument);
        }

        public static void InitializeAll(Dictionary<SCPI_VISA_IDs, Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, Instrument> i in instruments) if (IsEL_34143A(i.Value)) Initialize(i.Value); }

        public static Boolean IsOff(Instrument instrument) { return !IsOn(instrument); }

        public static Boolean AreOnAll(Dictionary<SCPI_VISA_IDs, Instrument> instruments) {
            Boolean AreOn = true;
            foreach (KeyValuePair<SCPI_VISA_IDs, Instrument> i in instruments) if (IsEL_34143A(i.Value)) AreOn = AreOn && IsOn(i.Value);
            return AreOn;
        }

        public static Boolean IsOn(Instrument instrument) {
            ((AgEL30000)instrument.Instance).SCPI.OUTPut.STATe.Query(out Boolean State);
            return State;
        }

        public static Boolean AreOffAll(Dictionary<SCPI_VISA_IDs, Instrument> instruments) { return !AreOnAll(instruments); }

        public static String Mode(Instrument instrument) {
            ((AgEL30000)instrument.Instance).SCPI.SOURce.MODE.Query(null, out String Mode);
            return Mode;
        }

        public static void Off(Instrument instrument) { ((AgEL30000)instrument.Instance).SCPI.OUTPut.STATe.Command(false, null); }

        public static void OffAll(Dictionary<SCPI_VISA_IDs, Instrument> instruments) { foreach (KeyValuePair<SCPI_VISA_IDs, Instrument> i in instruments) if (IsEL_34143A(i.Value)) Off(i.Value); }

        public static void OnConstantCurrent(Instrument instrument, Double amps) {
            ((AgEL30000)instrument.Instance).SCPI.SOURce.MODE.Command("CURRent", null);
            ((AgEL30000)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Command(amps, null);
            ((AgEL30000)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MINimum", null, out Double min);
            ((AgEL30000)instrument.Instance).SCPI.SOURce.CURRent.LEVel.IMMediate.AMPLitude.Query("MAXimum", null, out Double max);
            if ((amps < min) || (amps > max)) {
                String s = $"> MINimum/MAXimum Current.{Environment.NewLine}"
                    + $" - MINimum   :  Current={min} A.{Environment.NewLine}"
                    + $" - Programmed:  Current={amps} A.{Environment.NewLine}"
                    + $" - MAXimum   :  Current={max} A.";
                throw new InvalidOperationException(SCPI99.GetMessage(instrument, s));
            }
            // TODO: ((AgEL30000)instrument.Instance).SCPI.SOURce.CURRent.PROTection.STATe.Command(false, null);
            ((AgEL30000)instrument.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgEL30000)instrument.Instance).SCPI.OUTPut.STATe.Command(true, null);
        }

        public static void OnConstantVoltage(Instrument instrument, Double volts) {
            ((AgEL30000)instrument.Instance).SCPI.SOURce.MODE.Command("VOLTage", null);
            ((AgEL30000)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Command(volts, null);
            ((AgEL30000)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MINimum", null, out Double[] min);
            ((AgEL30000)instrument.Instance).SCPI.SOURce.VOLTage.LEVel.IMMediate.AMPLitude.Query("MAXimum", null, out Double[] max);
            if ((volts < min[0]) || (volts > max[1])) {
                String s = $"< MINimum/MAXimum Voltage.{Environment.NewLine}"
                    + $" - MINimum   :  Voltage={min[0]} V.{Environment.NewLine}"
                    + $" - Programmed:  Voltage={volts} V.{Environment.NewLine}"
                    + $" - MAXimum   :  Voltage={max[1]} V.";
                throw new InvalidOperationException(SCPI99.GetMessage(instrument, s));
            }
            ((AgEL30000)instrument.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgEL30000)instrument.Instance).SCPI.OUTPut.STATe.Command(true, null);
        }

        public static void OnConstantPower(Instrument instrument, Double watts) {
            ((AgEL30000)instrument.Instance).SCPI.SOURce.MODE.Command("POWer", null);
            ((AgEL30000)instrument.Instance).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Command(watts, null);
            ((AgEL30000)instrument.Instance).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Query("MINimum", null, out Double[] min);
            ((AgEL30000)instrument.Instance).SCPI.SOURce.POWer.LEVel.IMMediate.AMPLitude.Query("MAXimum", null, out Double[] max);
            if ((watts < min[0]) || (watts > max[1])) {
                String s = $"< MINimum/MAXimum Wattage.{Environment.NewLine}"
                    + $" - MINimum   :  Wattage={min[0]} W.{Environment.NewLine}"
                    + $" - Programmed:  Wattage={watts} W.{Environment.NewLine}"
                    + $" - MAXimum   :  Wattage={max[1]} W.";
                throw new InvalidOperationException(SCPI99.GetMessage(instrument, s));
            }
            ((AgEL30000)instrument.Instance).SCPI.SOURce.POWer.PROTection.STATe.Command(false, null);
            ((AgEL30000)instrument.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgEL30000)instrument.Instance).SCPI.OUTPut.STATe.Command(true, null);
        }

        public static void OnConstantResistance(Instrument instrument, Double ohms) {
            ((AgEL30000)instrument.Instance).SCPI.SOURce.MODE.Command("RESistance", null);
            ((AgEL30000)instrument.Instance).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Command(ohms, null);
            ((AgEL30000)instrument.Instance).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Query("MINimum", null, out Double[] min);
            ((AgEL30000)instrument.Instance).SCPI.SOURce.RESistance.LEVel.IMMediate.AMPLitude.Query("MAXimum", null, out Double[] max);
            if ((ohms < min[0]) || (ohms > max[0])) {
                String s = $"< MINimum/MAXimum Resistance.{Environment.NewLine}"
                    + $" - MINimum   :  Resistance={min[0]} Ω.{Environment.NewLine}"
                    + $" - Programmed:  Resistance={ohms} Ω.{Environment.NewLine}"
                    + $" - MAXimum   :  Resistance={max[0]} Ω.";
                throw new InvalidOperationException(SCPI99.GetMessage(instrument, s));
            }
            ((AgEL30000)instrument.Instance).SCPI.SOURce.VOLTage.SENSe.SOURce.Command("EXTernal");
            ((AgEL30000)instrument.Instance).SCPI.OUTPut.STATe.Command(true, null);
        }
    }
}