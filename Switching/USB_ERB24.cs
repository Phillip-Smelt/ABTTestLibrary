using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using MccDaq; // MCC DAQ Universal Library 6.73 from https://www.mccdaq.com/Software-Downloads.
using static TestLibrary.Switching.RelayForms;

namespace TestLibrary.Switching {
    public static class USB_ERB24 {
        // TODO: Once this class' internal methods are fully verified by TestLibraryTests, refactor them to private methods & delete TestLibraryTest's Unit tests for the now private methods.
        // NOTE: This class assumes all USB-ERB24 relays are configured for Non-Inverting Logic & Pull-Down/de-energized at power-up.
        // NOTE: USB-ERB24 relays are configurable for either Non-Inverting or Inverting logic, via hardware DIP switch S1.
        //  - Non-Inverting:  Logic low de-energizes the relays, logic high energizes them.
        //  - Inverting:      Logic low energizes the relays, logic high de-energizes them.  
        // NOTE: USB-ERB24 relays are configurable with default power-up states, via hardware DIP switch S2.
        //  - Pull-Up:        Relays are energized at power-up.
        //  - Pull-Down:      Relays are de-energized at power-up.
        //  - https://www.mccdaq.com/PDFs/Manuals/usb-erb24.pdf.
        // NOTE: UE24 enum is a static definition of TestLibrary's MCC USB-ERB24(s).
        // Potential dynamic definition methods for UE24:
        //  - Read them from MCC InstaCal's cb.cfg file.
        //  - Dynamically discover them programmatically: https://www.mccdaq.com/pdfs/manuals/Mcculw_WebHelp/ULStart.htm.
        //  - Specify MCC USB-ERB24s in App.config, then confirm existence during TestLibrary's initialization.
        // NOTE: MCC's InstaCal USB-ERB24's UE24 number indexing begins at 0, guessing because USB device indexing is likely also zero based.
        // - So UE24.E01 numerical value is 0, which is used when constructing a new MccBoard UE24.E01 object:
        // - Instantiation 'new MccBoard((Int32)UE24.E01)' is equivalent to 'new MccBoard(0)'.
        public enum UE24 { E01 }
        public enum R : Byte { C01, C02, C03, C04, C05, C06, C07, C08, C09, C10, C11, C12, C13, C14, C15, C16, C17, C18, C19, C20, C21, C22, C23, C24 }
        // NOTE: enum named R instead of RELAYS for concision; consider below:
        //  - Set(UE24.E01, new Dictionary<R, C>() {{R.C01,C.NC}, {R.C02,C.NO}, ... {R.C24,C.NC} });
        //  - Set(UE24.E01, new Dictionary<R, C>() {{RELAYS.C01,C.NC}, {RELAYS.C02,C.NO}, ... {RELAYS.C24,C.NC} });
        // NOTE: R's elements named C## because USB-ERB24's relays are all Form C.
        //  - Also because can't name them R.01, R.02...R.24; identifiers cannot begin with numbers.
        // NOTE: Enumerate Form A relays as R.A01, R.A02...
        // NOTE: Enumerate Form B relays as R.B01, R.B02...
        internal enum PORTS { A, B, CL, CH }

        private const String PORT_INVALID = "Invalid USB-ERB24 DigitalPortType, must be in set '{ FirstPortA, FirstPortB, FirstPortCL, FirstPortCH }'.";

        #region public methods
        public static Boolean Is(UE24 UE24, R R, C C) { return Get(UE24, R) == C; }

        public static Boolean Are(UE24 UE24, HashSet<R> Rs, C C) {
            Dictionary<R, C> RεC = Rs.ToDictionary(r => r, r => C);
            Dictionary<R, C> Are = Get(UE24, Rs);
            return (RεC.Count == Are.Count) && !RεC.Except(Are).Any();
        }

        public static Boolean Are(UE24 UE24, Dictionary<R, C> RεC) {
            Dictionary<R, C> Are = Get(UE24, new HashSet<R>(RεC.Keys));
            return (RεC.Count == Are.Count) && !RεC.Except(Are).Any();
        }

        public static Boolean Are(UE24 UE24, C C) {
            Dictionary<R, C> Are = Get(UE24);
            Boolean areEqual = true;
            foreach(KeyValuePair<R, C> kvp in Are) areEqual &= kvp.Value == C;
            return areEqual;
        }

        public static Boolean Are(C C) {
            Boolean areEqual = true;
            foreach (UE24 ue24 in Enum.GetValues(typeof(UE24))) areEqual &= Are(ue24, C);
            return areEqual;
        }

        public static C Get(UE24 UE24, R R) {
            MccBoard mccBoard = new MccBoard((Int32)UE24);
            ErrorInfo errorInfo = mccBoard.DBitIn(DigitalPortType.FirstPortA, (Int32)R, out DigitalLogicState digitalLogicState);
            ProcessErrorInfo(mccBoard, errorInfo);
            return (digitalLogicState == DigitalLogicState.Low) ? C.NC : C.NO;
        }

        public static Dictionary<R, C> Get(UE24 UE24, HashSet<R> Rs) {
            Dictionary<R, C> RεC = Get(UE24);
            foreach(R R in Rs) if (!RεC.ContainsKey(R)) RεC.Remove(R);
            return RεC;
        }

