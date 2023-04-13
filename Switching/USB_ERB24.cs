using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using MccDaq; // MCC DAQ Universal Library 6.73 from https://www.mccdaq.com/Software-Downloads.

namespace TestLibrary.Switching {
    public static class USB_ERB24 {
        // TODO: Convert the USB_ERB24 class to a Singleton, like the USB_TO_GPIO class.  If there are multiple USB-ERB24s, copy the USB_ERB24 class & append numbers?  USB_UE24_1, USB_UE24_2...
        // NOTE: This class assumes all USB-ERB24 relays are configured for Non-Inverting Logic & Pull-Down/de-energized at power-up.
        // NOTE: USB-ERB24 relays are configurable for either Non-Inverting or Inverting logic, via hardware DIP switch S1.
        //  - Non-Inverting:  Logic low de-energizes the relays, logic high energizes them.
        //  - Inverting:      Logic low energizes the relays, logic high de-energizes them.  
        // NOTE: USB-ERB24 relays are configurable with default power-up states, via hardware DIP switch S2.
        //  - Pull-Up:        Relays are energized at power-up.
        //  - Pull-Down:      Relays are de-energized at power-up.
        //  - https://www.mccdaq.com/PDFs/Manuals/usb-erb24.pdf.
        // NOTE: Below UE24_BOARDS enum is a static definition of TestLibrary's MCC USB-ERB24(s).
        public enum UE24_BOARDS { E01 }
        // Dynamic definition methods for UE24_BOARDS:
        //  - Read them from MCC InstaCal's cb.cfg file.
        //  - Dynamically discover them programmatically: https://www.mccdaq.com/pdfs/manuals/Mcculw_WebHelp/ULStart.htm.
        //  - Specify MCC USB-ERB24s in App.config, then confirm existence during TestLibrary's initialization.
        // NOTE: MCC's InstaCal USB-ERB24's board number indexing begins at 0, guessing because USB device indexing is likely also zero based.
        // - So UE24_BOARDS.E01 numerical value is 0, which is used when constructing a new MccBoard UE24_BOARDS.E01 object:
        // - Instantiation 'new MccBoard((Int32)UE24_BOARDS.E01)' is equivalent to 'new MccBoard(0)'.
        public enum UE24 {
            R01, R02, R03, R04, R05, R06, R07, R08,
            R09, R10, R11, R12, R13, R14, R15, R16,
            R17, R18, R19, R20, R21, R22, R23, R24
        }
        internal enum UE24_PORTS { A, B, CL, CH }
        private const Int32 UE24_PORT_COUNT = 4;
        [Flags]
        internal enum UE24_BITS : UInt32 {
            None = 0,
            B00 = 1 << 00, B01 = 1 << 01, B02 = 1 << 02, B03 = 1 << 03, B04 = 1 << 04, B05 = 1 << 05, B06 = 1 << 06, B07 = 1 << 07,
            B08 = 1 << 08, B09 = 1 << 09, B10 = 1 << 10, B11 = 1 << 11, B12 = 1 << 12, B13 = 1 << 13, B14 = 1 << 14, B15 = 1 << 15,
            B16 = 1 << 16, B17 = 1 << 17, B18 = 1 << 18, B19 = 1 << 19, B20 = 1 << 20, B21 = 1 << 21, B22 = 1 << 22, B23 = 1 << 23
        }
        [Flags]
        internal enum UE24_PORTS_AεB : UInt32 {
            None = 0,
            B00 = UE24_BITS.B00, B01 = UE24_BITS.B01, B02 = UE24_BITS.B02, B03 = UE24_BITS.B03, B04 = UE24_BITS.B04, B05 = UE24_BITS.B05, B06 = UE24_BITS.B06, B07 = UE24_BITS.B07,
            All = B00 | B01 | B02 | B03 | B04 | B05 | B06 | B07
        }
        [Flags]
        internal enum UE24_PORTS_CLεCH : UInt32 {
            None = 0,
            B00 = UE24_BITS.B00, B01 = UE24_BITS.B01, B02 = UE24_BITS.B02, B03 = UE24_BITS.B03,
            All = B00 | B01 | B02 | B03
        }
        internal static readonly Dictionary<UE24, UE24_BITS> UE24_RεB = new Dictionary<UE24, UE24_BITS>() {
            { UE24.R01, UE24_BITS.B00 }, { UE24.R02, UE24_BITS.B01 }, { UE24.R03, UE24_BITS.B02 }, { UE24.R04, UE24_BITS.B03 },
            { UE24.R05, UE24_BITS.B04 }, { UE24.R06, UE24_BITS.B05 }, { UE24.R07, UE24_BITS.B06 }, { UE24.R08, UE24_BITS.B07 },
            { UE24.R09, UE24_BITS.B08 }, { UE24.R10, UE24_BITS.B09 }, { UE24.R11, UE24_BITS.B10 }, { UE24.R12, UE24_BITS.B11 },
            { UE24.R13, UE24_BITS.B12 }, { UE24.R14, UE24_BITS.B13 }, { UE24.R15, UE24_BITS.B14 }, { UE24.R16, UE24_BITS.B15 },
            { UE24.R17, UE24_BITS.B16 }, { UE24.R18, UE24_BITS.B17 }, { UE24.R19, UE24_BITS.B18 }, { UE24.R20, UE24_BITS.B19 },
            { UE24.R21, UE24_BITS.B20 }, { UE24.R22, UE24_BITS.B21 }, { UE24.R23, UE24_BITS.B22 }, { UE24.R24, UE24_BITS.B23 },
        };
        //  - Wish MCC had zero-indexed their Relays, numbering from R0 to R23 instead of R1 to R24.
        //  - Would've been optimal, as Relays 1 to 24 are controlled by digital port bits that are zero-indexed, from 0 to 23.
        public static Boolean AreUE24_BoardsReset() {
            Boolean areUE24_BoardsReset = true;
            foreach (UE24_BOARDS ue24Board in Enum.GetValues(typeof(UE24_BOARDS))) {
                MccBoard mccBoard = new MccBoard((Int32)ue24Board);
                areUE24_BoardsReset = areUE24_BoardsReset && IsPortReset(mccBoard, DigitalPortType.FirstPortA);
                areUE24_BoardsReset = areUE24_BoardsReset && IsPortReset(mccBoard, DigitalPortType.FirstPortB);
                areUE24_BoardsReset = areUE24_BoardsReset && IsPortReset(mccBoard, DigitalPortType.FirstPortCL);
                areUE24_BoardsReset = areUE24_BoardsReset && IsPortReset(mccBoard, DigitalPortType.FirstPortCH);
            }
            return areUE24_BoardsReset;
        }

