using System;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using Nest;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Infrastructure.Elasticsearch;
using Sfa.Das.Sas.Infrastructure.Mapping;
using SFA.DAS.NLog.Logger;

namespace Sfa.Das.Sas.Infrastructure.Repositories
{
    public sealed class StandardElasticRepository : IGetStandards
    {
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly ILog _applicationLogger;
        private readonly IConfigurationSettings _applicationSettings;
        private readonly IStandardMapping _standardMapping;
        private readonly IElasticsearchHelper _elasticsearchHelper;

        public StandardElasticRepository(
            IElasticsearchCustomClient elasticsearchCustomClient,
            ILog applicationLogger,
            IConfigurationSettings applicationSettings,
            IStandardMapping standardMapping,
            IElasticsearchHelper elasticsearchHelper)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _applicationLogger = applicationLogger;
            _applicationSettings = applicationSettings;
            _standardMapping = standardMapping;
            _elasticsearchHelper = elasticsearchHelper;
        }

        public Standard GetStandardById(string id)
        {
            //var results =
            //    _elasticsearchCustomClient.Search<StandardSearchResultsItem>(
            //        s =>
            //        s.Index(_applicationSettings.ApprenticeshipIndexAlias)
            //            .Type(Types.Parse("standarddocument"))
            //            .From(0)
            //            .Size(1)
            //            .Query(q => q.QueryString(qs => qs.Fields(fs => fs.Field(e => e.StandardId)).Query(id.ToString()))));

            var results = new MockSearchResponse();

            if (results.ApiCall.HttpStatusCode != 200)
            {
                throw new ApplicationException($"Failed query standard with id {id}");
            }

            var document = results.Documents.Any() ? results.Documents.First() : null;

            return document != null ? _standardMapping.MapToStandard(document) : null;
        }

        public class MockProviderSearchResponse : ISearchResponse<StandardProviderSearchResultsItem>
        {
            public IApiCallDetails CallDetails
            {
                get;
                set;
            }

            public bool IsValid => true;

            public IApiCallDetails ApiCall => new MockCallDetails();

            public ServerError ServerError => null;

            public Exception OriginalException => null;

            public string DebugInformation => $"";

            public ShardsMetaData Shards => null;

            public HitsMetaData<StandardProviderSearchResultsItem> HitsMetaData => null;

            public IDictionary<string, IAggregate> Aggregations => new Dictionary<string, IAggregate> { { "test", new MockAggregate() } };

            public Profile Profile => new Profile ();

            public AggregationsHelper Aggs => new MockAggregationsHelper();

            public IDictionary<string, Suggest[]> Suggest => new Dictionary<string, Suggest[]>();

            public int Took => 1;

            public bool TimedOut => false;

            public bool TerminatedEarly => false;

            public string ScrollId => $"";

            public long Total => 0;

            public double MaxScore => 0;

            public IEnumerable<StandardProviderSearchResultsItem> Documents => new List<StandardProviderSearchResultsItem>
            {
                new StandardProviderSearchResultsItem
                {
                    StandardCode = 1,
                    ProviderName = "Provider",
                    IsNew = true,
                    Ukprn = 1,
                    Email = "dave_howlett@hotmail.com", 
                    EmployerSatisfaction = 1,
                    IsHigherEducationInstitute = false,
                    LearnerSatisfaction = 1,
                    MatchingLocationId = 1,
                    DeliveryModes = new List<string>(),
                    TrainingLocations = new List<TrainingLocation>
                    {
                        new TrainingLocation
                        {
                            LocationId = 1,
                            LocationName = "location name",
                            Address = new Address
                            {
                                Address1 = "test address",
                                County = "county",
                                Postcode = "pe11 3tq",
                                Town = "peterborough",
                                Address2 = "address 2",
                                Lat = 1,
                                Long = 2
                            },
                        }
                    }
                }
            };


