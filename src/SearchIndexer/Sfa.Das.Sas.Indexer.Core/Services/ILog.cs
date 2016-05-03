using System;
using System.Collections.Generic;

namespace Sfa.Das.Sas.Indexer.Core.Services
{
    public interface ILog
    {
        void Debug(object message);

        void Debug(string message, Dictionary<string, object> properties);

        void Info(string message, Dictionary<string, object> properties);

        void Info(object message);

        void Warn(object message);

        void Warn(Exception exception, object message);

        void Error(Exception exception, object message);

        void Fatal(Exception exception, object message);
    }
}