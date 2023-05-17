using System;
using System.Security.Cryptography;
using TIDP.SAA; // https://www.ti.com/tool/FUSION_USB_ADAPTER_API/

namespace ABT.TestSpace.InterfaceAdapters {
    public sealed class USB_TO_GPIO {
        // TODO: Separate this USB_TO_GPIO class into it's own Project, like USB-ERB24, so it can easily be included/excluded as needed.
        // NOTE: If USB_TO_GPIO is externally linked to TestExecutive, like USB-ERB24, it will be easier to update TestExecutive to .Net 7.0 & C# 11.0.
        // TODO: Update to .Net 7.0 & C# 11.0 if possible.
        // - Used .Net FrameWork 4.8 instead of .Net 7.0 because required Texas instruments' TIDP.SAA Fusion API targets
        //   .Net FrameWork 2.0, incompatible with .Net 7.0, C# 11.0 & UWP.
        // https://www.ti.com/tool/FUSION_USB_ADAPTER_API
        // NOTE: Below hopefully "value-added" wrapper methods for some commonly used SMBusAdapter commands are conveniences, not necessities.
        // NOTE: Will never fully implement wrapper methods for the complete set of SMBusAdapter commands, just some of the most commonly used ones.
        // - In general, TestExecutive's InterfaceAdapters, Logging, SCPI_VISA_Instruments & Switching namespaces exist partly to eliminate
        //   the need to reference TestExecutive's various DLLs directly from TestExecutor client apps.
        // - As long as suitable wrapper methods exists in USB_TO_GPIO, needn't directly reference TIDP.SAA from TestExecutor client apps,
        //   as referencing TestExecutive suffices.
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
        public const String Description = "Texas Instruments © 2006 USB Interface Adapter";
        private readonly static USB_TO_GPIO _only = new USB_TO_GPIO();
        private static SAAStatus _saaStatus;
        private static BlockEncodedResult _blockEncodedResult;
        static USB_TO_GPIO() { }
        // Singleton pattern requires explicit static constructor to tell C# compiler not to mark type as beforefieldinit.
        // https://csharpindepth.com/articles/singleton

        private USB_TO_GPIO() {
            if (SMBusAdapter.Discover() == 0) throw new InvalidOperationException($"No '{Description}' found!");
            this.Adapter = SMBusAdapter.Adapter;
            this.Adapter.Set_Bus_Speed(SMBusAdapter.BusSpeed.Speed400KHz);
            this.Adapter.Set_PEC_Enabled(true);
            this.Adapter.Set_Pull_Ups(SMBusAdapter.ResistorValue.Ohm22k, SMBusAdapter.ResistorValue.Ohm22k, SMBusAdapter.ResistorValue.Ohm22k);
            this.Adapter.Set_Parallel_Mode(false);
        }
        
        public static USB_TO_GPIO Only { get { return _only; } }

        public static BlockEncodedResult BlockWriteBlockReadProcessCall(Byte Address, Byte CommandCode, Byte[] WriteBlock) {
            _blockEncodedResult = USB_TO_GPIO.Only.Adapter.Block_Write_Block_Read_Process_Call(Address, CommandCode, WriteBlock);
            // Documented in TIDP.SAA's SMBusAdapter Class as method blockWriteBlockReadProcessCall(Byte, Byte, Byte[])
            if (!_blockEncodedResult.Success) throw new InvalidOperationException(_blockEncodedResult.ToString());
            return _blockEncodedResult;
        }

        public static Byte[] BlockWriteBlockReadProcessCallConvertToBytes(Byte Address, Byte CommandCode, Byte[] WriteBlock) {
            _blockEncodedResult = BlockWriteBlockReadProcessCall(Address, CommandCode, WriteBlock);
            Array.Reverse(_blockEncodedResult.Bytes); // Accommodate low/high to high/low byte ordering.
            return _blockEncodedResult.Bytes;
        }

        public static String BlockWriteBlockReadProcessCallConvertToText(Byte Address, Byte CommandCode, Byte[] WriteBlock) {
            return BlockWriteBlockReadProcessCallConvertToBytes(Address, CommandCode, WriteBlock).ToString();
        }

        public static SAAStatus SendByte(Byte Address, Byte CommandCode) {
            _saaStatus = USB_TO_GPIO.Only.Adapter.Send_Byte(Address, CommandCode);
            if (_saaStatus != SAAStatus.Success) throw new InvalidOperationException(_saaStatus.ToString());
            return _saaStatus;
        }

        public static void SendByteStripStatus(Byte Address, Byte CommandCode) { _ = USB_TO_GPIO.Only.Adapter.Send_Byte(Address, CommandCode); }

        public static BlockEncodedResult ReadBlock(Byte Address, Byte CommandCode) {
            _blockEncodedResult = USB_TO_GPIO.Only.Adapter.Read_Block(Address, CommandCode);
            // Documented in TIDP.SAA's SMBusAdapter Class as method readBlock(Byte, Byte)
            if (!_blockEncodedResult.Success) throw new InvalidOperationException(_blockEncodedResult.ToString());
            return _blockEncodedResult;
        }

