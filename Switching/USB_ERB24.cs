using System;
using MccDaq; // MCC DAQ Universal Library 6.73 from https://www.mccdaq.com/Software-Downloads.

namespace TestLibrary.Switching {
    // https://forum.digikey.com/t/understanding-form-a-form-b-form-c-contact-configuration/811
    public enum RELAY { denergized, ENERGIZED }
    // deenergized is 1st state in enum, lower-cased & normal state.
    // ENERGIZED is 2nd state in enum, UPPER-CASED & abnormal state.
    public enum FORM_A { no, C }
    // FORM_A.no; relay is deenergized and in normally opened state.
    // FORM_A.C; relay is ENERGIZED and in CLOSED state.
    public enum FORM_B { nc, O }
    // FORM_B.nc; relay is deenergized in normally closed state.
    // FORM_B.O; relay is ENERGIZED and in OPENED state.
    public enum FORM_C { nc, NO }
    // FORM_C.nc; relay is deenergized and in normally closed state.
    // FORM_C.NO; relay is ENERGIZED and in NORMALLY OPENED state.

    public static class USB_ERB24 {
        // NOTE: Below ERB24_BOARDS & ERB24_RELAYS enums are a simple approach to defining the Test System's MCC USB-ERB24s.
        // Other ways:
        //  - Read them from InstaCal's cb.cfg file.
        //  - Dynamically discover them programmatically: https://www.mccdaq.com/pdfs/manuals/Mcculw_WebHelp/ULStart.htm.
        // NOTE: Might be useful to specify MCC Relay Boards in App.config, then confirm they're available during TestLibrary's initialization, semi-similar to AppConfigSCPI_VISA_Instruments.
        public enum ERB24_BOARDS { ERB24_0 }
        public enum ERB24_BITS {
            B00 = 00, B01 = 01, B02 = 02, B03 = 03, B04 = 04, B05 = 05, B06 = 06, B07 = 07,
            B08 = 08, B09 = 09, B10 = 10, B11 = 11, B12 = 12, B13 = 13, B14 = 14, B15 = 15,
            B16 = 16, B17 = 17, B18 = 18, B19 = 19, B20 = 20, B21 = 21, B22 = 22, B23 = 23
        }

        public enum ERB24_RELAYS {
            Relay_01 = ERB24_BITS.B00, Relay_02 = ERB24_BITS.B01, Relay_03 = ERB24_BITS.B02, Relay_04 = ERB24_BITS.B03, Relay_05 = ERB24_BITS.B04, Relay_06 = ERB24_BITS.B05, Relay_07 = ERB24_BITS.B06, Relay_08 = ERB24_BITS.B07,
            Relay_09 = ERB24_BITS.B08, Relay_10 = ERB24_BITS.B09, Relay_11 = ERB24_BITS.B10, Relay_12 = ERB24_BITS.B11, Relay_13 = ERB24_BITS.B12, Relay_14 = ERB24_BITS.B13, Relay_15 = ERB24_BITS.B14, Relay_16 = ERB24_BITS.B15,
            Relay_17 = ERB24_BITS.B16, Relay_18 = ERB24_BITS.B17, Relay_19 = ERB24_BITS.B18, Relay_20 = ERB24_BITS.B19, Relay_21 = ERB24_BITS.B20, Relay_22 = ERB24_BITS.B21, Relay_23 = ERB24_BITS.B22, Relay_24 = ERB24_BITS.B23
        }

