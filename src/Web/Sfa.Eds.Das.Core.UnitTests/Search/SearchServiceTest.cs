namespace Sfa.Eds.Das.Core.UnitTests.Search
{
    using System.Linq;
    using System.Threading;

    using ElasticsearchInside;
    using NUnit.Framework;
    using Moq;
    using Nest;
    using Interfaces;

    using Core.Search;

    using Interfaces.Search;

    using Models;

    using Assert = NUnit.Framework.Assert;

    [TestFixture]
    [Category("Nightly")]
    public class SearchServiceTest
    {
        private IElasticClient client;

        private Elasticsearch elasticsearch;

        private ISearchService classSearchService;

        private readonly string indexname = "testindex";

        [SetUp]
        public void SetUp()
        {
            elasticsearch = new Elasticsearch(e => e.Port(4444));
            client = new ElasticClient(new ConnectionSettings(elasticsearch.Url));

            client.Index(new SearchResultsItem() { Title = "Standard One", StandardId = 1 }, i => i.Index(indexname));
            client.Index(new SearchResultsItem() { Title = "Standard 2", StandardId = 2 }, i => i.Index(indexname));
            client.Index(new SearchResultsItem() { Title = "Standard 3", StandardId = 3 }, i => i.Index(indexname));
            client.Index(new SearchResultsItem() { Title = "StandardFour", StandardId = 0 }, i => i.Index(indexname));

            Thread.Sleep(2 * 1000);

            var elasticClientFacoryMoq = new Mock<IElasticsearchClientFactory>();
            elasticClientFacoryMoq.Setup(m => m.Create()).Returns(client);

            classSearchService = new SearchService(elasticClientFacoryMoq.Object, null);
        }

        [TearDown]
        public void TearDown()
        {
            this.client.DeleteIndex(i => i.Index(indexname));
            elasticsearch.Dispose();
        }

        [Category("Nightly")]
        [TestCase("standard", 3, TestName = "Search on title keyword")]
        [TestCase("Standard%20One", 3, TestName = "Search with space")]
        [TestCase("StandardFour", 1, TestName = "Search unique name")]
        public void SearchByKeyword(string keyword, int countTotal)
        {
            var searchResult = classSearchService.SearchByKeyword(keyword, 0, 0);

            Assert.AreEqual(countTotal, searchResult.TotalResults);
            Assert.AreEqual(countTotal, searchResult.Results.Count());
        }

        [Category("Nightly")]
        [TestCase(@"\/", 1, TestName = @"Search with \/")]
        [TestCase("/", 1, TestName = "Search with /")]
        [TestCase("~", 0, TestName = "Search with ~")]
        [TestCase("}", 0, TestName = "Search with }")]
    public void SearchWithIllegalCharacter(string keyword, int countTotal)
        {
            var searchResult = classSearchService.SearchByKeyword(keyword, 0, 10);

            Assert.IsFalse(searchResult.HasError);
        }

        [Category("Nightly")]
        [TestCase("Standard", 0, 2, 2, 3, TestName = "Take 3 of 4")]
        [TestCase("Standard", 2, 10, 1, 3, TestName = "Skip 2 take 10 return 2")]
        public void SearchByKeywordSkipTake(string keyword, int skip, int take, int countTotal, int resultsTotal)
        {
            var searchResult = classSearchService.SearchByKeyword(keyword, skip, take);

            Assert.AreEqual(countTotal, searchResult.Results.Count());
            Assert.AreEqual(resultsTotal, searchResult.TotalResults);
        }

        [Category("Nightly"), Category("CIBuild")]
        [TestCase(null, 4, 4, TestName = "Keyword is null")]
        public void SearchByKeywordNullInput(string keyword, int countTotal, int resultsTotal)
        {
            var searchResult = classSearchService.SearchByKeyword(keyword, 0, 0);

            Assert.AreEqual(countTotal, searchResult.Results.Count());
            Assert.AreEqual(resultsTotal, searchResult.TotalResults);
        }
    }
}
