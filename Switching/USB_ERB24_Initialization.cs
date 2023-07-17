using System;
using System.Collections.Generic;
using System.Text;
using static ABT.TestSpace.TestExec.Switching.UE24;

namespace ABT.TestSpace.TestExec.Switching {
    // UE24 is an abbreviation of the USB_ERB24 initialisation (Universal Serial Bus Electronic Relay Board with 24 Form C relays).
    public enum N {
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
        NULL // NULL acts as N's substitute for null, since enumerations cannot be null.  Assign to UE24_R NC or NO terminals that aren't connected.
    }

    public abstract class UE24_Rs {
        public readonly HashSet<UE24_R> Rs;

        public UE24_Rs() {
            this.Rs = new HashSet<UE24_R>() {
                { new UE24_R(B.B0, R.C01, C: N.DMM_IN_P, NC: N.NULL, NO: N.VIN_Sense) },
                { new UE24_R(B.B0, R.C02, C: N.DMM_IN_N, NC: N.NULL, NO: N.VIN_Sense) },
                { new UE24_R(B.B0, R.C03, C: N.DMM_IN_P, NC: N.NULL, NO: N.VOUT_Sense) },
                { new UE24_R(B.B0, R.C04, C: N.DMM_IN_N, NC: N.NULL, NO: N.VOUT_Sense) },
                { new UE24_R(B.B0, R.C05, C: N.DMM_IN_P, NC: N.NULL, NO: N.PRI_3V3) },
                { new UE24_R(B.B0, R.C06, C: N.DMM_IN_N, NC: N.NULL, NO: N.PRI_3V3) },
                { new UE24_R(B.B0, R.C07, C: N.DMM_IN_P, NC: N.NULL, NO: N.SEC_3V3) },
                { new UE24_R(B.B0, R.C08, C: N.DMM_IN_N, NC: N.NULL, NO: N.SEC_3V3) },
                { new UE24_R(B.B0, R.C09, C: N.DMM_IN_P, NC: N.NULL, NO: N.POWER_GOOD) },
                { new UE24_R(B.B0, R.C10, C: N.DMM_IN_N, NC: N.NULL, NO: N.POWER_GOOD) },
                { new UE24_R(B.B0, R.C11, C: N.DMM_IN_P, NC: N.NULL, NO: N.VIN_RTN_Sense) },
                { new UE24_R(B.B0, R.C12, C: N.DMM_IN_N, NC: N.NULL, NO: N.VOUT_RTN_Sense) },
                { new UE24_R(B.B0, R.C13, C: N.DMM_IN_P, NC: N.NULL, NO: N.PRI_BIAS) },
                { new UE24_R(B.B0, R.C14, C: N.DMM_IN_N, NC: N.NULL, NO: N.PRI_BIAS) },
                { new UE24_R(B.B0, R.C15, C: N.DMM_IN_P, NC: N.NULL, NO: N.SEC_BIAS) },
                { new UE24_R(B.B0, R.C16, C: N.DMM_IN_N, NC: N.NULL, NO: N.SEC_BIAS) },
                { new UE24_R(B.B0, R.C17, C: N.DMM_IN_P, NC: N.NULL, NO: N.SYNC) },
                { new UE24_R(B.B0, R.C18, C: N.DMM_IN_N, NC: N.NULL, NO: N.SYNC) },
                { new UE24_R(B.B0, R.C19, C: N.DMM_IN_P, NC: N.NULL, NO: N.ENABLE_N) },
                { new UE24_R(B.B0, R.C20, C: N.DMM_IN_N, NC: N.NULL, NO: N.ENABLE_N) },
                { new UE24_R(B.B0, R.C21, C: N.DMM_IN_P, NC: N.NULL, NO: N.START_SYNC) },
                { new UE24_R(B.B0, R.C22, C: N.DMM_IN_N, NC: N.NULL, NO: N.START_SYNC) },
                { new UE24_R(B.B0, R.C23, C: N.DMM_IN_P, NC: N.NULL, NO: N.SCL) },
                { new UE24_R(B.B0, R.C24, C: N.DMM_IN_N, NC: N.NULL, NO: N.SCL) },
                { new UE24_R(B.B1, R.C01, C: N.DMM_IN_P, NC: N.NULL, NO: N.SDA) },
                { new UE24_R(B.B1, R.C02, C: N.DMM_IN_N, NC: N.NULL, NO: N.SDA) },
                { new UE24_R(B.B1, R.C03, C: N.NULL, NC: N.NULL, NO: N.FAN_PWR) },
                { new UE24_R(B.B1, R.C04, C: N.VDC_5_RTN, NC: N.NULL, NO: N.CTL_1_3) },
                { new UE24_R(B.B1, R.C05, C: N.VDC_5_RTN, NC: N.NULL, NO: N.CTL_4_6) },
                { new UE24_R(B.B1, R.C06, C: N.PRI_BIAS_OUT_P, NC: N.PRI_BIAS_PS, NO: N.DMM_I) },
                { new UE24_R(B.B1, R.C07, C: N.SEC_BIAS_OUT_P, NC: N.SEC_BIAS_PS, NO: N.DMM_I) },
                { new UE24_R(B.B1, R.C08, C: N.PRE_BIAS_OUT_P, NC: N.PRE_BIAS_PS, NO: N.DMM_I) },
                { new UE24_R(B.B1, R.C09, C: N.DMM_IN_N, NC: N.NULL, NO: N.DMM_IN_N) },
                { new UE24_R(B.B1, R.C10, C: N.DMM_IN_N, NC: N.NULL, NO: N.DMM_IN_N) },
                { new UE24_R(B.B1, R.C11, C: N.DMM_IN_N, NC: N.NULL, NO: N.DMM_IN_N) },
            };
        }
    }

