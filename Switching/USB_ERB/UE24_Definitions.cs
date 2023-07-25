using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ABT.TestSpace.TestExec.Switching.Forms;
using static ABT.TestSpace.TestExec.Switching.USB_ERB.UE24;
using static ABT.TestSpace.TestExec.Switching.USB_ERB.UE24_R;

namespace ABT.TestSpace.TestExec.Switching.USB_ERB {
    // UE24 is an abbreviation of Measurement Computing Corporation's USB-ERB24 initialisation (Universal Serial Bus Electronic Relay Board with 24 Form C relays).
    public enum N {
        /// <summary>
        /// Nets.
        /// </summary>
        CTL_1_3, CTL_4_6,
        DMM_IN_P, DMM_IN_N, DMM_I,
        ENABLE_N,
        FAN_PWR,
        POWER_GOOD,
        PRE_BIAS_OUT_P, PRE_BIAS_PS,
        PRI_3V3, PRI_BIAS, PRI_BIAS_OUT_P, PRI_BIAS_PS,
        SCL, SDA,
        SEC_3V3, SEC_BIAS, SEC_BIAS_OUT_P, SEC_BIAS_PS,
        START_SYNC, SYNC,
        VDC_5, VDC_5_RTN,
        VIN_Sense, VIN_RTN_Sense,
        VOUT_Sense, VOUT_RTN_Sense,
        NULL // NULL acts as N's substitute for null, since enumeration elements cannot be null.  Assign to UE24_R NC or NO terminals that aren't connected.
    }

    public sealed class UE24_T {
        /// <summary>
        /// USB-ERB24 Terminal.
        /// </summary>
        public readonly UE24.UE UE;
        public readonly R R;
        public readonly C.T T;

        public UE24_T(UE24.UE UE, R R, C.T T) { this.UE = UE; this.R = R; this.T = T; }

        public override Boolean Equals(Object obj) {
            UE24_T ue24_t = obj as UE24_T;
            if (ReferenceEquals(this, ue24_t)) return true;
            return ue24_t != null && ue24_t.UE == this.UE && ue24_t.R == this.R && this.T == ue24_t.T;
        }

        public override Int32 GetHashCode() { return 3 * this.UE.GetHashCode() + this.R.GetHashCode() + this.T.GetHashCode(); }
    }

    public sealed class UE24_R {
        /// <summary>
        /// USB-ERB24 Relay.
        /// </summary>
        public readonly UE UE;
        public readonly R R;
        public readonly N C;
        public readonly N NC;
        public readonly N NO;

        public UE24_R(UE UE, R R, N C, N NC, N NO) {
            this.UE = UE; this.R = R; this.C = C; this.NC = NC; this.NO = NO;
            Validate();
        }

        private void Validate() {
            if (this.C == N.NULL) throw new ArgumentException($"Relay terminal Common '{GetN(this.C)}' cannot be NULL.");
            if (this.C == this.NO) throw new ArgumentException($"Relay terminals Common '{GetN(this.C)}' & Normally Open '{GetN(this.NO)}' cannot be identical.");
            if (this.C == this.NC) throw new ArgumentException($"Relay terminals Common '{GetN(this.C)}' & Normally Closed '{GetN(this.NC)}' cannot be identical.");
            if (this.NC == this.NO) throw new ArgumentException($"Relay terminals Normally Closed '{GetN(this.NC)}' & Normally Open '{GetN(this.NO)}' cannot be identical.");
        }

        public static String GetUE(UE ue) { return Enum.GetName(typeof(UE), ue); }
        public static String GetR(R r) { return Enum.GetName(typeof(R), r); }
        public static String GetN(N n) { return Enum.GetName(typeof(N), n); }

        public C.S Get() { return UE24.Get(this.UE, this.R); }

        public void Set(C.S state) { UE24.Set(this.UE, this.R, state); }

        public Boolean Is(C.S state) { return UE24.Is(this.UE, this.R, state); }

        public override Boolean Equals(Object obj) {
            UE24_R ue24_r = obj as UE24_R;
            if (ReferenceEquals(this, ue24_r)) return true;
            return ue24_r != null && ue24_r.UE == this.UE && ue24_r.R == this.R;
        }

