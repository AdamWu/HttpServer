using System;
using System.Diagnostics;
using System.IO;

public class Logger
{
    public enum LogLevel
    {
        Info = 0,
        Warning = 1,
        Error = 2
    }

    static Logger()
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\";
        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);
        string fileName = basePath + string.Format("Log-{0}.txt", DateTime.Now.ToString("yyyyMMdd-HH-mm-ss"));

        Trace.AutoFlush = true;
        Trace.Listeners.Clear();
        Trace.Listeners.Add(new ConsoleTraceListener());
        Trace.Listeners.Add(new TextWriterTraceListener(fileName));
    }

    public static void Error(string message)
    {
        Log(LogLevel.Error, message);
    }

    public static void Warning(string message)
    {
        Log(LogLevel.Warning, message);
    }

    public static void Info(string message)
    {
        Log(LogLevel.Info, message);
    }

    private static void Log(LogLevel type, string message)
    {
        Trace.WriteLine(string.Format("[{0}] {1} {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), type.ToString(), message));
    }
}