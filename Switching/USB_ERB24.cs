using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using MccDaq; // MCC DAQ Universal Library 6.73 from https://www.mccdaq.com/Software-Downloads.

namespace ABT.TestSpace.TestExec.Switching {
    public sealed class UE24 {
        // UE24 is an abbreviation of the USB_ERB24 initialisation (Universal Serial Bus Electronic Relay Board with 24 Form C relays).
        public Dictionary<B, MccBoard> UE24s;
        private readonly static UE24 _only = new UE24();
        public static UE24 Only { get { return _only; } }
        static UE24() { }
        // Singleton pattern requires explicit static constructor to tell C# compiler not to mark type as beforefieldinit.
        // https://csharpindepth.com/articles/singleton
        private UE24() {
            this.UE24s = new Dictionary<B, MccBoard>() {
                {B.B0, new MccBoard((Int32)B.B0)},
                {B.B1, new MccBoard((Int32)B.B1)}
            };
        }

        public enum B { B0, B1 } // USB_ERB24 Boards.
        public enum R : Byte { C01, C02, C03, C04, C05, C06, C07, C08, C09, C10, C11, C12, C13, C14, C15, C16, C17, C18, C19, C20, C21, C22, C23, C24 } // USB_ERB24 Relays, all Form C.
        // NOTE: This class assumes all USB-ERB24 relays are configured for Non-Inverting Logic & Pull-Down/de-energized at power-up.
        // NOTE: USB-ERB24 relays are configurable for either Non-Inverting or Inverting logic, via hardware DIP switch S1.
        //  - Non-Inverting:  Logic low de-energizes the relays, logic high energizes them.
        //  - Inverting:      Logic low energizes the relays, logic high de-energizes them.  
        // NOTE: USB-ERB24 relays are configurable with default power-up states, via hardware DIP switch S2.
        //  - Pull-Up:        Relays are energized at power-up.
        //  - Pull-Down:      Relays are de-energized at power-up.
        //  - https://www.mccdaq.com/PDFs/Manuals/usb-erb24.pdf.
        // NOTE: B enum is a static definition of TestExecutive's MCC USB-ERB24(s).
        // Potential dynamic definition methods for UE24s:
        //  - Read them from MCC InstaCal's cb.cfg file.
        //  - Dynamically discover them programmatically: https://www.mccdaq.com/pdfs/manuals/Mcculw_WebHelp/ULStart.htm.
        //  - Specify MCC USB-ERB24s in TestExecutive.config.xml.
        // NOTE: MCC's InstaCal USB-ERB24 indexing begins at 0, guessing because USB device indexing is likely also zero based.
        // - So B.B0's numerical value is 0, which is used when constructing a new MccBoard B.B0 object:
        // - Instantiation 'new MccBoard((Int32)B.B0)' is equivalent to 'new MccBoard(0)'.
        // NOTE: enum named R instead of RELAYS for concision; consider below:
        //  - Set(B.B0, new Dictionary<R, FC.S>() {{R.C01,FC.S.NC}, {R.C02,FC.S.NO}, ... {R.C24,FC.S.NC} });
        //  - Set(B.B0, new Dictionary<RELAYS, FC.S>() {{RELAYS.C01,FC.S.NC}, {RELAYS.C02,FC.S.NO}, ... {RELAYS.C24,FC.S.NC} });
        // NOTE: Enumerate Form A relays as public enum R { A01, A02, A03... }
        // NOTE: Enumerate Form B relays as public enum R { B01, B02, B03... }
        // NOTE: R's items named C## because USB-ERB24's relays are all Form C.
        // NOTE: Some manufacturer's Relay Boards contain heterogenous assortments of Form A, and/or Form B, and/or Form C relays.
        //  - In such case, R would be enumerated as { A01, A02, A03, A04, A05, A06, A07, A08, B09, B10, B11, B12, B13, B14, B15, B16, C17, C18, C19, C20, C21, C22, C23, C24 }
        //    if the first 8 of 24 relays are Form A, the 2nd 8 are Form B, and the last 8 Form C.

        internal enum PORTS { A, B, CL, CH }
        internal static Int32[] _ue24bitVector32Masks = GetUE24BitVector32Masks();

        #region Is/Are
        public static Boolean Is(B b, R r, FC.S s) { return Get(b, r) == s; }

        public static Boolean Are(B b, HashSet<R> rs, FC.S s) {
            Dictionary<R, FC.S> RεS = rs.ToDictionary(r => r, r => s);
            Dictionary<R, FC.S> Are = Get(b, rs);
            return RεS.Count == Are.Count && !RεS.Except(Are).Any();
        }

