using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using MccDaq; // MCC DAQ Universal Library 6.73 from https://www.mccdaq.com/Software-Downloads.
// using static TestLibrary.Switching.RelayForms;

namespace TestLibrary.Switching {
    public static class USB_ERB24 {
        // TODO: Convert the UE24 class to a Singleton, like the USB_TO_GPIO class.  If there are multiple USB-ERB24s, copy the UE24 class & append numbers?  1, 2...
        // TODO: Once this class' internal methods are fully verified by TestLibraryTests, refactor them to private methods & delete TestLibraryTest's Unit tests for the now private methods.
        // NOTE: This class assumes all USB-ERB24 relays are configured for Non-Inverting Logic & Pull-Down/de-energized at power-up.
        // NOTE: USB-ERB24 relays are configurable for either Non-Inverting or Inverting logic, via hardware DIP switch S1.
        //  - Non-Inverting:  Logic low de-energizes the relays, logic high energizes them.
        //  - Inverting:      Logic low energizes the relays, logic high de-energizes them.  
        // NOTE: USB-ERB24 relays are configurable with default power-up states, via hardware DIP switch S2.
        //  - Pull-Up:        Relays are energized at power-up.
        //  - Pull-Down:      Relays are de-energized at power-up.
        //  - https://www.mccdaq.com/PDFs/Manuals/usb-erb24.pdf.
        // NOTE: BOARDS enum is a static definition of TestLibrary's MCC USB-ERB24(s).
        // Dynamic definition methods for BOARDS:
        //  - Read them from MCC InstaCal's cb.cfg file.
        //  - Dynamically discover them programmatically: https://www.mccdaq.com/pdfs/manuals/Mcculw_WebHelp/ULStart.htm.
        //  - Specify MCC USB-ERB24s in App.config, then confirm existence during TestLibrary's initialization.
        // NOTE: MCC's InstaCal USB-ERB24's board number indexing begins at 0, guessing because USB device indexing is likely also zero based.
        // - So BOARDS.E01 numerical value is 0, which is used when constructing a new MccBoard BOARDS.E01 object:
        // - Instantiation 'new MccBoard((Int32)BOARDS.E01)' is equivalent to 'new MccBoard(0)'.
        #region public properties
        public enum BOARDS { E01 }
        public enum RELAYS : Byte { R01, R02, R03, R04, R05, R06, R07, R08, R09, R10, R11, R12, R13, R14, R15, R16, R17, R18, R19, R20, R21, R22, R23, R24 }
        public const String R01 = "R01"; public const String R02 = "R01"; public const String R03 = "R02";
        public const String R04 = "R03"; public const String R05 = "R04"; public const String R06 = "R05";
        public const String R07 = "R06"; public const String R08 = "R07"; public const String R09 = "R08";
        public const String R10 = "R09"; public const String R11 = "R10"; public const String R12 = "R11";
        public const String R13 = "R12"; public const String R14 = "R13"; public const String R15 = "R14";
        public const String R16 = "R15"; public const String R17 = "R16"; public const String R18 = "R17";
        public const String R19 = "R18"; public const String R20 = "R19"; public const String R21 = "R20";
        public const String R22 = "R21"; public const String R23 = "R22"; public const String R24 = "R23";
        #endregion public properties

        #region internal properties
        internal enum PORTS { A, B, CL, CH }
        internal readonly static UInt16[] Portslow  = { 0x0000, 0x0000, 0x0000, 0x0000 };
        internal readonly static UInt16[] PortsHIGH = { 0x00FF, 0x00FF, 0x000F, 0x000F };
        #endregion internal properties

        #region private properties
        private const String PORT_INVALID = "Invalid USB-ERB24 DigitalPortType, must be in set '{ FirstPortA, FirstPortB, FirstPortCL, FirstPortCH }'.";
        #endregion private properties

        #region public methods
        public static Boolean AreNC() {
            Boolean boardsAreNC = true;
            foreach (BOARDS board in Enum.GetValues(typeof(BOARDS))) { boardsAreNC &= IsNC(board); }
            return boardsAreNC;
        }

        public static Boolean AreNO() {
            Boolean boardsAreNO = true;
            foreach (BOARDS board in Enum.GetValues(typeof(BOARDS))) { boardsAreNO &= IsNO(board); }
            return boardsAreNO;
        }

        public static Boolean IsNC(BOARDS Board) { return (Read(new MccBoard((Int32)Board)) == Portslow); }

        public static Boolean IsNO(BOARDS Board) { return (Read(new MccBoard((Int32)Board)) == PortsHIGH); }

        public static void SetNC(BOARDS Board) { Write(new MccBoard((Int32)Board), Portslow); }

        public static void SetNO(BOARDS Board) { Write(new MccBoard((Int32)Board), PortsHIGH); }

