using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.NLogTarget;
using NLog;
using NLog.Config;
using NLog.Targets.ElasticSearch;
using Sfa.Das.ApprenticeshipInfoService.Core.Configuration;
using Sfa.Das.ApprenticeshipInfoService.Core.Logging;

namespace Sfa.Das.ApprenticeshipInfoService.Infrastructure.Logging
{
    public class NLogLogger : ILog
    {
        private readonly IConfigurationSettings _settings;

        private readonly string _loggerType;
        private readonly IRequestContext _context;
#pragma warning disable S1144, 0169// Unused private types or members should be removed
        private ElasticSearchTarget dummy; // Reference so assembly is copied to Primary output.
        private ApplicationInsightsTarget dummy2; // Reference so assembly is copied to Primary output.
        //private AzureEventHubTarget dummy3; // Reference so assembly is copied to Primary output.
#pragma warning restore S1144, 0169 // Unused private types or members should be removed

        public NLogLogger(Type loggerType, IConfigurationSettings settings, IRequestContext context)
        {
            _settings = settings;
            _loggerType = loggerType?.ToString() ?? "DefaultWebLogger";
            _context = context;
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

        public void Info(string msg, ILogEntry logEntry)
        {
            SendLog(msg, LogLevel.Info, new Dictionary<string, object> { { GetLogEntryName(logEntry), logEntry } });
        }

        public void Debug(string msg)
        {
            SendLog(msg, LogLevel.Debug);
        }

        public void Debug(string msg, Dictionary<string, object> properties)
        {
            SendLog(msg, LogLevel.Debug, properties);
        }

        public void Debug(string msg, ILogEntry logEntry)
        {
            SendLog(msg, LogLevel.Debug, new Dictionary<string, object> { { GetLogEntryName(logEntry), logEntry } });
        }

        public void Warn(string msg)
        {
            SendLog(msg, LogLevel.Warn);
        }

        public void Warn(string msg, ILogEntry logEntry)
        {
            SendLog(msg, LogLevel.Warn, new Dictionary<string, object> { { GetLogEntryName(logEntry), logEntry } });
        }

        public void Error(string msg, Dictionary<string, object> properties)
        {
            SendLog(msg, LogLevel.Error, properties);
        }

        public void Error(string msg, ILogEntry logEntry)
        {
            SendLog(msg, LogLevel.Error, new Dictionary<string, object> { { GetLogEntryName(logEntry), logEntry } });
        }

        public void Error(Exception ex, string msg)
        {
            SendLog(msg, LogLevel.Error, ex);
        }

        public void Fatal(Exception ex, string msg)
        {
            SendLog(msg, LogLevel.Fatal, ex);
        }

        public void Fatal(string msg, ILogEntry logEntry)
        {
            SendLog(msg, LogLevel.Fatal, new Dictionary<string, object> { { GetLogEntryName(logEntry), logEntry } });
        }

        private static string GetLogEntryName(ILogEntry logEntry)
        {
            return logEntry.GetType().Name.Replace("LogEntry", string.Empty);
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

            propertiesLocal.Add("Application", _settings.ApplicationName);
            propertiesLocal.Add("Environment", _settings.EnvironmentName);
            propertiesLocal.Add("LoggerType", _loggerType);
            propertiesLocal.Add("RequestCtx", _context);

            var logEvent = new LogEventInfo(level, _loggerType, msg.ToString());

            if (exception != null)
            {
                propertiesLocal.Add("Exception", new { message = exception.Message, source = exception.Source, innerException = exception.InnerException, stackTrace = exception.StackTrace });
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
