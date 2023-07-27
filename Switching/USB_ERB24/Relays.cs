using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ABT.TestSpace.TestExec.Switching.RelayForms;
using static ABT.TestSpace.TestExec.Switching.USB_ERB24.ERB24;

namespace ABT.TestSpace.TestExec.Switching.USB_ERB24 {
    public class SwitchedNet {
        public readonly String Name;

        public SwitchedNet(String name) { this.Name = name; }

        public override Boolean Equals(Object obj) {
            SwitchedNet sn = obj as SwitchedNet;
            if (ReferenceEquals(this, sn)) return true;
            return sn != null && this.Name == sn.Name;
        }

        public override Int32 GetHashCode() { return 3 * this.Name.GetHashCode(); }

        public override string ToString() { return this.Name; }
    }

    public sealed class Relay {
        public readonly UE UE;
        public readonly R R;
        public readonly SwitchedNet C;
        public readonly SwitchedNet NC;
        public readonly SwitchedNet NO;

        public Relay(UE UE, R R, SwitchedNet C, SwitchedNet NC, SwitchedNet NO) {
            this.UE = UE; this.R = R; this.C = C; this.NC = NC; this.NO = NO;
            Validate();
        }

        private void Validate() {
            if (C.Name == String.Empty) throw new ArgumentException($"Relay terminal Common '{C.Name}' cannot be String.Empty.");
            if (C == NO) throw new ArgumentException($"Relay terminals Common '{C}' & Normally Open '{NO}' cannot be identical.");
            if (C == NC) throw new ArgumentException($"Relay terminals Common '{C}' & Normally Closed '{NC}' cannot be identical.");
            if (NC == NO) throw new ArgumentException($"Relay terminals Normally Closed '{NC}' & Normally Open '{NO}' cannot be identical.");
        }

        public static String GetUE(UE ue) { return Enum.GetName(typeof(UE), ue); }
        public static String GetR(R r) { return Enum.GetName(typeof(R), r); }

        public C.S Get() { return ERB24.Get(this.UE, this.R); }

        public void Set(C.S state) { ERB24.Set(this.UE, this.R, state); }

        public Boolean Is(C.S state) { return ERB24.Is(this.UE, this.R, state); }

        public override Boolean Equals(Object obj) {
            Relay r = obj as Relay;
            if (ReferenceEquals(this, r)) return true;
            return r != null && r.UE == this.UE && r.R == this.R && this.C == r.C && this.NC == r.NC && this.NO == r.NO;
        }

        public override Int32 GetHashCode() { return 3 * this.UE.GetHashCode() + this.R.GetHashCode(); }
    }

    public sealed class State {
        public readonly UE UE;
        public readonly R R;
        public readonly C.S S;

        public State(ERB24.UE UE, R R, C.S S) { this.UE = UE; this.R = R; this.S = S; }

        public override Boolean Equals(Object obj) {
            State s = obj as State;
            if (ReferenceEquals(this, s)) return true;
            return s != null && s.UE == this.UE && s.R == this.R && s.S == this.S;
        }

        public override Int32 GetHashCode() { return 3 * this.UE.GetHashCode() + this.R.GetHashCode() + this.S.GetHashCode(); }
    }

    public sealed class Terminal {
        public readonly UE UE;
        public readonly R R;
        public readonly C.T T;

        public Terminal(ERB24.UE UE, R R, C.T T) { this.UE = UE; this.R = R; this.T = T; }

        public override Boolean Equals(Object obj) {
            Terminal t = obj as Terminal;
            if (ReferenceEquals(this, t)) return true;
            return t != null && t.UE == this.UE && t.R == this.R && t.T == this.T;
        }

        public override Int32 GetHashCode() { return 3 * this.UE.GetHashCode() + this.R.GetHashCode() + this.T.GetHashCode(); }
    }

    public sealed class Route {
        public readonly Tuple<SwitchedNet, SwitchedNet> SwitchedNetPair;
        public Route(Tuple<SwitchedNet, SwitchedNet> switchedNetPair) { this.SwitchedNetPair = switchedNetPair; }

