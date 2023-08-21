using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using ReactiveUI;
using System;
using Pen.Logging;

namespace Bluetooth.ViewModels;

public class MainViewModel : ViewModelBase
{ 
    public string Greeting => "Welcome to Avalonia!";

    private bool _keepRunning;

    public MainViewModel()
    {
        
    }

    public async Task Disconnect()
    {
        _keepRunning = false;
    }

    public async Task Host()
    {
        if(_keepRunning)
            return;
        try
        {
            Logger.Debug("Starting listener...");
            BluetoothListener listener = new BluetoothListener(BluetoothService.SerialPort);
            BluetoothClient client = null;
            _keepRunning = true;
            listener.Start();

            string message = "Hello, world!";
            byte[] buffer = new byte[100];

            while(_keepRunning)
            {
                if(listener.Pending())
                {
                    client = listener.AcceptBluetoothClient();
                    //Note: this is never called on Android
                    Logger.Debug("Accepted client: {0}", client.RemoteMachineName);
                }

                if(client != null)
                {
                    if(client.Connected)
                    {
                        await client.GetStream().WriteAsync(System.Text.Encoding.UTF8.GetBytes(message));
                        //Note: this hangs / blocks on Android
                        int read = await client.GetStream().ReadAsync(buffer,0,buffer.Length);
                        
                        if(read > 0)
                        {
                            Logger.Debug("Data read: {0}", read);
                        }
                    }
                }
                await Task.Delay(10);
            }

            Logger.Debug("Stopping listener...");

            listener.Stop();
        } catch(System.Exception e)
        {
            Logger.Error("Server Error: {0}", e);
        }
    }

    public async Task Client()
    {
        if(_keepRunning)
            return;
        try
        {
            Logger.Debug("Picking device...");
            BluetoothDevicePicker picker = new BluetoothDevicePicker();
            var device = await picker.PickSingleDeviceAsync();
            if(device == null)
                return;
            Logger.Debug("Device chosen: {0}",device.DeviceAddress);

            BluetoothClient client = new BluetoothClient();
            Logger.Debug("Client attempting connection...");
            await client.ConnectAsync(device.DeviceAddress, BluetoothService.SerialPort);
            Logger.Debug("Client connection successful!");
            _keepRunning = true;

            string message = "Hello, world!";
            byte[] buffer = new byte[100];

            while(_keepRunning)
            {
                if(client.Connected)
                {
                    await client.GetStream().WriteAsync(System.Text.Encoding.UTF8.GetBytes(message));
                    //Note: this hangs / blocks on Android
                    int read = await client.GetStream().ReadAsync(buffer,0,buffer.Length);
                    
                    if(read > 0)
                    {
                        Logger.Debug("Data read: {0}", read);
                    }
                }
                await Task.Delay(10);
            }

            Logger.Debug("Client disconnecting...");
            client.Dispose();
        } catch(System.Exception e)
        {
            Logger.Error("Client Error: {0}", e);
        }
    }
}
