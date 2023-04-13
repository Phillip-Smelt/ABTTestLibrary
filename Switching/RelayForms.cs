namespace TestLibrary.Switching {
    public static class RelayForms {
        // https://forum.digikey.com/t/understanding-form-a-form-b-form-c-contact-configuration/811
        public const string NC = "NC";
        public const string NO = "NO";
        public const string C  = "C";
        public const string O  = "O";

        public enum FORM_A { NO, C }
        // FORM_A.NO; Form A relay is deenergized and in normally opened state.
        // FORM_A.C;  relay is energized and in abnormally closed state.

        public enum FORM_B { NC, O }
                                 // Form B.NC; relay is deenergized and in normally closed state.
                                 // Form B.O;  relay is energized and in abnormally opened state.

        public enum FORM_C { NC, NO }
        // FORM_C.NC; Form C relay is deenergized and in normally closed/normally open state, with common C terminal connected to NC terminal & disconnected from NO terminal.
        // FORM_C.NO; Form C relay is energized and in abnormally opened/abnormally closed state, with common C terminal disconnected from NC terminal & connected to NO terminal.
    }
}
