namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces
{
    public interface ILog4NetLogger
    {
        void Debug(string msg);
        void Error(string msg);
        void Info(string msg);
        void Warn(string msg);
    }
}