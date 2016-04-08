namespace Sfa.Eds.Das.Indexer.Core.Services
{
    using System;
    using System.Collections.Generic;

    public interface ILog
    {
        void Trace(object message);

        void Debug(object message);

        void Info(string message, Dictionary<string, object> properties);

        void Debug(object message, Exception exception);

        void Info(object message);

        void Info(object message, Exception exception);

        void Warn(object message);

        void Warn(object message, Exception exception);

        void Error(object message);

        void Error(object message, Exception exception);

        void Fatal(object message);

        void Fatal(object message, Exception exception);
    }
}