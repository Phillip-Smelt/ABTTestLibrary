using System;
using System.Collections.Generic;
using MccDaq; // MCC DAQ Universal Library 6.73 from https://www.mccdaq.com/Software-Downloads.
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TestLibrary.SwitchMatrices.MeasurementComputing {
    public static class USB_ERB24 {
        // NOTE: This class assumes all USB-ERB24 relays are configured for Non-Inverting Logic & Pull-Down/de-energized at power-up.
        // NOTE: USB-ERB24 Relays are divided into 4 configurable groups, for hardware DIP switches S1 & S2.
        //  - A :  relays 1 - 8.
        //  - B :  relays 9 - 16.
        //  - CL:  relays 17 - 20.
        //  - CH:  relays 21 - 24.
        // NOTE: USB-ERB24 groups are configurable for either Non-Inverting or Inverting Iogic, via hardware DIP switch S1.
        //  - Non-Inverting:  Logic low de-energizes the relays, logic high energizes them.
        //  - Inverting:      Logic low energizes the relays, logic high de-energizes them.  
        // NOTE: USB-ERB24 groups are configurable with default power-up states, via hardware DIP switch S2.
        //  - Pull-Up:        Relays are energized at power-up.
        //  - Pull-Down:      Relays are de-energized at power-up.
        //  - https://www.mccdaq.com/PDFs/Manuals/usb-erb24.pdf.
        private static MccBoard ERB24;
        private static ErrorInfo EI;
        public static readonly List<Int32> ERB24s = new List<Int32>() {0};
        // TODO: Add 2nd USB-ERB24 board.  Use InstaCal to configure as Board Number 1.
        // TODO: Above member ERB24s is a very quick & dirty approach to defining the Test System's MCC USB-ERB24s.  Better ways:
        //  - Read them from InstaCal's cb.cfg file.
        //  - Dynamically discover them programmatically: https://www.mccdaq.com/pdfs/manuals/Mcculw_WebHelp/ULStart.htm.
        //  - Read them from Test Library's forthcoming app.config XML configuration file, then configure them dynamically/programmatically.

        public static void RelaysReset(List<Int32> BoardNumbers) {
            MccBoard ERB24;
            ErrorInfo EI;
            foreach (Int32 boardNumber in BoardNumbers) {
                ERB24 = new MccBoard(boardNumber);
                EI = ERB24.DOut(DigitalPortType.FirstPortA, 0);
                if (EI.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(ERB24, EI);
                EI = ERB24.DOut(DigitalPortType.FirstPortB, 0);
                if (EI.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(ERB24, EI);
                EI = ERB24.DOut(DigitalPortType.FirstPortCL, 0);
                if (EI.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(ERB24, EI);
                EI = ERB24.DOut(DigitalPortType.FirstPortCH, 0);
                if (EI.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(ERB24, EI);
            }
        }

        public static void RelayOn((Int32 Board, Int32 Relay) BR) {
            ERB24 = new MccBoard(BR.Board);
            EI = ERB24.DBitOut(DigitalPortType.FirstPortA, BR.Relay, DigitalLogicState.High);
            if (EI.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(ERB24, EI);
        }

        public static void RelayOff((Int32 Board, Int32 Relay) BR) {
            ERB24 = new MccBoard(BR.Board);
            EI = ERB24.DBitOut(DigitalPortType.FirstPortA, BR.Relay, DigitalLogicState.Low);
            if (EI.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(ERB24, EI);
        }
    }
}