        public override Int32 GetHashCode() { return 3 * this.UE.GetHashCode() + this.R.GetHashCode(); }
    }


public abstract class UE24_Rs {
        public readonly HashSet<UE24_R> Rs;
        public readonly Dictionary<N, HashSet<UE24_T>> NTs = new Dictionary<N, HashSet<UE24_T>>();

        public UE24_Rs() {
            this.Rs = new HashSet<UE24_R>() {
                { new UE24_R(UE.B0, R.C01, C: N.DMM_IN_P, NC: N.NULL, NO: N.VIN_Sense) },
                { new UE24_R(UE.B0, R.C02, C: N.DMM_IN_N, NC: N.NULL, NO: N.VIN_Sense) },
                { new UE24_R(UE.B0, R.C03, C: N.DMM_IN_P, NC: N.NULL, NO: N.VOUT_Sense) },
                { new UE24_R(UE.B0, R.C04, C: N.DMM_IN_N, NC: N.NULL, NO: N.VOUT_Sense) },
                { new UE24_R(UE.B0, R.C05, C: N.DMM_IN_P, NC: N.NULL, NO: N.PRI_3V3) },
                { new UE24_R(UE.B0, R.C06, C: N.DMM_IN_N, NC: N.NULL, NO: N.PRI_3V3) },
                { new UE24_R(UE.B0, R.C07, C: N.DMM_IN_P, NC: N.NULL, NO: N.SEC_3V3) },
                { new UE24_R(UE.B0, R.C08, C: N.DMM_IN_N, NC: N.NULL, NO: N.SEC_3V3) },
                { new UE24_R(UE.B0, R.C09, C: N.DMM_IN_P, NC: N.NULL, NO: N.POWER_GOOD) },
                { new UE24_R(UE.B0, R.C10, C: N.DMM_IN_N, NC: N.NULL, NO: N.POWER_GOOD) },
                { new UE24_R(UE.B0, R.C11, C: N.DMM_IN_P, NC: N.NULL, NO: N.VIN_RTN_Sense) },
                { new UE24_R(UE.B0, R.C12, C: N.DMM_IN_N, NC: N.NULL, NO: N.VOUT_RTN_Sense) },
                { new UE24_R(UE.B0, R.C13, C: N.DMM_IN_P, NC: N.NULL, NO: N.PRI_BIAS) },
                { new UE24_R(UE.B0, R.C14, C: N.DMM_IN_N, NC: N.NULL, NO: N.PRI_BIAS) },
                { new UE24_R(UE.B0, R.C15, C: N.DMM_IN_P, NC: N.NULL, NO: N.SEC_BIAS) },
                { new UE24_R(UE.B0, R.C16, C: N.DMM_IN_N, NC: N.NULL, NO: N.SEC_BIAS) },
                { new UE24_R(UE.B0, R.C17, C: N.DMM_IN_P, NC: N.NULL, NO: N.SYNC) },
                { new UE24_R(UE.B0, R.C18, C: N.DMM_IN_N, NC: N.NULL, NO: N.SYNC) },
                { new UE24_R(UE.B0, R.C19, C: N.DMM_IN_P, NC: N.NULL, NO: N.ENABLE_N) },
                { new UE24_R(UE.B0, R.C20, C: N.DMM_IN_N, NC: N.NULL, NO: N.ENABLE_N) },
                { new UE24_R(UE.B0, R.C21, C: N.DMM_IN_P, NC: N.NULL, NO: N.START_SYNC) },
                { new UE24_R(UE.B0, R.C22, C: N.DMM_IN_N, NC: N.NULL, NO: N.START_SYNC) },
                { new UE24_R(UE.B0, R.C23, C: N.DMM_IN_P, NC: N.NULL, NO: N.SCL) },
                { new UE24_R(UE.B0, R.C24, C: N.DMM_IN_N, NC: N.NULL, NO: N.SCL) },
                { new UE24_R(UE.B1, R.C01, C: N.DMM_IN_P, NC: N.NULL, NO: N.SDA) },
                { new UE24_R(UE.B1, R.C02, C: N.DMM_IN_N, NC: N.NULL, NO: N.SDA) },
                { new UE24_R(UE.B1, R.C03, C: N.NULL, NC: N.NULL, NO: N.FAN_PWR) },
                { new UE24_R(UE.B1, R.C04, C: N.VDC_5_RTN, NC: N.NULL, NO: N.CTL_1_3) },
                { new UE24_R(UE.B1, R.C05, C: N.VDC_5_RTN, NC: N.NULL, NO: N.CTL_4_6) },
                { new UE24_R(UE.B1, R.C06, C: N.PRI_BIAS_OUT_P, NC: N.PRI_BIAS_PS, NO: N.DMM_I) },
                { new UE24_R(UE.B1, R.C07, C: N.SEC_BIAS_OUT_P, NC: N.SEC_BIAS_PS, NO: N.DMM_I) },
                { new UE24_R(UE.B1, R.C08, C: N.PRE_BIAS_OUT_P, NC: N.PRE_BIAS_PS, NO: N.DMM_I) },
                { new UE24_R(UE.B1, R.C09, C: N.DMM_IN_N, NC: N.NULL, NO: N.DMM_IN_N) },
                { new UE24_R(UE.B1, R.C10, C: N.DMM_IN_N, NC: N.NULL, NO: N.DMM_IN_N) },
                { new UE24_R(UE.B1, R.C11, C: N.DMM_IN_N, NC: N.NULL, NO: N.DMM_IN_N) },
            };

            ValidateRs();

            //foreach (UE24_R r in this.Rs) {
            //    if (!this.NTs.ContainsKey(r.C)) 
            //}
        }

