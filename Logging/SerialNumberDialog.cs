//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Enumeration;
using Windows.Devices.PointOfService;
using Windows.Security.Cryptography;

namespace ABT.TestSpace.TestExec.Logging {
    public partial class SerialNumberDialog : Form {
        // TODO: Convert to Singleton, like USB_TO_GPIO.  Also, debug on Windows 10.
        // NOTE: SerialNumberDialog derived from https://learn.microsoft.com/en-us/samples/microsoft/windows-universal-samples/barcodescanner/.  Thanks Bill!
        // NOTE: SerialNumberDialog tested with Honeywell Voyager 1200G USB Barcode Scanner:
        //  - Works fine in:   Windows 11 Professional,    Version 22H2, OS Build 22621.2361, Windows Feature Experience Pack 1000.22674.1000.0.
        //  - Doesn't work in: Windows 10 Enterprise,      Version 22H2, OS Build 19045.3570, Windows Feature Experience Pack 1000.19052.1000.0.
        //  - Doesn't work in: Windows 10 Enterprise LTSC, Version 1809, OS Build 17763.4974, no Windows Feature Experience Pack listed.
        //      - ClaimedBarcodeScanner Events don't fire in Windows 10 Enterprise, suspect same issue as
        //      - https://learn.microsoft.com/en-us/answers/questions/820762/c-claimedbarcodescanner-events-not-firing-in-windo?orderBy=Newest.
        // NOTE: Honeywell Voyager 1200G USB Barcode Scanner is a Microsoft supported Point of Service peripheral.
        //  - https://learn.microsoft.com/en-us/windows/uwp/devices-sensors/pos-device-support
        // NOTE: Honeywell Voyager 1200G Scanner must be programmed into USB HID mode to work correctly with TestExecutive to read ABT Serial #s.
        //       - Scan PAP131 label from "Honeywell Voyager 1200G User's Guide ReadMe.pdf" to program 1200 into USB HID mode.
        //       - Both "ReadMe" & "User's Guide reside in this folder for convenience.
        // NOTE: Voyager 1200G won't scan ABT Serial #s into Notepad/Wordpad/Text Editor of Choice when in USB HID mode:
        //       - It will only deliver scanned data to a USB HID application like TestExecutive's SerialNumberDialog class.
        //       - You must either scan the Voyager 1200G's  DEFALT or PAP124 barcodes to restore "normal" scanning.
        // NOTE: The 1200G must also be programmed to read the Barcode Symbology of ABT's Serial #s, which at the time of this writing is Code39.
        private BarcodeScanner _scanner = null;
        private ClaimedBarcodeScanner _claimedScanner = null;

        public SerialNumberDialog(String InitialSerialNumber) {
            InitializeComponent();
            GetBarcodeScanner();
            FormUpdate(InitialSerialNumber);
        }

        public String Get() { return BarCodeText.Text; }

        private async void GetBarcodeScanner() {
            _scanner = await GetFirstBarcodeScannerAsync();
            if (_scanner == null) throw new InvalidOperationException("Barcode scanner not found.");
            _claimedScanner = await _scanner.ClaimScannerAsync(); // Claim exclusively.
            if (_claimedScanner == null) throw new InvalidOperationException("Barcode scanner not found.");
            _claimedScanner.ReleaseDeviceRequested += ClaimedScanner_ReleaseDeviceRequested;
            _claimedScanner.DataReceived += ClaimedScanner_DataReceived;
            _claimedScanner.ErrorOccurred += ClaimedScanner_ErrorOccurred;
            _claimedScanner.IsDecodeDataEnabled = true; // Decode raw data from scanner and sends the ScanDataLabel and ScanDataType in the DataReceived event.
            await _claimedScanner.EnableAsync(); // Scanner must be enabled in order to receive the DataReceived event.
        }

        private void ClaimedScanner_ReleaseDeviceRequested(Object sender, ClaimedBarcodeScanner e) { e.RetainDevice(); } // Mine, don't touch!  Prevent other apps claiming scanner.

        private void ClaimedScanner_ErrorOccurred(ClaimedBarcodeScanner sender, BarcodeScannerErrorOccurredEventArgs args) {
            _ = MessageBox.Show("ErrorOccurred!", "ErrorOccurred!", MessageBoxButtons.OK);
        }

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
            if (Regex.IsMatch(text, "^01BB2-[0-9]{5}$")) {
                OK.Enabled = true;
                OK.BackColor = System.Drawing.Color.Green;
            } else {
                OK.Enabled = false;
                OK.BackColor = System.Drawing.Color.DimGray;
            }
        }

        private static async Task<BarcodeScanner> GetFirstBarcodeScannerAsync(PosConnectionTypes connectionTypes = PosConnectionTypes.Local) {
            return await GetFirstDeviceAsync(BarcodeScanner.GetDeviceSelector(connectionTypes), async (id) => await BarcodeScanner.FromIdAsync(id));
        }

        private static async Task<T> GetFirstDeviceAsync<T>(String selector, Func<String, Task<T>> convertAsync) where T : class {
            TaskCompletionSource<T> completionSource = new TaskCompletionSource<T>();
            List<Task> pendingTasks = new List<Task>();
            DeviceWatcher watcher = DeviceInformation.CreateWatcher(selector);

            watcher.Added += (DeviceWatcher sender, DeviceInformation device) => {
                pendingTasks.Add(((Func<String, Task>)(async (id) => {
                    T t = await convertAsync(id);
                    if (t != null) completionSource.TrySetResult(t);
                }))(device.Id));
            };

            watcher.EnumerationCompleted += async (DeviceWatcher sender, Object args) => {
                await Task.WhenAll(pendingTasks);
                completionSource.TrySetResult(null);
            };

            watcher.Removed += (DeviceWatcher sender, DeviceInformationUpdate args) => { }; // Event must be "handled" to enable realtime updates; empty block suffices.
            watcher.Updated += (DeviceWatcher sender, DeviceInformationUpdate args) => { }; // Ditto.
            watcher.Start();
            T result = await completionSource.Task;
            watcher.Stop();
            return result;
        }
    }
}