        public static Dictionary<R, C> Get(UE24 UE24) {
            MccBoard mccBoard = new MccBoard((Int32)UE24);
            UInt16[] bits = Read(mccBoard);
            UInt32[] biggerBits = Array.ConvertAll(bits, delegate (UInt16 uInt16) { return (UInt32)uInt16; });
            UInt32 relayBits = 0x0000;
            relayBits |= biggerBits[(Int32)PORTS.A] << 00;
            relayBits |= biggerBits[(Int32)PORTS.B] << 08;
            relayBits |= biggerBits[(Int32)PORTS.CL] << 12;
            relayBits |= biggerBits[(Int32)PORTS.CH] << 16;
            BitVector32 bitVector32 = new BitVector32((Int32)relayBits);

            Dictionary<R, C> RεC = new Dictionary<R, C>();
            R R; C C;
            for (Int32 i = 0; i < 32; i++) {
                R = (R)Enum.ToObject(typeof(R), bitVector32[i]);
                C = bitVector32[i] ? C.NO : C.NC;
                RεC.Add(R, C);
            }
            return RεC;
        }

        public static Dictionary<UE24, Dictionary<R, C>> Get() {
            Dictionary<UE24, Dictionary<R, C>> UE24εRεC = new Dictionary<UE24, Dictionary<R, C>>();
            foreach (UE24 ue24 in Enum.GetValues(typeof(UE24))) UE24εRεC.Add(ue24, Get(ue24));
            return UE24εRεC;
        }

        public static void Set(UE24 UE24, R R, C C) {
            MccBoard mccBoard = new MccBoard((Int32)UE24);
            ErrorInfo errorInfo = mccBoard.DBitOut(DigitalPortType.FirstPortA, (Int32)R, (C is C.NC) ? DigitalLogicState.Low : DigitalLogicState.High);
            ProcessErrorInfo(mccBoard, errorInfo);
        }

        public static void Set(UE24 UE24, HashSet<R> R, C C) { Set(UE24, R.ToDictionary(r => r, r => C)); }

        public static void Set(UE24 UE24, Dictionary<R, C> RεC) {
            MccBoard mccBoard = new MccBoard((Int32)UE24);
            UInt32 portBits = 0x0000_0000;
            foreach (R R in RεC.Keys) portBits |= (UInt32)1 << (Byte)R; // Sets a 1 in each bit corresponding to relay state in RεC.
            Byte[] bite = BitConverter.GetBytes(portBits);
            UInt16[] biggerBite = Array.ConvertAll(bite, delegate (Byte b) { return (UInt16)b; });
            UInt16[] ports = Read(mccBoard);
            ports[(Int32)PORTS.A] |= biggerBite[(Int32)PORTS.A];
            ports[(Int32)PORTS.B] |= biggerBite[(Int32)PORTS.B];
            ports[(Int32)PORTS.CL] |= (biggerBite[(Int32)PORTS.CL] &= 0x0F); // Clear CH bits.
            ports[(Int32)PORTS.CH] |= (biggerBite[(Int32)PORTS.CH] &= 0xF0); // Clear CL bits.
            Write(mccBoard, ports);
        }

        public static void Set(UE24 UE24, C C) {
            Dictionary<R, C> RεC = new Dictionary<R, C>();
            foreach (R R in Enum.GetValues(typeof(R))) RεC.Add(R, C);
            Set(UE24, RεC);
        }

        public static void Set(C C) { foreach (UE24 ue24 in Enum.GetValues(typeof(UE24))) Set(ue24, C); }

        public static void Set(Dictionary<UE24, Dictionary<R, C>> UE24εRεC) { foreach (KeyValuePair<UE24, Dictionary<R, C>> kvp in UE24εRεC) Set(kvp.Key, kvp.Value); }
        #endregion public methods

        #region internal methods
        internal static UInt16 Read(MccBoard mccBoard, DigitalPortType digitalPortType) {
            ErrorInfo errorInfo = mccBoard.DIn(digitalPortType, out UInt16 dataValue);
            ProcessErrorInfo(mccBoard, errorInfo);
            return dataValue;
        }

        internal static UInt16[] Read(MccBoard mccBoard) {
            return new UInt16[] {
                Read(mccBoard, DigitalPortType.FirstPortA),
                Read(mccBoard, DigitalPortType.FirstPortB),
                Read(mccBoard, DigitalPortType.FirstPortCL),
                Read(mccBoard, DigitalPortType.FirstPortCH)
            };
        }

        internal static void Write(MccBoard mccBoard, DigitalPortType digitalPortType, UInt16 dataValue) {
            ErrorInfo errorInfo = mccBoard.DOut(digitalPortType, dataValue);
            ProcessErrorInfo(mccBoard, errorInfo);
        }

        internal static void Write(MccBoard mccBoard, UInt16[] ports) {
            Write(mccBoard, DigitalPortType.FirstPortA, ports[(Int32)PORTS.A]);
            Write(mccBoard, DigitalPortType.FirstPortB, ports[(Int32)PORTS.B]);
            Write(mccBoard, DigitalPortType.FirstPortCL, ports[(Int32)PORTS.CL]);
            Write(mccBoard, DigitalPortType.FirstPortCH, ports[(Int32)PORTS.CH]);
        }

        internal static Boolean Is(MccBoard mccBoard, DigitalPortType digitalPortType, UInt16 portState) { return Read(mccBoard, digitalPortType) == portState; }

        internal static DigitalPortType Get(R R) {
            switch (R) {
                case R r when R.C01 <= r && r <= R.C08: return DigitalPortType.FirstPortA;
                case R r when R.C09 <= r && r <= R.C16: return DigitalPortType.FirstPortB;
                case R r when R.C17 <= r && r <= R.C20: return DigitalPortType.FirstPortCL;
                case R r when R.C21 <= r && r <= R.C24: return DigitalPortType.FirstPortCH;
                default: throw new ArgumentException(PORT_INVALID);
            }
        }

        internal static void ProcessErrorInfo(MccBoard mccBoard, ErrorInfo errorInfo) {
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
        #endregion internal methods
    }
}
