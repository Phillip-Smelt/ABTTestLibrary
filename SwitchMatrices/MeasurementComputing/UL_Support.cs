using System;
using MccDaq; // MCC DAQ Universal Library 6.73 from https://www.mccdaq.com/Software-Downloads.

namespace TestLibrary.SwitchMatrices.MeasurementComputing {
    public static class UL_Support {
        public static void MccBoardErrorHandler(MccBoard mccb, ErrorInfo ei) {
            throw new InvalidOperationException(
                $"MccBoard BoardNum   : {mccb.BoardNum}.{Environment.NewLine}" +
                $"MccBoard BoardName  : {mccb.BoardName}.{Environment.NewLine}" +
                $"MccBoard Descriptor : {mccb.Descriptor}.{Environment.NewLine}" +
                $"ErrorInfo Value     : {ei.Value}.{Environment.NewLine}" +
                $"ErrorInfo Message   : {ei.Message}.{Environment.NewLine}");
        }
    }
}

