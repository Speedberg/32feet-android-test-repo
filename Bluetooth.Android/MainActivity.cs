using Android.App;
using Android.Content.PM;
using Avalonia.Android;
using Android.OS;

using Pen.Logging;

using Manifest = Android.Manifest;

namespace Bluetooth.Android;

[Activity(Label = "Bluetooth.Android", Theme = "@style/MyTheme.NoActionBar", Icon = "@drawable/icon", LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
public class MainActivity : AvaloniaMainActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        InTheHand.AndroidActivity.CurrentActivity = this;

        base.OnCreate(savedInstanceState);

        RequestPermissions(null, null);
    }

    public void RequestPermissions(object sender, System.EventArgs args)
    {
        Logger.Debug("Requesting permissions...");
        //https://github.com/inthehand/32feet/issues/194
        //https://github.com/inthehand/32feet/issues/185
        string[] permissions = new string[] {Manifest.Permission.Bluetooth, Manifest.Permission.BluetoothAdmin,
        Manifest.Permission.BluetoothConnect, Manifest.Permission.BluetoothAdvertise, Manifest.Permission.BluetoothScan,
        Manifest.Permission.BluetoothPrivileged,
        
        Manifest.Permission.AccessBackgroundLocation,
        Manifest.Permission.AccessFineLocation,
        Manifest.Permission.AccessCoarseLocation};

        foreach(string perm in permissions)
        {
            Logger.Debug("{0}",CheckSelfPermission(perm));
        }
        RequestPermissions(permissions,77);
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
        Logger.Debug("OnRequestPermissionsResult: {0}, {1}, {2}", requestCode, permissions, grantResults);
        for (int i = 0; i < permissions.Length; i++)
        {
            Logger.Debug("Permission: {0} Granted?: {1}", permissions[i], grantResults[i]);
        }
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }
}
