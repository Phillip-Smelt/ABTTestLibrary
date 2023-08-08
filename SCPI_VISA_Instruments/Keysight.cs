﻿using System;
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
    public static class Keysight {
        public enum CHANNELS { C1, C2 }

        public static readonly Dictionary<CHANNELS, String> Channels = new Dictionary<CHANNELS, String> {
            { CHANNELS.C1, "(@1)" },
            { CHANNELS.C2, "(@2)" }
        };
        
        public const String AUTO = "AUTO";
        public const String MINimum = "MINimum";
        public const String MAXimum = "MAXimum";
        public const String DEFault = "DEFault";
    }
}