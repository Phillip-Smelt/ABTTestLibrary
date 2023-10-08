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
            _claimedScanner = await _scanner.ClaimScannerAsync(); // Claim exclusively & enable.
            if (_claimedScanner == null) throw new InvalidOperationException("Barcode scanner not found.");
            _claimedScanner.ReleaseDeviceRequested += ClaimedScanner_ReleaseDeviceRequested;
            _claimedScanner.DataReceived += ClaimedScanner_DataReceived;
            _claimedScanner.ErrorOccurred += ClaimedScanner_ErrorOccurred;
            _claimedScanner.TriggerPressed += ClaimedScanner_TriggerPressed;
            _claimedScanner.TriggerReleased  += ClaimedScanner_TriggerReleased;
            _claimedScanner.IsDecodeDataEnabled = true; // Decode raw data from scanner and sends the ScanDataLabel and ScanDataType in the DataReceived event.
            await _claimedScanner.EnableAsync(); // Scanner must be enabled in order to receive the DataReceived event.
        }

        private void ClaimedScanner_ReleaseDeviceRequested(Object sender, ClaimedBarcodeScanner e) { e.RetainDevice(); } // Mine, don't touch!  Prevent other apps claiming scanner.

        private void ClaimedScanner_ErrorOccurred(ClaimedBarcodeScanner sender, BarcodeScannerErrorOccurredEventArgs args) {
            _ = MessageBox.Show("ErrorOccurred!", "ErrorOccurred!", MessageBoxButtons.OK);
        }

        private void ClaimedScanner_TriggerPressed(Object sender, ClaimedBarcodeScanner e) {
            _ = MessageBox.Show("TriggerPressed!", "TriggerPressed!", MessageBoxButtons.OK);
        }

        private void ClaimedScanner_TriggerReleased(Object sender, ClaimedBarcodeScanner e) {
            _ = MessageBox.Show("TriggerReleased !", "TriggerReleased !", MessageBoxButtons.OK);
        }

        private void ClaimedScanner_DataReceived(ClaimedBarcodeScanner sender, BarcodeScannerDataReceivedEventArgs args) {
            _ = MessageBox.Show("DataReceived!", "DataReceived!", MessageBoxButtons.OK);
            Invoke(new DataReceived(DelegateMethod), args);
        }

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

