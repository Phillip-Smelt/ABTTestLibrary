using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using MccDaq; // MCC DAQ Universal Library 6.73 from https://www.mccdaq.com/Software-Downloads.

namespace TestLibrary.Switching {
    // TODO: Convert the USB_ERB24 class to a Singleton, like the USB_TO_GPIO class.  If there are multiple USB_ERB24s, copy the USB_ERB24 class & append numbers?  USB_ERB24_1, USB_ERB24_2...
    // https://forum.digikey.com/t/understanding-form-a-form-b-form-c-contact-configuration/811
    // FORM_A.NO; relay is deenergized and in normally opened state.
    // FORM_A.C; relay is energized and in abnormally closed state.
    public enum FORM_A { NO, C }
    // FORM_B.NC; relay is deenergized and in normally closed state.
    // FORM_B.O; relay is energized and in abnormally opened state.
    public enum FORM_B { NC, O }
    // FORM_C.C_NC; relay is deenergized and in normally closed/normally open state, with common C terminal connected to NC terminal & disconnected from NO terminal.
    // FORM_C.C_NO; relay is energized and in abnormally opened/abnormally closed state, with common C terminal disconnected from NC terminal & connected to NO terminal.
    public enum FORM_C { C_NC, C_NO }
    // NOTE: Below ERB24_BOARDS enum is a static definition of TestLibrary's MCC USB-ERB24(s).
    public enum ERB24_BOARDS { E01 }
    // Dynamic definition methods for ERB24_BOARDS:
    //  - Read them from MCC InstaCal's cb.cfg file.
    //  - Dynamically discover them programmatically: https://www.mccdaq.com/pdfs/manuals/Mcculw_WebHelp/ULStart.htm.
    //  - Specify USB-ERB24s in App.config, then confirm existence during TestLibrary's initialization.
    // NOTE: MCC's InstaCal USB_ERB24's board number indexing begins at 0, guessing because USB device indexing is likely also zero based.
    // - So ERB24_BOARDS.E01 numerical value is 0, which is used when constructing a new MccBoard ERB24_BOARDS.E01 object:
    // - Instantiation 'new MccBoard((Int32)ERB24_BOARDS.E01)' is equivalent to 'new MccBoard(0)'.
    public enum ERB24_RELAYS {
        R01, R02, R03, R04, R05, R06, R07, R08,
        R09, R10, R11, R12, R13, R14, R15, R16,
        R17, R18, R19, R20, R21, R22, R23, R24
    }

    public enum ERB24_PORTS { A, B, CL, CH }

    [Flags]
    public enum ERB24_BITS : UInt32 {
        None = 0,
        B00 = 1 << 00, B01 = 1 << 01, B02 = 1 << 02, B03 = 1 << 03, B04 = 1 << 04, B05 = 1 << 05, B06 = 1 << 06, B07 = 1 << 07,
        B08 = 1 << 08, B09 = 1 << 09, B10 = 1 << 10, B11 = 1 << 11, B12 = 1 << 12, B13 = 1 << 13, B14 = 1 << 14, B15 = 1 << 15,
        B16 = 1 << 16, B17 = 1 << 17, B18 = 1 << 18, B19 = 1 << 19, B20 = 1 << 20, B21 = 1 << 21, B22 = 1 << 22, B23 = 1 << 23
    }

