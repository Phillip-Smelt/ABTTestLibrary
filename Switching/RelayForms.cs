namespace TestLibrary.Switching {
    public static class RelayForms {
        // https://forum.digikey.com/t/understanding-form-a-form-b-form-c-contact-configuration/811
        public const string Cl = "Cl";
        public const string NO = "NO";
        public const string NC = "NC";
        public const string Op = "Op";
        // NOTE: Couldn't identify relay State Closed as desired "C", due to identifying relay Form C as "C".
        // - So identified State Closed as "Cl".
        // - To be semi-consistent, identified relay State Open as "Op".
        // - Currently don't have any Form A or Form B relays, so these identifier kludges will go unnoticed.
        // - Reason Form "C" identifier more critical than State "C" is it allows setting USB-ERB24 relays as "(C, NO)" & "(C, NC)".
        //   These concisely describe "Form C relay set to State Normally Open" & "Form C relay set to State Normally Closed".

        public enum A { NO, Cl } // Form A
        // A.NO; relay is deenergized and in normally opened state.
        // A.Cl; relay is energized and in abnormally closed state.

        public enum B { NC, Op } // Form B
        // B.NC; relay is deenergized and in normally closed state.
        // B.Op; relay is energized and in abnormally opened state.

        public enum C { NC, NO } // Form C
        // C.NC; relay is deenergized and in normally closed/normally open state, with common C terminal connected to NC terminal & disconnected from NO terminal.
        // C.NO; relay is energized and in abnormally opened/abnormally closed state, with common C terminal disconnected from NC terminal & connected to NO terminal.
    }
}
