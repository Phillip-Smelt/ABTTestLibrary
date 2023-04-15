using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using MccDaq; // MCC DAQ Universal Library 6.73 from https://www.mccdaq.com/Software-Downloads.
using static TestLibrary.Switching.RelayForms;

namespace TestLibrary.Switching {
    public static class UE24 {
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
        public enum RELAYS { R01, R02, R03, R04, R05, R06, R07, R08, R09, R10, R11, R12, R13, R14, R15, R16, R17, R18, R19, R20, R21, R22, R23, R24 }
        public readonly static String R01 = R[00]; public readonly static String R02 = R[01]; public readonly static String R03 = R[02];
        public readonly static String R04 = R[03]; public readonly static String R05 = R[04]; public readonly static String R06 = R[05];
        public readonly static String R07 = R[06]; public readonly static String R08 = R[07]; public readonly static String R09 = R[08];
        public readonly static String R10 = R[09]; public readonly static String R11 = R[10]; public readonly static String R12 = R[11];
        public readonly static String R13 = R[12]; public readonly static String R14 = R[13]; public readonly static String R15 = R[14];
        public readonly static String R16 = R[15]; public readonly static String R17 = R[16]; public readonly static String R18 = R[17];
        public readonly static String R19 = R[18]; public readonly static String R20 = R[19]; public readonly static String R21 = R[20];
        public readonly static String R22 = R[21]; public readonly static String R23 = R[22]; public readonly static String R24 = R[23];
        #endregion public properties

        #region internal properties
        internal const UInt16 UINT16_0000 = 0x0000;
        internal const UInt16 UINT16_000F = 0x000F;
        internal const UInt16 UINT16_00FF = 0x00FF;
        internal enum PORTS { A, B, CL, CH }
        internal readonly static UInt16[] PortsAllLow  = { UINT16_0000, UINT16_0000, UINT16_0000, UINT16_0000 };
        internal readonly static UInt16[] PortsAllHigh = { UINT16_00FF, UINT16_00FF, UINT16_000F, UINT16_000F };
        [Flags]
        internal enum BITS : UInt32 {
            B00 = 1 << 00, B01 = 1 << 01, B02 = 1 << 02, B03 = 1 << 03, B04 = 1 << 04, B05 = 1 << 05, B06 = 1 << 06, B07 = 1 << 07,
            B08 = 1 << 08, B09 = 1 << 09, B10 = 1 << 10, B11 = 1 << 11, B12 = 1 << 12, B13 = 1 << 13, B14 = 1 << 14, B15 = 1 << 15,
            B16 = 1 << 16, B17 = 1 << 17, B18 = 1 << 18, B19 = 1 << 19, B20 = 1 << 20, B21 = 1 << 21, B22 = 1 << 22, B23 = 1 << 23
        }

        internal readonly static Dictionary<RELAYS, BITS> RεB = new Dictionary<RELAYS, BITS>() {
            { RELAYS.R01, BITS.B00 }, { RELAYS.R02, BITS.B01 }, { RELAYS.R03, BITS.B02 }, { RELAYS.R04, BITS.B03 },
            { RELAYS.R05, BITS.B04 }, { RELAYS.R06, BITS.B05 }, { RELAYS.R07, BITS.B06 }, { RELAYS.R08, BITS.B07 },
            { RELAYS.R09, BITS.B08 }, { RELAYS.R10, BITS.B09 }, { RELAYS.R11, BITS.B10 }, { RELAYS.R12, BITS.B11 },
            { RELAYS.R13, BITS.B12 }, { RELAYS.R14, BITS.B13 }, { RELAYS.R15, BITS.B14 }, { RELAYS.R16, BITS.B15 },
            { RELAYS.R17, BITS.B16 }, { RELAYS.R18, BITS.B17 }, { RELAYS.R19, BITS.B18 }, { RELAYS.R20, BITS.B19 },
            { RELAYS.R21, BITS.B20 }, { RELAYS.R22, BITS.B21 }, { RELAYS.R23, BITS.B22 }, { RELAYS.R24, BITS.B23 },
        };
        //  - Wish MCC had zero-indexed their USB-ERB24 relays, numbering them from R0 to R23 instead of R1 to R24.
        //  - Would've been optimal, as relays 1 to 24 are controlled by digital port bits that are zero-indexed, from 0 to 23.
        #endregion internal properties

        #region private properties
        private readonly static String[] R = Enum.GetNames(typeof(RELAYS));
        private readonly static UInt32[] B = (UInt32[])Enum.GetValues(typeof(BITS));
        private readonly static Dictionary<String, (RELAYS, BITS)> SεRεB = GetSεRεB();

        private static Dictionary<String, (RELAYS Relays, BITS Bits)> GetSεRεB() {
            String[] S = Enum.GetNames(typeof(RELAYS));
            Dictionary<String, (RELAYS, BITS)> SεRεB = new Dictionary<String, (RELAYS, BITS)>();
            for (Int32 i = 0; i < R.Length; i++) { SεRεB.Add(S[i], ((RELAYS)i, (BITS)i)); }
            return SεRεB;
        }
        private const String PORT_INVALID = "Invalid USB-ERB24 DigitalPortType, must be in set '{ FirstPortA, FirstPortB, FirstPortCL, FirstPortCH }'.";
        #endregion private properties