        public override Boolean Equals(Object obj) {
            Route r = obj as Route;
            if (r == null) return false;
            if (ReferenceEquals(this, r)) return true;
            if (r.SwitchedNetPair.Item1 == this.SwitchedNetPair.Item1 && r.SwitchedNetPair.Item2 == this.SwitchedNetPair.Item2) return true;
            if (r.SwitchedNetPair.Item1 == this.SwitchedNetPair.Item2 && r.SwitchedNetPair.Item2 == this.SwitchedNetPair.Item1) return true;
            return false;
        }

        public override Int32 GetHashCode() { return 3 * this.SwitchedNetPair.GetHashCode(); }
    }

    public sealed class RouteStates {
        public readonly Dictionary<Route, HashSet<State>> SwitchedNetStates;

        public RouteStates(Dictionary<Route, HashSet<State>> switchedNetStates) { this.SwitchedNetStates = switchedNetStates; }

        public static void Connect(SwitchedNet SN1, SwitchedNet SN2) {
            // Intersect, Verify & Connect:
            // - Index this.SNT to get the HashSet<Terminal> correlated to SN1; call it HashSet<T1>.
            // - Index this.SNT to get the HashSet<Terminal> correlated to SN2; call it HashSet<T2>.
            // - HashSet.Intersect(T1, T2) to get resulting HashSet<Terminal> that have matching UE & R pairs.
            //   - If there are no Terminal intersections with matching B & R pairs, throw ArgumentException.
            // - Verify "You can get there from here":
            //   - Verify for each relay UE & R pairs one is a Common terminal, the other a Normally Closed or Open terminal.
            //   - If not throw ArgumentException.
            //   - There should be at least one B & R matching Terminal pair that can connect SN1 to SN2.
            //   - There may be multiple UE & R matching Terminal pairs that can can connect SN1 to SN2.
            //     - Possibly for current capacity/ampacity, 4 wire Kelvin sensing or intentional duplications.  Or unintentional duplications :-)
            // - Connect all available Terminals to SNs SN1 & SN2 using ERB24 class:
            //   - Will always be either C to NC or C to NO path to connect SN1 to SN2.
            //   - If C to NC connects SN1 to SN2, invoke Set(UE, R, C.S.NC).
            //   - If C to NO connects SN1 to SN2, invoke Set(UE, R, C.S.NO).
        }

        public static void Connect(HashSet<SwitchedNet> SNs) {
            // Connect all HashSet<SN> SNs simultaneously to one another.
            // Superset of Connect(SN SN1, SN SN2).
            // Sequentially invoke Connect(SN SN1, SN SN2) with foreach SN in SNs.
            // Then invoke AreConnected((HashSet<SN> SNs) to verify all are still *simultaneously* connected.
            // - Possible to wire SN1 to T.C, SN2 to T.NO & Net3 to T.NC all having matching Relay addresses (UE, R).
            // - Invoking Connect(HashSet<SN1, SN2, Net3>) is impossible to simultaneously achieve.
            // - But will easily be sequentially achieved by Connect(SN1, SN2) & Connect(SN2, Net3).
            // Used for Shorts/Opens testing.
        }

        public static void DisConnect(SwitchedNet SN1, SwitchedNet SN2) {
            // Same as Connect, except disconnect SN1 from SN2 with opposite C state:
            // - If C to NC connects SN1 to SN2, invoke Set(UE, C, C.NO).
            // - If C to NO connects sN1 to SN2, invoke Set(UE, C, C.NC).
        }

        public static void DisConnect(HashSet<SwitchedNet> SNs) {
            // Disonnect all HashSet<SN> SNs simultaneously from one another.
            // Superset of DisConnect(SN SN1, SN SN2).
            // Sequentially invoke DisConnect(SN SN1, SN SN2) with foreach SN in SNs.
            // Then invoke AreDisConnected((HashSet<SN> SNs) to verify all are still *simultaneously* disconnected.
            // - Possible to wire SN1 to T.C, SN2 to T.NO & Net3 to T.NC all having matching Relay addresses (UE, R).
            // - Invoking DisConnect(HashSet<SN1, SN2, Net3>) is impossible to simultaneously achieve.
            // - But will easily be sequentially achieved by DisConnect(SN1, SN2) & Connect(SN2, Net3).
            // Used for Shorts/Opens testing.
        }