        public static void DeEnergizeUE24(UE24_BOARDS ue24Board) {
            MccBoard mccBoard = new MccBoard((Int32)ue24Board);
            DigitalPortsWrite(mccBoard, new UInt16[UE24_PORT_COUNT] { (UInt16)UE24_PORTS_AεB.None, (UInt16)UE24_PORTS_AεB.None, (UInt16)UE24_PORTS_AεB.None, (UInt16)UE24_PORTS_AεB.None });
        }

        public static void EnergizeUE24(UE24_BOARDS ue24Board) {
            MccBoard mccBoard = new MccBoard((Int32)ue24Board);
            DigitalPortsWrite(mccBoard, new UInt16[UE24_PORT_COUNT] { (UInt16)UE24_PORTS_AεB.All, (UInt16)UE24_PORTS_AεB.All, (UInt16)UE24_PORTS_AεB.All, (UInt16)UE24_PORTS_AεB.All });
        }

        public static void DeEnergizeUE24_All() { foreach (UE24_BOARDS ue24Board in Enum.GetValues(typeof(UE24_BOARDS))) DeEnergizeUE24(ue24Board); }

        public static void EnergizeUE24_All() { foreach (UE24_BOARDS ue24Board in Enum.GetValues(typeof(UE24_BOARDS))) EnergizeUE24(ue24Board); }

