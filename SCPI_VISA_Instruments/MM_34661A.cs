using System;
using System.Windows.Forms;
using Agilent.CommandExpert.ScpiNet.Ag3466x_2_08;
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
    public static class MM_34661A {
        public const String MODEL = "34461A";

        public static Boolean IsMM_34661A(SCPI_VISA_Instrument SVI) { return (SVI.Instrument.GetType() == typeof(Ag3466x)); }

        public static void Local(SCPI_VISA_Instrument SVI) { ((Ag3466x)SVI.Instrument).SCPI.SYSTem.LOCal.Command(); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            if (GetTerminals(SVI) == TERMINALS.Front) _ = MessageBox.Show("Please depress Keysight 34661A Front/Rear button.", "Paused, click OK to continue.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            SetDelay(SVI, MMD.DEFault);
            SetDelayAuto(SVI, true);
            SCPI99.Initialize(SVI);
        }

        public static TERMINALS GetTerminals(SCPI_VISA_Instrument SVI) {
            ((Ag3466x)SVI.Instrument).SCPI.ROUTe.TERMinals.Query(out String terminals);
            return String.Equals(terminals, "REAR") ? TERMINALS.Rear : TERMINALS.Front;
        }

        public static Double MeasureVDC(SCPI_VISA_Instrument SVI) {
            // SCPI FORMAT:DATA(ASCii/REAL) command unavailable on KS 34661A.
            ((Ag3466x)SVI.Instrument).SCPI.MEASure.VOLTage.DC.QueryAsciiRealClone(AUTO, DEFault, out Double voltsDC);
            return voltsDC;
        }

        public static Double MeasureADC(SCPI_VISA_Instrument SVI) {
            // SCPI FORMAT:DATA(ASCii/REAL) command unavailable on KS 34661A.
            ((Ag3466x)SVI.Instrument).SCPI.MEASure.CURRent.DC.QueryAsciiReal(AUTO, DEFault, out Double ampsDC);
            return ampsDC;
        }

        public static Double MeasureΩ(SCPI_VISA_Instrument SVI) {
            // SCPI FORMAT:DATA(ASCii/REAL) command unavailable on KS 34661A.
            ((Ag3466x)SVI.Instrument).SCPI.MEASure.RESistance.QueryAsciiReal(AUTO, DEFault, out Double resistance);
            return resistance;
        }

        public static void SetDelay(SCPI_VISA_Instrument SVI, MMD mmd) { ((Ag3466x)SVI.Instrument).SCPI.TRIGger.DELay.Command(Enum.GetName(typeof(MMD), mmd)); }

        public static void SetDelay(SCPI_VISA_Instrument SVI, Double Seconds) { ((Ag3466x)SVI.Instrument).SCPI.TRIGger.DELay.Command(Seconds); }

        public static Double GetDelay(SCPI_VISA_Instrument SVI) {
            ((Ag3466x)SVI.Instrument).SCPI.TRIGger.DELay.Query(MINimum, out Double seconds);
            return seconds;
        }

        public static void SetDelayAuto(SCPI_VISA_Instrument SVI, Boolean state) { ((Ag3466x)SVI.Instrument).SCPI.TRIGger.DELay.AUTO.Command(state); }
                
        public static Boolean IsDelayAuto(SCPI_VISA_Instrument SVI) {
            ((Ag3466x)SVI.Instrument).SCPI.TRIGger.DELay.AUTO.Query(out Boolean state);
            return state;
        }
    }
}