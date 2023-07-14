using System;
using System.Collections.Generic;

public enum C { NC, NO }
// C.NC; Form C relay is deenergized and in normally closed/normally open state, with common C terminal connected to NC terminal & disconnected from NO terminal.
// C.NO; Form C relay is energized and in abnormally opened/abnormally closed state, with common C terminal disconnected from NC terminal & connected to NO terminal.

public enum T { C, NC, NO }   // Terminals for Form C relay.

// https://forum.digikey.com/t/understanding-form-a-form-b-form-c-contact-configuration/811

namespace ABT.TestSpace.TestExec.Switching {
    public sealed class UE24_Address {
        public readonly UE24 RB;
        public readonly R R;
        public UE24_Address(UE24 RB, R R) { this.RB = RB; this.R = R; }
    }

    public sealed class UE24_Terminal {
        public readonly UE24_Address Address;
        public readonly T T;

        public UE24_Terminal(UE24 RB, R R, T T) {
            this.Address = new UE24_Address(RB, R);
            this.T = T;
        }

        public UE24_Terminal(UE24_Address Address, T T) {
            this.Address = Address;
            this.T = T;
        }
    }

    public enum UE24_NETS {
        CTL_1_3, CTL_4_6,
        DMM_IN_P, DMM_IN_N, DMM_I,
        ENABLE_N,
        FAN_PWR,
        POWER_GOOD,
        PRE_BIAS_OUT_P, PRE_BIAS_OUT_N, PRE_BIAS_PS, PRE_BIAS_PS_RTN,
        PRI_3V3, PRI_BIAS, PRI_BIAS_OUT_P, PRI_BIAS_OUT_N, PRI_BIAS_PS, PRI_BIAS_PS_RTN,
        SCL, SDA,
        SEC_3V3, SEC_BIAS, SEC_BIAS_OUT_P, SEC_BIAS_OUT_N, SEC_BIAS_PS, SEC_BIAS_PS_RTN,
        START_SYNC, SYNC,
        VDC_5, VDC_5_RTN,
        VIN_Sense, VIN_RTN_Sense,
        VOUT_RTN_Sense
    }

