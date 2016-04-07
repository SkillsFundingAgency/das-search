using System;

namespace Sfa.Eds.Das.Core.Logging
{
    using System.Collections.Generic;

    public interface ILog
    {
        void Info(string msg);

        void Info(string msg, Dictionary<string, string> properties);

        void Debug(string msg);

        void Warn(string msg);

        void Error(string msg);

        void Error(Exception ex, string msg);

        void Fatal(string msg);
    }
}