        public static Boolean AreConnected(SwitchedNet SN1, SwitchedNet SN2) {
            // Verify/refute SN1 currently connected to SN2.
            // Use GetConnections(N1) returning HashSet<SN>.Intersect<SN2> to verify/refute if SN1 currently connected to SN2.
            // For Debug.Assert() statements.
            return false;
        }

        public static Boolean AreDisConnected(SwitchedNet SN1, SwitchedNet SN2) { return !AreConnected(SN1, SN2); }

        public static Boolean AreConnected(HashSet<SwitchedNet> SNs) {
            // Verify/refute all SNs in HashSet<SN> are interconnected to one another.
            // Superset of AreConnected(SN SN1, SN SN2).
            // Can recursively invoke AreConnected(SN SN1, SN SN2) with foreach SN in SNs.
            // For Debug.Assert() statements.
            return false;
        }

        public static Boolean AreDisConnected(HashSet<SwitchedNet> SNs) {
            return false;
        }

        private static Boolean AreConnectable(SwitchedNet SN1, SwitchedNet SN2) {
            // Verify/refute SN1 can be connected to SN2.
            // Use GetConnections(SN1) returning HashSet<SN>.Intersect<SN2> to verify/refute if SN1 currently connected to SN2.
            // Reccommend programming 
            // For Debug.Assert() statements.
            return false;
        }

        private static Boolean AreConnectable(HashSet<SwitchedNet> SNs) {
            // Verify/refute HashSet<SN> SNs can be interconnected to one another.
            // Use AreConnectable(SN1) returning HashSet<SN>.Intersect<SN2> to verify/refute if SN1 connected to SN2.
            // Reccommend programming 
            // For Debug.Assert() statements.
            return false;
        }

        public static Dictionary<SwitchedNet, HashSet<SwitchedNet>> GetConnections(SwitchedNet SN1) {
            return new Dictionary<SwitchedNet, HashSet<SwitchedNet>>();
            // Use Get() function, returning Dictionary<UE, Dictionary<R, C.S>>, and convert to Dictionary<SN, HashSet<SN>>.
        }
        // If can convert:
        //      - Dictionary<String, HashSet<Terminal>>
        // to/from:
        //      - Dictionary<UE, Dictionary<R, C.S>>
        // Can invoke:
        //       - Set(Dictionary<UE, Dictionary<R, C.S>> UEεRεS)
        //       - Are(Dictionary<UE, Dictionary<R, C.SC>> UEεRεS)
        //       - Get()
        //
        // Initially support ERB24's Set(), Is() & Get() functions for discrete/single (UE, R, C.S)

    }

    public sealed class Relays {
        public readonly HashSet<Relay> Rs;
        public readonly Dictionary<String, HashSet<Terminal>> SNTs = new Dictionary<String, HashSet<Terminal>>();

        public Relays(HashSet<Relay> rs) {
            this.Rs = rs;
            Validate();

            //foreach (Relay r in this.Rs) {
            //    if (!this.SNTs.ContainsKey(r.C)) 
            //}
        }

        private void Validate() {
            StringBuilder sb = new StringBuilder($"Cannot currently accomodate USB-ERB24 Relays connected serially:{Environment.NewLine}Boards/Relays");
            List<(Relay, Relay)> rs =
                (from r1 in this.Rs
                 from r2 in this.Rs
                 where (r1.C == r2.NC || r1.C == r2.NO)
                 select (r1, r2)).ToList();
            if (rs.Count() != 0) {
                foreach ((Relay r1, Relay r2) rr in rs) {
                    sb.AppendLine("Below relay pair {R1, R2} serially connected, C1 to (NC2 ⨁ NO2)");
                    sb.AppendLine($"   B1='{Relay.GetUE(rr.r1.UE)}', R1='{Relay.GetR(rr.r1.R)}', C1='{rr.r1.C}', NC1='{rr.r1.NC}', NO1='{rr.r1.NO}'.");
                    sb.AppendLine($"   B2='{Relay.GetUE(rr.r2.UE)}', R2='{Relay.GetR(rr.r2.R)}', C2='{rr.r2.C}', NC2='{rr.r2.NC}', NO2='{rr.r2.NO}'.");
                    sb.AppendLine("");
                }
                throw new InvalidOperationException(sb.ToString());
            }
        }
    }

