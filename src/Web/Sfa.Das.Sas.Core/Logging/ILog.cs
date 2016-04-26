using System;
using System.Collections.Generic;

namespace Sfa.Das.Sas.Core.Logging
{
    public interface ILog
    {
        void Info(string msg);

        void Info(string msg, Dictionary<string, object> properties);

        void Debug(string msg);

        void Debug(string msg, Dictionary<string, object> properties);

        void Warn(string msg);

        void Error(string msg);

        void Error(Exception ex, string msg);

        void Fatal(string msg);
    }
}
