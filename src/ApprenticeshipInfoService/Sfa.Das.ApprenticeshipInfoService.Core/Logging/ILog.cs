using System;
using System.Collections.Generic;

namespace Sfa.Das.ApprenticeshipInfoService.Core.Logging
{
    public interface ILog
    {
        void Info(string msg);

        void Info(string msg, Dictionary<string, object> properties);

        void Info(string msg, ILogEntry logEntry);

        void Debug(string msg);

        void Debug(string msg, Dictionary<string, object> properties);

        void Debug(string msg, ILogEntry logEntry);

        void Warn(string msg);

        void Warn(string msg, ILogEntry logEntry);

        void Error(Exception ex, string msg);

        void Error(string msg, Dictionary<string, object> properties);

        void Error(string msg, ILogEntry logEntry);

        void Fatal(Exception ex, string msg);

        void Fatal(string msg, ILogEntry logEntry);
    }
}
