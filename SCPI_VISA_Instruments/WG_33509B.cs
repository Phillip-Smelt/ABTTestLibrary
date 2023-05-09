using System;
using Agilent.CommandExpert.ScpiNet.Ag33500B_33600A_2_09;
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
    // TODO: Convert the WG_33509B class to a Singleton, like the USB_TO_GPIO class?
    //  - Realize Singletons often considered "anti-patterns", but handy for objects that can only have 1 instance.
    //  - If there are more than one WG_33509B in the test system, make the WG_33509B Singleton class a Dictionary of WG_33509Bs, rather than just one WG_33509B.
    //  - Each WG_33509B in the Singleton's Dictionary can be accessed by its enum; WG.S01, WG.S02...WG.Snn, for Wave Generator Singletons 01, 02...nn.
    public static class WG_33509B {
        public const String MODEL = "33509B";

        public static Boolean IsWG_33509(SCPI_VISA_Instrument SVI) { return (SVI.Instrument.GetType() == typeof(Ag33500B_33600A)); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            SCPI99.Initialize(SVI);
            ((Ag33500B_33600A)SVI.Instrument).SCPI.DISPlay.TEXT.CLEar.Command();
        }

        public static void Off(SCPI_VISA_Instrument SVI) {
            ((Ag33500B_33600A)SVI.Instrument).SCPI.RST.Command();
            ((Ag33500B_33600A)SVI.Instrument).SCPI.OUTPut.Command(null, false);
        }

        public static void ApplyWaveformSquare(SCPI_VISA_Instrument SVI, Double FrequencyHz, Double AmplitudeV, Double OffsetV) {
            ((Ag33500B_33600A)SVI.Instrument).SCPI.SOURce.APPLy.SQUare.Command(null, FrequencyHz, "HZ", AmplitudeV, "V", OffsetV, "V");
        }

        public static Boolean IsOff(SCPI_VISA_Instrument SVI) {
            ((Ag33500B_33600A)SVI.Instrument).SCPI.OUTPut.Query(null, out Boolean Output);
            return Output == false;
        }

        public static Boolean IsOn(SCPI_VISA_Instrument SVI) { return !IsOff(SVI); }
        
        public static String GetWaveform(SCPI_VISA_Instrument SVI) {
            ((Ag33500B_33600A)SVI.Instrument).SCPI.SOURce.APPLy.Query(null, out String Waveform);
            return Waveform;
        }
    }
}