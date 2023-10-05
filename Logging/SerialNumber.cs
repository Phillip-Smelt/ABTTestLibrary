using ABT.TestSpace.Logging;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Windows.Devices.PointOfService;
using Windows.Security.Cryptography;

namespace ABT.TestSpace.TestExec.Logging {
    public partial class SerialNumber : Form {
        BarcodeScanner scanner = null;
        ClaimedBarcodeScanner claimedScanner = null;

        public SerialNumber() {
            InitializeComponent();
            GetBarcodeScanner();
            BarCodeText.Text = String.Empty;
            OK.Enabled = false; OK.BackColor = System.Drawing.Color.DimGray;
            Cancel.Enabled = true;
        }

        private async void GetBarcodeScanner() {
            scanner = await SerialNumberHelper.GetFirstBarcodeScannerAsync();
            if (scanner == null) throw new InvalidOperationException("Barcode scanner not found.");
            claimedScanner = await scanner.ClaimScannerAsync(); // Claim exclusively & enable.
            if (claimedScanner == null) throw new InvalidOperationException("Barcode scanner not found.");
            claimedScanner.ReleaseDeviceRequested += claimedScanner_ReleaseDeviceRequested;
            claimedScanner.DataReceived += claimedScanner_DataReceived;
            claimedScanner.IsDecodeDataEnabled = true; // Decode raw data from scanner and sends the ScanDataLabel and ScanDataType in the DataReceived event.
            await claimedScanner.EnableAsync(); // Scanner must be enabled in order to receive the DataReceived event.
        }

        private void claimedScanner_ReleaseDeviceRequested(Object sender, ClaimedBarcodeScanner e) { e.RetainDevice(); } // Mine!  Don't touch!  Disallow other apps claiming scanner.

        private void claimedScanner_DataReceived(ClaimedBarcodeScanner sender, BarcodeScannerDataReceivedEventArgs args) { Invoke(new SetBarCodeText(DelegateMethod), args); }

        private delegate void SetBarCodeText(BarcodeScannerDataReceivedEventArgs args);

        private void DelegateMethod(BarcodeScannerDataReceivedEventArgs args) { if (Regex.IsMatch(ScanDataLabelGet(args), "^01BB2-[0-9]{5}$")) BarCodeText.Text = ScanDataLabelGet(args); }

        private String ScanDataLabelGet(BarcodeScannerDataReceivedEventArgs args) {
            if (args.Report.ScanDataLabel == null || args.Report.ScanDataType != BarcodeSymbologies.Code39) return String.Empty;
            OK.Enabled = true; OK.BackColor = System.Drawing.Color.Green;
            return CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, args.Report.ScanDataLabel);
        }

        private void OK_Clicked(Object sender, EventArgs e) {
            BarCodeText.Text = String.Empty;
            OK.Enabled = false; OK.BackColor = System.Drawing.Color.DimGray;
        }

        private void Cancel_Clicked(Object sender, EventArgs e) { Close(); }
    }
}

