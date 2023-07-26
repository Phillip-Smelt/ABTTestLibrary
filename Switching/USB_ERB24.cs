using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MccDaq; // MCC DAQ Universal Library 6.73 from https://www.mccdaq.com/Software-Downloads.
using static ABT.TestSpace.TestExec.Switching.RelayForms;
using static ABT.TestSpace.TestExec.Switching.USB_ERB24;

namespace ABT.TestSpace.TestExec.Switching {
    public sealed class USB_ERB24 {
        // NOTE: UE24 is an abbreviation of Measurement Computing Corporation's USB-ERB24 initialisation (Universal Serial Bus Electronic Relay Board with 24 Form C relays).
        // NOTE: Most of this class is compatible with MCC's USB-ERB08 Relay Board, essentially a USB-ERB24 but with only 8 Form C relays instead of the USB-ERB24's 24.
        // - Some portions are specific to the USB-ERB24 however; examples are enum R containing 24 relays & enum PORTS containing 24 bits.
        // NOTE: This class assumes all USB-ERB24 relays are configured for Non-Inverting Logic & Pull-Down/de-energized at power-up.
        // NOTE: USB-ERB24 relays are configurable for either Non-Inverting or Inverting logic, via hardware DIP switch S1.
        //  - Non-Inverting:  Logic low de-energizes the relays, logic high energizes them.
        //  - Inverting:      Logic low energizes the relays, logic high de-energizes them.  
        // NOTE: USB-ERB24 relays are configurable with default power-up states, via hardware DIP switch S2.
        //  - Pull-Up:        Relays are energized at power-up.
        //  - Pull-Down:      Relays are de-energized at power-up.
        //  - https://www.mccdaq.com/PDFs/Manuals/usb-erb24.pdf.
        public Dictionary<UE, MccBoard> USB_ERB24s;
        private readonly static USB_ERB24 _only = new USB_ERB24();
        public static USB_ERB24 Only { get { return _only; } }
        static USB_ERB24() { }
        // Singleton pattern requires explicit static constructor to tell C# compiler not to mark type as beforefieldinit.
        // https://csharpindepth.com/articles/singleton
        private USB_ERB24() {
            this.USB_ERB24s = new Dictionary<UE, MccBoard>() {
                {UE.B0, new MccBoard((Int32)UE.B0)},
                {UE.B1, new MccBoard((Int32)UE.B1)}
            };
        }

        public enum UE { B0, B1 } // USB-ERB24 Boards.
        public enum R : Byte { C01, C02, C03, C04, C05, C06, C07, C08, C09, C10, C11, C12, C13, C14, C15, C16, C17, C18, C19, C20, C21, C22, C23, C24 } // USB-ERB24 Relays, all Form C.
        // NOTE: UE enum is a static definition of TestExecutive's MCC USB-ERB24(s).
        // Potential dynamic definition methods for USB_ERB24s:
        //  - Read them from MCC InstaCal's cb.cfg file.
        //  - Dynamically discover them programmatically: https://www.mccdaq.com/pdfs/manuals/Mcculw_WebHelp/ULStart.htm.
        //  - Specify MCC USB-ERB24s in TestExecutive.config.xml.
        // NOTE: MCC's InstaCal USB-ERB24 indexing begins at 0, guessing because USB device indexing is likely also zero based.
        // - So UE.B0's numerical value is 0, which is used when constructing a new MccBoard UE.B0 object:
        // - Instantiation 'new MccBoard((Int32)UE.B0)' is equivalent to 'new MccBoard(0)'.
        // NOTE: enum named R instead of RELAYS for concision; consider below:
        //  - Set(UE.B0, new Dictionary<R, C.S>() {{R.C01,C.S.NC}, {R.C02,C.S.NO}, ... {R.C24,C.S.NC} });
        //  - Set(UE.B0, new Dictionary<RELAYS, C.S>() {{RELAYS.C01,C.S.NC}, {RELAYS.C02,C.S.NO}, ... {RELAYS.C24,C.S.NC} });
        // NOTE: R's items named C## because USB-ERB24's relays are all Form C.