    public static class USB_ERB24 {
        // NOTE: This class assumes all USB-ERB24 relays are configured for Non-Inverting Logic & Pull-Down/de-energized at power-up.
        // NOTE: USB-ERB24 relays are configurable for either Non-Inverting or Inverting Iogic, via hardware DIP switch S1.
        //  - Non-Inverting:  Logic low de-energizes the relays, logic high energizes them.
        //  - Inverting:      Logic low energizes the relays, logic high de-energizes them.  
        // NOTE: USB-ERB24 relays are configurable with default power-up states, via hardware DIP switch S2.
        //  - Pull-Up:        Relays are energized at power-up.
        //  - Pull-Down:      Relays are de-energized at power-up.
        //  - https://www.mccdaq.com/PDFs/Manuals/usb-erb24.pdf.
        private const Int32 TotalERB24_Ports = 4;
        private static readonly Dictionary<ERB24_RELAYS, ERB24_BITS> ERB24_RelaysToBits = new Dictionary<ERB24_RELAYS, ERB24_BITS>() {
            { ERB24_RELAYS.R01, ERB24_BITS.B00 }, { ERB24_RELAYS.R02, ERB24_BITS.B01 }, { ERB24_RELAYS.R03, ERB24_BITS.B02 }, { ERB24_RELAYS.R04, ERB24_BITS.B03 },
            { ERB24_RELAYS.R05, ERB24_BITS.B04 }, { ERB24_RELAYS.R06, ERB24_BITS.B05 }, { ERB24_RELAYS.R07, ERB24_BITS.B06 }, { ERB24_RELAYS.R08, ERB24_BITS.B07 },
            { ERB24_RELAYS.R09, ERB24_BITS.B08 }, { ERB24_RELAYS.R10, ERB24_BITS.B09 }, { ERB24_RELAYS.R11, ERB24_BITS.B10 }, { ERB24_RELAYS.R12, ERB24_BITS.B11 },
            { ERB24_RELAYS.R13, ERB24_BITS.B12 }, { ERB24_RELAYS.R14, ERB24_BITS.B13 }, { ERB24_RELAYS.R15, ERB24_BITS.B14 }, { ERB24_RELAYS.R16, ERB24_BITS.B15 },
            { ERB24_RELAYS.R17, ERB24_BITS.B16 }, { ERB24_RELAYS.R18, ERB24_BITS.B17 }, { ERB24_RELAYS.R19, ERB24_BITS.B18 }, { ERB24_RELAYS.R20, ERB24_BITS.B19 },
            { ERB24_RELAYS.R21, ERB24_BITS.B20 }, { ERB24_RELAYS.R22, ERB24_BITS.B21 }, { ERB24_RELAYS.R23, ERB24_BITS.B22 }, { ERB24_RELAYS.R24, ERB24_BITS.B23 },
        };
        //  - Wish MCC had zero-indexed their Relays, numbering from R0 to R23 instead of R1 to R24.
        //  - Would've been optimal, as Relays 1 to 24 are controlled by digital port bits that are zero-indexed, from 0 to 23.
        public static Boolean AreERB24BoardsReset() {
            Boolean areERB24BoardsReset = true;
            foreach (ERB24_BOARDS erb24_board in Enum.GetValues(typeof(ERB24_BOARDS))) {
                MccBoard mccBoard = new MccBoard((Int32)erb24_board);
                areERB24BoardsReset = areERB24BoardsReset && IsPortReset(mccBoard, DigitalPortType.FirstPortA);
                areERB24BoardsReset = areERB24BoardsReset && IsPortReset(mccBoard, DigitalPortType.FirstPortB);
                areERB24BoardsReset = areERB24BoardsReset && IsPortReset(mccBoard, DigitalPortType.FirstPortCL);
                areERB24BoardsReset = areERB24BoardsReset && IsPortReset(mccBoard, DigitalPortType.FirstPortCH);
            }
            return areERB24BoardsReset;
        }

        public static void DeEnergizeERB24(ERB24_BOARDS erb24_board) {
            MccBoard mccBoard = new MccBoard((Int32)erb24_board);
            DigitalPortsWrite(mccBoard, new UInt16[TotalERB24_Ports] { (UInt16)ERB24_BITS.None, (UInt16)ERB24_BITS.None, (UInt16)ERB24_BITS.None, (UInt16)ERB24_BITS.None });
        }

        public static void DeEnergizeERB24_All() { foreach (ERB24_BOARDS erb24_board in Enum.GetValues(typeof(ERB24_BOARDS))) DeEnergizeERB24(erb24_board); }