    public abstract class UE24NetsToTerminals {
        public readonly Dictionary<UE24_NETS, HashSet<UE24_Terminal>> NT;
        UE24NetsToTerminals() {
            this.NT = new Dictionary<UE24_NETS, HashSet<UE24_Terminal>>() {
                {UE24_NETS.CTL_1_3, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB1,R.C04,T.NO)}},
                {UE24_NETS.CTL_4_6, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.DMM_IN_P, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.DMM_IN_N, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.DMM_I, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.ENABLE_N, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.FAN_PWR, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.POWER_GOOD, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.PRE_BIAS_OUT_P, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.PRE_BIAS_OUT_N, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.PRE_BIAS_PS, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.PRE_BIAS_PS_RTN, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.PRI_3V3, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.PRI_BIAS, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.PRI_BIAS_OUT_P, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.PRI_BIAS_OUT_N, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.PRI_BIAS_PS, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.PRI_BIAS_PS_RTN, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.SCL, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.SDA, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.SEC_3V3, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.SEC_BIAS, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.SEC_BIAS_OUT_P, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.SEC_BIAS_OUT_N, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.SEC_BIAS_PS, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.SEC_BIAS_PS_RTN, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.START_SYNC, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.SYNC, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.VDC_5, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.VDC_5_RTN, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.VIN_Sense, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.VIN_RTN_Sense, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}},
                {UE24_NETS.VOUT_RTN_Sense, new HashSet<UE24_Terminal>() {new UE24_Terminal(UE24.RB0, R.C01, T.C)}}
            };

            Validate();
        }

        private void Validate() {
            // Validate this.NT:
            // - Verify the union of all this.NT HashSet<UE24_Terminal> has no duplicated UE24_Terminal objects.
            // - Verify every unique UE24.RB & R.C has at least a UE24_Terminal for T.C and one/both for { T.NC and/or T.NO }
            //   - That is, every FormC relay Common terminal is connected to a UE24_NETS, and one/both of it's Normally Closed/Normally Open terminals
            //     are connected to UE24_NETS.
            // - Verify every UE24_Terminal per unique UE24.RB & R.C pair has different UE24_NETS on each T.
            //   - That is, verify the Common, Normally Open & Normally Closed terminals are all different UE24_NETS.
            // - Verify every UE24_NETS exists in this.NT
        }

        public static void Connect(UE24_NETS N1, UE24_NETS N2) {
            // Intersect, Verify & Connect:
            // - Index this.NT to get the HashSet<UE24_Terminal> correlated to N1; call it HashSet<Terminals1>.
            // - Index this.NT to get the HashSet<UE24_Terminal> correlated to N2; call it HashSet<Terminals2>.
            // - HashSet.Intersect(Terminals1, Terminals2) to get resulting HashSet<UE24_Terminal> that have matching UE24.RB & R.C pairs.
            //   - If there are no UE24_Terminal intersections with matching UE24.RB & R.C pairs, throw ArgumentException.
            // - Verify "You can get there from here":
            //   - Verify for each relay UE24.RB & R.C pairs one is a Common terminal, the other a Normally Closed or Open terminal.
            //   - If not throw ArgumentException.
            //   - There should be at least one UE24.RB & R.C matching UE24_Terminal pair that can connect N1 to N2.
            //   - There may be multiple UE24.RB & R.C matching UE24_Terminal pairs that can can connect N1 to N2.
            //     - Possibly for current capacity/ampacity, 4 wire Kelvin sensing or intentional duplications.  Or unintentional duplications :-)
            // - Connect all available UE24_Terminal UE24.RB & R.C relays to Ns N1 & N2 using USB_ERB24 class:
            //   - Will always be either C to NC or C to NO path to connect N1 to N2.
            //   - If C to NC connects N1 to N2, invoke Set(RB, C, C.NC).
            //   - If C to NO connects N1 to N2, invoke Set(RB, C, C.NO).
        }

        public static void Connect(HashSet<UE24_NETS> Ns) {
            // Connect all HashSet<UE24_NETS> Ns simultaneously to one another.
            // Superset of Connect(UE24_NETS N1, UE24_NETS N2).
            // Sequentially invoke Connect(UE24_NETS N1, UE24_NETS N2) with foreach UE24_NETS in Ns.
            // Then invoke AreConnected((HashSet<UE24_NETS> Ns) to verify all are still *simultaneously* connected.
            // - Possible to wire N1 to UE24_Terminal.C, N2 to UE24_Terminal.NO & Net3 to UE24_Terminal.NC all having matching Relay addresses (RB, R).
            // - Invoking Connect(HashSet<N1, N2, Net3>) is impossible to simultaneously achieve.
            // - But will easily be sequentially achieved by Connect(N1, N2) & Connect(N2, Net3).
            // Used for Shorts/Opens testing.
        }

        public static void DisConnect(UE24_NETS N1, UE24_NETS N2) {
            // Same as Connect, except disconnect N1 from N2 with opposite C state:
            // - If C to NC connects N1 to N2, invoke invoke Set(RB, C, C.NO).
            // - If C to NO connects N1 to N2, invoke invoke Set(RB, C, C.NC).
        }

        public static void DisConnect(HashSet<UE24_NETS> Ns) {
            // Disonnect all HashSet<UE24_NETS> Ns Ns simultaneously from one another.
            // Superset of DisConnect(UE24_NETS N1, UE24_NETS N2).
            // Sequentially invoke DisConnect(UE24_NETS N1, UE24_NETS N2) with foreach UE24_NETS in Ns.
            // Then invoke AreDisConnected((HashSet<UE24_NETS> Ns) to verify all are still *simultaneously* disconnected.
            // - Possible to wire N1 to UE24_Terminal.C, N2 to UE24_Terminal.NO & Net3 to UE24_Terminal.NC all having matching Relay addresses (RB, R).
            // - Invoking DisConnect(HashSet<N1, N2, Net3>) is impossible to simultaneously achieve.
            // - But will easily be sequentially achieved by DisConnect(N1, N2) & Connect(N2, Net3).
            // Used for Shorts/Opens testing.
        }

        public static Boolean AreConnected(UE24_NETS N1, UE24_NETS N2) {
            // Verify/refute N1 currently connected to N2.
            // Use GetConnections(N1) returning HashSet<UE24_NETS>.Intersect<N2> to verify/refute if N1 currently connected to N2.
            // For Debug.Assert() statements.
            return false;
        }

        public static Boolean AreDisConnected(UE24_NETS N1, UE24_NETS N2) { return !AreConnected(N1, N2); }

        public static Boolean AreConnected(HashSet<UE24_NETS> Ns) {
            // Verify/refute all Ns in HashSet<UE24_NETS> are interconnected to one another.
            // Superset of AreConnected(UE24_NETS N1, UE24_NETS N2).
            // Can recursively invoke AreConnected(UE24_NETS N1, UE24_NETS N2) with foreach UE24_NETS in Ns.
            // For Debug.Assert() statements.
            return false;
        }

        public static Boolean AreDisConnected(HashSet<UE24_NETS> Ns) {
            return false;
        }

        private static Boolean AreConnectable(UE24_NETS N1, UE24_NETS N2) {
            // Verify/refute N1 can be connected to N2.
            // Use GetConnections(N1) returning HashSet<UE24_NETS>.Intersect<N2> to verify/refute if N1 currently connected to N2.
            // Reccommend programming 
            // For Debug.Assert() statements.
            return false;
        }

        private static Boolean AreConnectable(HashSet<UE24_NETS> Ns) {
            // Verify/refute HashSet<UE24_NETS> Ns can be interconnected to one another.
            // Use AreConnectable(N1) returning HashSet<UE24_NETS>.Intersect<N2> to verify/refute if N1 connected to N2.
            // Reccommend programming 
            // For Debug.Assert() statements.
            return false;
        }

        public static Dictionary<UE24_NETS, HashSet<UE24_NETS>> GetConnections(UE24_NETS N1) {
            return new Dictionary<UE24_NETS, HashSet<UE24_NETS>>();
            // Use Get() function, returning Dictionary<UE24, Dictionary<R, C>>, and convert to Dictionary<UE24_NETS, HashSet<UE24_NETS>>.
        }
        // If can convert:
        //      - Dictionary<UE24_NETS, HashSet<UE24_Terminal>>
        // to/from:
        //      - Dictionary<UE24, Dictionary<R, C>>
        // Can invoke:
        //       - Set(Dictionary<UE24, Dictionary<R, C>> UE24εRεC)
        //       - Are(Dictionary<UE24, Dictionary<R, C>> UE24εRεC)
        //       - Get()
        //
        // Initially support USB_ERB24's Set(), Is() & Get() functions for discrete/single (RB, R, C)
    }
}

