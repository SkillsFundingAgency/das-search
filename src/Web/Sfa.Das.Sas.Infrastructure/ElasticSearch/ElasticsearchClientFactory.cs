using Elasticsearch.Net;
using FeatureToggle.Core.Fluent;
using Nest;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Infrastructure.Extensions;
using Sfa.Das.Sas.Infrastructure.FeatureToggles;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    public sealed class ElasticsearchClientFactory : IElasticsearchClientFactory
    {
        private readonly IConfigurationSettings _applicationSettings;

        public ElasticsearchClientFactory(IConfigurationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
        }

        public IElasticClient Create()
        {
            ConnectionSettings settings;
            if (Is<IgnoreSslCertificateFeature>.Enabled)
            {
                settings = new ConnectionSettings(
                    new StaticConnectionPool(_applicationSettings.ElasticServerUrls),
                    new MyCertificateIgnoringHttpConnection());
            }
            else
            {
                settings = new ConnectionSettings(
                    new StaticConnectionPool(_applicationSettings.ElasticServerUrls));
            }

            settings.BasicAuthentication(_applicationSettings.ElasticsearchUsername, _applicationSettings.ElasticsearchPassword);
            settings.DisableDirectStreaming();
            settings.MapDefaultTypeNames(d => d.Add(typeof(StandardSearchResultsItem), "standarddocument"));
            settings.MapDefaultTypeNames(d => d.Add(typeof(FrameworkSearchResultsItem), "frameworkdocument"));
            settings.MapDefaultTypeNames(d => d.Add(typeof(StandardProviderSearchResultsItem), "standardprovider"));
            settings.MapDefaultTypeNames(d => d.Add(typeof(FrameworkProviderSearchResultsItem), "frameworkprovider"));

            return new ElasticClient(settings);
        }
    }
}