using System.Diagnostics;

namespace Pen.Logging;

public readonly struct LogMessage
{
    public readonly TraceEventType EventType;
    public readonly string Message;
    public readonly object[] Args;

    public LogMessage(TraceEventType eventType, string message, params object[] args)
    {
        EventType = eventType;
        Message = message;
        Args = args;
    }
}