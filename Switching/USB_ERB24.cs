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
        public enum R : Byte { C01, C02, C03, C04, C05, C06, C07, C08, C09, C10, C11, C12, C13, C14, C15, C16, C17, C18, C19, C20, C21, C22, C23, C24 } // USB_ERB24 Relays, all Form C.
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
        public abstract class SwitchedNets { }
        /// <summary>
        /// SwitchedNets correlates Customer UUT nets to switched ABT test system nets:
        /// - SwitchedNets exclusively correlates switched nets connected via relays, which can be temporarily connected/disconnected as desired.
        /// - SwitchedNets specifically excludes unswitched nets connected via permanent circuitry; signal conditioners, wire harness continuities, etc.
        /// In TestExecutor.cs, concrete class SN inherits abstract class SwitchedNets and cross-references Customer UUT input stimuli & output signals switched into ABT test system inputs/outputs.
        /// - SN name is Customer UUT's net name, SN value is correlated ABT test system switched net name.
        /// - If ABT test system has multiple names for nets, prefer utilizing the switched net name over permanently connected name.
        /// 
        /// So, in TestExecutor.cs, Customer UUT power supplies, inputs & outputs might be meaningfully correlated to switched ABT test system fixturing & instrumentation as follows:
        ///     internal sealed class SN : SwitchedNets {
        ///         internal const String P3V3 = "3.3V";
        ///         internal const String P5V  = "5V";
        ///         internal const String P12V = "+12V";
        ///         internal const String N12V = "-12V";
        ///         internal const String EnableN = "~Enable";
        ///     }
        /// </summary>

        public sealed class UE24_R {
            public readonly UE UE;
            public readonly R R;
            public readonly String C;
            public readonly String NC;
            public readonly String NO;

            public UE24_R(UE UE, R R, String C, String NC, String NO) {
                this.UE = UE; this.R = R; this.C = C; this.NC = NC; this.NO = NO;
                Validate();
            }

            private void Validate() {
                if (String.Equals(C, String.Empty)) throw new ArgumentException($"Relay terminal Common '{this.C}' cannot be String.Empty.");
                if (String.Equals(C, NO)) throw new ArgumentException($"Relay terminals Common '{this.C}' & Normally Open '{this.NO}' cannot be identical.");
                if (String.Equals(C, NC)) throw new ArgumentException($"Relay terminals Common '{this.C}' & Normally Closed '{this.NC}' cannot be identical.");
                if (String.Equals(NC, NO)) throw new ArgumentException($"Relay terminals Normally Closed '{this.NC}' & Normally Open '{this.NO}' cannot be identical.");
            }

            public static String GetUE(UE ue) { return Enum.GetName(typeof(UE), ue); }
            public static String GetR(R r) { return Enum.GetName(typeof(R), r); }

            public C.S Get() { return USB_ERB24.Get(this.UE, this.R); }

            public void Set(C.S state) { USB_ERB24.Set(this.UE, this.R, state); }

            public Boolean Is(C.S state) { return USB_ERB24.Is(this.UE, this.R, state); }

            public override Boolean Equals(Object obj) {
                UE24_R ue24_r = obj as UE24_R;
                if (ReferenceEquals(this, ue24_r)) return true;
                return ue24_r != null && ue24_r.UE == this.UE && ue24_r.R == this.R;
            }

            public override Int32 GetHashCode() { return 3 * this.UE.GetHashCode() + this.R.GetHashCode(); }
        }

        public sealed class UE24_S {
            public readonly UE UE;
            public readonly R R;
            public readonly C.S S;

            public UE24_S(USB_ERB24.UE UE, R R, C.S S) { this.UE = UE; this.R = R; this.S = S; }

            public override Boolean Equals(Object obj) {
                UE24_S ue24_s = obj as UE24_S;
                if (ReferenceEquals(this, ue24_s)) return true;
                return ue24_s != null && ue24_s.UE == this.UE && ue24_s.R == this.R;
            }

            public override Int32 GetHashCode() { return 3 * this.UE.GetHashCode() + this.R.GetHashCode() + this.S.GetHashCode(); }
        }

        public sealed class UE24_T {
            public readonly UE UE;
            public readonly R R;
            public readonly C.T T;

            public UE24_T(USB_ERB24.UE UE, R R, C.T T) { this.UE = UE; this.R = R; this.T = T; }

            public override Boolean Equals(Object obj) {
                UE24_T ue24_t = obj as UE24_T;
                if (ReferenceEquals(this, ue24_t)) return true;
                return ue24_t != null && ue24_t.UE == this.UE && ue24_t.R == this.R && this.T == ue24_t.T;
            }

            public override Int32 GetHashCode() { return 3 * this.UE.GetHashCode() + this.R.GetHashCode() + this.T.GetHashCode(); }
        }


        public sealed class UE24_Route {
            public readonly (String SN1, String SN2) Route;
            public UE24_Route((String SN1, String SN2) route) { this.Route = route; }
        }

        public sealed class UE24_Routes {
            public readonly Dictionary<UE24_Route, HashSet<UE24_S>> Routes;

            public UE24_Routes(Dictionary<UE24_Route, HashSet<UE24_S>> routes) { this.Routes = routes; }
        }

        public sealed class UE24_Rs {
            public readonly HashSet<UE24_R> Rs;
            public readonly Dictionary<String, HashSet<UE24_T>> NTs = new Dictionary<String, HashSet<UE24_T>>();

            public UE24_Rs(HashSet<UE24_R> rs) {
                this.Rs = rs;
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
                        sb.AppendLine($"   B1='{UE24_R.GetUE(rr.r1.UE)}', R1='{UE24_R.GetR(rr.r1.R)}', C1='{rr.r1.C}', NC1='{rr.r1.NC}', NO1='{rr.r1.NO}'.");
                        sb.AppendLine($"   B2='{UE24_R.GetUE(rr.r2.UE)}', R2='{UE24_R.GetR(rr.r2.R)}', C2='{rr.r2.C}', NC2='{rr.r2.NC}', NO2='{rr.r2.NO}'.");
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

            public static void Switch(String N1, String N2) {
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

            public static void Switch(HashSet<String> Ns) {
                // Connect all HashSet<N> Ns simultaneously to one another.
                // Superset of Connect(N N1, N N2).
                // Sequentially invoke Connect(N N1, N N2) with foreach N in Ns.
                // Then invoke AreConnected((HashSet<N> Ns) to verify all are still *simultaneously* connected.
                // - Possible to wire N1 to T.C, N2 to T.NO & Net3 to T.NC all having matching Relay addresses (UE, R).
                // - Invoking Connect(HashSet<N1, N2, Net3>) is impossible to simultaneously achieve.
                // - But will easily be sequentially achieved by Connect(N1, N2) & Connect(N2, Net3).
                // Used for Shorts/Opens testing.
            }

            public static void DisConnect(String N1, String N2) {
                // Same as Connect, except disconnect N1 from N2 with opposite C state:
                // - If C to NC connects N1 to N2, invoke invoke Set(UE, C, C.NO).
                // - If C to NO connects N1 to N2, invoke invoke Set(UE, C, C.NC).
            }

            public static void DisConnect(HashSet<String> Ns) {
                // Disonnect all HashSet<N> Ns Ns simultaneously from one another.
                // Superset of DisConnect(N N1, N N2).
                // Sequentially invoke DisConnect(N N1, N N2) with foreach N in Ns.
                // Then invoke AreDisConnected((HashSet<N> Ns) to verify all are still *simultaneously* disconnected.
                // - Possible to wire N1 to T.C, N2 to T.NO & Net3 to T.NC all having matching Relay addresses (UE, R).
                // - Invoking DisConnect(HashSet<N1, N2, Net3>) is impossible to simultaneously achieve.
                // - But will easily be sequentially achieved by DisConnect(N1, N2) & Connect(N2, Net3).
                // Used for Shorts/Opens testing.
            }

            public static Boolean AreConnected(String N1, String N2) {
                // Verify/refute N1 currently connected to N2.
                // Use GetConnections(N1) returning HashSet<N>.Intersect<N2> to verify/refute if N1 currently connected to N2.
                // For Debug.Assert() statements.
                return false;
            }

            public static Boolean AreDisConnected(String N1, String N2) { return !AreConnected(N1, N2); }

            public static Boolean AreConnected(HashSet<String> Ns) {
                // Verify/refute all Ns in HashSet<N> are interconnected to one another.
                // Superset of AreConnected(N N1, N N2).
                // Can recursively invoke AreConnected(N N1, N N2) with foreach N in Ns.
                // For Debug.Assert() statements.
                return false;
            }

            public static Boolean AreDisConnected(HashSet<String> Ns) {
                return false;
            }

            private static Boolean AreConnectable(String N1, String N2) {
                // Verify/refute N1 can be connected to N2.
                // Use GetConnections(N1) returning HashSet<N>.Intersect<N2> to verify/refute if N1 currently connected to N2.
                // Reccommend programming 
                // For Debug.Assert() statements.
                return false;
            }

            private static Boolean AreConnectable(HashSet<String> Ns) {
                // Verify/refute HashSet<N> Ns can be interconnected to one another.
                // Use AreConnectable(N1) returning HashSet<N>.Intersect<N2> to verify/refute if N1 connected to N2.
                // Reccommend programming 
                // For Debug.Assert() statements.
                return false;
            }

            public static Dictionary<String, HashSet<String>> GetConnections(String N1) {
                return new Dictionary<String, HashSet<String>>();
                // Use Get() function, returning Dictionary<UE, Dictionary<R, C.S>>, and convert to Dictionary<N, HashSet<N>>.
            }
            // If can convert:
            //      - Dictionary<String, HashSet<T>>
            // to/from:
            //      - Dictionary<UE, Dictionary<R, C.S>>
            // Can invoke:
            //       - Set(Dictionary<UE, Dictionary<R, C.S>> UEεRεS)
            //       - Are(Dictionary<UE, Dictionary<R, C.SC>> UEεRεS)
            //       - Get()
            //
            // Initially support UE24's Set(), Is() & Get() functions for discrete/single (UE, R, C.S)
        }
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