        #region public methods
        public static Boolean AreBoardStatesNC() {
            Boolean boardsAreStateNC = true;
            foreach (BOARDS board in Enum.GetValues(typeof(BOARDS))) { boardsAreStateNC &= IsBoardStateNC(board); }
            return boardsAreStateNC;
        }

        public static Boolean AreBoardStatesNO() {
            Boolean boardsAreStateNO = true;
            foreach (BOARDS board in Enum.GetValues(typeof(BOARDS))) { boardsAreStateNO &= IsBoardStateNO(board); }
            return boardsAreStateNO;
        }

        public static Boolean IsBoardStateNC(BOARDS Board) { return (DigitalPortsRead(new MccBoard((Int32)Board)) == PortsAllLow); }

        public static Boolean IsBoardStateNO(BOARDS Board) { return (DigitalPortsRead(new MccBoard((Int32)Board)) == PortsAllHigh); }

        public static void SetBoardStateNO(BOARDS Board) { DigitalPortsWrite(new MccBoard((Int32)Board), PortsAllLow); }

        public static void SetBoardStateNC(BOARDS Board) { DigitalPortsWrite(new MccBoard((Int32)Board), PortsAllHigh); }

        public static void SetStateNC_Boards() { foreach (BOARDS board in Enum.GetValues(typeof(BOARDS))) SetStateNC_Board(board); }

        public static void SetStateNO_Boards() { foreach (BOARDS board in Enum.GetValues(typeof(BOARDS))) SetStateNO_Board(board); }

        public static void SetState((BOARDS Board, RELAYS Relay) BεR, FORM_C form_C) {
            MccBoard mccBoard = new MccBoard((Int32)RELAYS.Board);
            DigitalLogicState desiredState = (form_C is FORM_C.NC) ? DigitalLogicState.Low : DigitalLogicState.High;
            _ = GetPortType(RELAYS.Relay);
            DigitalBitWrite(mccBoard, RELAYS.Relay, desiredState);
            DigitalLogicState outputState = DigitalBitRead(mccBoard, RELAYS.Relay);
            if (outputState != desiredState) throw new InvalidOperationException($"USB-ERB24 '({RELAYS.Board}, {RELAYS.Relay})' failed to set to '{form_C}'.");
        }

        public static void SetStates(BOARDS Board, Dictionary<RELAYS, FORM_C> relayStates) {
            MccBoard mccBoard = new MccBoard((Int32)Board);
            UInt32 relayBits = 0x00000000;
            BITS bit;
            foreach (KeyValuePair<RELAYS, FORM_C> kvp in relayStates) {
                bit = ((FORM_C)RεB[kvp.Key] is FORM_C.NC) ? UINT16_0000 : (BITS)Enum.ToObject(typeof(BITS), (Int32)kvp.Key);
                relayBits |= (UInt32)bit; // Sets a 1 in each bit corresponding to relay state in relayStates.
            }
            Byte[] bits = BitConverter.GetBytes(relayBits);
            UInt16[] biggerBits = Array.ConvertAll(bits, delegate (Byte b) { return (UInt16)b; });
            UInt16[] ports = DigitalPortsRead(mccBoard);
            ports[(Int32)PORTS.A]  |= biggerBits[(Int32)PORTS.A];
            ports[(Int32)PORTS.B]  |= biggerBits[(Int32)PORTS.B];
            ports[(Int32)PORTS.CL] |= (biggerBits[(Int32)PORTS.CL] &= 0x0F); // Remove CH bits.
            ports[(Int32)PORTS.CH] |= (biggerBits[(Int32)PORTS.CH] &= 0xF0); // Remove CL bits.
            DigitalPortsWrite(mccBoard, ports);
        }

        public static FORM_C GetState(BOARDS Board, String Relay) {
            String[] relays = Enum.GetNames(typeof(RELAYS));
            if (!Array.Exists(relays, r => r.Equals(Relay))) throw new ArgumentException($"Invalid relay '{Relay}', must be in set '{relays}'.");
            MccBoard mccBoard = new MccBoard((Int32)Board);
            DigitalLogicState outputState = DigitalBitRead(mccBoard, (RELAYS)Enum.Parse(typeof(RELAYS), Relay));
            return (outputState == DigitalLogicState.Low) ? FORM_C.NC : FORM_C.NO;
        }