        private void ValidateRs() {
            StringBuilder sb = new StringBuilder($"Cannot currently accomodate USB-ERB24 Relays connected serially:{Environment.NewLine}Boards/Relays");
            List<(UE24_R, UE24_R)> rs = 
                (from r1 in this.Rs
                 from r2 in this.Rs
                 where (r1.C == r2.NC || r1.C == r2.NO)
                 select (r1, r2)).ToList();
            if (rs.Count() != 0) {
                foreach ((UE24_R r1, UE24_R r2) rr in rs) {
                    sb.AppendLine("Below relay pair {R1, R2} serially connected, C1 to (NC2 ⨁ NO2)");
                    sb.AppendLine($"   B1='{GetUE(rr.r1.UE)}', R1='{GetR(rr.r1.R)}', C1='{GetN(rr.r1.C)}', NC1='{GetN(rr.r1.NC)}', NO1='{GetN(rr.r1.NO)}'.");
                    sb.AppendLine($"   B2='{GetUE(rr.r2.UE)}', R2='{GetR(rr.r2.R)}', C2='{GetN(rr.r2.C)}', NC2='{GetN(rr.r2.NC)}', NO2='{GetN(rr.r2.NO)}'.");
                    sb.AppendLine("");
                }
                throw new InvalidOperationException(sb.ToString());
            }
        }

        //public abstract class UE24_NT {
        //    public readonly Dictionary<N, HashSet<T>> NT;
        //    UE24_NT() {
        //        this.NT = new Dictionary<N, HashSet<T>>() {
        //            {N.CTL_1_3, new HashSet<T>() {new T(UE.B1, R.C04, C.T.NO)}},
        //            {N.CTL_4_6, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.DMM_IN_P, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.DMM_IN_N, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.DMM_I, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.ENABLE_N, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.FAN_PWR, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.POWER_GOOD, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.PRE_BIAS_OUT_P, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.PRE_BIAS_OUT_N, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.PRE_BIAS_PS, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.PRE_BIAS_PS_RTN, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.PRI_3V3, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.PRI_BIAS, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.PRI_BIAS_OUT_P, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.PRI_BIAS_OUT_N, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.PRI_BIAS_PS, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.PRI_BIAS_PS_RTN, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.SCL, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.SDA, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.SEC_3V3, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.SEC_BIAS, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.SEC_BIAS_OUT_P, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.SEC_BIAS_OUT_N, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.SEC_BIAS_PS, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.SEC_BIAS_PS_RTN, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.START_SYNC, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.SYNC, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.VDC_5, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.VDC_5_RTN, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.VIN_Sense, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.VIN_RTN_Sense, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}},
        //            {N.VOUT_RTN_Sense, new HashSet<T>() {new T(UE.B0, R.C01, C.T.C)}}
        //        };

