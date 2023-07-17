using System;

namespace ABT.TestSpace.TestExec.Switching {
    // UE24 is an abbreviation of the USB_ERB24 initialisation (Universal Serial Bus Electronic Relay Board with 24 Form C relays).
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
            if (this.C == N.NULL) throw new ArgumentException($"Relay terminal Common '{GetName(this.C)}' cannot be NULL.");
            if (this.C == this.NO) throw new ArgumentException($"Relay terminals Common '{GetName(this.C)}' & Normally Open '{GetName(this.NO)}' cannot be identical.");
            if (this.C == this.NC) throw new ArgumentException($"Relay terminals Common '{GetName(this.C)}' & Normally Closed '{GetName(this.NC)}' cannot be identical.");
            if (this.NC == this.NO) throw new ArgumentException($"Relay terminals Normally Closed '{GetName(this.NC)}' & Normally Open '{GetName(this.NO)}' cannot be identical.");
        }

        private String GetName(N ue24_net) { return Enum.GetName(typeof(N), ue24_net); }

        public FC.S Get() { return UE24.Get(this.B, this.R); }

        public void Set(FC.S state) { UE24.Set(this.B, this.R, state); }

        public Boolean Is(FC.S state) { return UE24.Is(this.B, this.R, state); }
    }
}
