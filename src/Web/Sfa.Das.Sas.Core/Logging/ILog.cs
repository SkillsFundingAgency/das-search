using System;
using System.Collections.Generic;

namespace Sfa.Das.Sas.Core.Logging
{
    public interface ILog
    {
        void Info(string msg);

        void Info(string msg, Dictionary<string, object> properties);

        void Debug(string msg);

        void Warn(string msg);

        void Debug(string msg, Dictionary<string, object> properties);

        void Error(Exception ex, string msg);

        void Error(string msg, Dictionary<string, object> properties);

        void Fatal(Exception ex, string msg);
    }
}
