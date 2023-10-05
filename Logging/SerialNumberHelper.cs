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
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.PointOfService;

namespace ABT.TestSpace.Logging {
    internal class SerialNumberHelper {
        internal static async Task<BarcodeScanner> GetFirstBarcodeScannerAsync(PosConnectionTypes connectionTypes = PosConnectionTypes.Local) {
            return await GetFirstDeviceAsync(BarcodeScanner.GetDeviceSelector(connectionTypes), async (id) => await BarcodeScanner.FromIdAsync(id));
        }

        internal static async Task<T> GetFirstDeviceAsync<T>(String selector, Func<String, Task<T>> convertAsync)
            where T : class {
            TaskCompletionSource<T> completionSource = new TaskCompletionSource<T>();
            List<Task> pendingTasks = new List<Task>();
            DeviceWatcher watcher = DeviceInformation.CreateWatcher(selector);

            watcher.Added += (DeviceWatcher sender, DeviceInformation device) => {
                Func<String, Task> lambda = async (id) => {
                    T t = await convertAsync(id);
                    if (t != null) completionSource.TrySetResult(t);
                };
                pendingTasks.Add(lambda(device.Id));
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
