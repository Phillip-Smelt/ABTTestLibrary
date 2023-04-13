namespace TestLibrary.Switching {
    public static class RelayForms {
        // https://forum.digikey.com/t/understanding-form-a-form-b-form-c-contact-configuration/811
        public const string NC = "NC";
        public const string NO = "NO";
        public const string Cl = "Cl";
        public const string Op = "Op";
        // NOTE: Identifying Form C relays as enum "C" more critical than identifying relay State Closed as const String "C":
        //  - This allows setting USB-ERB24's Form C relays as "(R#, NO)" & "(R#, NC)".
        //  - This concisely describes "Form C relay R# set to State Normally Open" & "Form C relay R# set to State Normally Closed".
        // NOTE: Couldn't identify relay State Closed as optimal "C", due to identifying relay Form C as "C".
        //  - So identified State Closed as "Cl".
        //  - To be semi-consistent, identified relay State Open as "Op".
        //  - Didn't want to identify States Normally Open & Normally Closed as "NOp" & "NCl", hence semi-consistency.
        //  - Currently don't have any Form A or Form B relays, so these identifier kludges will go unnoticed.
        //      - May eventually have to re-identify relay States Normally Open as "NOp" & Normally Closed as "NCl"

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
