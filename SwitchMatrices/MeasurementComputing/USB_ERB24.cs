using System;
using System.Collections.Generic;
using MccDaq; // MCC DAQ Universal Library 6.73 from https://www.mccdaq.com/Software-Downloads.

namespace TestLibrary.SwitchMatrices.MeasurementComputing {
    public static class USB_ERB24 {
        // NOTE: This class assumes all USB-erb24 relays are configured for Non-Inverting Logic & Pull-Down/de-energized at power-up.
        // NOTE: USB-erb24 Relays are divided into 4 configurable groups, for hardware DIP switches S1 & S2.
        //  - A :  relays 1 - 8.
        //  - B :  relays 9 - 16.
        //  - CL:  relays 17 - 20.
        //  - CH:  relays 21 - 24.
        // NOTE: USB-erb24 groups are configurable for either Non-Inverting or Inverting Iogic, via hardware DIP switch S1.
        //  - Non-Inverting:  Logic low de-energizes the relays, logic high energizes them.
        //  - Inverting:      Logic low energizes the relays, logic high de-energizes them.  
        // NOTE: USB-erb24 groups are configurable with default power-up states, via hardware DIP switch S2.
        //  - Pull-Up:        Relays are energized at power-up.
        //  - Pull-Down:      Relays are de-energized at power-up.
        //  - https://www.mccdaq.com/PDFs/Manuals/usb-erb24.pdf.
        public static readonly List<Int32> ERB24s = new List<Int32>() { 0 };
        // TODO: Add 2nd USB-ERB24 board.  Use InstaCal to configure as Board Number 1.
        // TODO: Above member ERB24s is a very quick & dirty approach to defining the Test System's MCC USB-ERB24s.  Better ways:
        //  - Read them from InstaCal's cb.cfg file.
        //  - Dynamically discover them programmatically: https://www.mccdaq.com/pdfs/manuals/Mcculw_WebHelp/ULStart.htm.
        //  - Read them from TestLibrary's forthcoming app.config XML configuration file, then configure them dynamically/programmatically.
        //  - Pass them in from TestProgram during instantiation of TestExecutive form.

        public static Boolean AreReset() {
            MccBoard erb24;
            ErrorInfo ei;
            Boolean relaysAreReset = true;
            UInt16 dataValue;
            foreach (Int32 boardNumber in ERB24s) {
                erb24 = new MccBoard(boardNumber);
                ei = erb24.DIn(DigitalPortType.FirstPortA, out dataValue);
                if (ei.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(erb24, ei);
                relaysAreReset = relaysAreReset && dataValue == 0;
                ei = erb24.DIn(DigitalPortType.FirstPortB, out dataValue);
                if (ei.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(erb24, ei);
                relaysAreReset = relaysAreReset && dataValue == 0;
                ei = erb24.DIn(DigitalPortType.FirstPortCL, out dataValue);
                if (ei.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(erb24, ei);
                relaysAreReset = relaysAreReset && dataValue == 0;
                ei = erb24.DIn(DigitalPortType.FirstPortCH, out dataValue);
                if (ei.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(erb24, ei);
                relaysAreReset = relaysAreReset && dataValue == 0;
            }
            return relaysAreReset;
        }

        public static void Reset(List<Int32> boardNumbers) {
            MccBoard erb24;
            ErrorInfo ei;
            foreach (Int32 boardNumber in boardNumbers) {
                erb24 = new MccBoard(boardNumber);
                ei = erb24.DOut(DigitalPortType.FirstPortA, 0);
                if (ei.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(erb24, ei);
                ei = erb24.DOut(DigitalPortType.FirstPortB, 0);
                if (ei.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(erb24, ei);
                ei = erb24.DOut(DigitalPortType.FirstPortCL, 0);
                if (ei.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(erb24, ei);
                ei = erb24.DOut(DigitalPortType.FirstPortCH, 0);
                if (ei.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(erb24, ei);
            }
        }

        public static void Off((Int32 board, Int32 relay) br) {
            MccBoard erb24;
            ErrorInfo ei;
            erb24 = new MccBoard(br.board);
            ei = erb24.DBitOut(DigitalPortType.FirstPortA, br.relay, DigitalLogicState.Low);
            if (ei.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(erb24, ei);
        }

        public static void On((Int32 board, Int32 relay) br) {
            MccBoard erb24;
            ErrorInfo ei;
            erb24 = new MccBoard(br.board);
            ei = erb24.DBitOut(DigitalPortType.FirstPortA, br.relay, DigitalLogicState.High);
            ei = erb24.DBitIn(DigitalPortType.FirstPortA, br.relay, out DigitalLogicState bitValue);
            if (ei.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(erb24, ei);
        }

        public static Boolean IsOff((Int32 board, Int32 relay) br) {
            return !(IsOn(br));
        }

        public static Boolean IsOn((Int32 board, Int32 relay) br) {
            MccBoard erb24;
            ErrorInfo ei;
            erb24 = new MccBoard(br.board);
            ei = erb24.DBitIn(DigitalPortType.FirstPortA, br.relay, out DigitalLogicState bitValue);
            if (ei.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(erb24, ei);
            return (bitValue == DigitalLogicState.High);
        }
    }
}
