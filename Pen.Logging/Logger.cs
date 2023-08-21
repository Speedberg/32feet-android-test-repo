using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Collections.Generic;

namespace Pen.Logging;

//TODO make this non-static
public static class Logger
{
    private const string SOURCE_NAME = "Pen";

    private static TraceSource _source;
    private static ILogger _logger;
    private static Queue<LogMessage> _queue;

    private static bool _useSingleLogger;
    private static bool _enabled;

    private static int _id;
    private static int _nextID
    {
        get
        {
            return _id++;
        }
    }

    static Logger()
    {
        _queue = new Queue<LogMessage>();
        _useSingleLogger = false;
        _enabled = false;
    }

    public static void Initialize(SourceLevels levels, params TraceListener[] traceListeners)
    {
        _source = new TraceSource(SOURCE_NAME);
        _source.Switch.Level = levels;
        _source.Listeners.AddRange(traceListeners);

        _useSingleLogger = false;
        _enabled = true;

        Trace.AutoFlush = true;

        //Empty message queue
        Debug("Dispatching messages from message queue...");
        emptyMessageQueue();
        Debug("Messages dispatched from message queue successfully!");
    }

    public static void Initialize(ILogger logger)
    {
        _logger = logger;
        _enabled = true;
        _useSingleLogger = true;

        //Empty message queue
        Debug("Dispatching messages from message queue...");
        emptyMessageQueue();
        Debug("Messages dispatched from message queue successfully!");
    }

    private static void emptyMessageQueue()
    {
        IEnumerator<LogMessage> msgQueue = _queue.GetEnumerator();
        while(msgQueue.MoveNext())
        {
            log(msgQueue.Current.EventType, msgQueue.Current.Message);
        }
    }

    private static void log(TraceEventType type, string message)
    {
        if(!_enabled)
        {
            _queue.Enqueue(new LogMessage(type,message));
            return;
        }

        if(_useSingleLogger)
        {
            _logger.Log(new LogMessage(type, message));
            return;
        }

        _source.TraceEvent(type, _nextID, message);
    }

    private static void log(TraceEventType type, string message, params object[] args)
    {
        if(!_enabled)
        {
            _queue.Enqueue(new LogMessage(type,message, args));
            return;
        }

        if(_useSingleLogger)
        {
            _logger.Log(new LogMessage(type, message, args));
            return;
        }

        _source.TraceEvent(type, _nextID, message, args);
    }

    public static void Debug(string message)
    {
        log(TraceEventType.Verbose, message);
    }

    public static void Log(string message)
    {
        log(TraceEventType.Information, message);
    }

    public static void Warn(string message)
    {
        log(TraceEventType.Warning, message);
    }

    public static void Error(string message)
    {
        log(TraceEventType.Error, message);
    }

    public static void Fatal(string message)
    {
        log(TraceEventType.Critical, message);
    }

    public static void Debug(string message, params object[] args)
    {
        log(TraceEventType.Verbose, message, args);
    }

    public static void Log(string message, params object[] args)
    {
        log(TraceEventType.Information, message, args);
    }

    public static void Warn(string message, params object[] args)
    {
        log(TraceEventType.Warning, message, args);
    }

    public static void Error(string message, params object[] args)
    {
        log(TraceEventType.Error, message, args);
    }

    public static void Fatal(string message, params object[] args)
    {
        log(TraceEventType.Critical, message, args);
    }

    public static void Dispose()
    {
        _source.Close();
    }
}