        //        Validate();
        //    }

        //private void Validate() {
        //    HashSet<N> missing = new HashSet<N>();
        //    foreach (N n in Enum.GetValues(typeof(N))) if (!this.NT.ContainsKey(n)) missing.Add(n);
        //    if (missing.Count != 0) throw new InvalidOperationException($"Dictionary UE24_NTs.NT does not contain N '{String.Join(", ", missing)}'.");

        //    foreach (KeyValuePair<N, HashSet<T>> kvp in this.NT) if (kvp.Value.Count == 0) missing.Add(kvp.Key);
        //    if (missing.Count != 0) throw new InvalidOperationException($"Dictionary UE24_NetsToTs.NT N correlate to empty HashSet<T> '{String.Join(", ", missing)}'.");

        //    Dictionary<N, HashSet<T>> duplicates = new Dictionary<N, HashSet<T>>();
        //    HashSet<T> ts = new HashSet<T>();
        //    foreach (KeyValuePair<N, HashSet<T>> kvp in this.NT) {
        //        foreach (T t in kvp.Value) {
        //            if (ts.Contains(t)) {
        //                if (duplicates.ContainsKey(kvp.Key)) duplicates[kvp.Key].Add(t);
        //                else duplicates.Add(kvp.Key, new HashSet<T> { t });
        //            }
        //            ts.Add(t);
        //        }
        //    }
        //    if (duplicates.Count != 0) {
        //        StringBuilder sb = new StringBuilder($"Dictionary UE24_NetsToTs.NT has duplicated Ts:{Environment.NewLine}");
        //        foreach (KeyValuePair<N, HashSet<T>> kvp in duplicates) sb.AppendLine($"Key '{kvp.Key}' T '{kvp.Value}'.");
        //        throw new InvalidOperationException(sb.ToString());
        //    }
        //    // - Verify every unique UE & R has at least a T for T.C and one/both for { T.NC and/or T.NO }
        //    //   - That is, every Form C relay Common terminal is connected to a N, and one/both of it's Normally Closed/Normally Open terminals
        //    //     are connected to N.
        //    // - Verify every T per unique B & Rpair has different N on each T.
        //    //   - That is, verify the Common, Normally Open & Normally Closed terminals are all different N.
        //}

        public static void Connect(N N1, N N2) {
            // Intersect, Verify & Connect:
            // - Index this.NT to get the HashSet<T> correlated to N1; call it HashSet<T1>.
            // - Index this.NT to get the HashSet<T> correlated to N2; call it HashSet<T2>.
            // - HashSet.Intersect(T1, T2) to get resulting HashSet<T> that have matching UE & R pairs.
            //   - If there are no T intersections with matching B & Rpairs, throw ArgumentException.
            // - Verify "You can get there from here":
            //   - Verify for each relay UE & Rpairs one is a Common terminal, the other a Normally Closed or Open terminal.
            //   - If not throw ArgumentException.
            //   - There should be at least one B & Rmatching T pair that can connect N1 to N2.
            //   - There may be multiple UE & R matching T pairs that can can connect N1 to N2.
            //     - Possibly for current capacity/ampacity, 4 wire Kelvin sensing or intentional duplications.  Or unintentional duplications :-)
            // - Connect all available Ts to Ns N1 & N2 using UE24 class:
            //   - Will always be either C to NC or C to NO path to connect N1 to N2.
            //   - If C to NC connects N1 to N2, invoke Set(UE, R, C.S.NC).
            //   - If C to NO connects N1 to N2, invoke Set(UE, R, C.S.NO).
        }

