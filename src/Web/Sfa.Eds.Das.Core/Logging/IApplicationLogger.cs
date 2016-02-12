namespace Sfa.Eds.Das.Core.Logging
{
    public interface IApplicationLogger
    {
        void Debug(string msg);
        void Info(string msg);
        void Warn(string msg);
        void Error(string msg);
    }
}
