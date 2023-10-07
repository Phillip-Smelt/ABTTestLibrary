﻿//*********************************************************
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
    public partial class SerialNumber : Form {
        private BarcodeScanner _scanner = null;
        private ClaimedBarcodeScanner _claimedScanner = null;

        public SerialNumber() {
            InitializeComponent();
            GetBarcodeScanner();
            BarCodeText.Text = String.Empty;
            OK.Enabled = false; OK.BackColor = System.Drawing.Color.DimGray;
            Cancel.Enabled = true;
        }

        private async void GetBarcodeScanner() {
            _scanner = await SerialNumberHelper.GetFirstBarcodeScannerAsync();
            if (_scanner == null) throw new InvalidOperationException("Barcode scanner not found.");
            _claimedScanner = await _scanner.ClaimScannerAsync(); // Claim exclusively & enable.
            if (_claimedScanner == null) throw new InvalidOperationException("Barcode scanner not found.");
            _claimedScanner.ReleaseDeviceRequested += ClaimedScanner_ReleaseDeviceRequested;
            _claimedScanner.DataReceived += ClaimedScanner_DataReceived;
            _claimedScanner.IsDecodeDataEnabled = true; // Decode raw data from scanner and sends the ScanDataLabel and ScanDataType in the DataReceived event.
            await _claimedScanner.EnableAsync(); // Scanner must be enabled in order to receive the DataReceived event.
        }

        private void ClaimedScanner_ReleaseDeviceRequested(Object sender, ClaimedBarcodeScanner e) { e.RetainDevice(); } // Mine!  Don't touch!  Disallow other apps claiming scanner.

        private void ClaimedScanner_DataReceived(ClaimedBarcodeScanner sender, BarcodeScannerDataReceivedEventArgs args) { Invoke(new SetBarCodeText(DelegateMethod), args); }

        private delegate void SetBarCodeText(BarcodeScannerDataReceivedEventArgs args);

        private void DelegateMethod(BarcodeScannerDataReceivedEventArgs args) { if (Regex.IsMatch(ScanDataLabelGet(args), "^01BB2-[0-9]{5}$")) BarCodeText.Text = ScanDataLabelGet(args); }

        private String ScanDataLabelGet(BarcodeScannerDataReceivedEventArgs args) {
            if (args.Report.ScanDataLabel == null || args.Report.ScanDataType != BarcodeSymbologies.Code39) return String.Empty;
            OK.Enabled = true; OK.BackColor = System.Drawing.Color.Green;
            return CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, args.Report.ScanDataLabel);
        }

        public void OK_Clicked(Object sender, EventArgs e) {
            BarCodeText.Text = String.Empty;
            OK.Enabled = false; OK.BackColor = System.Drawing.Color.DimGray;
        }

        public void Cancel_Clicked(Object sender, EventArgs e) { Close(); }
    }

    internal class SerialNumberHelper {
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