        public static void Connect(HashSet<N> Ns) {
            // Connect all HashSet<N> Ns simultaneously to one another.
            // Superset of Connect(N N1, N N2).
            // Sequentially invoke Connect(N N1, N N2) with foreach N in Ns.
            // Then invoke AreConnected((HashSet<N> Ns) to verify all are still *simultaneously* connected.
            // - Possible to wire N1 to T.C, N2 to T.NO & Net3 to T.NC all having matching Relay addresses (UE, R).
            // - Invoking Connect(HashSet<N1, N2, Net3>) is impossible to simultaneously achieve.
            // - But will easily be sequentially achieved by Connect(N1, N2) & Connect(N2, Net3).
            // Used for Shorts/Opens testing.
        }

        public static void DisConnect(N N1, N N2) {
            // Same as Connect, except disconnect N1 from N2 with opposite C state:
            // - If C to NC connects N1 to N2, invoke invoke Set(UE, C, C.NO).
            // - If C to NO connects N1 to N2, invoke invoke Set(UE, C, C.NC).
        }

        public static void DisConnect(HashSet<N> Ns) {
            // Disonnect all HashSet<N> Ns Ns simultaneously from one another.
            // Superset of DisConnect(N N1, N N2).
            // Sequentially invoke DisConnect(N N1, N N2) with foreach N in Ns.
            // Then invoke AreDisConnected((HashSet<N> Ns) to verify all are still *simultaneously* disconnected.
            // - Possible to wire N1 to T.C, N2 to T.NO & Net3 to T.NC all having matching Relay addresses (UE, R).
            // - Invoking DisConnect(HashSet<N1, N2, Net3>) is impossible to simultaneously achieve.
            // - But will easily be sequentially achieved by DisConnect(N1, N2) & Connect(N2, Net3).
            // Used for Shorts/Opens testing.
        }

        public static Boolean AreConnected(N N1, N N2) {
            // Verify/refute N1 currently connected to N2.
            // Use GetConnections(N1) returning HashSet<N>.Intersect<N2> to verify/refute if N1 currently connected to N2.
            // For Debug.Assert() statements.
            return false;
        }

        public static Boolean AreDisConnected(N N1, N N2) { return !AreConnected(N1, N2); }

        public static Boolean AreConnected(HashSet<N> Ns) {
            // Verify/refute all Ns in HashSet<N> are interconnected to one another.
            // Superset of AreConnected(N N1, N N2).
            // Can recursively invoke AreConnected(N N1, N N2) with foreach N in Ns.
            // For Debug.Assert() statements.
            return false;
        }

        public static Boolean AreDisConnected(HashSet<N> Ns) {
            return false;
        }

        private static Boolean AreConnectable(N N1, N N2) {
            // Verify/refute N1 can be connected to N2.
            // Use GetConnections(N1) returning HashSet<N>.Intersect<N2> to verify/refute if N1 currently connected to N2.
            // Reccommend programming 
            // For Debug.Assert() statements.
            return false;
        }

        private static Boolean AreConnectable(HashSet<N> Ns) {
            // Verify/refute HashSet<N> Ns can be interconnected to one another.
            // Use AreConnectable(N1) returning HashSet<N>.Intersect<N2> to verify/refute if N1 connected to N2.
            // Reccommend programming 
            // For Debug.Assert() statements.
            return false;
        }

        public static Dictionary<N, HashSet<N>> GetConnections(N N1) {
            return new Dictionary<N, HashSet<N>>();
            // Use Get() function, returning Dictionary<UE, Dictionary<R, C.S>>, and convert to Dictionary<N, HashSet<N>>.
        }
        // If can convert:
        //      - Dictionary<N, HashSet<T>>
        // to/from:
        //      - Dictionary<UE, Dictionary<R, C.S>>
        // Can invoke:
        //       - Set(Dictionary<UE, Dictionary<R, C.S>> UEεRεS)
        //       - Are(Dictionary<UE, Dictionary<R, C.SC>> UEεRεS)
        //       - Get()
        //
        // Initially support UE24's Set(), Is() & Get() functions for discrete/single (UE, R, C.S)
    }
}
