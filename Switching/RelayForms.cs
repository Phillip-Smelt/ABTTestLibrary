namespace ABT.TestSpace.TestExec.Switching {
    public static class RelayForms {
        // https://forum.digikey.com/t/understanding-form-a-form-b-form-c-contact-configuration/811

        public enum A { NO, C }
        // A.NO; Form A relay is deenergized and in normally opened state.
        // A.C;  Form A relay is energized and in abnormally closed state.

        public enum B { NC, O }
        // B.NC; Form B relay is deenergized and in normally closed state.
        // B.O;  Form B relay is energized and in abnormally opened state.

        public enum C { NC, NO }
        // C.NC; Form C relay is deenergized and in normally closed/normally open state, with common C terminal connected to NC terminal & disconnected from NO terminal.
        // C.NO; Form C relay is energized and in abnormally opened/abnormally closed state, with common C terminal disconnected from NC terminal & connected to NO terminal.

        //public enum STATE { Open, Closed }
        //public enum TERMINALS { C, NC, NO }

        //public abstract class FormA {
        //    public const A StateDefault = A.NO;
        //    public const HashSet<TERMINALS> Terminals = new HashSet<TERMINALS>() { TERMINALS.C, TERMINALS.NO };
        //    public abstract void SetState(A State);
        //    public abstract A GetState();
        //    public abstract Boolean IsState(A State);
        //}

        //public abstract class FormB : Relay {
        //    public new const B StateDefault = B.NC;
        //    public FormB(STATE StateInitial) : base(StateInitial) {
        //        this.Terminals = new HashSet<TERMINALS>() { TERMINALS.C, TERMINALS.NC };
        //    }
        //    public abstract void SetState(B State);
        //    public abstract B GetState();
        //    public abstract Boolean IsState(B State);
        //}

        //public abstract class FormC : Relay {
        //    public new const C StateDefault = C.NC;
        //    public FormC(STATE StateInitial) : base(StateInitial) {
        //        this.Terminals = new HashSet<TERMINALS>() { TERMINALS.C, TERMINALS.NC, TERMINALS.NO };
        //    }
        //    public abstract void SetState(C State);
        //    public abstract C GetState();
        //    public abstract Boolean IsState(C State);
        //}

        public sealed class FormC {
            public readonly USB_ERB24.UE24 RB;
            public readonly USB_ERB24.R R;
            public FormC(USB_ERB24.UE24 RB, USB_ERB24.R R) { this.RB = RB; this.R = R; }
        }
    }
}
