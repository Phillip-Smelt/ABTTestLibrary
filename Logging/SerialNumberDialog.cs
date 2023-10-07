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
        private static BarcodeScanner _scanner = null;
        private static ClaimedBarcodeScanner _claimedScanner = null;
        private static SerialNumberDialog _snd = null;

        private SerialNumberDialog() {
            InitializeComponent();
            GetBarcodeScanner();
            _snd.BarCodeText.Text = String.Empty;
        }

        public static String Get(String InitialSerialNumber) {
            _snd = new SerialNumberDialog();
            FormUpdate(InitialSerialNumber);
            DialogResult dr = _snd.ShowDialog(); // Modal Dialog, wait until operator clicks OK or Cancel buttons.
            String serialNumber;
            if (dr.Equals(DialogResult.OK)) serialNumber = _snd.BarCodeText.ToString();
            else serialNumber = String.Empty;
            _scanner.Dispose();
            _claimedScanner.Dispose();
            _snd.Dispose();
            return serialNumber;
        }

        private async void GetBarcodeScanner() {
            _scanner = await Device.GetFirstBarcodeScannerAsync();
            if (_scanner == null) throw new InvalidOperationException("Barcode scanner not found.");
            _claimedScanner = await _scanner.ClaimScannerAsync(); // Claim exclusively & enable.
            if (_claimedScanner == null) throw new InvalidOperationException("Barcode scanner not found.");
            _claimedScanner.ReleaseDeviceRequested += ClaimedScanner_ReleaseDeviceRequested;
            _claimedScanner.DataReceived += ClaimedScanner_DataReceived;
            _claimedScanner.IsDecodeDataEnabled = true; // Decode raw data from scanner and sends the ScanDataLabel and ScanDataType in the DataReceived event.
            await _claimedScanner.EnableAsync(); // Scanner must be enabled in order to receive the DataReceived event.
        }

        private void ClaimedScanner_ReleaseDeviceRequested(Object sender, ClaimedBarcodeScanner e) { e.RetainDevice(); } // Mine, don't touch!  Prevent other apps claiming scanner.

        private void ClaimedScanner_DataReceived(ClaimedBarcodeScanner sender, BarcodeScannerDataReceivedEventArgs args) { Invoke(new DataReceived(DelegateMethod), args); }

        private delegate void DataReceived(BarcodeScannerDataReceivedEventArgs args);

        private void DelegateMethod(BarcodeScannerDataReceivedEventArgs args) {
            if (args.Report.ScanDataLabel == null || args.Report.ScanDataType != BarcodeSymbologies.Code39) return;
            FormUpdate(CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, args.Report.ScanDataLabel));
        }

        private void OK_Clicked(Object sender, EventArgs e) { DialogResult = DialogResult.OK; }

        private void Cancel_Clicked(Object sender, EventArgs e) { DialogResult = DialogResult.Cancel; }

        private static void FormUpdate(String text) {
            if (Regex.IsMatch(text, "^01BB2-[0-9]{5}$")) {
                _snd.BarCodeText.Text = text;
                _snd.OK.Enabled = true;
                _snd.OK.BackColor = System.Drawing.Color.Green;
            }
            if (String.Equals(text, String.Empty)) {
                _snd.BarCodeText.Text = String.Empty;
                _snd.OK.Enabled = false;
                _snd.OK.BackColor = System.Drawing.Color.Red;
            }
        }
    }

    internal static class Device {
        internal static async Task<BarcodeScanner> GetFirstBarcodeScannerAsync(PosConnectionTypes connectionTypes = PosConnectionTypes.Local) {
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

