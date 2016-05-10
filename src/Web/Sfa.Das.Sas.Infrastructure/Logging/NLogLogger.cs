using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights.NLogTarget;
using NLog;
using NLog.Config;
using NLog.Targets.ElasticSearch;
using Sfa.Das.Sas.Core.Logging;

namespace Sfa.Das.Sas.Infrastructure.Logging
{
    using System.Dynamic;

    using Newtonsoft.Json;

    public class NLogLogger : ILog
    {
        private readonly string _loggerType;
#pragma warning disable S1144, 0169// Unused private types or members should be removed
        private ElasticSearchTarget dummy; // Reference so assembly is copied to Primary output.
        private ApplicationInsightsTarget dummy2; // Reference so assembly is copied to Primary output.
#pragma warning restore S1144, 0169 // Unused private types or members should be removed

        public NLogLogger(Type loggerType)
        {
            _loggerType = loggerType?.ToString() ?? "DefaultWebLogger";
        }

        public LoggingConfiguration LoggingConfiguration { get; set; }

        public void Info(string msg)
        {
            SendLog(msg, LogLevel.Info);
        }

        public void Info(string msg, Dictionary<string, object> properties)
        {
            SendLog(msg, LogLevel.Info, properties);
        }

        public void Debug(string msg)
        {
            SendLog(msg, LogLevel.Debug);
        }

        public void Debug(string msg, Dictionary<string, object> properties)
        {
            SendLog(msg, LogLevel.Debug, properties);
        }

        public void Warn(string msg)
        {
            SendLog(msg, LogLevel.Warn);
        }

        public void Error(Exception ex, string msg)
        {
            SendLog(msg, LogLevel.Error, ex);
        }

        public void Fatal(Exception ex, string msg)
        {
            SendLog(msg, LogLevel.Fatal, ex);
        }

        private void SendLog(object message, LogLevel level, Exception exception = null)
        {
            SendLog(message, level, new Dictionary<string, object>(), exception);
        }

        private void SendLog(object msg, LogLevel level, Dictionary<string, object> properties, Exception exception = null)
        {
            var propertiesLocal = new Dictionary<string, object>();
            if (properties != null)
            {
                propertiesLocal = new Dictionary<string, object>(properties);
            }

            propertiesLocal.Add("Application", "Sfa.Das.Web");
            propertiesLocal.Add("LoggerType", _loggerType);

            var logEvent = new LogEventInfo(level, _loggerType, msg.ToString());

            if (exception != null)
            {
                propertiesLocal.Add("application_exception", JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(exception)));
            }

            foreach (var property in propertiesLocal)
            {
                logEvent.Properties[property.Key] = property.Value;
            }

            ILogger log = LogManager.GetCurrentClassLogger();
            log.Log(logEvent);
        }
    }
}
