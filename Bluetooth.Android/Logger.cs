using log = Android.Util.Log;
using Pen.Logging;
using System.Diagnostics;

namespace Pen.Android;

//TODO - //Avalonia.Logging.Logger.Sink
public class Logger : ILogger
{
    public void Log(LogMessage message)
    {
        switch(message.EventType)
        {
            case TraceEventType.Information:
                log.Info("Bluetooth", message.Message, message.Args);
                break;
            case TraceEventType.Warning:
                log.Warn("Bluetooth", message.Message, message.Args);
                break;
            case TraceEventType.Error:
                log.Error("Bluetooth", message.Message, message.Args);
                break;
            case TraceEventType.Critical:
                log.Wtf("Bluetooth", message.Message, message.Args);
                break;
            default:
            case TraceEventType.Verbose:
                log.Debug("Bluetooth", message.Message, message.Args);
                break;
        }
    }
}