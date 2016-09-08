namespace Sfa.Das.Sas.Indexer.Infrastructure.DapperBD
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using Core.Logging;
    using Core.Logging.Models;
    using Dapper;
    using Settings;

    public class DatabaseProvider : IDatabaseProvider
    {
        private readonly IInfrastructureSettings _infrastructureSettings;

        private readonly ILog _logger;

        public DatabaseProvider(IInfrastructureSettings infrastructureSettings, ILog logger)
        {
            _infrastructureSettings = infrastructureSettings;
            _logger = logger;
        }

        public IEnumerable<T> Query<T>(string query, object param = null)
        {
            if (string.IsNullOrEmpty(_infrastructureSettings.AchievementRateDataBaseConnectionString))
            {
                _logger.Error("Missing connectionstring for achievementrates database");
                return default(IEnumerable<T>);
            }

            using (IDbConnection dataConnection = new SqlConnection(_infrastructureSettings.AchievementRateDataBaseConnectionString))
            {
                var timer = Stopwatch.StartNew();
                var data = dataConnection.Query<T>(query, param);
                LogDependency(timer.Elapsed.TotalMilliseconds);

                return data;
            }
        }

        public T ExecuteScalar<T>(string query)
        {
            if (string.IsNullOrEmpty(_infrastructureSettings.AchievementRateDataBaseConnectionString))
            {
                _logger.Error("Missing connectionstring for achievementrates database");
                return default(T);
            }

            using (IDbConnection dataConnection = new SqlConnection(_infrastructureSettings.AchievementRateDataBaseConnectionString))
            {
                var timer = Stopwatch.StartNew();
                var data = dataConnection.ExecuteScalar<T>(query);
                LogDependency(timer.Elapsed.TotalMilliseconds);
                return data;
            }
        }

        private void LogDependency(double elaspedMilliseconds)
        {
            var logEntry = new DependencyLogEntry
            {
                Identifier = "DatabaseQuery",
                ResponseTime = elaspedMilliseconds,
                Url = string.Empty
            };
            _logger.Debug("Database query", logEntry);
        }
    }
}
