using System;
using TIDP.SAA; // https://www.ti.com/tool/FUSION_USB_ADAPTER_API/

namespace TestLibrary.Instruments.TexasInstruments {
    public sealed class USB_TO_GPIO {
        // NOTE: Update to .Net 7.0 & C# 11.0 if possible.  See 2nd Note below.
        // - Used .Net FrameWork 4.8 instead of .Net 7.0 because required Texas instruments' TIDP.SAA Fusion API targets
        //   .Net FrameWork 2.0, incompatible with .Net 7.0, C# 11.0 & UWP.
        // https://www.ti.com/tool/FUSION_USB_ADAPTER_API
        // NOTE: Microsoft supports I2C with their Windows.Devices.I2c & System.Device.I2c Namespaces:
        // - Conceivable above TI's TIDP.SAA Fusion Library could be replaced by these Namespaces, though they'd need
        //   to communicate with a GPIO adapter capable of interfacing to the customer's UUT.
        // - https://learn.microsoft.com/en-us/uwp/api/windows.devices.i2c?view=winrt-22621
        // - https://learn.microsoft.com/en-us/dotnet/api/system.device.i2c?view=iot-dotnet-latest
        // - https://github.com/Microsoft/Windows-universal-samples/tree/main/Samples/IoT-I2C
        // - Texas instruments has 2 USB-TO-GPIO adapters, and there are others, so potentially doable.
        // - Migrating from TI's TIDP.SAA to Microsoft's Windows.Devices.I2c & System.Device.I2c would also
        //   allow migration from .Net FrameWork 4.8, C# 7.3 & WinForms to .Net 7.0, C# 11.0 & UWP.
        // - Would require OS to be Windows Enterprise IoT 10 or 11, but that's eminently doable.
        // NOTE: This class applies to the original, 2006 USB-TO-GPIO adapter, recently obsoleted by the 2022 USB-TO-GPIO2 adapter.
        // - https://www.ti.com/tool/USB-TO-GPIO, © 2006
        // - https://www.ti.com/tool/USB-TO-GPIO2, © 2022
        // NOTE: USB-TO-GPIO2 requires the most current version of TI's Fusion API/DLL (TIDP.SAA/TIDP.SAA.dll) and Fusion GUI apps.
        // - https://e2e.ti.com/support/power-management-group/power-management/f/power-management-forum/1136746/usb-to-gpio2-difference-between-usb-to-gpio-and-usb-to-gpio2
        // - https://e2e.ti.com/support/power-management-group/power-management/f/power-management-forum/1132257/usb-to-gpio-the-difference-between-usb-to-gpio-and-usb-to-gpio2
        public readonly SMBusAdapter Adapter;
        public static readonly String Description = "Texas Instruments © 2006 USB Interface Adapter";
        private static readonly USB_TO_GPIO _instance = new USB_TO_GPIO();
        private static SAAStatus _saaStatus;
        private static BlockEncodedResult _blockEncodedResult;
        static USB_TO_GPIO() { }
        // Singleton pattern requires explicit static constructor to tell C# compiler not to mark type as beforefieldinit.
        // https://csharpindepth.com/articles/singleton

        private USB_TO_GPIO() {
            if (SMBusAdapter.Discover() == 0) throw new InvalidOperationException($"No '{Description}' found!");
            Adapter = SMBusAdapter.Adapter;
            Adapter.Set_Bus_Speed(SMBusAdapter.BusSpeed.Speed400KHz);
            Adapter.Set_PEC_Enabled(true);
            Adapter.Set_Pull_Ups(SMBusAdapter.ResistorValue.Ohm22k, SMBusAdapter.ResistorValue.Ohm22k, SMBusAdapter.ResistorValue.Ohm22k);
            Adapter.Set_Parallel_Mode(false);
        }
        
        public static USB_TO_GPIO Instance { get { return _instance; } }

        public static BlockEncodedResult BlockWriteBlockReadProcessCall(Byte Address, Byte CommandCode, Byte[] WriteBlock) {
            _blockEncodedResult = USB_TO_GPIO.Instance.Adapter.Block_Write_Block_Read_Process_Call(Address, CommandCode, WriteBlock);
            if (!_blockEncodedResult.Success) throw new InvalidOperationException(_blockEncodedResult.ToString());
            return _blockEncodedResult;
        }

        public static String BlockWriteBlockReadProcessCallConvertToText(Byte Address, Byte CommandCode, Byte[] WriteBlock) {
            _blockEncodedResult = BlockWriteBlockReadProcessCall(Address, CommandCode, WriteBlock);
            Array.Reverse(_blockEncodedResult.Bytes); // Accomodate low/high to high/low byte ordering.
            return _blockEncodedResult.ToString();
        }

        public static BlockEncodedResult ReadBlock(Byte Address, Byte CommandCode) {
            _blockEncodedResult = USB_TO_GPIO.Instance.Adapter.Read_Block(Address, CommandCode);
            if (!_blockEncodedResult.Success) throw new InvalidOperationException(_blockEncodedResult.ToString());
            return _blockEncodedResult;
        }

        public static String ReadBlockConvertToText(Byte Address, Byte CommandCode) {
            _blockEncodedResult = ReadBlock(Address, CommandCode);
            return HexStringToTextString(_blockEncodedResult.ToString());
        }

        public static SAAStatus WriteBlock(Byte Address, Byte CommandCode, Byte[] Data) {
            _saaStatus = USB_TO_GPIO.Instance.Adapter.Write_Block(Address, CommandCode, Data);
            if (_saaStatus != SAAStatus.Success) throw new InvalidOperationException(_saaStatus.ToString());
            return _saaStatus;
        }

        public static void WriteBlockStripStatus(Byte Address, Byte CommandCode, Byte[] Data) { WriteBlock(Address, CommandCode, Data); }

        public static SAAStatus WriteByte(Byte Address, Byte CommandCode, Byte Data) {
            _saaStatus = USB_TO_GPIO.Instance.Adapter.Write_Byte(Address, CommandCode, Data);
            if (_saaStatus != SAAStatus.Success) throw new InvalidOperationException(_saaStatus.ToString());
            return _saaStatus;
        }

        public static void WriteByteStripStatus(Byte Address, Byte CommandCode, Byte Data) { WriteByte(Address, CommandCode, Data); }

        public static SAAStatus WriteWord(Byte Address, Byte CommandCode, Byte ByteHigh, Byte ByteLow) {
            _saaStatus = USB_TO_GPIO.Instance.Adapter.Write_Word(Address, CommandCode, ByteHigh, ByteLow);
            if (_saaStatus != SAAStatus.Success) throw new InvalidOperationException(_saaStatus.ToString());
            return _saaStatus;
        }

        public static void WriteWordStripStatus(Byte Address, Byte CommandCode, Byte ByteHigh, Byte ByteLow) { WriteWord(Address, CommandCode, ByteHigh, ByteLow); }

        public static String HexStringToTextString(String hexString) {
            hexString = hexString.Replace("0x", "");
            Byte[] bytes = new Byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2) bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            return System.Text.Encoding.ASCII.GetString(bytes, 0, bytes.Length);
        }

        public static String TextStringToHexString(String textString) {
            Byte[] HexString = System.Text.Encoding.ASCII.GetBytes(textString);
            return BitConverter.ToString(HexString).Replace("-", "");
        }
    }
}