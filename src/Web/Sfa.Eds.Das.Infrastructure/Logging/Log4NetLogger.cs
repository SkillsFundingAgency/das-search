namespace Sfa.Eds.Das.Infrastructure.Logging
{
    using log4net;

    public sealed class Log4NetLogger : Core.Logging.ILog
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