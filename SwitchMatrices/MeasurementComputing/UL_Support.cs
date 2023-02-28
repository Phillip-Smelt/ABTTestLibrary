using System;
using MccDaq; // MCC DAQ Universal Library 6.73 from https://www.mccdaq.com/Software-Downloads.

namespace TestLibrary.SwitchMatrices.MeasurementComputing {
    public static class UL_Support {
        public static void MccBoardErrorHandler(MccBoard MCCB, ErrorInfo EI) {
            throw new InvalidOperationException(
                $"MccBoard BoardNum   : {MCCB.BoardNum}.{Environment.NewLine}" +
                $"MccBoard BoardName  : {MCCB.BoardName}.{Environment.NewLine}" +
                $"MccBoard Descriptor : {MCCB.Descriptor}.{Environment.NewLine}" +
                $"ErrorInfo Value     : {EI.Value}.{Environment.NewLine}" +
                $"ErrorInfo Message   : {EI.Message}.{Environment.NewLine}");
        }
    }
}

