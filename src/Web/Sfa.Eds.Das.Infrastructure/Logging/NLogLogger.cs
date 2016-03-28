using System;
using Microsoft.ApplicationInsights.NLogTarget;
using NLog;
using NLog.Targets.ElasticSearch;
using Sfa.Eds.Das.Core.Logging;

namespace Sfa.Eds.Das.Infrastructure.Logging
{
    public class NLogLogger : ILog
    {
        private readonly ILogger _logger;
#pragma warning disable S1144, 0169// Unused private types or members should be removed
        private ElasticSearchTarget dummy; // Reference so assembly is copied to Primary output.
        private ApplicationInsightsTarget dummy2; // Reference so assembly is copied to Primary output.
#pragma warning restore S1144, 0169 // Unused private types or members should be removed

        public NLogLogger(Type loggerType)
        {
            _logger = LogManager.GetLogger(loggerType?.ToString() ?? "NullLogger");
        }

        public void Info(string msg)
        {
            _logger.Info(msg);
        }

        public void Debug(string msg)
        {
            _logger.Debug(msg);
        }

        public void Error(string msg)
        {
            _logger.Error(msg);
        }

        public void Error(Exception ex, string msg)
        {
            _logger.Error(ex, msg);
        }

        public void Warn(string msg)
        {
            _logger.Warn(msg);
        }

        public void Fatal(string msg)
        {
            _logger.Fatal(msg);
        }
    }
}
