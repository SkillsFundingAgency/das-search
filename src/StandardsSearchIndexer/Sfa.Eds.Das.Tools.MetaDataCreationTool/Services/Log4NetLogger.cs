namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using log4net;

    using Sfa.Eds.Das.Indexer.Common.Configuration;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces;

    public sealed class Log4NetLogger : ILog4NetLogger
    {
        private readonly ILog _logger;

        public Log4NetLogger()
        {
            _logger = LogManager.GetLogger(Log4NetSettings.LoggerName);
        }

        public void Debug(string msg)
        {
            _logger.Debug(msg);
        }

        public void Info(string msg)
        {
            _logger.Info(msg);
        }

        public void Warn(string msg)
        {
            _logger.Warn(msg);
        }

        public void Error(string msg)
        {
            _logger.Error(msg);
        }
    }
}