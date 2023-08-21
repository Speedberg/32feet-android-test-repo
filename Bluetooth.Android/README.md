# Bluetooth.Android

Run: `dotnet build -t:Run -f net6.0-android`

For debugging using logcat:

```
cd "C:\Program Files (x86)\Android\android-sdk\platform-tools"
.\adb usb
.\adb devices
.\adb logcat -c
.\adb logcat ActivityManager:I Bluetooth:D *:S
```