            public IEnumerable<IHit<StandardProviderSearchResultsItem>> Hits => new List<IHit<StandardProviderSearchResultsItem>>
            {
                new MockHitProvider()
            };

            public IEnumerable<FieldValues> Fields => null;

            public HighlightDocumentDictionary Highlights => null;
        }

        public class MockHitProvider : IHit<StandardProviderSearchResultsItem>
        {
            public FieldValues Fields
            {
                get;
            }

            public StandardProviderSearchResultsItem Source => new StandardProviderSearchResultsItem
            {
                StandardCode = 1,
                ProviderName = "Provider",
                IsNew = true,
                Ukprn = 1,
                Email = "dave_howlett@hotmail.com",
                EmployerSatisfaction = 1,
                IsHigherEducationInstitute = false,
                LearnerSatisfaction = 1,
                MatchingLocationId = 1,
                DeliveryModes = new List<string>(),
                TrainingLocations = new List<TrainingLocation>
                {
                    new TrainingLocation
                    {
                        LocationId = 1,
                        LocationName = "location name",
                        Address = new Address
                        {
                            Address1 = "test address",
                            County = "county",
                            Postcode = "pe11 3tq",
                            Town = "peterborough",
                            Address2 = "address 2",
                            Lat = 1,
                            Long = 2
                        }
                    }
                }
            };

            public string Index
            {
                get;
            }

            public string Type
            {
                get;
            }

            public long? Version
            {
                get;
            }

            public double Score
            {
                get;
            }

            public string Id
            {
                get;
            }

            public string Parent
            {
                get;
            }

            public string Routing
            {
                get;
            }

            public long? Timestamp
            {
                get;
            }

            public long? Ttl
            {
                get;
            }

            public IEnumerable<object> Sorts => new List<object> { 0 };

            public HighlightFieldDictionary Highlights
            {
                get;
            }

            public Explanation Explanation
            {
                get;
            }

            public IEnumerable<string> MatchedQueries
            {
                get;
            }

            public IDictionary<string, InnerHitsResult> InnerHits => new Dictionary<string, InnerHitsResult>
            {
                {"test", new MockInnerHitsResult() }
            };
        }

        public class MockInnerHitsResult : InnerHitsResult
        {
            public new InnerHitsMetaData Hits => new MockInnerHitsMetaData();
        }

        public class MockInnerHitsMetaData : InnerHitsMetaData
        {
            public new List<Hit<ILazyDocument>> Hits => new List<Hit<ILazyDocument>> { new MockDocument() };
        }

        public class MockDocument : Hit<ILazyDocument>
        {
            public new ILazyDocument Source => new MockLazyDocument();
        }

        public class MockLazyDocument : LazyDocument
        {
            public new T1 As<T1>()
                where T1 : TrainingLocation
            {
                var result = new TrainingLocation
                {
                    LocationId = 1,
                    LocationName = "location name",
                    Address = new Address
                    {
                        Address1 = "test address",
                        County = "county",
                        Postcode = "pe11 3tq",
                        Town = "peterborough",
                        Address2 = "address 2",
                        Lat = 1,
                        Long = 2
                    }
                };

                return result as T1;
            }
        }

        public class MockSearchResponse : ISearchResponse<StandardSearchResultsItem>
        {
            public IApiCallDetails CallDetails
            {
                get;
                set;
            }

            public bool IsValid => true;

            public IApiCallDetails ApiCall => new MockCallDetails();

            public ServerError ServerError => null;

            public Exception OriginalException => null;

            public string DebugInformation => $"";

            public ShardsMetaData Shards => null;

            public HitsMetaData<StandardSearchResultsItem> HitsMetaData => null;

            public IDictionary<string, IAggregate> Aggregations => null;

            public Profile Profile => null;

            public AggregationsHelper Aggs => null;

            public IDictionary<string, Suggest[]> Suggest => null;

            public int Took => 1;

            public bool TimedOut => false;