        public static Boolean Are(B b, Dictionary<R, FC.S> RεS) {
            Dictionary<R, FC.S> Are = Get(b, new HashSet<R>(RεS.Keys));
            return RεS.Count == Are.Count && !RεS.Except(Are).Any();
        }

        public static Boolean Are(B b, FC.S s) {
            Dictionary<R, FC.S> Are = Get(b);
            Boolean areEqual = true;
            foreach (KeyValuePair<R, FC.S> kvp in Are) areEqual &= kvp.Value == s;
            return areEqual;
        }

        // Below 3 methods mainly useful for parallelism, when testing multiple UUTs concurrently, with each B wired identically to test 1 UUT.
        public static Boolean Are(HashSet<B> bs, FC.S s) {
            Boolean areEqual = true;
            foreach (B b in bs) areEqual &= Are(b, s);
            return areEqual;
        }

        public static Boolean Are(HashSet<B> bs, HashSet<R> rs, FC.S s) {
            Boolean areEqual = true;
            foreach (B b in bs) areEqual &= Are(b, rs, s);
            return areEqual;
        }

        public static Boolean Are(Dictionary<B, Dictionary<R, FC.S>> BεRεS) {
            Boolean areEqual = true;
            foreach (KeyValuePair<B, Dictionary<R, FC.S>> kvp in BεRεS) areEqual &= Are(kvp.Key, kvp.Value);
            return areEqual;
        }

        public static Boolean Are(FC.S s) {
            Boolean areEqual = true;
            foreach (B b in Enum.GetValues(typeof(B))) areEqual &= Are(b, s);
            return areEqual;
        }
        #endregion Is/Are

        #region Get
        public static FC.S Get(B b, R r) {
            ErrorInfo errorInfo = Only.UE24s[b].DBitIn(DigitalPortType.FirstPortA, (Int32)r, out DigitalLogicState digitalLogicState);
            ProcessErrorInfo(Only.UE24s[b], errorInfo);
            return digitalLogicState == DigitalLogicState.Low ? FC.S.NC : FC.S.NO;
        }

        public static Dictionary<R, FC.S> Get(B b, HashSet<R> rs) {
            Dictionary<R, FC.S> RεS = Get(b);
            foreach (R r in rs) if (!RεS.ContainsKey(r)) RεS.Remove(r);
            return RεS;
        }

        public static Dictionary<R, FC.S> Get(B b) {
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
            R r;  FC.S s;  Dictionary<R, FC.S> RεS = new Dictionary<R, FC.S>();
            for (Int32 i = 0; i < Enum.GetValues(typeof(R)).Length; i++) {
                errorInfo = Only.UE24s[b].DBitIn(DigitalPortType.FirstPortA, i, out digitalLogicState);
                ProcessErrorInfo (Only.UE24s[b], errorInfo);
                r = (R)Enum.ToObject(typeof(R), i);
                s = digitalLogicState == DigitalLogicState.Low ? FC.S.NC : FC.S.NO;
                RεS.Add(r, s);
            }
            return RεS;
            */

            UInt16[] portBits = PortsRead(Only.UE24s[b]);
            UInt32[] biggerPortBits = Array.ConvertAll(portBits, delegate (UInt16 uInt16) { return (UInt32)uInt16; });
            UInt32 relayBits = 0x0000;
            relayBits |= biggerPortBits[(UInt32)PORTS.CH] << 00;
            relayBits |= biggerPortBits[(UInt32)PORTS.CL] << 04;
            relayBits |= biggerPortBits[(UInt32)PORTS.B] << 08;
            relayBits |= biggerPortBits[(UInt32)PORTS.A] << 16;
            BitVector32 bitVector32 = new BitVector32((Int32)relayBits);

            R r; FC.S s; Dictionary<R, FC.S> RεS = new Dictionary<R, FC.S>();
            for (Int32 i = 0; i < _ue24bitVector32Masks.Length; i++) {
                r = (R)Enum.ToObject(typeof(R), i);
                s = bitVector32[_ue24bitVector32Masks[i]] ? FC.S.NO : FC.S.NC;
                RεS.Add(r, s);
            }
            return RεS;
        }