        public static void SetState((UE24_BOARDS Board, UE24 Relay) UE24, RelayForms.C State) {
            MccBoard mccBoard = new MccBoard((Int32)UE24.Board);
            DigitalLogicState desiredState = (State is RelayForms.C.NC) ? DigitalLogicState.Low : DigitalLogicState.High;
            _ = GetPortType(UE24.Relay);
            DigitalBitWrite(mccBoard, UE24.Relay, desiredState);
            DigitalLogicState outputState = DigitalBitRead(mccBoard, UE24.Relay);
            if (outputState != desiredState) throw new InvalidOperationException($"USB-ERB24 '({UE24.Board}, {UE24.Relay})' failed to set to '{State}'.");
        }

        public static void SetStates(UE24_BOARDS Board, Dictionary<UE24, RelayForms.C> relayStates) {
            MccBoard mccBoard = new MccBoard((Int32)Board);
            UInt32 relayBits = 0x0000;
            UE24_BITS ue24bit;
            foreach (KeyValuePair<UE24, RelayForms.C> kvp in relayStates) {
                ue24bit = ((RelayForms.C)UE24_RεB[kvp.Key] is RelayForms.C.NC) ? UE24_BITS.None : (UE24_BITS)Enum.ToObject(typeof(UE24_BITS), (Int32)kvp.Key);
                relayBits |= (UInt32)ue24bit; // Sets a 1 in each bit corresponding to relay state in relayStates.
            }
            Byte[] bits = BitConverter.GetBytes(relayBits);
            UInt16[] biggerBits = Array.ConvertAll(bits, delegate (Byte b) { return (UInt16)b; });
            UInt16[] ports = DigitalPortsRead(mccBoard);
            ports[(Int32)UE24_PORTS.A] |= biggerBits[(Int32)UE24_PORTS.A];
            ports[(Int32)UE24_PORTS.B] |= biggerBits[(Int32)UE24_PORTS.B];
            ports[(Int32)UE24_PORTS.CL] |= (biggerBits[(Int32)UE24_PORTS.CL] &= 0x0F); // Remove CH bits.
            ports[(Int32)UE24_PORTS.CH] |= (biggerBits[(Int32)UE24_PORTS.CH] &= 0xF0); // Remove CL bits.
            DigitalPortsWrite(mccBoard, ports);
        }

        public static RelayForms.C GetState((UE24_BOARDS Board, UE24 Relay) UE24) {
            MccBoard mccBoard = new MccBoard((Int32)UE24.Board);
            DigitalLogicState outputState = DigitalBitRead(mccBoard, UE24.Relay);
            return (outputState == DigitalLogicState.Low) ? RelayForms.C.NC : RelayForms.C.NO;
        }

        public static Dictionary<UE24, RelayForms.C> GetStates(UE24_BOARDS ue24Board) {
            MccBoard mccBoard = new MccBoard((Int32)ue24Board);
            UInt16[] bits = DigitalPortsRead(mccBoard);
            UInt32[] biggerBits = Array.ConvertAll(bits, delegate (UInt16 ui) { return (UInt32)ui; });
            UInt32 relayBits= 0x0000;
            relayBits |= biggerBits[(Int32)UE24_PORTS.A]  << 00;
            relayBits |= biggerBits[(Int32)UE24_PORTS.B]  << 08;
            relayBits |= biggerBits[(Int32)UE24_PORTS.CL] << 12;
            relayBits |= biggerBits[(Int32)UE24_PORTS.CH] << 16;
            BitVector32 bitVector32 = new BitVector32((Int32)relayBits);
            Dictionary<UE24, RelayForms.C> relayStates = new Dictionary<UE24, RelayForms.C>();

            UE24 ue24relay;
            RelayForms.C cState;
            for (Int32 i=0; i < 32; i++) {
                ue24relay = (UE24)Enum.ToObject(typeof(UE24), bitVector32[i]);
                cState = bitVector32[i] ? RelayForms.C.NO : RelayForms.C.NC;
                relayStates.Add(ue24relay, cState);
            }
            return relayStates;
        }