        public static (Int32 VinCountsController, Int32 VinCountsResponder) ReadBlockStripStatus(Byte Address, Byte CommandCode) {
            _saaStatus = USB_TO_GPIO.Only.Adapter.Read_Block(Address, CommandCode, out Byte[] VinCounts);
            // Documented in TIDP.SAA's SMBusAdapter Class as method readBlock(Byte, Byte)
            if (_saaStatus != SAAStatus.Success) throw new InvalidOperationException(_saaStatus.ToString());
            return (256 * VinCounts[1] + VinCounts[0], 256 * VinCounts[3] + VinCounts[2]);
        }

        public static String ReadBlockConvertToHexString(Byte Address, Byte CommandCode) { return ReadBlock(Address, CommandCode).ToString(); }

        public static String ReadBlockConvertToTextString(Byte Address, Byte CommandCode) { return HexStringToTextString(ReadBlockConvertToHexString(Address, CommandCode)); }

        public static String ReadWord(Byte Address, Byte CommandCode) {
            WordEncodedResult wordEncodedResult = USB_TO_GPIO.Only.Adapter.Read_Word(Address, CommandCode);
            // Documented in TIDP.SAA's SMBusAdapter Class as method readWord(Byte, Byte)
            if (!wordEncodedResult.Success) throw new InvalidOperationException(wordEncodedResult.SAA_Status_String);
            return wordEncodedResult.ToString();
        }

        public static Int32 ReadWordStripStatus(Byte Address, Byte CommandCode) {
            _saaStatus = USB_TO_GPIO.Only.Adapter.Read_Word(Address, CommandCode, out Byte ByteHigh, out Byte ByteLow);
            // Documented in TIDP.SAA's SMBusAdapter Class as method readWord(Byte, Byte, Byte, Byte)
            if (_saaStatus != SAAStatus.Success) throw new InvalidOperationException(_saaStatus.ToString());
            return ByteHigh * 256 + ByteLow;
        }

        public static SAAStatus WriteBlock(Byte Address, Byte CommandCode, Byte[] Data) {
            _saaStatus = USB_TO_GPIO.Only.Adapter.Write_Block(Address, CommandCode, Data);
            // Documented in TIDP.SAA's SMBusAdapter Class as method writeBlock(Byte, Byte, Byte[])
            if (_saaStatus != SAAStatus.Success) throw new InvalidOperationException(_saaStatus.ToString());
            return _saaStatus;
        }

        public static void WriteBlockStripStatus(Byte Address, Byte CommandCode, Byte[] Data) { _ = WriteBlock(Address, CommandCode, Data); }
        // NOTE: the void *StripStatus methods exist solely to eliminate referencing TI's TIDP.SAA.dll library from TestExecutive client TestExecutor programs.

        public static SAAStatus WriteByte(Byte Address, Byte CommandCode, Byte Data) {
            _saaStatus = USB_TO_GPIO.Only.Adapter.Write_Byte(Address, CommandCode, Data);
            // Documented in TIDP.SAA's SMBusAdapter Class as method writeByte(Byte, Byte, Byte)
            if (_saaStatus != SAAStatus.Success) throw new InvalidOperationException(_saaStatus.ToString());
            return _saaStatus;
        }

        public static void WriteByteStripStatus(Byte Address, Byte CommandCode, Byte Data) { _ = WriteByte(Address, CommandCode, Data); }

        public static SAAStatus WriteWord(Byte Address, Byte CommandCode, Byte ByteHigh, Byte ByteLow) {
            _saaStatus = USB_TO_GPIO.Only.Adapter.Write_Word(Address, CommandCode, ByteHigh, ByteLow);
            // Documented in TIDP.SAA's SMBusAdapter Class as method writeWord(Byte, Byte, Byte, Byte)
            if (_saaStatus != SAAStatus.Success) throw new InvalidOperationException(_saaStatus.ToString());
            return _saaStatus;
        }

        public static void WriteWordStripStatus(Byte Address, Byte CommandCode, Byte ByteHigh, Byte ByteLow) { _ = WriteWord(Address, CommandCode, ByteHigh, ByteLow); }

        public static String HexStringToTextString(String hexString) {
            hexString = hexString.Replace("0x", "");
            Byte[] bytes = new Byte[hexString.Length / 2];
            for (Int32 i = 0; i < hexString.Length; i += 2) bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            return System.Text.Encoding.ASCII.GetString(bytes, 0, bytes.Length);
        }

        public static String TextStringToHexString(String textString) {
            Byte[] HexString = System.Text.Encoding.ASCII.GetBytes(textString);
            return BitConverter.ToString(HexString).Replace("-", "");
        }
    }
}