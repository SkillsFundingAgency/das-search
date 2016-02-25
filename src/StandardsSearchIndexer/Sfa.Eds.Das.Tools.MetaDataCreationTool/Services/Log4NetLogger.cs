namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using log4net;

    using Sfa.Eds.Das.Indexer.Common.Configuration;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces;

    public sealed class Log4NetLogger : ILog4NetLogger
    {
        private readonly ILog logger;

        public Log4NetLogger()
        {
            logger = LogManager.GetLogger(Log4NetSettings.LoggerName);
        }

        public void Debug(string msg)
        {
            logger.Debug(msg);
        }

        public void Info(string msg)
        {
            logger.Info(msg);
        }

        public void Warn(string msg)
        {
            logger.Warn(msg);
        }

        public void Error(string msg)
        {
            logger.Error(msg);
        }
    }
}