using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace Enhima.Logging
{
    public class ConsoleLoggerFactory : ILoggerFactory
    {
        private readonly IEnumerable<string> _loggersToPrint;

        public ConsoleLoggerFactory()
        {
        }

        public ConsoleLoggerFactory(IEnumerable<string> loggersToPrint)
        {
            _loggersToPrint = loggersToPrint;
        }

        public IInternalLogger LoggerFor(string keyName)
        {
            if(_loggersToPrint == null)
            {
                return new ConsoleLogger(keyName);
            }
            if(_loggersToPrint.Any(logger => logger == keyName))
            {
                return new ConsoleLogger(keyName);
            }
            return new NoLoggingInternalLogger();
        }

        public IInternalLogger LoggerFor(Type type)
        {
            return LoggerFor(type.Name);
        }
    }
}