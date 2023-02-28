using System;
using System.Collections.Generic;
using MccDaq; // MCC DAQ Universal Library 6.73 from https://www.mccdaq.com/Software-Downloads.

namespace TestLibrary.SwitchMatrices.MeasurementComputing {
    public static class ERB24 {
        private static MccBoard Erb24;
        private static ErrorInfo EI;
        public static readonly List<Int32> ERB24s = new List<Int32>() {0};
        // TODO: Add 2nd USB-ERB24 board.  Use InstaCal to configure as Board Number 1.
        // TODO: Above member ERB24s is a very quick & dirty approach to defining the Test System's MCC USB-ERB24s.  Better ways:
        //  - Read them from InstaCal's cb.cfg file.
        //  - Dynamically discover them programmatically: https://www.mccdaq.com/pdfs/manuals/Mcculw_WebHelp/ULStart.htm.
        //  - Read them from Test Library's forthcoming app.config XML configuration file, then configure them dynamically/programmatically.

        public static void RelaysReset(List<Int32> BoardNumbers) {
            MccBoard Erb24;
            ErrorInfo EI;
            foreach (Int32 boardNumber in BoardNumbers) {
                Erb24 = new MccBoard(boardNumber);
                EI = Erb24.DOut(DigitalPortType.FirstPortA, 0);
                if (EI.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(Erb24, EI);
                EI = Erb24.DOut(DigitalPortType.FirstPortB, 0);
                if (EI.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(Erb24, EI);
                EI = Erb24.DOut(DigitalPortType.FirstPortCL, 0);
                if (EI.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(Erb24, EI);
                EI = Erb24.DOut(DigitalPortType.FirstPortCH, 0);
                if (EI.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(Erb24, EI);
            }
        }

        public static void RelayOn(Int32 MccErb24Board, Int32 MccErb24Relay) {
            Erb24 = new MccBoard(MccErb24Board);
            EI = Erb24.DBitOut(DigitalPortType.FirstPortA, MccErb24Relay, DigitalLogicState.High);
            if (EI.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(Erb24, EI);
        }

        public static void RelayOff(Int32 MccErb24Board, Int32 MccErb24Relay) {
            Erb24 = new MccBoard(MccErb24Board);
            EI = Erb24.DBitOut(DigitalPortType.FirstPortA, MccErb24Relay, DigitalLogicState.Low);
            if (EI.Value != ErrorInfo.ErrorCode.NoErrors) UL_Support.MccBoardErrorHandler(Erb24, EI);
        }
    }
}