        internal enum PORTS { A, B, CL, CH }
        internal static Int32[] _ue24bitVector32Masks = GetUE24BitVector32Masks();

        #region Internal classes
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

            public C.S Get() { return USB_ERB24.Get(this.UE, this.R); }

            public void Set(C.S state) { USB_ERB24.Set(this.UE, this.R, state); }

            public Boolean Is(C.S state) { return USB_ERB24.Is(this.UE, this.R, state); }

            public override Boolean Equals(Object obj) {
                Relay r = obj as Relay;
                if (ReferenceEquals(this, r)) return true;
                return r != null && r.UE == this.UE && r.R == this.R && this.C == r.C && this.NC == r.NC && this.NO == r.NO;
            }

            public override Int32 GetHashCode() { return 3 * this.UE.GetHashCode() + this.R.GetHashCode(); }
        }

        public sealed class RelayState {
            public readonly UE UE;
            public readonly R R;
            public readonly C.S S;

            public RelayState(USB_ERB24.UE UE, R R, C.S S) { this.UE = UE; this.R = R; this.S = S; }

            public override Boolean Equals(Object obj) {
                RelayState rs = obj as RelayState;
                if (ReferenceEquals(this, rs)) return true;
                return rs != null && rs.UE == this.UE && rs.R == this.R && rs.S == this.S;
            }

            public override Int32 GetHashCode() { return 3 * this.UE.GetHashCode() + this.R.GetHashCode() + this.S.GetHashCode(); }
        }

        public sealed class Terminal {
            public readonly UE UE;
            public readonly R R;
            public readonly C.T T;

            public Terminal(USB_ERB24.UE UE, R R, C.T T) { this.UE = UE; this.R = R; this.T = T; }

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
                if(r.SwitchedNetPair.Item1 == this.SwitchedNetPair.Item1 && r.SwitchedNetPair.Item2 == this.SwitchedNetPair.Item2) return true;
                if(r.SwitchedNetPair.Item1 == this.SwitchedNetPair.Item2 && r.SwitchedNetPair.Item2 == this.SwitchedNetPair.Item1) return true;
                return false;
            }

