using System;
using MccDaq; // MCC DAQ Universal Library 6.73 from https://www.mccdaq.com/Software-Downloads.

namespace TestLibrary.Switching {
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
    //   - Instantiation 'new MccBoard((Int32)ERB24_BOARDS.E01)' is equivalent to 'new MccBoard(0)'.
    //   - Wish MCC had zero-indexed their Relays as well; numbering from R0 to R23 instead of R1 to R24.
    //     - Would've been optimal, as Relays 1 to 24 are controlled by digital port bits that are zero-indexed, from 0 to 23.
    //     - Because MCC one-indexed their relays from 1 to 24, followed their convention by one-indexing enum ERB24_BOARDS; first USB-ERB24 assigned to E01, not E00.
    public enum ERB24_RELAYS {
        R01 = ERB24_BITS.B00, R02 = ERB24_BITS.B01, R03 = ERB24_BITS.B02, R04 = ERB24_BITS.B03, R05 = ERB24_BITS.B04, R06 = ERB24_BITS.B05, R07 = ERB24_BITS.B06, R08 = ERB24_BITS.B07,
        R09 = ERB24_BITS.B08, R10 = ERB24_BITS.B09, R11 = ERB24_BITS.B10, R12 = ERB24_BITS.B11, R13 = ERB24_BITS.B12, R14 = ERB24_BITS.B13, R15 = ERB24_BITS.B14, R16 = ERB24_BITS.B15,
        R17 = ERB24_BITS.B16, R18 = ERB24_BITS.B17, R19 = ERB24_BITS.B18, R20 = ERB24_BITS.B19, R21 = ERB24_BITS.B20, R22 = ERB24_BITS.B21, R23 = ERB24_BITS.B22, R24 = ERB24_BITS.B23
    }

    public enum ERB24_BITS {
        B00 = 00, B01 = 01, B02 = 02, B03 = 03, B04 = 04, B05 = 05, B06 = 06, B07 = 07,
        B08 = 08, B09 = 09, B10 = 10, B11 = 11, B12 = 12, B13 = 13, B14 = 14, B15 = 15,
        B16 = 16, B17 = 17, B18 = 18, B19 = 19, B20 = 20, B21 = 21, B22 = 22, B23 = 23
    }
    // NOTE: USB-ERB24 Relays are divided into 4 configurable groups, controllable via digital Ports A, B, CL & CH.
    //  - Port A :  Relays 01 - 08.
    //  - Port B :  Relays 09 - 16.
    //  - Port CL:  Relays 17 - 20.  CL = Port C, Low bits.
    //  - Port CH:  Relays 21 - 24.  CH = Port C, High bits.
    [Flags]
    public enum ERB24_FLAGS {
        None = 0x0,
        F00 = 0x1, F01 = 0x2, F02 = 0x4, F03 = 0x8, F04 = 0x10, F05 = 0x20, F06 = 0x40, F07 = 0x80,
        F08 = 0x100, F09 = 0x200, F10 = 0x400, F11 = 0x800, F12 = 0x1000, F13 = 0x2000, F14 = 0x4000, F15 = 0x8000,
        F16 = 0x10000, F17 = 0x20000, F18 = 0x40000, F19 = 0x80000, F20 = 0x100000, F21 = 0x200000, F22 = 0x400000, F23 = 0x800000
    }
    [Flags]
    public enum ERB24_PORT_A { None = ERB24_FLAGS.None, R01 = ERB24_FLAGS.F00, R02 = ERB24_FLAGS.F01, R03 = ERB24_FLAGS.F02, R04 = ERB24_FLAGS.F03, R05 = ERB24_FLAGS.F04, R06 = ERB24_FLAGS.F05, R07 = ERB24_FLAGS.F06, R08 = ERB24_FLAGS.F07 }
    [Flags]
    public enum ERB24_PORT_B { None = ERB24_FLAGS.None, R09 = ERB24_FLAGS.F00, R10 = ERB24_FLAGS.F01, R11 = ERB24_FLAGS.F02, R12 = ERB24_FLAGS.F03, R13 = ERB24_FLAGS.F04, R14 = ERB24_FLAGS.F05, R15 = ERB24_FLAGS.F06, R16 = ERB24_FLAGS.F07 }
    [Flags]
    public enum ERB24_PORT_CL { None = ERB24_FLAGS.None, R17 = ERB24_FLAGS.F00, R18 = ERB24_FLAGS.F01, R19 = ERB24_FLAGS.F02, R20 = ERB24_FLAGS.F03 }
    [Flags]
    public enum ERB24_PORT_CH { None = ERB24_FLAGS.None, R21 = ERB24_FLAGS.F00, R22 = ERB24_FLAGS.F01, R23 = ERB24_FLAGS.F02, R24 = ERB24_FLAGS.F03 }