        public static void SetState((ERB24_BOARDS Board, ERB24_RELAYS Relay) ERB24, FORM_C State) {
            MccBoard mccBoard = new MccBoard((Int32)ERB24.Board);
            DigitalLogicState desiredState = (State is FORM_C.C_NC) ? DigitalLogicState.Low : DigitalLogicState.High;
            DigitalPortType digitalPortType;

            switch(ERB24.Relay) {
                case ERB24_RELAYS r when r <= ERB24_RELAYS.R08:
                    digitalPortType = DigitalPortType.FirstPortA;
                    break;
                case ERB24_RELAYS r when r <= ERB24_RELAYS.R16:
                    digitalPortType = DigitalPortType.FirstPortB;
                    break;
                case ERB24_RELAYS r when r <= ERB24_RELAYS.R20:
                    digitalPortType = DigitalPortType.FirstPortCL;
                    break;
                case ERB24_RELAYS r when r <= ERB24_RELAYS.R24:
                    digitalPortType = DigitalPortType.FirstPortCH;
                    break;
                default:
                    throw new NotImplementedException("Invalid ERB24 relay, must be in enum 'ERB24_RELAYS')");
            }
            DigitalBitWrite(mccBoard, digitalPortType, ERB24.Relay, desiredState);
            DigitalLogicState outputState = DigitalBitRead(mccBoard, DigitalPortType.FirstPortA, ERB24.Relay);
            if (outputState != desiredState) throw new InvalidOperationException($"MCC ERB24 '({ERB24.Board}, {ERB24.Relay})' failed to set to '{State}'.");
        }

        public static void SetStates(ERB24_BOARDS Board, Dictionary<ERB24_RELAYS, FORM_C> relayStates) {
            MccBoard mccBoard = new MccBoard((Int32)Board);
            UInt32 relayBits = 0x0000;
            ERB24_BITS erb24_bit;
            foreach (KeyValuePair<ERB24_RELAYS, FORM_C> kvp in relayStates) {
                erb24_bit = ((FORM_C)ERB24_RelaysToBits[kvp.Key] is FORM_C.C_NC) ? ERB24_BITS.None : (ERB24_BITS)Enum.ToObject(typeof(ERB24_BITS), (Int32)kvp.Key);
                relayBits |= (UInt32)erb24_bit; // Sets a 1 in each bit corresponding to relay state in relayStates.
            }
            Byte[] bits = BitConverter.GetBytes(relayBits);
            UInt16[] biggerBits = Array.ConvertAll(bits, delegate (Byte b) { return (UInt16)b; });
            UInt16[] ports = DigitalPortsRead(mccBoard);
            ports[(Int32)ERB24_PORTS.A] |= biggerBits[(Int32)ERB24_PORTS.A];
            ports[(Int32)ERB24_PORTS.B] |= biggerBits[(Int32)ERB24_PORTS.B];
            ports[(Int32)ERB24_PORTS.CL] |= (biggerBits[(Int32)ERB24_PORTS.CL] &= 0x0F); // Remove CH bits.
            ports[(Int32)ERB24_PORTS.CH] |= (biggerBits[(Int32)ERB24_PORTS.CH] &= 0xF0); // Remove CL bits.
            DigitalPortsWrite(mccBoard, ports);
        }

        public static FORM_C GetState((ERB24_BOARDS Board, ERB24_RELAYS Relay) ERB24) {
            MccBoard mccBoard = new MccBoard((Int32)ERB24.Board);
            DigitalLogicState outputState = DigitalBitRead(mccBoard, DigitalPortType.FirstPortA, ERB24.Relay);
            return (outputState == DigitalLogicState.Low) ? FORM_C.C_NC : FORM_C.C_NO;
        }