            public bool TerminatedEarly => false;

            public string ScrollId => $"";

            public long Total => 0;

            public double MaxScore => 0;

            public IEnumerable<StandardSearchResultsItem> Documents => new List<StandardSearchResultsItem> { new StandardSearchResultsItem { StandardId = "1", Title = "Standard", Level = 1, Duration = 2 } };


            public IEnumerable<IHit<StandardSearchResultsItem>> Hits => new List<IHit<StandardSearchResultsItem>> { new Hit<StandardSearchResultsItem>() };

            public IEnumerable<FieldValues> Fields => null;

            public HighlightDocumentDictionary Highlights => null;
        }

        // TODO: Review this for performance againt using filters instead
        public IEnumerable<Standard> GetStandardsByIds(IEnumerable<int> ids)
        {
            var standardIds = ids as IList<int> ?? ids.ToList();

            if (!standardIds.Any())
            {
                return new List<Standard>();
            }

            var queryString = standardIds.Select(x => x.ToString()).Aggregate((x1, x2) => x1 + " OR " + x2);

            var results =
                   _elasticsearchCustomClient.Search<StandardSearchResultsItem>(
                       s =>
                       s.Index(_applicationSettings.ApprenticeshipIndexAlias)
                           .Type(Types.Parse("standarddocument"))
                           .From(0)
                           .Size(standardIds.Count)
                           .Query(q => q.QueryString(qs => qs.Fields(fs => fs.Field(e => e.StandardId)).Query(queryString))));

            if (!results.Documents.Any())
            {
                return new List<Standard>();
            }

            return results.Documents.Select(x => _standardMapping.MapToStandard(x))
                                    .Where(p => p != null);
        }

        public IEnumerable<Standard> GetAllStandards()
        {
            var results = _elasticsearchHelper.GetAllDocumentsFromIndex<StandardSearchResultsItem>(_applicationSettings.ApprenticeshipIndexAlias, "standarddocument");
            return results.Select(s => _standardMapping.MapToStandard(s));
        }

        public long GetStandardsAmount()
        {
            var results =
                   _elasticsearchCustomClient.Search<StandardSearchResultsItem>(
                       s =>
                       s.Index(_applicationSettings.ApprenticeshipIndexAlias)
                           .Type(Types.Parse("standarddocument"))
                           .From(0)
                           .MatchAll());
            return results.HitsMetaData.Total;
        }

        public long GetStandardsOffer()
        {
            var documents = _elasticsearchHelper.GetAllDocumentsFromIndex<StandardProviderSearchResultsItem>(_applicationSettings.ProviderIndexAlias, "standardprovider");
            var standardUkprnList = documents.Select(doc => string.Concat(doc.StandardCode, doc.Ukprn));

            return standardUkprnList.Distinct().Count();
        }
    }

    public class MockAggregationsHelper : AggregationsHelper
    {
        public new IDictionary<string, IAggregate> Aggregations => new Dictionary<string, IAggregate> { { "test", new MockAggregate() } };

        public new Dictionary<string, long?> Terms(string name)
        {
            return new Dictionary<string, long?>();
        }
    }

    public class MockAggregate : IAggregate
    {
        public IDictionary<string, object> Meta
        {
            get;
            set;
        }
    }

    public class MockCallDetails : IApiCallDetails
    {
        public bool Success
        {
            get;
        }

        public Exception OriginalException
        {
            get;
        }

        public ServerError ServerError
        {
            get;
        }

        public HttpMethod HttpMethod
        {
            get;
        }

        public Uri Uri
        {
            get;
        }

        public int? HttpStatusCode => 200;

        public byte[] ResponseBodyInBytes
        {
            get;
        }

        public byte[] RequestBodyInBytes
        {
            get;
        }

        public List<Audit> AuditTrail
        {
            get;
        }

        public string DebugInformation
        {
            get;
        }
    }
}
