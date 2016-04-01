namespace Sfa.Infrastructure.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;

    public class InfrastructureSettings : BaseSettings, IInfrastructureSettings
    {
        public string CourseDirectoryUri => ConfigurationManager.AppSettings["CourseDirectoryUri"];

        public IEnumerable<Uri> ElasticServerUrls => GetElasticIPs("ElasticServerUrls");
    }
}