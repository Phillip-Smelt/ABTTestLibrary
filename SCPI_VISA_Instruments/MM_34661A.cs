using System;
using System.Windows.Forms;
using Agilent.CommandExpert.ScpiNet.Ag3466x_2_08;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new SCPI VISA Instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new SVIs are added.
//  - The Agilent.CommandExpert.ScpiNet drivers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
// NOTE: Below hopefully "value-added" wrapper methods for some commonly used SCPI commands are conveniences, not necessities.
namespace ABT.TestSpace.TestExec.SCPI_VISA_Instruments {
    public static class MM_34661A {
        public enum TERMINALS { Front, Rear };

        public const String MODEL = "34461A";

        public static Boolean IsMM_34661A(SCPI_VISA_Instrument SVI) { return (SVI.Instrument.GetType() == typeof(Ag3466x)); }

        public static void Local(SCPI_VISA_Instrument SVI) { ((Ag3466x)SVI.Instrument).SCPI.SYSTem.LOCal.Command(); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            if (GetTerminals(SVI) == TERMINALS.Front) _ = MessageBox.Show("Please depress Keysight 34661A Front/Rear button.", "Paused, click OK to continue.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            SCPI99.Initialize(SVI);
        }

        public static TERMINALS GetTerminals(SCPI_VISA_Instrument SVI) {
            ((Ag3466x)SVI.Instrument).SCPI.ROUTe.TERMinals.Query(out String terminals);
            return String.Equals(terminals, "REAR") ? TERMINALS.Rear : TERMINALS.Front;
        }

        public static Double MeasureVDC(SCPI_VISA_Instrument SVI) {
            ((Ag3466x)SVI.Instrument).SCPI.MEASure.VOLTage.DC.QueryAsciiRealClone(SCPI.AUTO, SCPI.MAXimum, out Double voltsDC);
            return voltsDC;
        }

        public static Double MeasureADC(SCPI_VISA_Instrument SVI) {
            ((Ag3466x)SVI.Instrument).SCPI.MEASure.CURRent.DC.QueryAsciiReal(SCPI.AUTO, SCPI.MAXimum, out Double ampsDC);
            return ampsDC;
        }

        public static Double MeasureΩ(SCPI_VISA_Instrument SVI) {
            ((Ag3466x)SVI.Instrument).SCPI.MEASure.RESistance.QueryAsciiReal(SCPI.AUTO, SCPI.MAXimum, out Double resistance);
            return resistance;
        }

        public static void SetDelay(SCPI_VISA_Instrument SVI) {
            ((Ag3466x)SVI.Instrument).SCPI.TRIGger.DELay.Command(SCPI.MINimum);
            ((Ag3466x)SVI.Instrument).SCPI.TRIGger.DELay.Command(SCPI.MAXimum);
            ((Ag3466x)SVI.Instrument).SCPI.TRIGger.DELay.Command(SCPI.DEFault);
        }
        
        public static void GetDelay(SCPI_VISA_Instrument SVI) {
            ((Ag3466x)SVI.Instrument).SCPI.TRIGger.DELay.AUTO.Command(true);
            ((Ag3466x)SVI.Instrument).SCPI.TRIGger.DELay.AUTO.Command(false);
            ((Ag3466x)SVI.Instrument).SCPI.TRIGger.DELay.AUTO.Query(out Boolean state);
            ((Ag3466x)SVI.Instrument).SCPI.TRIGger.DELay.Query(SCPI.MINimum, out Double delay);
            ((Ag3466x)SVI.Instrument).SCPI.TRIGger.DELay.Query(SCPI.MAXimum, out Double delay1);
            ((Ag3466x)SVI.Instrument).SCPI.TRIGger.DELay.Query(SCPI.DEFault, out Double delay2);
        }
                
        public static void IsDelay(SCPI_VISA_Instrument SVI) {
            ((Ag3466x)SVI.Instrument).SCPI.TRIGger.DELay.AUTO.Query(out Boolean state);
            ((Ag3466x)SVI.Instrument).SCPI.TRIGger.DELay.Query(SCPI.MAXimum, out Double delay);
            ((Ag3466x)SVI.Instrument).SCPI.TRIGger.DELay.Query(SCPI.MAXimum, out Double delay1);
            ((Ag3466x)SVI.Instrument).SCPI.TRIGger.DELay.Query(SCPI.DEFault, out Double delay2);
        }
                
        public static void IsDelayAuto(SCPI_VISA_Instrument SVI) {
            ((Ag3466x)SVI.Instrument).SCPI.TRIGger.DELay.AUTO.Query(out Boolean state);
        }
    }
}