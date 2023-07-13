using System;
using System.Collections.Generic;

// All Agilent.CommandExpert.ScpiNet drivers are created by adding new SCPI VISA Instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new SVIs are added.
//  - The Agilent.CommandExpert.ScpiNet drivers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
// NOTE: Unlike all other classes in namespace ABT.TestSpace.TestExec.SCPI_VISA_Instruments, classes in SCPI_VISA utilize only VISA Addresses,
// not Instrument objects contained in their SCPI_VISA_Instrument objects.
namespace ABT.TestSpace.TestExec.SCPI_VISA_Instruments {
    public enum OUTPUT { off, ON }
    public enum LOGIC { low, HIGH }
    public enum BINARY { zero, ONE }
    public enum SENSE_MODE { EXTernal, INTernal }
    public enum CHANNELS { C1, C2 }

    // Consistent convention for lower-cased inactive states off/low/zero as 1st states in enums, UPPER-CASED active ON/HIGH/ONE as 2nd states.

    public static class SCPI {
        // This class contains properties & methods relevant to some SCPI VISA instruments, but which may not be SCPI99 specific or compliant.
        //  - Example; Keysight's 33509B Multi-Meter & 34661A Waveform Generator don't support output states, but their E3610xB & E36234A Power Supplies & EL34143A Electronic Load do.
        // TODO: Create methods for simple Commands/Queries applicable to multiples SCPI VISA instruments, invoked as SCPI99 Commands/Queries.
        //  - Thus passed directly to SCPI instrument interpreters with Keysight's AgSCPI99 SCPI-99 driver.
        //  - Eliminates instrument specific methods for each SCPI VISA instrument.
        //  - That is, won't need Get/Set/Is_OutputState() methods for the E3610xB & E36234A Power Supplies & EL34143A Electronic Load.
        // NOTE: Could also have declared class SCPI_VISA_Instrument's Instrument property as type dynamic, instead of Object.
        //  - This would permit invoking any applicable SCPI command directly on a SCPI_VISA_Instrument.Instrument property.
        //      - Given: public dynamic Instrument { get; private set; }
        //      - Then:  Instrument.SCPI.OUTPut.STATe.Query(out Boolean state); should work for E3610xB & E36234A Power Supplies & EL34143A Electronic Load.
        // - May ultimately implement this, obviating below methods and need to cast public Object Instrument to specific instrument, ala ((AgE3610XB)SVI.Instrument).
        public static readonly Dictionary<CHANNELS, String> Channels = new Dictionary<CHANNELS, String> {
            { CHANNELS.C1, "(@1)" },
            { CHANNELS.C2, "(@2)" }
        };

        public static OUTPUT GetOutputState(SCPI_VISA_Instrument SVI) {
            if (String.Equals(SCPI99.Query(SVI, ":OUTPUT?"), "0")) return OUTPUT.off;
            else return OUTPUT.ON;
        }

        public static void SetOutputState(SCPI_VISA_Instrument SVI, OUTPUT State) { SCPI99.Command(SVI, (State is OUTPUT.off) ? ":OUTPUT 0" : ":OUTPUT 1"); }

        public static Boolean IsOutputState(SCPI_VISA_Instrument SVI, OUTPUT State) { return (GetOutputState(SVI) == State); }

        internal static String GetErrorMessage(SCPI_VISA_Instrument SVI) { return SCPI_VISA_Instrument.GetInfo(SVI, $"SCPI VISA Instrument Address '{0}' failed.{Environment.NewLine}"); }

        internal static String GetErrorMessage(SCPI_VISA_Instrument SVI, String errorMessage) { return $"{GetErrorMessage(SVI)}{errorMessage}{Environment.NewLine}"; }

        internal static Boolean IsCloseEnough(Double D1, Double D2, Double Delta) { return Math.Abs(D1 - D2) <= Delta; }
        // Close is good enough for horseshoes, hand grenades, nuclear weapons, and Doubles!  Shamelessly plagiarized from the Internet!
    }
}