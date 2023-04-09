using System;
using MccDaq; // MCC DAQ Universal Library 6.73 from https://www.mccdaq.com/Software-Downloads.

namespace TestLibrary.Switching {
    public static class USB_ERB24 {
        // NOTE: This class assumes all USB-ERB24 relays are configured for Non-Inverting Logic & Pull-Down/de-energized at power-up.
        // NOTE: USB-ERB24 Relays are divided into 4 configurable groups, for hardware DIP switches S1 & S2.
        //  - A :  Relays 1 - 8.
        //  - B :  Relays 9 - 16.
        //  - CL:  Relays 17 - 20.
        //  - CH:  Relays 21 - 24.
        // NOTE: USB-ERB24 groups are configurable for either Non-Inverting or Inverting Iogic, via hardware DIP switch S1.
        //  - Non-Inverting:  Logic low de-energizes the relays, logic high energizes them.
        //  - Inverting:      Logic low energizes the relays, logic high de-energizes them.  
        // NOTE: USB-ERB24 groups are configurable with default power-up states, via hardware DIP switch S2.
        //  - Pull-Up:        Relays are energized at power-up.
        //  - Pull-Down:      Relays are de-energized at power-up.
        //  - https://www.mccdaq.com/PDFs/Manuals/usb-erb24.pdf.
        public enum ERB24_BOARDS { ERB24_0 }
        public enum ERB24_RELAYS {
            RELAY_0, RELAY_1, RELAY_2, RELAY_3, RELAY_4, RELAY_5, RELAY_6, RELAY_7,
            RELAY_8, RELAY_9, RELAY_10, RELAY_11, RELAY_12, RELAY_13, RELAY_14, RELAY_15,
            RELAY_16, RELAY_17, RELAY_18, RELAY_19, RELAY_20, RELAY_21, RELAY_22, RELAY_23
        }
        // NOTE: Above enums ERB24_BOARDS & ERB24_RELAYS are a simple approach to defining the Test System's MCC USB-ERB24s.  Other ways:
        //  - Read them from InstaCal's cb.cfg file.
        //  - Dynamically discover them programmatically: https://www.mccdaq.com/pdfs/manuals/Mcculw_WebHelp/ULStart.htm.
        // NOTE: Might be useful to specify MCC Relay Boards in App.config, then check they're available during TestLibrary initialization, semi-similar to AppConfigSCPI_VISA_Instruments.
        // NOTE: Below hopefully "value-added" wrapper methods for some commonly used MccDaq commands are conveniences, not necessities.
        public static Boolean BoardsAreReset() {
            Boolean boardsAreReset = true;
            foreach (ERB24_BOARDS erb24_board in Enum.GetValues(typeof(ERB24_BOARDS))) {
                MccBoard mccBoard = new MccBoard((Int32)erb24_board);
                boardsAreReset = boardsAreReset && PortIsReset(mccBoard, DigitalPortType.FirstPortA);
                boardsAreReset = boardsAreReset && PortIsReset(mccBoard, DigitalPortType.FirstPortB);
                boardsAreReset = boardsAreReset && PortIsReset(mccBoard, DigitalPortType.FirstPortCL);
                boardsAreReset = boardsAreReset && PortIsReset(mccBoard, DigitalPortType.FirstPortCH);
            }
            return boardsAreReset;
        }

        private static Boolean PortIsReset(MccBoard mccBoard, DigitalPortType digitalPortType) { return (DIn(mccBoard, digitalPortType) == 0); }

        public static UInt16 DIn(MccBoard mccBoard, DigitalPortType digitalPortType) {
            ErrorInfo errorInfo = mccBoard.DIn(digitalPortType, out UInt16 dataValue);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
            return dataValue;
        }

        public static void Reset(ERB24_BOARDS erb24_board) {
            MccBoard mccBoard = new MccBoard((Int32)erb24_board);
            DOut(mccBoard, DigitalPortType.FirstPortA, 0);
            DOut(mccBoard, DigitalPortType.FirstPortB, 0);
            DOut(mccBoard, DigitalPortType.FirstPortCL, 0);
            DOut(mccBoard, DigitalPortType.FirstPortCH, 0);
        }

        public static void DOut(MccBoard mccBoard, DigitalPortType digitalPortType, Int16 dataValue) {
            ErrorInfo errorInfo = mccBoard.DOut(digitalPortType, dataValue);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
        }

        public static void ResetAll() { foreach (ERB24_BOARDS erb24_board in Enum.GetValues(typeof(ERB24_BOARDS))) Reset(erb24_board); }

        public static void SetState( (ERB24_BOARDS Board, ERB24_RELAYS Relay) ERB24, STATE State) {
            MccBoard mccBoard = new MccBoard((Int32)ERB24.Board);
            DigitalLogicState desiredState = (State is STATE.off) ? DigitalLogicState.Low : DigitalLogicState.High;
            DBitOut(mccBoard, DigitalPortType.FirstPortA, ERB24.Relay, desiredState);
            DigitalLogicState outputState = DBitIn(mccBoard, DigitalPortType.FirstPortA, ERB24.Relay);
            if (outputState != desiredState) throw new InvalidOperationException($"MCC ERB24 '({ERB24.Board}, {ERB24.Relay})' failed to set to '{State}'.");
        }

        public static STATE GetState((ERB24_BOARDS Board, ERB24_RELAYS Relay) ERB24) {
            MccBoard mccBoard = new MccBoard((Int32)ERB24.Board);
            DigitalLogicState outputState = DBitIn(mccBoard, DigitalPortType.FirstPortA, ERB24.Relay);
            return (outputState == DigitalLogicState.Low) ? STATE.off : STATE.ON;
        }

        private static void DBitOut(MccBoard mccBoard, DigitalPortType digitalPortType, ERB24_RELAYS erb24Relay, DigitalLogicState inputLogicState) {
            ErrorInfo errorInfo = mccBoard.DBitOut(digitalPortType, (Int32)erb24Relay, inputLogicState);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
        }

        private static DigitalLogicState DBitIn(MccBoard mccBoard, DigitalPortType digitalPortType, ERB24_RELAYS erb24Relay) {
            ErrorInfo errorInfo = mccBoard.DBitIn(digitalPortType, (Int32)erb24Relay, out DigitalLogicState bitValue);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
            return bitValue;
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