            public override Int32 GetHashCode() { return 3 * this.SwitchedNetPair.GetHashCode(); }
        }

        public sealed class RouteStates {
            public readonly Dictionary<Route, HashSet<RelayState>> SwitchedNetStates;

            public RouteStates(Dictionary<Route, HashSet<RelayState>> switchedNetStates) { this.SwitchedNetStates = switchedNetStates; }

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
                // - Connect all available Terminals to SNs SN1 & SN2 using USB_ERB24 class:
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
            // Initially support UE24's Set(), Is() & Get() functions for discrete/single (UE, R, C.S)

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
        #endregion internal classes

        #region methods
        #region Is/Are
        public static Boolean Is(UE ue, R r, C.S s) { return Get(ue, r) == s; }

        public static Boolean Are(UE ue, HashSet<R> rs, C.S s) {
            Dictionary<R, C.S> RεS = rs.ToDictionary(r => r, r => s);
            Dictionary<R, C.S> Are = Get(ue, rs);
            return RεS.Count == Are.Count && !RεS.Except(Are).Any();
        }

        public static Boolean Are(UE ue, Dictionary<R, C.S> RεS) {
            Dictionary<R, C.S> Are = Get(ue, new HashSet<R>(RεS.Keys));
            return RεS.Count == Are.Count && !RεS.Except(Are).Any();
        }

        public static Boolean Are(UE ue, C.S s) {
            Dictionary<R, C.S> Are = Get(ue);
            Boolean areEqual = true;
            foreach (KeyValuePair<R, C.S> kvp in Are) areEqual &= kvp.Value == s;
            return areEqual;
        }

        // Below 3 methods mainly useful for parallelism, when testing multiple UUTs concurrently, with each B wired identically to test 1 UUT.
        public static Boolean Are(HashSet<UE> ues, C.S s) {
            Boolean areEqual = true;
            foreach (UE ue in ues) areEqual &= Are(ue, s);
            return areEqual;
        }

        public static Boolean Are(HashSet<UE> ues, HashSet<R> rs, C.S s) {
            Boolean areEqual = true;
            foreach (UE ue in ues) areEqual &= Are(ue, rs, s);
            return areEqual;
        }

        public static Boolean Are(Dictionary<UE, Dictionary<R, C.S>> UEεRεS) {
            Boolean areEqual = true;
            foreach (KeyValuePair<UE, Dictionary<R, C.S>> kvp in UEεRεS) areEqual &= Are(kvp.Key, kvp.Value);
            return areEqual;
        }

        public static Boolean Are(C.S s) {
            Boolean areEqual = true;
            foreach (UE ue in Enum.GetValues(typeof(UE))) areEqual &= Are(ue, s);
            return areEqual;
        }
        #endregion Is/Are

        #region Get
        public static C.S Get(UE ue, R r) {
            ErrorInfo errorInfo = Only.USB_ERB24s[ue].DBitIn(DigitalPortType.FirstPortA, (Int32)r, out DigitalLogicState digitalLogicState);
            ProcessErrorInfo(Only.USB_ERB24s[ue], errorInfo);
            return digitalLogicState == DigitalLogicState.Low ? C.S.NC : C.S.NO;
        }

        public static Dictionary<R, C.S> Get(UE ue, HashSet<R> rs) {
            Dictionary<R, C.S> RεS = Get(ue);
            foreach (R r in rs) if (!RεS.ContainsKey(r)) RεS.Remove(r);
            return RεS;
        }

        public static Dictionary<R, C.S> Get(UE ue) {
            // Obviously, can utilize MccBoard.DBitIn to read individual bits, instead of MccBoard.DIn to read multiple bits:
            // - But, the USB-ERB24's reads it's relay states by reading its internal 82C55's ports.
            // - These ports appear to operate similarly to MccBoard's DIn function, that is, they read the 82C55's 
            //   port bits simultaneously.
            // - If correct, then utilizing MccBoard's DBitIn function could be very inefficient compared to
            //   the DIn function, since DBitIn would have to perform similar bit-shifting/bit-setting functions as this method does,
            //   once for each of the USB-ERB24's 24 relays, as opposed to 4 times for this method.
            // - Regardless, if preferred, below /*,*/commented code can replace the entirety of this method.
            /*
            ErrorInfo errorInfo;  DigitalLogicState digitalLogicState;
            R r;  C.S s;  Dictionary<R, C.S> RεS = new Dictionary<R, C.S>();
            for (Int32 i = 0; i < Enum.GetValues(typeof(R)).Length; i++) {
                errorInfo = Only.USB_ERB24s[ue].DBitIn(DigitalPortType.FirstPortA, i, out digitalLogicState);
                ProcessErrorInfo (Only.USB_ERB24s[ue], errorInfo);
                r = (R)Enum.ToObject(typeof(R), i);
                s = digitalLogicState == DigitalLogicState.Low ? C.S.NC : C.S.NO;
                RεS.Add(r, s);
            }
            return RεS;
            */

            UInt16[] portBits = PortsRead(Only.USB_ERB24s[ue]);
            UInt32[] biggerPortBits = Array.ConvertAll(portBits, delegate (UInt16 uInt16) { return (UInt32)uInt16; });
            UInt32 relayBits = 0x0000;
            relayBits |= biggerPortBits[(UInt32)PORTS.CH] << 00;
            relayBits |= biggerPortBits[(UInt32)PORTS.CL] << 04;
            relayBits |= biggerPortBits[(UInt32)PORTS.B] << 08;
            relayBits |= biggerPortBits[(UInt32)PORTS.A] << 16;
            BitVector32 bitVector32 = new BitVector32((Int32)relayBits);

            R r; C.S s; Dictionary<R, C.S> RεS = new Dictionary<R, C.S>();
            for (Int32 i = 0; i < _ue24bitVector32Masks.Length; i++) {
                r = (R)Enum.ToObject(typeof(R), i);
                s = bitVector32[_ue24bitVector32Masks[i]] ? C.S.NO : C.S.NC;
                RεS.Add(r, s);
            }
            return RεS;
        }

        // Below 3 methods mainly useful for parallelism, when testing multiple UUTs concurrently, with each B wired identically to test 1 UUT.
        public static Dictionary<UE, Dictionary<R, C.S>> Get(HashSet<UE> ues) {
            Dictionary<UE, Dictionary<R, C.S>> UEεRεS = Get();
            foreach (UE ue in ues) if (!UEεRεS.ContainsKey(ue)) UEεRεS.Remove(ue);
            return UEεRεS;
        }

        public static Dictionary<UE, Dictionary<R, C.S>> Get(HashSet<UE> ues, HashSet<R> rs) {
            Dictionary<UE, Dictionary<R, C.S>> UEεRεS = new Dictionary<UE, Dictionary<R, C.S>>();
            foreach (UE ue in ues) UEεRεS.Add(ue, Get(ue, rs));
            return UEεRεS;
        }

        public static Dictionary<UE, Dictionary<R, C.S>> Get(Dictionary<UE, R> UEεR) {
            Dictionary<UE, Dictionary<R, C.S>> UEεRεS = new Dictionary<UE, Dictionary<R, C.S>>();
            Dictionary<R, C.S> RεS = new Dictionary<R, C.S>();
            foreach (KeyValuePair<UE, R> kvp in UEεR) {
                RεS.Add(kvp.Value, Get(kvp.Key, kvp.Value));
                UEεRεS.Add(kvp.Key, RεS);
            }
            return UEεRεS;
        }

        public static Dictionary<UE, Dictionary<R, C.S>> Get() {
            Dictionary<UE, Dictionary<R, C.S>> UEεRεS = new Dictionary<UE, Dictionary<R, C.S>>();
            foreach (UE ue in Enum.GetValues(typeof(UE))) UEεRεS.Add(ue, Get(ue));
            return UEεRεS;
        }
        #endregion Get

        #region Set
        public static void Set(UE ue, R r, C.S s) {
            ErrorInfo errorInfo = Only.USB_ERB24s[ue].DBitOut(DigitalPortType.FirstPortA, (Int32)r, s is C.S.NC ? DigitalLogicState.Low : DigitalLogicState.High);
            ProcessErrorInfo(Only.USB_ERB24s[ue], errorInfo);
        }

        public static void Set(UE ue, HashSet<R> rs, C.S s) { Set(ue, rs.ToDictionary(r => r, r => s)); }

        public static void Set(UE ue, Dictionary<R, C.S> RεS) {
            // This method only sets relay states for relays explicitly declared in RεS.
            //  - That is, if RεS = {{R.C01, C.S.NO}, {R.C02, C.S.NC}}, then only relays R.C01 & R.C02 will have their states actively set, respectively to NO & NC.
            //  - Relay states R.C03, R.C04...R.C24 remain as they were:
            //      - Relays that were NC remain NC.
            //      - Relays that were NO remain NO.
            //
            // Obviously, can utilize MccBoard.DBitOut to write individual bits, instead of MccBoard.DOut to write multiple bits:
            // - But, the USB-ERB24's energizes/de-energizes it's relay by writing its internal 82C55's ports.
            // - These ports appear to operate similarly to MccBoard's DOut function, that is, they write the 
            //   entire port's bits simultaneously.
            // - If correct, then utilizing MccBoard's DBitOut function could be very inefficient compared to
            //   the DOut function, since it'd have to perform similar And/Or functions as this method does,
            //   once for every call to DBitOut.
            //  - Thought is that DOut will write the bits as simultaneously as possible, at least more so than DBitOut.
            // - Regardless, if preferred, below /*,*/commented code can replace the entirety of this method.
            /*
            ErrorInfo errorInfo;
            foreach (KeyValuePair<R, C.S> kvp in RεS) {
                errorInfo = Only.USB_ERB24s[ue].DBitOut(DigitalPortType.FirstPortA, (Int32)kvp.Key, kvp.Value == C.S.NC ? DigitalLogicState.Low: DigitalLogicState.High);
                ProcessErrorInfo(Only.USB_ERB24s[ue], errorInfo);
            }
            */

            UInt32 relayBit;
            UInt32 bits_NC = 0xFFFF_FFFF; // bits_NC utilize Boolean And logic.
            UInt32 bits_NO = 0x0000_0000; // bits_NO utilize Boolean Or logic.

            foreach (KeyValuePair<R, C.S> kvp in RεS) {
                relayBit = (UInt32)1 << (Byte)kvp.Key;
                if (kvp.Value == C.S.NC) bits_NC ^= relayBit; // Sets a 0 in bits_NC for each explicitly assigned NC state in RεS.
                else bits_NO |= relayBit;                      // Sets a 1 in bits_NO for each explicitly assigned NO state in RεS.
            }

            BitVector32 bv32_NC = new BitVector32((Int32)bits_NC);
            BitVector32 bv32_NO = new BitVector32((Int32)bits_NO);
            BitVector32.Section sectionPortA = BitVector32.CreateSection(0b1111_1111);
            BitVector32.Section sectionPortB = BitVector32.CreateSection(0b1111_1111, sectionPortA);
            BitVector32.Section sectionPortCL = BitVector32.CreateSection(0b1111, sectionPortB);
            BitVector32.Section sectionPortCH = BitVector32.CreateSection(0b1111, sectionPortCL);

            UInt16[] portStates = PortsRead(Only.USB_ERB24s[ue]);

            portStates[(Int32)PORTS.A] &= (UInt16)bv32_NC[sectionPortA]; // &= sets portStates bits low for each explicitly assigned NC state in RεS.
            portStates[(Int32)PORTS.B] &= (UInt16)bv32_NC[sectionPortB];
            portStates[(Int32)PORTS.CL] &= (UInt16)bv32_NC[sectionPortCL];
            portStates[(Int32)PORTS.CH] &= (UInt16)bv32_NC[sectionPortCH];

            portStates[(Int32)PORTS.A] |= (UInt16)bv32_NO[sectionPortA]; // |= sets portStates bits high for each explicitly assigned NO state in RεS.
            portStates[(Int32)PORTS.B] |= (UInt16)bv32_NO[sectionPortB];
            portStates[(Int32)PORTS.CL] |= (UInt16)bv32_NO[sectionPortCL];
            portStates[(Int32)PORTS.CH] |= (UInt16)bv32_NO[sectionPortCH];

            PortsWrite(Only.USB_ERB24s[ue], portStates);
        }

        public static void Set(UE ue, C.S s) {
            Dictionary<R, C.S> RεS = new Dictionary<R, C.S>();
            foreach (R r in Enum.GetValues(typeof(R))) RεS.Add(r, s);
            Set(ue, RεS);
        }

        // Below 3 methods mainly useful for parallelism, when testing multiple UUTs concurrently, with each B wired identically to test 1 UUT.
        public static void Set(HashSet<UE> ues, C.S s) { foreach (UE ue in ues) { Set(ue, s); } }

        public static void Set(HashSet<UE> ues, HashSet<R> rs, C.S s) { foreach (UE ue in ues) Set(ue, rs, s); }

        public static void Set(Dictionary<UE, Dictionary<R, C.S>> UEεRεS) { foreach (KeyValuePair<UE, Dictionary<R, C.S>> kvp in UEεRεS) Set(kvp.Key, kvp.Value); }

        public static void Set(C.S s) { foreach (UE ue in Enum.GetValues(typeof(UE))) Set(ue, s); }
        #endregion Set

        #region private methods
        private static UInt16 PortRead(MccBoard mccBoard, DigitalPortType digitalPortType) {
            ErrorInfo errorInfo = mccBoard.DIn(digitalPortType, out UInt16 dataValue);
            ProcessErrorInfo(mccBoard, errorInfo);
            return dataValue;
        }

        private static UInt16[] PortsRead(MccBoard mccBoard) {
            return new UInt16[] {
                PortRead(mccBoard, DigitalPortType.FirstPortA),
                PortRead(mccBoard, DigitalPortType.FirstPortB),
                PortRead(mccBoard, DigitalPortType.FirstPortCL),
                PortRead(mccBoard, DigitalPortType.FirstPortCH)
            };
        }

        private static void PortWrite(MccBoard mccBoard, DigitalPortType digitalPortType, UInt16 dataValue) {
            ErrorInfo errorInfo = mccBoard.DOut(digitalPortType, dataValue);
            ProcessErrorInfo(mccBoard, errorInfo);
        }

        private static void PortsWrite(MccBoard mccBoard, UInt16[] ports) {
            PortWrite(mccBoard, DigitalPortType.FirstPortA, ports[(Int32)PORTS.A]);
            PortWrite(mccBoard, DigitalPortType.FirstPortB, ports[(Int32)PORTS.B]);
            PortWrite(mccBoard, DigitalPortType.FirstPortCL, ports[(Int32)PORTS.CL]);
            PortWrite(mccBoard, DigitalPortType.FirstPortCH, ports[(Int32)PORTS.CH]);
        }

        private static DigitalPortType GetPort(R r) {
            switch (r) {
                case R relay when R.C01 <= relay && relay <= R.C08: return DigitalPortType.FirstPortA;
                case R relay when R.C09 <= relay && relay <= R.C16: return DigitalPortType.FirstPortB;
                case R relay when R.C17 <= relay && relay <= R.C20: return DigitalPortType.FirstPortCL;
                case R relay when R.C21 <= relay && relay <= R.C24: return DigitalPortType.FirstPortCH;
                default: throw new NotImplementedException(TestExecutive.NotImplementedMessageEnum(typeof(R)));
            }
        }

        private static void ProcessErrorInfo(MccBoard mccBoard, ErrorInfo errorInfo) {
            // Transform C style error-checking to .Net style exceptioning.
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) {
                throw new InvalidOperationException(
                    $"{Environment.NewLine}" +
                    $"MccBoard BoardNum   : {mccBoard.BoardNum}.{Environment.NewLine}" +
                    $"MccBoard BoardName  : {mccBoard.BoardName}.{Environment.NewLine}" +
                    $"MccBoard Descriptor : {mccBoard.Descriptor}.{Environment.NewLine}" +
                    $"ErrorInfo Value     : {errorInfo.Value}.{Environment.NewLine}" +
                    $"ErrorInfo Message   : {errorInfo.Message}.{Environment.NewLine}");
            }
        }

        private static Int32[] GetUE24BitVector32Masks() {
            Int32 ue24RelayCount = Enum.GetValues(typeof(R)).Length;
            Debug.Assert(ue24RelayCount == 24);
            Int32[] ue24BitVector32Masks = new Int32[ue24RelayCount];
            ue24BitVector32Masks[0] = BitVector32.CreateMask();
            for (Int32 i = 0; i < ue24RelayCount - 1; i++) ue24BitVector32Masks[i + 1] = BitVector32.CreateMask(ue24BitVector32Masks[i]);
            return ue24BitVector32Masks;
        }
        #endregion private methods
        #endregion methods
    }
}