        public static Dictionary<ERB24_RELAYS, FORM_C> GetStates(ERB24_BOARDS Board) {
            MccBoard mccBoard = new MccBoard((Int32)Board);
            UInt16[] bits = DigitalPortsRead(mccBoard);
            UInt32[] biggerBits = Array.ConvertAll(bits, delegate (UInt16 ui) { return (UInt32)ui; });
            UInt32 relayBits= 0x0000;
            relayBits |= biggerBits[(Int32)ERB24_PORTS.A]  << 00;
            relayBits |= biggerBits[(Int32)ERB24_PORTS.B]  << 08;
            relayBits |= biggerBits[(Int32)ERB24_PORTS.CL] << 12;
            relayBits |= biggerBits[(Int32)ERB24_PORTS.CH] << 16;
            BitVector32 bitVector32 = new BitVector32((Int32)relayBits);
            Dictionary<ERB24_RELAYS, FORM_C> relayStates = new Dictionary<ERB24_RELAYS, FORM_C>();

            ERB24_RELAYS erb24_relay;
            FORM_C form_C_State;
            for (int i=0; i < 32; i++) {
                erb24_relay = (ERB24_RELAYS)Enum.ToObject(typeof(ERB24_RELAYS), bitVector32[i]);
                form_C_State = bitVector32[i] ? FORM_C.C_NO : FORM_C.C_NC;
                relayStates.Add(erb24_relay, form_C_State);
            }
            return relayStates;
        }

        private static DigitalLogicState DigitalBitRead(MccBoard mccBoard, DigitalPortType digitalPortType, ERB24_RELAYS erb24Relay) {
            ErrorInfo errorInfo = mccBoard.DBitIn(digitalPortType, (Int32)erb24Relay, out DigitalLogicState bitValue);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
            return bitValue;
        }

        private static void DigitalBitWrite(MccBoard mccBoard, DigitalPortType digitalPortType, ERB24_RELAYS erb24Relay, DigitalLogicState inputLogicState) {
            ErrorInfo errorInfo = mccBoard.DBitOut(digitalPortType, (Int32)erb24Relay, inputLogicState);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
        }

        private static UInt16 DigitalPortRead(MccBoard mccBoard, DigitalPortType digitalPortType) {
            ErrorInfo errorInfo = mccBoard.DIn(digitalPortType, out UInt16 dataValue);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
            return dataValue;
        }

        private static UInt16[] DigitalPortsRead(MccBoard mccBoard) {
            return new UInt16[TotalERB24_Ports] {
                DigitalPortRead(mccBoard, DigitalPortType.FirstPortA),
                DigitalPortRead(mccBoard, DigitalPortType.FirstPortB),
                DigitalPortRead(mccBoard, DigitalPortType.FirstPortCL),
                DigitalPortRead(mccBoard, DigitalPortType.FirstPortCH)
            };
        }

        private static void DigitalPortWrite(MccBoard mccBoard, DigitalPortType digitalPortType, UInt16 dataValue) {
            ErrorInfo errorInfo = mccBoard.DOut(digitalPortType, dataValue);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
        }

        private static void DigitalPortsWrite(MccBoard mccBoard, UInt16[] Ports) {
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortA,  Ports[(Int32)ERB24_PORTS.A]);
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortB,  Ports[(Int32)ERB24_PORTS.B]);
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortCL, Ports[(Int32)ERB24_PORTS.CL]);
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortCH, Ports[(Int32)ERB24_PORTS.CH]);
        }

        private static Boolean IsPortReset(MccBoard mccBoard, DigitalPortType digitalPortType) { return DigitalPortRead(mccBoard, digitalPortType) == (UInt16)ERB24_BITS.None; }

        private static void MccBoardErrorHandler(MccBoard mccBoard, ErrorInfo errorInfo) {
            throw new InvalidOperationException(
                $"MccBoard BoardNum   : {mccBoard.BoardNum}.{Environment.NewLine}" +
                $"MccBoard BoardName  : {mccBoard.BoardName}.{Environment.NewLine}" +
                $"MccBoard Descriptor : {mccBoard.Descriptor}.{Environment.NewLine}" +
                $"ErrorInfo Value     : {errorInfo.Value}.{Environment.NewLine}" +
                $"ErrorInfo Message   : {errorInfo.Message}.{Environment.NewLine}");
        }
    }
}
