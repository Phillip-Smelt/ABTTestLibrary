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
        public enum ERB24_BOARDS : Int32 { ERB24_0, ERB24_1 }
        public enum ERB24_RELAYS : Int32 {
            RELAY_0, RELAY_1, RELAY_2, RELAY_3, RELAY_4, RELAY_5, RELAY_6, RELAY_7,
            RELAY_8, RELAY_9, RELAY_10, RELAY_11, RELAY_12, RELAY_13, RELAY_14, RELAY_15,
            RELAY_16, RELAY_17, RELAY_18, RELAY_19, RELAY_20, RELAY_21, RELAY_22, RELAY_23
        }
        // TODO: Add 2nd USB-ERB24 board.  Use InstaCal to configure as Board Number 1.
        // NOTE: Above member ERB24s is a simple approach to defining the Test System's MCC USB-ERB24s.  Better ways:
        //  - Read them from InstaCal's cb.cfg file.
        //  - Dynamically discover them programmatically: https://www.mccdaq.com/pdfs/manuals/Mcculw_WebHelp/ULStart.htm.
        //  - Read them from TestLibrary's forthcoming app.config XML configuration file, then configure them dynamically/programmatically.
        //  - Pass them in from TestProgram during instantiation of TestExecutive form.
        //
        // NOTE: Below hopefully "value-added" wrapper methods for some commonly used MccDaq commands are conveniences, not necessities.
        // NOTE: Will never fully implement wrapper methods for the complete set of MccDaq commands, just some of the most commonly used ones.
        // - In general, TestLibrary's InterfaceAdapters, Logging, SCPI_VISA_Instruments & Switching namespaces exist partly to eliminate
        //   the need to reference TestLibrary's various DLLs directly from TestProgram client apps.
        // - As long as suitable wrapper methods exists in USB_ERB24, needn't directly reference MccDaq from TestProgram client apps,
        //   as referencing TestLibrary suffices.
        public static Boolean AreReset() {
            ErrorInfo errorInfo;
            MccBoard mccBoard;
            Boolean relaysAreReset = true;
            UInt16 dataValue;
            foreach (ERB24_BOARDS erb24_board in Enum.GetValues(typeof(ERB24_BOARDS))) {
                mccBoard = new MccBoard((Int32)erb24_board);
                errorInfo = mccBoard.DIn(DigitalPortType.FirstPortA, out dataValue);
                if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
                relaysAreReset = relaysAreReset && dataValue == 0;
                errorInfo = mccBoard.DIn(DigitalPortType.FirstPortB, out dataValue);
                if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
                relaysAreReset = relaysAreReset && dataValue == 0;
                errorInfo = mccBoard.DIn(DigitalPortType.FirstPortCL, out dataValue);
                if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
                relaysAreReset = relaysAreReset && dataValue == 0;
                errorInfo = mccBoard.DIn(DigitalPortType.FirstPortCH, out dataValue);
                if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
                relaysAreReset = relaysAreReset && dataValue == 0;
            }
            return relaysAreReset;
        }

        public static void Reset(ERB24_BOARDS erb24_board) {
            ErrorInfo errorInfo;
            MccBoard mccBoard = new MccBoard((Int32)erb24_board);
            errorInfo = mccBoard.DOut(DigitalPortType.FirstPortA, 0);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
            errorInfo = mccBoard.DOut(DigitalPortType.FirstPortB, 0);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
            errorInfo = mccBoard.DOut(DigitalPortType.FirstPortCL, 0);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
            errorInfo = mccBoard.DOut(DigitalPortType.FirstPortCH, 0);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
        }

        public static void ResetAll() { foreach (ERB24_BOARDS erb24_board in Enum.GetValues(typeof(ERB24_BOARDS))) Reset(erb24_board); }

        public static void Off((ERB24_BOARDS Board, ERB24_RELAYS Relay) ERB24) {
            ErrorInfo errorInfo;
            MccBoard mccBoard = new MccBoard((Int32)ERB24.Board);
            errorInfo = mccBoard.DBitOut(DigitalPortType.FirstPortA, (Int32)ERB24.Relay, DigitalLogicState.Low);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
        }

        public static void On((ERB24_BOARDS Board, ERB24_RELAYS Relay) ERB24) {
            ErrorInfo errorInfo;
            MccBoard mccBoard = new MccBoard((Int32)ERB24.Board);
            errorInfo = mccBoard.DBitOut(DigitalPortType.FirstPortA,(Int32)ERB24.Relay, DigitalLogicState.High);
            errorInfo = mccBoard.DBitIn(DigitalPortType.FirstPortA, (Int32)ERB24.Relay, out DigitalLogicState bitValue);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
        }

        public static Boolean IsOff((ERB24_BOARDS Board, ERB24_RELAYS Relay) ERB24) { return !(IsOn(ERB24)); }

        public static Boolean IsOn((ERB24_BOARDS Board, ERB24_RELAYS Relay) ERB24) {
            ErrorInfo errorInfo;
            MccBoard mccBoard = new MccBoard((Int32)ERB24.Board);
            errorInfo = mccBoard.DBitIn(DigitalPortType.FirstPortA, (Int32)ERB24.Relay, out DigitalLogicState bitValue);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
            return (bitValue == DigitalLogicState.High);
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