    public static class USB_ERB24 {
        // NOTE: This class assumes all USB-ERB24 relays are configured for Non-Inverting Logic & Pull-Down/de-energized at power-up.
        // NOTE: USB-ERB24 relays are configurable for either Non-Inverting or Inverting Iogic, via hardware DIP switch S1.
        //  - Non-Inverting:  Logic low de-energizes the relays, logic high energizes them.
        //  - Inverting:      Logic low energizes the relays, logic high de-energizes them.  
        // NOTE: USB-ERB24 relays are configurable with default power-up states, via hardware DIP switch S2.
        //  - Pull-Up:        Relays are energized at power-up.
        //  - Pull-Down:      Relays are de-energized at power-up.
        //  - https://www.mccdaq.com/PDFs/Manuals/usb-erb24.pdf.

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

        public static void DeEnergize(ERB24_BOARDS erb24_board) {
            MccBoard mccBoard = new MccBoard((Int32)erb24_board);
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortA, (Int16)ERB24_FLAGS.None);
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortB, (Int16)ERB24_FLAGS.None);
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortCL, (Int16)ERB24_FLAGS.None);
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortCH, (Int16)ERB24_FLAGS.None);
        }

        public static void DeEnergizeAll() { foreach (ERB24_BOARDS erb24_board in Enum.GetValues(typeof(ERB24_BOARDS))) DeEnergize(erb24_board); }

        public static void SetState((ERB24_BOARDS Board, ERB24_RELAYS Relay) ERB24, FORM_C State) {
            MccBoard mccBoard = new MccBoard((Int32)ERB24.Board);
            DigitalLogicState desiredState = (State is FORM_C.C_NC) ? DigitalLogicState.Low : DigitalLogicState.High;
            DigitalBitWrite(mccBoard, DigitalPortType.FirstPortA, ERB24.Relay, desiredState);
            DigitalLogicState outputState = DigitalBitRead(mccBoard, DigitalPortType.FirstPortA, ERB24.Relay);
            if (outputState != desiredState) throw new InvalidOperationException($"MCC ERB24 '({ERB24.Board}, {ERB24.Relay})' failed to set to '{State}'.");
        }

        public static FORM_C GetState((ERB24_BOARDS Board, ERB24_RELAYS Relay) ERB24) {
            MccBoard mccBoard = new MccBoard((Int32)ERB24.Board);
            DigitalLogicState outputState = DigitalBitRead(mccBoard, DigitalPortType.FirstPortA, ERB24.Relay);
            return (outputState == DigitalLogicState.Low) ? FORM_C.C_NC : FORM_C.C_NO;
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

        private static void DigitalPortWrite(MccBoard mccBoard, DigitalPortType digitalPortType, Int16 dataValue) {
            ErrorInfo errorInfo = mccBoard.DOut(digitalPortType, dataValue);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
        }

        private static Boolean IsPortReset(MccBoard mccBoard, DigitalPortType digitalPortType) { return DigitalPortRead(mccBoard, digitalPortType) == (UInt16)ERB24_FLAGS.None; }

        public static void SetPortAState(ERB24_BOARDS Board, ERB24_PORT_A PortA) {
            MccBoard mccBoard = new MccBoard((Int32)Board);
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortA, (Int16)PortA);
        }

        public static ERB24_PORT_A GetPortAState(ERB24_BOARDS Board) {
            MccBoard mccBoard = new MccBoard((Int32)Board);
            UInt16 PortA = DigitalPortRead(mccBoard, DigitalPortType.FirstPortA);
            return (ERB24_PORT_A)PortA;
        }

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
