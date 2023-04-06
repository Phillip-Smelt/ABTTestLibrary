using System;
using System.Collections.Generic;
using Agilent.CommandExpert.ScpiNet.Ag33500B_33600A_2_09;
using TestLibrary.AppConfig;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new SCPI VISA Instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new SVIs are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
// NOTE: Below hopefully "value-added" wrapper methods for some commonly used Ag33500B_33600A commands are conveniences, not necessities.
// NOTE: Will never fully implement wrapper methods for the complete set of Ag33500B_33600A commands, just some of the most commonly used ones.
// - In general, TestLibrary's InterfaceAdapters, Logging, SCPI_VISA_Instruments & Switching namespaces exist partly to eliminate
//   the need to reference TestLibrary's various DLLs directly from TestProgram client apps.
// - As long as suitable wrapper methods exists in WG_33509B, needn't directly reference Ag33500B_33600A_2_09
//   from TestProgram client apps, as referencing TestLibrary suffices.
namespace TestLibrary.SCPI_VISA_Instruments {
    public static class WG_33509B {
        public const String MODEL = "33509B";

        public static Boolean IsWG_33509(SCPI_VISA_Instrument SVI) { return (SVI.Instrument.GetType() == typeof(Ag33500B_33600A)); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            SCPI_VISA.Initialize(SVI);
            ((Ag33500B_33600A)SVI.Instrument).SCPI.DISPlay.TEXT.CLEar.Command();
        }
    }
}