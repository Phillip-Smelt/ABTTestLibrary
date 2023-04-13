namespace TestLibrary.Switching {
    public static class RelayForms {
        // https://forum.digikey.com/t/understanding-form-a-form-b-form-c-contact-configuration/811
        public enum A { NO, C } // Form A
        // A.NO; relay is deenergized and in normally opened state.
        // A.C; relay is energized and in abnormally closed state.

        public enum B { NC, O } // Form B
        // B.NC; relay is deenergized and in normally closed state.
        // B.O; relay is energized and in abnormally opened state.

        public enum C { NC, NO } // Form C
        // C.NC; relay is deenergized and in normally closed/normally open state, with common C terminal connected to NC terminal & disconnected from NO terminal.
        // C.NO; relay is energized and in abnormally opened/abnormally closed state, with common C terminal disconnected from NC terminal & connected to NO terminal.
    }
}
