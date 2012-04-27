using System;
using NHibernate;

namespace Enhima.Logging
{
    public class ConsoleLogger : IInternalLogger
    {
        private class Level
        {
            public const string Warn = "Warn";
            public const string Debug = "Debug";
            public const string Info = "Info";
            public const string Error = "Error";
            public const string Fatal = "Fatal";
        }
        
        private readonly string _key;

        public ConsoleLogger(string key)
        {
            _key = key;
        }

        public void Error(object message)
        {
            Log(Level.Error, message);
        }

        public void Error(object message, Exception exception)
        {
            Log(Level.Error, message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            Log(Level.Error, format, args);
        }

        public void Fatal(object message)
        {
            Log(Level.Fatal, message);
        }

        public void Fatal(object message, Exception exception)
        {
            Log(Level.Fatal, message, exception);
        }

        public void Debug(object message)
        {
            Log(Level.Debug, message);
        }

        public void Debug(object message, Exception exception)
        {
            Log(Level.Debug, message, exception);
        }

        public void DebugFormat(string format, params object[] args)
        {
            Log(Level.Debug, format, args);
        }

        public void Info(object message)
        {
            Log(Level.Info, message);
        }

        public void Info(object message, Exception exception)
        {
            Log(Level.Info, message, exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            Log(Level.Info, format, args);
        }

        public void Warn(object message)
        {
            Log(Level.Warn, message);
        }

        public void Warn(object message, Exception exception)
        {
            Log(Level.Warn, message, exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            Log(Level.Warn, format, args);
        }

        public bool IsErrorEnabled
        {
            get { return true; }
        }

        public bool IsFatalEnabled
        {
            get { return true; }
        }

        public bool IsDebugEnabled
        {
            get { return true; }
        }

        public bool IsInfoEnabled
        {
            get { return true; }
        }

        public bool IsWarnEnabled
        {
            get { return true; }
        }

        private void Log(string level, object message)
        {
            Console.WriteLine("{0}: [{1}] {2}", _key, level, message);
        }

        private void Log(string level, object message, Exception exception)
        {
            Console.WriteLine("{0}: [{1}] {2} {3} {4}", _key, level, message, Environment.NewLine, exception);
        }

        public void Log(string level, string format, params object[] args)
        {
            var message = string.Format(format, args);
            Console.WriteLine("{0}: [{1}] {2} {3}", _key, level, message);
        }
    }
}