using System.Configuration;
using System.Net;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Sfa.Eds.Das.Indexer.Common.Configuration
{
    public static class Log4NetSettings
    {
        public static string LoggerName => "MainLogger";

        public static void Initialise(string text = "")
        {
            var logserver = ConfigurationManager.AppSettings["ElasticServerIp"];
            var logstashport = 5960;

            var appender = CreateUdpAppender("UdpAppender", logserver, logstashport);
            AddAppender(LoggerName, appender);

            var logger = LogManager.GetLogger(LoggerName);
            logger.Info($"Log appender added: {logserver} - {text}");
        }

        private static void AddAppender(string loggerName, IAppender appender)
        {
            ILog log = LogManager.GetLogger(loggerName);
            var l = (Logger)log.Logger;

            l.Hierarchy.Root.AddAppender(appender);
            l.AddAppender(appender);
        }

        private static IAppender CreateUdpAppender(string name, string remoteip, int port)
        {
            var patternLayout = new PatternLayout { ConversionPattern = "%-5level %logger [%property{NDC}] - %message%newline" };
            patternLayout.ActivateOptions();

            var appender = new UdpAppender
            {
                Name = name,
                RemoteAddress = IPAddress.Parse(remoteip),
                RemotePort = port,
                Layout = patternLayout
            };
            appender.ActivateOptions();

            return appender;
        }
    }
}
