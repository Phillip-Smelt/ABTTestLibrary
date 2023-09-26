﻿using System;
using Agilent.CommandExpert.ScpiNet.Ag33500B_33600A_2_09;

// All Agilent.CommandExpert.ScpiNet drivers are procured by adding new SCPI VISA Instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new SVIs are added.
//  - The Agilent.CommandExpert.ScpiNet drivers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Strenuously recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
// https://www.keysight.com/us/en/search.html/command+expert
//
namespace ABT.TestSpace.TestExec.SCPI_VISA_Instruments {
    public static class WG_33509B {
        public const String MODEL = "33509B";

        public static Boolean IsWG_33509(SCPI_VISA_Instrument SVI) { return (SVI.Instrument.GetType() == typeof(Ag33500B_33600A)); }

        public static void Initialize(SCPI_VISA_Instrument SVI) {
            SCPI99.Initialize(SVI);
            ((Ag33500B_33600A)SVI.Instrument).SCPI.DISPlay.TEXT.CLEar.Command();
        }

        public static OUTPUT Get(SCPI_VISA_Instrument SVI) {
            ((Ag33500B_33600A)SVI.Instrument).SCPI.OUTPut.Query(null, out Boolean State);
            return State ? OUTPUT.ON : OUTPUT.off;
        }

        public static Boolean Is(SCPI_VISA_Instrument SVI, OUTPUT State) { return (State == Get(SVI)); }

        public static void Set(SCPI_VISA_Instrument SVI, OUTPUT State) { ((Ag33500B_33600A)SVI.Instrument).SCPI.OUTPut.Command(null, (State is OUTPUT.ON)); }

        public static String WaveformGet(SCPI_VISA_Instrument SVI) {
            ((Ag33500B_33600A)SVI.Instrument).SCPI.SOURce.APPLy.Query(null, out String Waveform);
            return Waveform;
        }

        public static void WaveformSquareApply(SCPI_VISA_Instrument SVI, Double FrequencyHz, Double AmplitudeV, Double OffsetV) {
            ((Ag33500B_33600A)SVI.Instrument).SCPI.OUTPut.LOAD.Command(null, "INFinity");
            ((Ag33500B_33600A)SVI.Instrument).SCPI.SOURce.APPLy.SQUare.Command(null, FrequencyHz, "HZ", AmplitudeV, "V", OffsetV, "V");
        }
    }
}