    //public abstract class SwitchedNetTerminals {
    //    public readonly Dictionary<SN, HashSet<Terminal>> SNT;
    //    SwitchedNetTerminals() {
    //        this.SNT = new Dictionary<SN, HashSet<TErminal>>() {
    //            {N.CTL_1_3, new HashSet<Terminal>() {new Terminal(UE.B1, R.C04, C.T.NO)}},
    //            {N.DMM_IN_P, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.DMM_IN_N, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.DMM_I, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.ENABLE_N, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.FAN_PWR, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.POWER_GOOD, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.PRE_BIAS_OUT_P, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.PRE_BIAS_OUT_N, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.PRE_BIAS_PS, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.PRE_BIAS_PS_RTN, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.PRI_3V3, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.PRI_BIAS, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.PRI_BIAS_OUT_P, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.PRI_BIAS_OUT_N, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.PRI_BIAS_PS, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.PRI_BIAS_PS_RTN, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.SCL, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.SDA, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.SEC_3V3, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.SEC_BIAS, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.SEC_BIAS_OUT_P, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.SEC_BIAS_OUT_N, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.SEC_BIAS_PS, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.SEC_BIAS_PS_RTN, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.START_SYNC, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.SYNC, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.VDC_5, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.VDC_5_RTN, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.VIN_Sense, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.VIN_RTN_Sense, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}},
    //            {N.VOUT_RTN_Sense, new HashSet<Terminal>() {new Terminal(UE.B0, R.C01, C.T.C)}}
    //        };

    //        Validate();
    //    }

    //private void Validate() {
    //    HashSet<SN> missing = new HashSet<SN>();
    //    foreach (SN sn in SN) if (!this.SNT.ContainsKey(sn)) missing.Add(sn);
    //    if (missing.Count != 0) throw new InvalidOperationException($"Dictionary SNT does not contain SN '{String.Join(", ", missing)}'.");

    //    foreach (KeyValuePair<SN, HashSet<Terminal>> kvp in this.SNT) if (kvp.Value.Count == 0) missing.Add(kvp.Key);
    //    if (missing.Count != 0) throw new InvalidOperationException($"Dictionary SNT's Switched Net SN correlate to empty HashSet<Terminal> '{String.Join(", ", missing)}'.");

    //    Dictionary<SN, HashSet<Terminal>> duplicates = new Dictionary<SN, HashSet<Terminal>>();
    //    HashSet<Terminal> ts = new HashSet<Terminal>();
    //    foreach (KeyValuePair<SN, HashSet<Terminal>> kvp in this.SNT) {
    //        foreach (Terminal t in kvp.Value) {
    //            if (ts.Contains(t)) {
    //                if (duplicates.ContainsKey(kvp.Key)) duplicates[kvp.Key].Add(t);
    //                else duplicates.Add(kvp.Key, new HashSet<Terminal> { t });
    //            }
    //            ts.Add(t);
    //        }
    //    }
    //    if (duplicates.Count != 0) {
    //        StringBuilder sb = new StringBuilder($"Dictionary SNT has duplicated Ts:{Environment.NewLine}");
    //        foreach (KeyValuePair<SN, HashSet<Terminal>> kvp in duplicates) sb.AppendLine($"Key '{kvp.Key}' Terminal '{kvp.Value}'.");
    //        throw new InvalidOperationException(sb.ToString());
    //    }
    //    // - Verify every unique UE & R has at least a Terminal for T.C and one/both for { T.NC and/or T.NO }
    //    //   - That is, every Form C relay Common terminal is connected to a SN, and one/both of it's Normally Closed/Normally Open terminals
    //    //     are connected to a SN.
    //    // - Verify every Terminal per unique B & R pair has different SN on each Terminal.
    //    //   - That is, verify the Common, Normally Open & Normally Closed terminals are all different SN.
    //}
}
