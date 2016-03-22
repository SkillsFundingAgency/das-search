namespace Sfa.Infrastructure.Services
{
    using System;
    using Eds.Das.Indexer.Core.Services;
    using Microsoft.ApplicationInsights.NLogTarget;
    using NLog;
    using NLog.Targets.ElasticSearch;

    public class NLogService : ILog
    {
        private readonly Logger _logger;
#pragma warning disable CS0169
#pragma warning disable S1144 // Unused private types or members should be removed
        private ElasticSearchTarget _dummy; // Reference so assembly is copied to Primary output.
        private ApplicationInsightsTarget _dummy2; // Reference so assembly is copied to Primary output.
#pragma warning restore S1144 // Unused private types or members should be removed
#pragma warning disable CS0169

        public NLogService(Type loggerType)
        {
            _logger = LogManager.GetLogger(loggerType?.ToString() ?? "NullIndexLogger");
        }

        public void Debug(object message)
        {
            _logger.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            _logger.Debug(exception, message.ToString());
        }

        public void DebugFormat(string format, params object[] args)
        {
            _logger.Debug(format, args);
        }

        public void Info(object message)
        {
            _logger.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            _logger.Info(exception, message.ToString());
        }

        public void InfoFormat(string format, params object[] args)
        {
            _logger.Info(format, args);
        }

        public void Warn(object message)
        {
            _logger.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            _logger.Warn(exception, message.ToString());
        }

        public void WarnFormat(string format, params object[] args)
        {
            _logger.Warn(format, args);
        }

        public void Error(object message)
        {
            _logger.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            _logger.Error(exception, message.ToString());
        }

        public void ErrorFormat(string format, params object[] args)
        {
            _logger.Error(format, args);
        }

        public void Fatal(object message)
        {
            _logger.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            _logger.Fatal(exception, message.ToString());
        }

        public void FatalFormat(string format, params object[] args)
        {
            _logger.Fatal(format, args);
        }
    }
}
