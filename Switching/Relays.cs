namespace ABT.TestSpace.TestExec.Switching {
    public static class FA {
        public enum S { NO, C }
        // Form A States. 
        // S.NO; Form A relay is deenergized and in normally opened state.
        // S.C;  Form A relay is energized and in abnormally closed state.

        public enum T { C, NO }
        // Form A Terminals.
        // T.C; Form A relay Common terminal.
        // T.NO; Form A relay Normally Open terminal.
    }

    public static class FB {
        public enum S { NC, O }
        // Form B States.
        // B.NC; Form B relay is deenergized and in normally closed state.
        // B.O;  Form B relay is energized and in abnormally opened state.

        public enum T { C, NC }
        // Form B Terminals.
        // T.C; Form B relay Common terminal.
        // T.NC; Form B relay Normally Closed terminal.
    }

    public static class FC {
        public enum S { NC, NO }
        // Form C States.
        // S.NC; Form C relay is deenergized and in normally closed/normally open state, with common C terminal connected to NC terminal & disconnected from NO terminal.
        // S.NO; Form C relay is energized and in abnormally opened/abnormally closed state, with common C terminal disconnected from NC terminal & connected to NO terminal.

        public enum T { C, NC, NO }
        // Form C Terminals.
        // T.C; Form C relay Common terminal.
        // T.NO; Form C relay Normally Closed terminal.
        // T.NO; Form C relay Normally Open terminal.
    }
}