        internal static DigitalLogicState DigitalBitRead(MccBoard mccBoard, UE24 ue24Relay) {
            ErrorInfo errorInfo = mccBoard.DBitIn(DigitalPortType.FirstPortA, (Int32)ue24Relay, out DigitalLogicState bitValue);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
            return bitValue;
        }

        internal static void DigitalBitWrite(MccBoard mccBoard, UE24 ue24Relay, DigitalLogicState inputLogicState) {
            ErrorInfo errorInfo = mccBoard.DBitOut(DigitalPortType.FirstPortA, (Int32)ue24Relay, inputLogicState);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
        }

        internal static UInt16 DigitalPortRead(MccBoard mccBoard, DigitalPortType digitalPortType) {
            ErrorInfo errorInfo = mccBoard.DIn(digitalPortType, out UInt16 dataValue);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
            return dataValue;
        }

        internal static UInt16[] DigitalPortsRead(MccBoard mccBoard) {
            return new UInt16[UE24_PORT_COUNT] {
                DigitalPortRead(mccBoard, DigitalPortType.FirstPortA),
                DigitalPortRead(mccBoard, DigitalPortType.FirstPortB),
                DigitalPortRead(mccBoard, DigitalPortType.FirstPortCL),
                DigitalPortRead(mccBoard, DigitalPortType.FirstPortCH)
            };
        }

        internal static void DigitalPortWrite(MccBoard mccBoard, DigitalPortType digitalPortType, UInt16 dataValue) {
            Console.WriteLine($"Data Value: '{dataValue}'.");
            Debug.WriteLine($"Data Value: '{dataValue}'.");
            ErrorInfo errorInfo = mccBoard.DOut(digitalPortType, dataValue);
            if (errorInfo.Value != ErrorInfo.ErrorCode.NoErrors) MccBoardErrorHandler(mccBoard, errorInfo);
        }

        internal static void DigitalPortsWrite(MccBoard mccBoard, UInt16[] Ports) {
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortA,  Ports[(Int32)UE24_PORTS.A]);
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortB,  Ports[(Int32)UE24_PORTS.B]);
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortCL, Ports[(Int32)UE24_PORTS.CL]);
            DigitalPortWrite(mccBoard, DigitalPortType.FirstPortCH, Ports[(Int32)UE24_PORTS.CH]);
        }

        internal static Boolean IsPortReset(MccBoard mccBoard, DigitalPortType digitalPortType) { return DigitalPortRead(mccBoard, digitalPortType) == (UInt16)UE24_BITS.None; }

        internal static DigitalPortType GetPortType(UE24 ue24relay) {
            switch (ue24relay) {
                case UE24 relay when relay <= UE24.R08:
                    return DigitalPortType.FirstPortA;
                case UE24 relay when relay <= UE24.R16:
                    return DigitalPortType.FirstPortB;
                case UE24 relay when relay <= UE24.R20:
                    return DigitalPortType.FirstPortCL;
                case UE24 relay when relay <= UE24.R24:
                    return DigitalPortType.FirstPortCH;
                default:
                    throw new NotImplementedException("Invalid USB-ERB24 relay, must be in enum 'UE24'.");
            }
        }

        internal static void MccBoardErrorHandler(MccBoard mccBoard, ErrorInfo errorInfo) {
            throw new InvalidOperationException(
                $"{Environment.NewLine}" +
                $"MccBoard BoardNum   : {mccBoard.BoardNum}.{Environment.NewLine}" +
                $"MccBoard BoardName  : {mccBoard.BoardName}.{Environment.NewLine}" +
                $"MccBoard Descriptor : {mccBoard.Descriptor}.{Environment.NewLine}" +
                $"ErrorInfo Value     : {errorInfo.Value}.{Environment.NewLine}" +
                $"ErrorInfo Message   : {errorInfo.Message}.{Environment.NewLine}");
        }
    }
}