        // Below 3 methods mainly useful for parallelism, when testing multiple UUTs concurrently, with each B wired identically to test 1 UUT.
        public static Dictionary<B, Dictionary<R, FC.S>> Get(HashSet<B> bs) {
            Dictionary<B, Dictionary<R, FC.S>> BεRεS = Get();
            foreach (B b in bs) if (!BεRεS.ContainsKey(b)) BεRεS.Remove(b);
            return BεRεS;
        }

        public static Dictionary<B, Dictionary<R, FC.S>> Get(HashSet<B> bs, HashSet<R> rs) {
            Dictionary<B, Dictionary<R, FC.S>> BεRεS = new Dictionary<B, Dictionary<R, FC.S>>();
            foreach (B b in bs) BεRεS.Add(b, Get(b, rs));
            return BεRεS;
        }

        public static Dictionary<B, Dictionary<R, FC.S>> Get(Dictionary<B, R> BεR) {
            Dictionary<B, Dictionary<R, FC.S>> BεRεS = new Dictionary<B, Dictionary<R, FC.S>>();
            Dictionary<R, FC.S> RεS = new Dictionary<R, FC.S>();
            foreach (KeyValuePair<B, R> kvp in BεR) {
                RεS.Add(kvp.Value, Get(kvp.Key, kvp.Value));
                BεRεS.Add(kvp.Key, RεS);
            }
            return BεRεS;
        }

        public static Dictionary<B, Dictionary<R, FC.S>> Get() {
            Dictionary<B, Dictionary<R, FC.S>> BεRεS = new Dictionary<B, Dictionary<R, FC.S>>();
            foreach (B b in Enum.GetValues(typeof(B))) BεRεS.Add(b, Get(b));
            return BεRεS;
        }
        #endregion Get

        #region Set
        public static void Set(B b, R r, FC.S s) {
            ErrorInfo errorInfo = Only.UE24s[b].DBitOut(DigitalPortType.FirstPortA, (Int32)r, s is FC.S.NC ? DigitalLogicState.Low : DigitalLogicState.High);
            ProcessErrorInfo(Only.UE24s[b], errorInfo);
        }

        public static void Set(B b, HashSet<R> rs, FC.S s) { Set(b, rs.ToDictionary(r => r, r => s)); }

        public static void Set(B b, Dictionary<R, FC.S> RεS) {
            // This method only sets relay states for relays explicitly declared in RεS.
            //  - That is, if RεS = {{R.C01, FC.S.NO}, {R.C02, FC.S.NC}}, then only relays R.C01 & R.C02 will have their states actively set, respectively to NO & NC.
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
            foreach (KeyValuePair<R, FC.S> kvp in RεS) {
                errorInfo = Only.UE24s[b].DBitOut(DigitalPortType.FirstPortA, (Int32)kvp.Key, kvp.Value == FC.S.NC ? DigitalLogicState.Low: DigitalLogicState.High);
                ProcessErrorInfo(Only.UE24s[b], errorInfo);
            }
            */

            UInt32 relayBit;
            UInt32 bits_NC = 0xFFFF_FFFF; // bits_NC utilize Boolean And logic.
            UInt32 bits_NO = 0x0000_0000; // bits_NO utilize Boolean Or logic.

            foreach (KeyValuePair<R, FC.S> kvp in RεS) {
                relayBit = (UInt32)1 << (Byte)kvp.Key;
                if (kvp.Value == FC.S.NC) bits_NC ^= relayBit; // Sets a 0 in bits_NC for each explicitly assigned NC state in RεS.
                else bits_NO |= relayBit;                      // Sets a 1 in bits_NO for each explicitly assigned NO state in RεS.
            }

            BitVector32 bv32_NC = new BitVector32((Int32)bits_NC);
            BitVector32 bv32_NO = new BitVector32((Int32)bits_NO);
            BitVector32.Section sectionPortA = BitVector32.CreateSection(0b1111_1111);
            BitVector32.Section sectionPortB = BitVector32.CreateSection(0b1111_1111, sectionPortA);
            BitVector32.Section sectionPortCL = BitVector32.CreateSection(0b1111, sectionPortB);
            BitVector32.Section sectionPortCH = BitVector32.CreateSection(0b1111, sectionPortCL);

            UInt16[] portStates = PortsRead(Only.UE24s[b]);

            portStates[(Int32)PORTS.A] &= (UInt16)bv32_NC[sectionPortA]; // &= sets portStates bits low for each explicitly assigned NC state in RεS.
            portStates[(Int32)PORTS.B] &= (UInt16)bv32_NC[sectionPortB];
            portStates[(Int32)PORTS.CL] &= (UInt16)bv32_NC[sectionPortCL];
            portStates[(Int32)PORTS.CH] &= (UInt16)bv32_NC[sectionPortCH];

            portStates[(Int32)PORTS.A] |= (UInt16)bv32_NO[sectionPortA]; // |= sets portStates bits high for each explicitly assigned NO state in RεS.
            portStates[(Int32)PORTS.B] |= (UInt16)bv32_NO[sectionPortB];
            portStates[(Int32)PORTS.CL] |= (UInt16)bv32_NO[sectionPortCL];
            portStates[(Int32)PORTS.CH] |= (UInt16)bv32_NO[sectionPortCH];

            PortsWrite(Only.UE24s[b], portStates);
        }

