using System;
using Agilent.CommandExpert.ScpiNet.Ag3466x_2_08;
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
    // TODO: Convert the MM_34661A class to a Singleton, like the USB_TO_GPIO class.
    //  - If there are more than one MM_34661A in the test system, make the MM_34661A Singleton class a Dictionary of MM_34661As, rather than just one MM_34661A.
    //  - Each MM_34661A in the Singleton's Dictionary can be accessed by its enum; MM.S01, MM.S02...MM.Snn, for Multi-Meter Singletons 01, 02...nn.
    public static class MM_34661A {
        public const String MODEL = "34461A";

        public static Boolean IsMM_34661A(SCPI_VISA_Instrument SVI) { return (SVI.Instrument.GetType() == typeof(Ag3466x)); }

        public static void Local(SCPI_VISA_Instrument SVI) { ((Ag3466x)SVI.Instrument).SCPI.SYSTem.LOCal.Command(); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            SCPI99.Initialize(SVI);
        }

        public static Double MeasureVDC(SCPI_VISA_Instrument SVI) {
            ((Ag3466x)SVI.Instrument).SCPI.MEASure.VOLTage.DC.QueryAsciiRealClone("AUTO", "MAXimum", out Double voltsDC);
            return voltsDC;
        }

        public static Double MeasureADC(SCPI_VISA_Instrument SVI) {
            ((Ag3466x)SVI.Instrument).SCPI.MEASure.CURRent.DC.QueryAsciiReal("AUTO", "MAXimum", out Double ampsDC);
            return ampsDC;
        }
    }
}