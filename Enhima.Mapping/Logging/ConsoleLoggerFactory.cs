using System;
using NHibernate;

namespace Enhima.Logging
{
    public class ConsoleLoggerFactory : ILoggerFactory
    {
        public IInternalLogger LoggerFor(string keyName)
        {
            return new ConsoleLogger(keyName);
        }

        public IInternalLogger LoggerFor(Type type)
        {
            return LoggerFor(type.Name);
        }
    }
}