        // NOTE: USB-ERB24 Relays are divided into 4 configurable groups, controllable via digital Ports A, B, CL & CH.
        //  - Port A :  Relays 01 - 08.
        //  - Port B :  Relays 09 - 16.
        //  - Port CL:  Relays 17 - 20.  CL = Port C, Low bits.
        //  - Port CH:  Relays 21 - 24.  CH = Port C, High bits.
        [Flags]
        public enum ERB24_BIT_VALUES {
            None = 0x0,
            B00 = 0x1, B01 = 0x2, B02 = 0x4, B03 = 0x8, B04 = 0x10, B05 = 0x20, B06 = 0x40, B07 = 0x80,
            B08 = 0x100, B09 = 0x200, B10 = 0x400, B11 = 0x800, B12 = 0x1000, B13 = 0x2000, B14 = 0x4000, B15 = 0x8000,
            B16 = 0x10000, B17 = 0x20000, B18 = 0x40000, B19 = 0x80000, B20 = 0x100000, B21 = 0x200000, B22 = 0x400000, B23 = 0x800000
        }
        [Flags]
        public enum ERB24_PORT_A { None = ERB24_BIT_VALUES.None, Relay_01 = ERB24_BIT_VALUES.B00, Relay_02 = ERB24_BIT_VALUES.B01, Relay_03 = ERB24_BIT_VALUES.B02, Relay_04 = ERB24_BIT_VALUES.B03, Relay_05 = ERB24_BIT_VALUES.B04, Relay_06 = ERB24_BIT_VALUES.B05, Relay_07 = ERB24_BIT_VALUES.B06, Relay_08 = ERB24_BIT_VALUES.B07 }
        [Flags]
        public enum ERB24_PORT_B { None = ERB24_BIT_VALUES.None, Relay_09 = ERB24_BIT_VALUES.B00, Relay_10 = ERB24_BIT_VALUES.B01, Relay_11 = ERB24_BIT_VALUES.B02, Relay_12 = ERB24_BIT_VALUES.B03, Relay_13 = ERB24_BIT_VALUES.B04, Relay_14 = ERB24_BIT_VALUES.B15, Relay_15 = ERB24_BIT_VALUES.B06, Relay_16 = ERB24_BIT_VALUES.B07 }
        [Flags]
        public enum ERB24_PORT_CL { None = ERB24_BIT_VALUES.None, Relay_17 = ERB24_BIT_VALUES.B00, Relay_18 = ERB24_BIT_VALUES.B01, Relay_19 = ERB24_BIT_VALUES.B02, Relay_20 = ERB24_BIT_VALUES.B03 }
        [Flags]
        public enum ERB24_PORT_CH { None = ERB24_BIT_VALUES.None, Relay_21 = ERB24_BIT_VALUES.B00, Relay_22 = ERB24_BIT_VALUES.B01, Relay_23 = ERB24_BIT_VALUES.B02, Relay_24 = ERB24_BIT_VALUES.B03 }
        // NOTE: This class assumes all USB-ERB24 relays are configured for Non-Inverting Logic & Pull-Down/de-energized at power-up.
        //  NOTE: USB-ERB24 relays are configurable for either Non-Inverting or Inverting Iogic, via hardware DIP switch S1.
        //  - Non-Inverting:  Logic low de-energizes the relays, logic high energizes them.
        //  - Inverting:      Logic low energizes the relays, logic high de-energizes them.  
        //  NOTE: USB-ERB24 relays are configurable with default power-up states, via hardware DIP switch S2.
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

        public static void Reset(ERB24_BOARDS erb24_board) {
            MccBoard mccBoard = new MccBoard((Int32)erb24_board);
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortA, (Int16)ERB24_BIT_VALUES.None);
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortB, (Int16)ERB24_BIT_VALUES.None);
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortCL, (Int16)ERB24_BIT_VALUES.None);
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortCH, (Int16)ERB24_BIT_VALUES.None);
        }

        public static void ResetAll() { foreach (ERB24_BOARDS erb24_board in Enum.GetValues(typeof(ERB24_BOARDS))) Reset(erb24_board); }

        public static void SetRelayState((ERB24_BOARDS Board, ERB24_RELAYS Relay) ERB24, FORM_C State) {
            MccBoard mccBoard = new MccBoard((Int32)ERB24.Board);
            DigitalLogicState desiredState = (State is FORM_C.nc) ? DigitalLogicState.Low : DigitalLogicState.High;
            DigitalBitWrite(mccBoard, DigitalPortType.FirstPortA, ERB24.Relay, desiredState);
            DigitalLogicState outputState = DigitalBitRead(mccBoard, DigitalPortType.FirstPortA, ERB24.Relay);
            if (outputState != desiredState) throw new InvalidOperationException($"MCC ERB24 '({ERB24.Board}, {ERB24.Relay})' failed to set to '{State}'.");
        }

        public static FORM_C GetRelayState((ERB24_BOARDS Board, ERB24_RELAYS Relay) ERB24) {
            MccBoard mccBoard = new MccBoard((Int32)ERB24.Board);
            DigitalLogicState outputState = DigitalBitRead(mccBoard, DigitalPortType.FirstPortA, ERB24.Relay);
            return (outputState == DigitalLogicState.Low) ? FORM_C.nc : FORM_C.NO;
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

        private static Boolean IsPortReset(MccBoard mccBoard, DigitalPortType digitalPortType) { return DigitalPortRead(mccBoard, digitalPortType) == (UInt16)ERB24_BIT_VALUES.None; }

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