    public abstract class UE24_NetsToBRTs {
        public readonly Dictionary<N, HashSet<BRT>> NBRT;
        UE24_NetsToBRTs() {
            this.NBRT = new Dictionary<N, HashSet<BRT>>() {
                {N.CTL_1_3, new HashSet<BRT>() {new BRT(B.B1, R.C04, FC.T.NO)}},
                {N.CTL_4_6, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.DMM_IN_P, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.DMM_IN_N, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.DMM_I, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.ENABLE_N, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.FAN_PWR, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.POWER_GOOD, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.PRE_BIAS_OUT_P, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.PRE_BIAS_OUT_N, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.PRE_BIAS_PS, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.PRE_BIAS_PS_RTN, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.PRI_3V3, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.PRI_BIAS, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.PRI_BIAS_OUT_P, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.PRI_BIAS_OUT_N, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.PRI_BIAS_PS, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.PRI_BIAS_PS_RTN, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.SCL, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.SDA, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.SEC_3V3, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.SEC_BIAS, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.SEC_BIAS_OUT_P, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.SEC_BIAS_OUT_N, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.SEC_BIAS_PS, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.SEC_BIAS_PS_RTN, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.START_SYNC, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.SYNC, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.VDC_5, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.VDC_5_RTN, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.VIN_Sense, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.VIN_RTN_Sense, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}},
                {N.VOUT_RTN_Sense, new HashSet<BRT>() {new BRT(B.B0, R.C01, FC.T.C)}}
            };

            Validate();
        }

        private void Validate() {
            HashSet<N> missing = new HashSet<N>();
            foreach (N n in Enum.GetValues(typeof(N))) if (!this.NBRT.ContainsKey(n)) missing.Add(n);
            if (missing.Count != 0) throw new InvalidOperationException($"Dictionary UE24_NetsToBRTs.NBRT does not contain N '{String.Join(", ", missing)}'.");

            foreach (KeyValuePair<N, HashSet<BRT>> kvp in this.NBRT) if (kvp.Value.Count == 0) missing.Add(kvp.Key);
            if (missing.Count != 0) throw new InvalidOperationException($"Dictionary UE24_NetsToBRTs.NBRT N correlate to empty HashSet<BRT> '{String.Join(", ", missing)}'.");

            Dictionary<N, HashSet<BRT>> duplicates = new Dictionary<N, HashSet<BRT>>();
            HashSet<BRT> brts = new HashSet<BRT>();
            foreach (KeyValuePair<N, HashSet<BRT>> kvp in this.NBRT) {
                foreach (BRT brt in kvp.Value) {
                    if (brts.Contains(brt)) {
                        if (duplicates.ContainsKey(kvp.Key)) duplicates[kvp.Key].Add(brt);
                        else duplicates.Add(kvp.Key, new HashSet<BRT> { brt });
                    }
                    brts.Add(brt);
                }
            }
            if (duplicates.Count != 0) {
                StringBuilder sb = new StringBuilder($"Dictionary UE24_NetsToBRTs.NBRT has duplicated BRTs:{Environment.NewLine}");
                foreach (KeyValuePair<N, HashSet<BRT>> kvp in duplicates) sb.AppendLine($"Key '{kvp.Key}' BRT '{kvp.Value}'.");
                throw new InvalidOperationException(sb.ToString());
            }
            // - Verify every unique B & R has at least a BRT for T.C and one/both for { T.NC and/or T.NO }
            //   - That is, every FormC relay Common terminal is connected to a N, and one/both of it's Normally Closed/Normally Open terminals
            //     are connected to N.
            // - Verify every BRT per unique B & Rpair has different N on each T.
            //   - That is, verify the Common, Normally Open & Normally Closed terminals are all different N.
        }

