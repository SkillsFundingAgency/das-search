using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights.NLogTarget;
using NLog;
using NLog.Targets.ElasticSearch;
using Sfa.Das.Sas.Indexer.Core.Logging;
using Sfa.Das.Sas.Indexer.Core.Logging.Models;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Services
{
    using System.Dynamic;

    using Newtonsoft.Json;

    public class NLogService : ILog
    {
        private readonly string _loggerType;
#pragma warning disable CS0169
#pragma warning disable S1144 // Unused private types or members should be removed
        private ElasticSearchTarget _dummy; // Reference so assembly is copied to Primary output.
        private ApplicationInsightsTarget _dummy2; // Reference so assembly is copied to Primary output.
#pragma warning restore S1144 // Unused private types or members should be removed
#pragma warning disable CS0169

        public NLogService(Type loggerType)
        {
            _loggerType = loggerType?.ToString() ?? "DefaultIndexLogger";
        }

        public void Debug(object message)
        {
            SendLog(message, LogLevel.Debug);
        }

        public void Debug(string message, Dictionary<string, object> properties)
        {
            SendLog(message, LogLevel.Debug, properties);
        }

        public void Debug(string message, ILogEntry entry)
        {
            SendLog(message, LogLevel.Debug, new Dictionary<string, object> { { entry.Name, entry } });
        }

        public void Info(string message, ILogEntry entry)
        {
            SendLog(message, LogLevel.Info, new Dictionary<string, object> { { entry.Name, entry } });
        }

        public void Info(object message)
        {
            SendLog(message, LogLevel.Info);
        }

        public void Info(string message, Dictionary<string, object> properties)
        {
            SendLog(message, LogLevel.Info, properties);
        }

        public void Warn(object message)
        {
            SendLog(message, LogLevel.Warn);
        }

        public void Warn(string message, Dictionary<string, object> properties)
        {
            SendLog(message, LogLevel.Warn, properties);
        }

        public void Warn(string message, ILogEntry entry)
        {
            SendLog(message, LogLevel.Warn, new Dictionary<string, object> { { entry.Name, entry } });
        }

        public void Warn(Exception exception, object message)
        {
            SendLog(message, LogLevel.Warn, exception);
        }

        public void Error(Exception exception, object message)
        {
            SendLog(message, LogLevel.Error, exception);
        }

        public void Error(string message, ILogEntry entry)
        {
            SendLog(message, LogLevel.Error, new Dictionary<string, object> { { entry.Name, entry } });
        }

        public void Fatal(Exception exception, object message)
        {
            SendLog(message, LogLevel.Fatal, exception);
        }

        public void Fatal(string message, ILogEntry entry)
        {
            SendLog(message, LogLevel.Fatal, new Dictionary<string, object> { { entry.Name, entry } });
        }

        private void SendLog(object message, LogLevel level, Exception exception = null)
        {
            SendLog(message, level, new Dictionary<string, object>(), exception);
        }

        private void SendLog(object message, LogLevel level, Dictionary<string, object> properties, Exception exception = null)
        {
            var propertiesLocal = new Dictionary<string, object>();
            if (properties != null)
            {
                propertiesLocal = new Dictionary<string, object>(properties);
            }

            propertiesLocal.Add("Application", "Sfa.Das.Indexer");
            propertiesLocal.Add("LoggerType", _loggerType);

            var logEvent = new LogEventInfo(level, _loggerType, message.ToString());

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
