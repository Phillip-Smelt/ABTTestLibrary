using System;
using MccDaq; // MCC DAQ Universal Library 6.73 from https://www.mccdaq.com/Software-Downloads.

namespace TestLibrary.Instruments.MeasurementComputing {
    public static class ERB24 {
        private static MccBoard Erb24;
        private static ErrorInfo EI;

        public static void RelaysReset() {
            EI = MccService.ErrHandling(ErrorReporting.PrintAll, ErrorHandling.DontStop);
            DaqDeviceManager.IgnoreInstaCal();
            DaqDeviceDescriptor[] UsbDeviceDescriptors = DaqDeviceManager.GetDaqDeviceInventory(DaqDeviceInterface.Usb);
            if (UsbDeviceDescriptors.Length > 0) {
                for (int device = 0; device < UsbDeviceDescriptors.Length; device++) {
                    MccBoard UsbDevice = DaqDeviceManager.CreateDaqDevice(device, UsbDeviceDescriptors[device]);
                    if (String.Equals(UsbDevice.BoardName, "USB-ERB24")) {
                        EI = UsbDevice.DOut(DigitalPortType.FirstPortCL, 0);
                        if (EI.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(EI);
                        EI = UsbDevice.DOut(DigitalPortType.FirstPortCH, 0);
                        if (EI.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(EI);
                    }
                }
            }
        }

        public static void RelayOn(Int32 MccErb24Board, Int32 MccErb24Relay) {
            Erb24 = new MccBoard(MccErb24Board);
            EI = Erb24.DBitOut(DigitalPortType.FirstPortA, MccErb24Relay, DigitalLogicState.High);
            if (EI.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(EI);
        }

        public static void RelayOff(Int32 MccErb24Board, Int32 MccErb24Relay) {
            Erb24 = new MccBoard(MccErb24Board);
            EI = Erb24.DBitOut(DigitalPortType.FirstPortA, MccErb24Relay, DigitalLogicState.Low);
            if (EI.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(EI);
        }

        private static void MccBoardErrorHandler(ErrorInfo EI) {
            throw new InvalidOperationException(
                $"MccDaq.ErrorInfo:{Environment.NewLine}" +
                $"Message : {EI.Message}{Environment.NewLine}" +
                $"Value   : {EI.Value}.");
        }
    }
}