        public static void Set(B b, FC.S s) {
            Dictionary<R, FC.S> RεS = new Dictionary<R, FC.S>();
            foreach (R r in Enum.GetValues(typeof(R))) RεS.Add(r, s);
            Set(b, RεS);
        }

        // Below 3 methods mainly useful for parallelism, when testing multiple UUTs concurrently, with each B wired identically to test 1 UUT.
        public static void Set(HashSet<B> bs, FC.S s) { foreach (B b in bs) { Set(b, s); } }

        public static void Set(HashSet<B> bs, HashSet<R> rs, FC.S s) { foreach (B b in bs) Set(b, rs, s); }

        public static void Set(Dictionary<B, Dictionary<R, FC.S>> BεRεS) { foreach (KeyValuePair<B, Dictionary<R, FC.S>> kvp in BεRεS) Set(kvp.Key, kvp.Value); }

        public static void Set(FC.S s) { foreach (B b in Enum.GetValues(typeof(B))) Set(b, s); }
        #endregion Set

        #region internal methods
        internal static UInt16 PortRead(MccBoard mccBoard, DigitalPortType digitalPortType) {
            ErrorInfo errorInfo = mccBoard.DIn(digitalPortType, out UInt16 dataValue);
            ProcessErrorInfo(mccBoard, errorInfo);
            return dataValue;
        }

        internal static UInt16[] PortsRead(MccBoard mccBoard) {
            return new UInt16[] {
                PortRead(mccBoard, DigitalPortType.FirstPortA),
                PortRead(mccBoard, DigitalPortType.FirstPortB),
                PortRead(mccBoard, DigitalPortType.FirstPortCL),
                PortRead(mccBoard, DigitalPortType.FirstPortCH)
            };
        }

        internal static void PortWrite(MccBoard mccBoard, DigitalPortType digitalPortType, UInt16 dataValue) {
            ErrorInfo errorInfo = mccBoard.DOut(digitalPortType, dataValue);
            ProcessErrorInfo(mccBoard, errorInfo);
        }

        internal static void PortsWrite(MccBoard mccBoard, UInt16[] ports) {
            PortWrite(mccBoard, DigitalPortType.FirstPortA, ports[(Int32)PORTS.A]);
            PortWrite(mccBoard, DigitalPortType.FirstPortB, ports[(Int32)PORTS.B]);
            PortWrite(mccBoard, DigitalPortType.FirstPortCL, ports[(Int32)PORTS.CL]);
            PortWrite(mccBoard, DigitalPortType.FirstPortCH, ports[(Int32)PORTS.CH]);
        }

        internal static DigitalPortType GetPort(R r) {
            switch (r) {
                case R relay when R.C01 <= relay && relay <= R.C08: return DigitalPortType.FirstPortA;
                case R relay when R.C09 <= relay && relay <= R.C16: return DigitalPortType.FirstPortB;
                case R relay when R.C17 <= relay && relay <= R.C20: return DigitalPortType.FirstPortCL;
                case R relay when R.C21 <= relay && relay <= R.C24: return DigitalPortType.FirstPortCH;
                default: throw new NotImplementedException(TestExecutive.NotImplementedMessageEnum(typeof(R)));
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

        internal static Int32[] GetUE24BitVector32Masks() {
            Int32 ue24RelayCount = Enum.GetValues(typeof(R)).Length;
            Debug.Assert(ue24RelayCount == 24);
            Int32[] ue24BitVector32Masks = new Int32[ue24RelayCount];
            ue24BitVector32Masks[0] = BitVector32.CreateMask();
            for (Int32 i = 0; i < ue24RelayCount - 1; i++) ue24BitVector32Masks[i + 1] = BitVector32.CreateMask(ue24BitVector32Masks[i]);
            return ue24BitVector32Masks;
        }
        #endregion internal methods
    }
}