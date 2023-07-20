using System;
using static ABT.TestSpace.TestExec.Switching.UE24;

namespace ABT.TestSpace.TestExec.Switching {
    // UE24 is an abbreviation of the USB_ERB24 initialisation (Universal Serial Bus Electronic Relay Board with 24 Form C relays).

    public sealed class UE24_T {
        public readonly B B;
        public readonly R R;
        public readonly FC.T T;

        public UE24_T(B b, R r, FC.T t) { this.B = b; this.R = r; this.T = t; }

        public override Boolean Equals(Object obj) {
            UE24_T ue24_t = obj as UE24_T;
            if (ReferenceEquals(this, ue24_t)) return true;
            return ue24_t != null && ue24_t.B == this.B && ue24_t.R == this.R && this.T == ue24_t.T;
        }

        public override Int32 GetHashCode() { return 3 * this.B.GetHashCode() + this.R.GetHashCode() + this.T.GetHashCode(); }
    }

    public class UE24_R {
        public readonly UE24.B B;
        public readonly UE24.R R;
        public readonly N C;
        public readonly N NC;
        public readonly N NO;

        public UE24_R(UE24.B B, UE24.R R, N C, N NC, N NO) { 
            this.B = B; this.R = R; this.C = C; this.NC = NC; this.NO = NO;
            Validate();
        }
        
        private void Validate() {
            if (this.C == N.NULL) throw new ArgumentException($"Relay terminal Common '{GetN(this.C)}' cannot be NULL.");
            if (this.C == this.NO) throw new ArgumentException($"Relay terminals Common '{GetN(this.C)}' & Normally Open '{GetN(this.NO)}' cannot be identical.");
            if (this.C == this.NC) throw new ArgumentException($"Relay terminals Common '{GetN(this.C)}' & Normally Closed '{GetN(this.NC)}' cannot be identical.");
            if (this.NC == this.NO) throw new ArgumentException($"Relay terminals Normally Closed '{GetN(this.NC)}' & Normally Open '{GetN(this.NO)}' cannot be identical.");
        }

        public static String GetB(UE24.B b) { return Enum.GetName(typeof(UE24.B), b); }
        public static String GetR(UE24.R r) { return Enum.GetName(typeof(UE24.R), r); }
        public static String GetN(N n) { return Enum.GetName(typeof(N), n); }

        public FC.S Get() { return UE24.Get(this.B, this.R); }

        public void Set(FC.S state) { UE24.Set(this.B, this.R, state); }

        public Boolean Is(FC.S state) { return UE24.Is(this.B, this.R, state); }

        public override Boolean Equals(Object obj) {
            UE24_R ue24_r = obj as UE24_R;
            if (ReferenceEquals(this, ue24_r)) return true;
            return ue24_r != null && ue24_r.B == this.B && ue24_r.R == this.R;
        }

        public override Int32 GetHashCode() { return 3 * this.B.GetHashCode() + this.R.GetHashCode(); }
    }
}
