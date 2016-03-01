using System;

namespace Sfa.Eds.Das.Core.Logging
{
    public interface ILog
    {
        void Info(string msg);

        void Debug(string msg);

        void Warn(string msg);

        void Error(string msg);

        void Error(Exception ex, string msg);
    }
}
