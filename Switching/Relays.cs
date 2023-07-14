using MccDaq;
using System;
using System.Collections.Generic;

public enum A { NO, C }
// A.NO; Form A relay is deenergized and in normally opened state.
// A.C;  Form A relay is energized and in abnormally closed state.

public enum B { NC, O }
// B.NC; Form B relay is deenergized and in normally closed state.
// B.O;  Form B relay is energized and in abnormally opened state.

public enum C { NC, NO }
// C.NC; Form C relay is deenergized and in normally closed/normally open state, with common C terminal connected to NC terminal & disconnected from NO terminal.
// C.NO; Form C relay is energized and in abnormally opened/abnormally closed state, with common C terminal disconnected from NC terminal & connected to NO terminal.

public enum TERMINALS_FORM_A { C, NO }
public enum TERMINALS_FORM_B { C, NC }
public enum TERMINALS_FORM_C { C, NC, NO }

// https://forum.digikey.com/t/understanding-form-a-form-b-form-c-contact-configuration/811

namespace ABT.TestSpace.TestExec.Switching {
     public sealed class FormC_Terminal {
        public readonly USB_ERB24.UE24 RB;
        public readonly USB_ERB24.R R;
        public readonly TERMINALS_FORM_C T;
        public FormC_Terminal(USB_ERB24.UE24 RB, USB_ERB24.R R, TERMINALS_FORM_C T) { this.RB = RB; this.R = R; this.T = T; }
    }

    public enum UUT_NETS { NET_1, NET_2, NET_3 }

    public abstract class NetsToFormC_Terminals {
        public readonly Dictionary<UUT_NETS, HashSet<FormC_Terminal>> NCT;
        NetsToFormC_Terminals() {
            this.NCT = new Dictionary<UUT_NETS, HashSet<FormC_Terminal>>() {
                {UUT_NETS.NET_1, new HashSet<FormC_Terminal>() {new FormC_Terminal(USB_ERB24.UE24.RB0, USB_ERB24.R.C01, TERMINALS_FORM_C.C)}},
                {UUT_NETS.NET_2, new HashSet<FormC_Terminal>() {new FormC_Terminal(USB_ERB24.UE24.RB0, USB_ERB24.R.C02, TERMINALS_FORM_C.NC)}},
                {UUT_NETS.NET_3, new HashSet<FormC_Terminal>() {new FormC_Terminal(USB_ERB24.UE24.RB0, USB_ERB24.R.C03, TERMINALS_FORM_C.NO)}}
            };

            Validate();
        }

        private void Validate() {
            // Validate this.NCT:
            // - Verify the union of all this.NCT HashSet<FormC_Terminal> has no duplicated FormC_Terminal objects.
            // - Verify every unique UE24.RB & R.C has at least a FormC_Terminal for TERMINALS_FORM_C.C and one/both for { TERMINALS_FORM_C.NC and/or TERMINALS_FORM_C.NO }
            //   - That is, every FormC relay Common terminal is connected to a UUT_NETS, and one/both of it's Normally Closed/Normally Open terminals
            //     are connected to UUT_NETS.
            // - Verify every FormC_Terminal per unique UE24.RB & R.C pair has different UUT_NETS on each TERMINALS_FORM_C.
            //   - That is, verify the Common, Normally Open & Normally Closed terminals are all different UUT_NETS.
        }

        public static void Connect(UUT_NETS Net1, UUT_NETS Net2) {
            // Intersect, Verify & Connect.
            // - Index this.NCT to get the HashSet<FormC_Terminal> correlated to Net1; call it HashSet<Terminals1>.
            // - Index this.NCT to get the HashSet<FormC_Terminal> correlated to Net2; call it HashSet<Terminals2>.
            // - HashSet.Intersect(Terminals1, Terminals2) to get resulting HashSet<FormC_Terminal> that have matching UE24.RB & R.C pairs.
            //   - If there are no FormC_Terminal intersections with matching UE24.RB & R.C pairs, throw ArgumentException.
            // - Verify "You can get there from here":
            //   - Verify for each relay UE24.RB & R.C pairs one is a Common terminal, the other a Normally Closed or Open terminal.
            //   - If not throw ArgumentException.
            //   - There should be at least one UE24.RB & R.C matching FormC_Terminal pair that can connect Net1 to Net2.
            //   - There may be multiple UE24.RB & R.C matching FormC_Terminal pairs that can can connect Net1 to Net2.
            //     - Possibly for current capacity/ampacity, 4 wire Kelvin sensing or intentional duplication.  Or unintentional duplication :-)
            // - Connect all available FormC_Terminal UE24.RB & R.C relays to Nets Net1 & Net2 using USB_ERB24 class.
            //   - Will always be either C to NC or C to NO to connect Net1 to Net2.
            //   - If C to NC connects Net1 to Net2, invoke USB_ERB24.Set(RB, C, C.NC).
            //   - If C to NO connects Net1 to Net2, invoke USB_ERB24.Set(RB, C, C.NO).
        }

        public static void Connect(HashSet<UUT_NETS> Nets) {
            // Connect all Nets to one another.
            // Superset of Connect(UUT_NETS Net1, UUT_NETS Net2).
            // Can recursively invoke Connect(UUT_NETS Net1, UUT_NETS Net2) with foreach UUT_NETS in Nets.
            // Used for Shorts testing.
        }

        public static void DisConnect(UUT_NETS Net1, UUT_NETS Net2) {
            // Same as Connect, except disconnect Net1 from Net2 with opposite C state:
            // - If C to NC connects Net1 to Net2, invoke invoke USB_ERB24.Set(RB, C, C.NO).
            // - If C to NO connects Net1 to Net2, invoke invoke USB_ERB24.Set(RB, C, C.NC).
        }

        public static void DisConnect(HashSet<UUT_NETS> Nets) {
            // Disonnect all Nets to from one another.
            // Superset of DisConnect(UUT_NETS Net1, UUT_NETS Net2).
            // Can recursively invoke DisConnect(UUT_NETS Net1, UUT_NETS Net2) with foreach UUT_NETS in Nets.
            // Used for Shorts testing.
        }

        public static Boolean AreConnected(UUT_NETS Net1, UUT_NETS Net2) {
            // Verify/refute Net1 connected to Net2.
            // For Debug.Assert() statements.
            return false;
        }

        public static Boolean AreConnected(HashSet<UUT_NETS> Nets) {
            // Verify/refute all Nets in HashSet<UUT_NETS> are interconnected to one another.
            // Superset of AreConnected(UUT_NETS Net1, UUT_NETS Net2).
            // Can recursively invoke AreConnected(UUT_NETS Net1, UUT_NETS Net2) with foreach UUT_NETS in Nets.
            // For Debug.Assert() statements.
            return false;
        }

        public static HashSet<UUT_NETS> GetConnections(UUT_NETS Net1) {

        }

        // Initially support USB_ERB24's Set(), Is() & Get() functions for discrete/single (RB, R, C)
        // - Respectively are NetsToFormC_Terminals Connect(), AreConnected() & GetConnections() functions.
        // Eventually support USB_ERB24's Set(), Is() & Get() functions for:
        // - HashSets<RB> (entire Relay Board(s)>
        // - RB, HashSets<R>
        // - Dictionaries<RB, HashSet<R>> & All 
    }
}