        public static void Connect(N N1, N N2) {
            // Intersect, Verify & Connect:
            // - Index this.NT to get the HashSet<BRT> correlated to N1; call it HashSet<BRT1>.
            // - Index this.NT to get the HashSet<BRT> correlated to N2; call it HashSet<BTT2>.
            // - HashSet.Intersect(BRT1, BRT2) to get resulting HashSet<BRT> that have matching B & Rpairs.
            //   - If there are no BRT intersections with matching B & Rpairs, throw ArgumentException.
            // - Verify "You can get there from here":
            //   - Verify for each relay B & Rpairs one is a Common terminal, the other a Normally Closed or Open terminal.
            //   - If not throw ArgumentException.
            //   - There should be at least one B & Rmatching BRT pair that can connect N1 to N2.
            //   - There may be multiple B & Rmatching BRT pairs that can can connect N1 to N2.
            //     - Possibly for current capacity/ampacity, 4 wire Kelvin sensing or intentional duplications.  Or unintentional duplications :-)
            // - Connect all available BRTs to Ns N1 & N2 using UE24 class:
            //   - Will always be either C to NC or C to NO path to connect N1 to N2.
            //   - If C to NC connects N1 to N2, invoke Set(B, R, FC.S.NC).
            //   - If C to NO connects N1 to N2, invoke Set(B, R, FC.S.NO).
        }

        public static void Connect(HashSet<N> Ns) {
            // Connect all HashSet<N> Ns simultaneously to one another.
            // Superset of Connect(N N1, N N2).
            // Sequentially invoke Connect(N N1, N N2) with foreach N in Ns.
            // Then invoke AreConnected((HashSet<N> Ns) to verify all are still *simultaneously* connected.
            // - Possible to wire N1 to BRT.C, N2 to BRT.NO & Net3 to BRT.NC all having matching Relay addresses (RB, R).
            // - Invoking Connect(HashSet<N1, N2, Net3>) is impossible to simultaneously achieve.
            // - But will easily be sequentially achieved by Connect(N1, N2) & Connect(N2, Net3).
            // Used for Shorts/Opens testing.
        }

        public static void DisConnect(N N1, N N2) {
            // Same as Connect, except disconnect N1 from N2 with opposite C state:
            // - If C to NC connects N1 to N2, invoke invoke Set(RB, C, C.NO).
            // - If C to NO connects N1 to N2, invoke invoke Set(RB, C, C.NC).
        }

        public static void DisConnect(HashSet<N> Ns) {
            // Disonnect all HashSet<N> Ns Ns simultaneously from one another.
            // Superset of DisConnect(N N1, N N2).
            // Sequentially invoke DisConnect(N N1, N N2) with foreach N in Ns.
            // Then invoke AreDisConnected((HashSet<N> Ns) to verify all are still *simultaneously* disconnected.
            // - Possible to wire N1 to BRT.C, N2 to BRT.NO & Net3 to BRT.NC all having matching Relay addresses (RB, R).
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
            // Use Get() function, returning Dictionary<B, Dictionary<R, FC.S>>, and convert to Dictionary<N, HashSet<N>>.
        }
        // If can convert:
        //      - Dictionary<N, HashSet<BRT>>
        // to/from:
        //      - Dictionary<B, Dictionary<R, FC.S>>
        // Can invoke:
        //       - Set(Dictionary<B, Dictionary<R, FC.S>> BεRεS)
        //       - Are(Dictionary<B, Dictionary<R, C.SC>> BεRεS)
        //       - Get()
        //
        // Initially support UE24's Set(), Is() & Get() functions for discrete/single (B, R, FC.S)
    }
}
