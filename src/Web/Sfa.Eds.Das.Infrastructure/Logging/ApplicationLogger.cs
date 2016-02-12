namespace Sfa.Eds.Das.Infrastructure.Logging
{
    using log4net;

    using Sfa.Eds.Das.Core.Logging;

    public class ApplicationLogger : IApplicationLogger
    {
        private ILog _logger;

        public ApplicationLogger()
        {
            this._logger = LogManager.GetLogger(Log4NetSettings.LoggerName);
        }

        public void Debug(string msg)
        {
            this._logger.Debug(msg);
        }

        public void Info(string msg)
        {
            this._logger.Info(msg);
        }

        public void Warn(string msg)
        {
            this._logger.Warn(msg);
        }

        public void Error(string msg)
        {
            this._logger.Error(msg);
        }

        

    }
}