        public static void SetNC() { foreach (BOARDS board in Enum.GetValues(typeof(BOARDS))) SetNC(board); }

        public static void SetNO() { foreach (BOARDS board in Enum.GetValues(typeof(BOARDS))) SetNO(board); }

        public static RelayForms.C Get(BOARDS Board, RELAYS Relay) {
            MccBoard mccBoard = new MccBoard((Int32)Board);
            ErrorInfo errorInfo = mccBoard.DBitIn(DigitalPortType.FirstPortA, (Int32)Relay, out DigitalLogicState bitValue);
            ProcessErrorInfo(mccBoard, errorInfo);
            return (bitValue == DigitalLogicState.Low) ? RelayForms.C.NC : RelayForms.C.NO;
        }

        public static Dictionary<RELAYS, RelayForms.C> Get(BOARDS Board) {
            MccBoard mccBoard = new MccBoard((Int32)Board);
            UInt16[] bits = Read(mccBoard);
            UInt32[] biggerBits = Array.ConvertAll(bits, delegate (UInt16 uInt16) { return (UInt32)uInt16; });
            UInt32 relayBits = 0x0000;
            relayBits |= biggerBits[(Int32)PORTS.A] << 00;
            relayBits |= biggerBits[(Int32)PORTS.B] << 08;
            relayBits |= biggerBits[(Int32)PORTS.CL] << 12;
            relayBits |= biggerBits[(Int32)PORTS.CH] << 16;
            BitVector32 bitVector32 = new BitVector32((Int32)relayBits);

            Dictionary<RELAYS, RelayForms.C> relayStates = new Dictionary<RELAYS, RelayForms.C>();
            RELAYS relay;
            RelayForms.C C;
            for (Int32 i = 0; i < 32; i++) {
                relay = (RELAYS)Enum.ToObject(typeof(RELAYS), bitVector32[i]);
                C = bitVector32[i] ? RelayForms.C.NO : RelayForms.C.NC;
                relayStates.Add(relay, C);
            }
            return relayStates;
        }

        public static Boolean Is(BOARDS Board, RELAYS Relay, RelayForms.C C) {
            MccBoard mccBoard = new MccBoard((Int32)Board);
            ErrorInfo errorInfo = mccBoard.DBitIn(DigitalPortType.FirstPortA, (Int32)Relay, out DigitalLogicState bitValue);
            ProcessErrorInfo(mccBoard, errorInfo);
            if (bitValue == DigitalLogicState.Low) return (C is RelayForms.C.NC);
            else return (C is RelayForms.C.NO);
        }

        public static void Set(BOARDS Board, RELAYS Relay, RelayForms.C C) {
            MccBoard mccBoard = new MccBoard((Int32)Board);
            ErrorInfo errorInfo = mccBoard.DBitOut(DigitalPortType.FirstPortA, (Int32)Relay, (C is RelayForms.C.NC) ? DigitalLogicState.Low : DigitalLogicState.High);
            ProcessErrorInfo(mccBoard, errorInfo);
        }

        public static void Set(BOARDS Board, Dictionary<RELAYS, RelayForms.C> RεC) {
            MccBoard mccBoard = new MccBoard((Int32)Board);
            UInt32 portBits = 0x0000_0000;
            foreach (RELAYS R in RεC.Keys) portBits |= (UInt32)1 << (Byte)R;
            // Sets a 1 in each bit corresponding to relay state in RεC.
            Byte[] bite = BitConverter.GetBytes(portBits);
            UInt16[] biggerBite = Array.ConvertAll(bite, delegate (Byte b) { return (UInt16)b; });
            UInt16[] ports = Read(mccBoard);
            ports[(Int32)PORTS.A]  |= biggerBite[(Int32)PORTS.A];
            ports[(Int32)PORTS.B]  |= biggerBite[(Int32)PORTS.B];
            ports[(Int32)PORTS.CL] |= (biggerBite[(Int32)PORTS.CL] &= 0x0F); // Clear CH bits.
            ports[(Int32)PORTS.CH] |= (biggerBite[(Int32)PORTS.CH] &= 0xF0); // Clear CL bits.
            Write(mccBoard, ports);
        }
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

        internal static DigitalPortType Get(RELAYS relay) {
            switch (relay) {
                case RELAYS r when RELAYS.R01 <= r && r <= RELAYS.R08: return DigitalPortType.FirstPortA;
                case RELAYS r when RELAYS.R09 <= r && r <= RELAYS.R16: return DigitalPortType.FirstPortB;
                case RELAYS r when RELAYS.R17 <= r && r <= RELAYS.R20: return DigitalPortType.FirstPortCL;
                case RELAYS r when RELAYS.R21 <= r && r <= RELAYS.R24: return DigitalPortType.FirstPortCH;
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
