using System.Configuration;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.Infrastructure.Elasticsearch;
using Sfa.Das.Sas.Infrastructure.Settings;
using SFA.DAS.NLog.Logger;

namespace Sfa.Das.Sas.Web.UnitTests.Application.ApplicationServices
{
    [TestFixture]
    public sealed class ProviderNameSearchServiceTests
    {
        //[Test]
        //public async Task SearchNamesAndAliasesBySearchTerm()
        //{
        //    var configurationSettings = new ApplicationSettings();
        //    var logger = new NLogLogger();
        //    var paginationSettings = new PaginationSettings();
        //    var elasticSearchFactory = new ElasticsearchClientFactory(configurationSettings);
        //    var elasticSearchClient = new ElasticsearchCustomClient(elasticSearchFactory, logger);
        //    var provider = new ProviderNameSearchProvider(elasticSearchClient, logger, configurationSettings);
        //    var service = new ProviderNameSearchService(paginationSettings, provider, logger);

        //    //var result = await service.SearchProviderNameAndAliases("coll", 2);

        //    var result = await service.SearchProviderNameAndAliases("!()", 2);

        //    Assert.That(result, Is.Not.Null);
        //}
    }
}