        public static Dictionary<RELAYS, FORM_C> GetStates(BOARDS Board) {
            MccBoard mccBoard = new MccBoard((Int32)Board);
            UInt16[] bits = DigitalPortsRead(mccBoard);
            UInt32[] biggerBits = Array.ConvertAll(bits, delegate (UInt16 uInt16) { return (UInt32)uInt16; });
            UInt32 relayBits= 0x0000;
            relayBits |= biggerBits[(Int32)PORTS.A]  << 00;
            relayBits |= biggerBits[(Int32)PORTS.B]  << 08;
            relayBits |= biggerBits[(Int32)PORTS.CL] << 12;
            relayBits |= biggerBits[(Int32)PORTS.CH] << 16;
            BitVector32 bitVector32 = new BitVector32((Int32)relayBits);

            Dictionary<RELAYS, FORM_C> relayStates = new Dictionary<RELAYS, FORM_C>();
            RELAYS relay;
            FORM_C form_C;
            for (Int32 i=0; i < 32; i++) {
                relay = (RELAYS)Enum.ToObject(typeof(RELAYS), bitVector32[i]);
                form_C = bitVector32[i] ? FORM_C.NO : FORM_C.NC;
                relayStates.Add(relay, form_C);
            }
            return relayStates;
        }

        public FORM_C GetRelayFromString(String Relay) {
            if (!Array.Exists(relays, r => r.Equals(Relay))) throw new ArgumentException($"Invalid relay '{Relay}', must be in set '{relays}'.");
            (RELAYS)Enum.Parse(typeof(RELAYS), Relay);
        }
        #endregion public methods

        #region internal methods
        internal static DigitalLogicState DigitalBitRead(MccBoard mccBoard, RELAYS Relay) {
            ErrorInfo errorInfo = mccBoard.DBitIn(DigitalPortType.FirstPortA, (Int32)Relay, out DigitalLogicState bitValue);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
            return bitValue;
        }

        internal static void DigitalBitWrite(MccBoard mccBoard, RELAYS Relay, DigitalLogicState inputLogicState) {
            ErrorInfo errorInfo = mccBoard.DBitOut(DigitalPortType.FirstPortA, (Int32)Relay, inputLogicState);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
        }

        internal static UInt16 DigitalPortRead(MccBoard mccBoard, DigitalPortType digitalPortType) {
            ErrorInfo errorInfo = mccBoard.DIn(digitalPortType, out UInt16 dataValue);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
            return dataValue;
        }

        internal static UInt16[] DigitalPortsRead(MccBoard mccBoard) {
            return new UInt16[] {
                DigitalPortRead(mccBoard, DigitalPortType.FirstPortA),
                DigitalPortRead(mccBoard, DigitalPortType.FirstPortB),
                DigitalPortRead(mccBoard, DigitalPortType.FirstPortCL),
                DigitalPortRead(mccBoard, DigitalPortType.FirstPortCH)
            };
        }

        internal static void DigitalPortWrite(MccBoard mccBoard, DigitalPortType digitalPortType, UInt16 dataValue) {
            ErrorInfo errorInfo = mccBoard.DOut(digitalPortType, dataValue);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
        }

        internal static void DigitalPortsWrite(MccBoard mccBoard, UInt16[] ports) {
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortA,  ports[(Int32)PORTS.A]);
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortB,  ports[(Int32)PORTS.B]);
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortCL, ports[(Int32)PORTS.CL]);
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortCH, ports[(Int32)PORTS.CH]);
        }

        internal static Boolean IsPortState(MccBoard mccBoard, DigitalPortType digitalPortType, UInt16 portState) { return DigitalPortRead(mccBoard, digitalPortType) == portState; }

        internal static Boolean IsBitState(MccBoard mccBoard, RELAYS relay, DigitalLogicState digitalLogicState) { return DigitalBitRead(mccBoard, relay) == digitalLogicState; }

        internal static DigitalPortType GetPortType(RELAYS relay) {
            switch (relay) {
                case RELAYS r when RELAYS.R01 <= r && r <= RELAYS.R08:  return DigitalPortType.FirstPortA;
                case RELAYS r when RELAYS.R09 <= r && r <= RELAYS.R16:  return DigitalPortType.FirstPortB;
                case RELAYS r when RELAYS.R17 <= r && r <= RELAYS.R20:  return DigitalPortType.FirstPortCL;
                case RELAYS r when RELAYS.R21 <= r && r <= RELAYS.R24:  return DigitalPortType.FirstPortCH;
                default: throw new ArgumentException(PORT_INVALID);
            }
        }

        internal static void MccBoardErrorHandler(MccBoard mccBoard, ErrorInfo errorInfo) {
            throw new InvalidOperationException(
                $"{Environment.NewLine}" +
                $"MccBoard BoardNum   : {mccBoard.BoardNum}.{Environment.NewLine}" +
                $"MccBoard BoardName  : {mccBoard.BoardName}.{Environment.NewLine}" +
                $"MccBoard Descriptor : {mccBoard.Descriptor}.{Environment.NewLine}" +
                $"ErrorInfo Value     : {errorInfo.Value}.{Environment.NewLine}" +
                $"ErrorInfo Message   : {errorInfo.Message}.{Environment.NewLine}");
        }
        #endregion internal methods
    }
}
