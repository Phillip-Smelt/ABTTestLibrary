using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using Windows.Devices.Enumeration;
using Windows.Devices.PointOfService;
using Windows.Security.Cryptography;

namespace ABT.TestSpace.TestExec.Logging {
    public sealed partial class SerialNumberDialog : Form {
        // NOTE: Honeywell Voyager 1200g Scanner must be programmed into USB HID mode to work correctly with TestExecutive to read ABT Serial #s.
        //       - Scan PAP131 label from "Honeywell Voyager 1200G User's Guide ReadMe.pdf" to program 1200 into USB HID mode.
        //       - Both "ReadMe" & "User's Guides" documents reside in this folder for convenience.
        //       - https://sps-support.honeywell.com/s/article/The-scanner-is-not-recognized-by-Microsoft-Universal-Windows-Platform-application
        // NOTE: Voyager 1200g won't scan ABT Serial #s into Notepad/Wordpad/Text Editor of Choice when in USB HID mode:
        //       - It will only deliver scanned data to a USB HID application like TestExecutive's SerialNumberDialog class.
        //       - You must scan the Voyager 1200G's PAP124 barcodes to restore "normal" keyboard wedge mode.
        // NOTE: Honeywell Voyager 1200g USB Barcode Scanner is a Microsoft supported Point of Service peripheral.
        //  - https://learn.microsoft.com/en-us/windows/uwp/devices-sensors/pos-device-support
        // NOTE: Annoyingly, Voyager 1200g isn't recognized when plugged into USB hub and both PC & equipment rack powered off/on.
        //       - May work better if plugged directly into PC, rather than rack's hub.  May not; didn't test this thought.
        //       - Well, what can you expect for $100?
        //       - https://sps-support.honeywell.com/s/article/Voyager-1202g-is-not-recognized-by-the-host-after-PC-boot-up.        // NOTE: The 1200G must also be programmed to read the Barcode Symbology of ABT's Serial #s, which at the time of this writing is Code39.
        // public static SerialNumberDialog Only { get; } = new SerialNumberDialog();
        private BarcodeScanner _scanner = null;
        private ClaimedBarcodeScanner _claimedScanner = null;
        private String _scannerID = null;
        private String _regEx = null;

        // static SerialNumberDialog() { }
        // Singleton pattern requires explicit static constructor to tell C# compiler not to mark type as beforefieldinit.
        // https://csharpindepth.com/articles/singleton

        public SerialNumberDialog() {
            InitializeComponent();
            GetConfiguration();
            GetBarcodeScanner();
            FormUpdate(String.Empty);
        }

        public void Set(String SerialNumber) { FormUpdate(SerialNumber); }

        public String Get() { return BarCodeText.Text; }

        private async void GetBarcodeScanner() {
#if DEBUG
            DeviceInformationCollection dic = await DeviceInformation.FindAllAsync(BarcodeScanner.GetDeviceSelector(PosConnectionTypes.Local));
            foreach (DeviceInformation di in dic) {
                Debug.Print($"Name: '{di.Name}'.");
                Debug.Print($"Kind: '{di.Kind}'.");
                Debug.Print($"ID  : '{di.Id}'.{Environment.NewLine}");
            // NOTE: If ever change USB corded Barcode Scanner from current Voyager 1200g with ID "\\?\HID#VID_0C2E&PID_0A07&MI_00#7&1f27e379&0&0000#{c243ffbd-3afc-45e9-b3d3-2ba18bc7ebc5}\posbarcodescanner":
            //       - Can discover new corded USB Scanner's ID by running above code in Visual Studio in Debug Configuration.
            // NOTE: Above enumeration will only work on explicitly Microsoft supported corded USB Barcode Scanners, listed at https://learn.microsoft.com/en-us/windows/uwp/devices-sensors/pos-device-support.
            // NOTE: Avove enumeration code will not work with BlueTooth or Wireless Barcode Scanners, even if explicitly Microsoft supported.
            }
#endif
            DeviceInformation DI = await DeviceInformation.CreateFromIdAsync(_scannerID);
            _scanner = await BarcodeScanner.FromIdAsync(DI.Id);
            if (_scanner == null) throw new InvalidOperationException($"Barcode scanner Device ID:{Environment.NewLine}{Environment.NewLine}'{_scannerID}'{Environment.NewLine}{Environment.NewLine}not found.");
            _claimedScanner = await _scanner.ClaimScannerAsync(); // Claim exclusively.
            if (_claimedScanner == null) throw new InvalidOperationException("Barcode scanner cannot be claimed.");
            _claimedScanner.DataReceived += ClaimedScanner_DataReceived;
            _claimedScanner.ErrorOccurred += ClaimedScanner_ErrorOccurred;
            _claimedScanner.ReleaseDeviceRequested += ClaimedScanner_ReleaseDeviceRequested;
            _claimedScanner.IsDecodeDataEnabled = true; // Decode raw data from scanner and sends the ScanDataLabel and ScanDataType in the DataReceived event.
            await _claimedScanner.EnableAsync(); // Scanner must be enabled in order to receive the DataReceived event.
        }

        private void GetConfiguration() {
            IEnumerable<String> scannerID = from xe in XElement.Load("TestExecutive.config.xml").Elements("SerialNumberDialog") select xe.Element("BarCodeScannerID").Value;
            _scannerID = scannerID.First();
            IEnumerable<String> regEx = from xe in XElement.Load("TestExecutive.config.xml").Elements("SerialNumberDialog") select xe.Element("SerialNumberRegEx").Value;
            _regEx = regEx.First();
        }

        private void ClaimedScanner_ReleaseDeviceRequested(Object sender, ClaimedBarcodeScanner e) { e.RetainDevice(); } // Mine, don't touch!  Prevent other apps claiming scanner.

        private void ClaimedScanner_ErrorOccurred(ClaimedBarcodeScanner sender, BarcodeScannerErrorOccurredEventArgs args) { _ = MessageBox.Show("ErrorOccurred!", "ErrorOccurred!", MessageBoxButtons.OK); }

        private void ClaimedScanner_DataReceived(ClaimedBarcodeScanner sender, BarcodeScannerDataReceivedEventArgs args) { Invoke(new DataReceived(DelegateMethod), args); }

        private delegate void DataReceived(BarcodeScannerDataReceivedEventArgs args);

        private void DelegateMethod(BarcodeScannerDataReceivedEventArgs args) {
            if (args.Report.ScanDataLabel == null) return;
            FormUpdate(CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, args.Report.ScanDataLabel));
        }

        private void OK_Clicked(Object sender, EventArgs e) { DialogResult = DialogResult.OK; }

        private void Cancel_Clicked(Object sender, EventArgs e) { DialogResult = DialogResult.Cancel; }

        private void FormUpdate(String text) {
            BarCodeText.Text = text;
            if (Regex.IsMatch(text, _regEx)) {
                OK.Enabled = true;
                OK.BackColor = System.Drawing.Color.Green;
            } else {
                OK.Enabled = false;
                OK.BackColor = System.Drawing.Color.DimGray;
            }
        }
    }
}
