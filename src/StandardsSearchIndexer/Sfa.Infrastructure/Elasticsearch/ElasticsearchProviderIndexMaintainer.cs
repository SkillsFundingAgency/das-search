using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
using Sfa.Eds.Das.Indexer.Core.Extensions;
using Sfa.Eds.Das.Indexer.Core.Models.Provider;
using Sfa.Eds.Das.Indexer.Core.Services;

namespace Sfa.Infrastructure.Elasticsearch
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core.Extensions;
    using Sfa.Eds.Das.Indexer.Core.Models.Provider;
    using Sfa.Eds.Das.Indexer.Core.Services;

    public sealed class ElasticsearchProviderIndexMaintainer : ElasticsearchIndexMaintainerBase, IMaintainProviderIndex
    {
        private readonly IGenerateIndexDefinitions<Provider> _indexDefinitionGenerator;

        private readonly IIndexSettings<IMaintainProviderIndex> _settings;

        public ElasticsearchProviderIndexMaintainer(IElasticsearchClientFactory factory, IElasticsearchMapper elasticsearchMapper, IGenerateIndexDefinitions<Provider> indexDefinitionGenerator, IIndexSettings<IMaintainProviderIndex> settings, ILog log)
            : base(factory, elasticsearchMapper, log, "Provider")
        {
            _indexDefinitionGenerator = indexDefinitionGenerator;
            _settings = settings;
        }

        public override void CreateIndex(string indexName)
        {
            Client.Raw.IndicesCreatePost(indexName, _indexDefinitionGenerator.Generate());
        }

        public async Task IndexEntries(string indexName, ICollection<Provider> indexEntries)
        {
            var documentCount = 0;

            foreach (var provider in indexEntries)
            {
                try
                {
                    documentCount += await IndexAll(indexName, provider.Standards, provider).ConfigureAwait(false);
                    documentCount += await IndexAll(indexName, provider.Frameworks, provider).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Log.Error("Error indexing provider: " + ex.Message, ex);
                    throw;
                }
            }

            Log.Debug($"Indexed a total of {documentCount} Provider documents");
        }

        private async Task<int> IndexAll(string indexName, IEnumerable<IApprenticeshipInformation> apprenticeships, Provider provider)
        {
            var documentCount = 0;
            foreach (var apprenticeship in apprenticeships)
            {
                documentCount += await Index(indexName, apprenticeship, provider).ConfigureAwait(false);
            }

            return documentCount;
        }

        private async Task<int> Index(string indexName, IApprenticeshipInformation apprenticeship, Provider provider)
        {
            var documentCount = 0;
            foreach (var location in apprenticeship.DeliveryLocations)
            {
                string apprenticeshipJson;
                string typeName;

                var standard = apprenticeship as StandardInformation;
                if (standard != null)
                {
                    apprenticeshipJson = CreateStandardJson(standard);
                    typeName = _settings.StandardProviderDocumentType;
                }
                else
                {
                    apprenticeshipJson = CreateFrameworkJson((FrameworkInformation)apprenticeship);
                    typeName = _settings.FrameworkProviderDocumentType;
                }

                var query = CreateRawFormat(provider, apprenticeship, location, apprenticeshipJson);
                var response = await Client.Raw.IndexAsync(indexName, typeName, query);

                if (!response.Success)
                {
                    Log.Warn($"Unable to index provider document - UKPRN:{provider.Ukprn}, Code:{apprenticeship.Code}, LocationId:{location.DeliveryLocation.Id}");
                }
                else
                {
                    documentCount++;
                }
            }

            return documentCount;
        }

        private string CreateStandardJson(StandardInformation standard)
        {
            var raw = string.Concat(
                @""", ""standardCode"": ",
                standard.Code);
            return raw;
        }

        private string CreateFrameworkJson(FrameworkInformation framework)
        {
            var raw = string.Concat(
                @""", ""frameworkCode"": ",
                framework.Code,
                @", ""pathwayCode"": ",
                framework.PathwayCode,
                @", ""level"": ",
                framework.Level);
            return raw;
        }

        private string CreateRawFormat(Provider provider, IApprenticeshipInformation apprenticeship, DeliveryInformation location, string typeSpecificJson)
        {
            var i = 0;
            var deliveryModes = new StringBuilder();

            foreach (var deliveryMode in location.DeliveryModes)
            {
                deliveryModes.Append(i == 0 ? string.Concat(@"""", deliveryMode.GetDescription(), @"""") : string.Concat(", ", @"""", deliveryMode, @""""));

                i++;
            }

            var rawProvider = string.Concat(
            @"{ ""ukprn"": """,
            provider.Ukprn,
            @""", ""name"": """,
            provider.Name,
            @""", ""id"": """,
            $"{provider.Ukprn}{apprenticeship.Code}{location.DeliveryLocation.Id}",
            typeSpecificJson,
            @", ""locationId"": ",
            location.DeliveryLocation.Id,
            @", ""locationName"": """,
            location.DeliveryLocation.Name,
            @""", ""providerMarketingInfo"": """,
            EscapeSpecialCharacters(provider.MarketingInfo),
            @""", ""apprenticeshipMarketingInfo"": """,
            EscapeSpecialCharacters(apprenticeship.MarketingInfo),
            @""", ""phone"": """,
            apprenticeship.ContactInformation.Phone,
            @""", ""email"": """,
            apprenticeship.ContactInformation.Email,
            @""", ""contactUsUrl"": """,
            apprenticeship.ContactInformation.Website,
            @""", ""standardInfoUrl"": """,
            apprenticeship.InfoUrl,
            @""", ""learnerSatisfaction"": ",
            provider.LearnerSatisfaction ?? 0,
            @", ""employerSatisfaction"": ",
            provider.EmployerSatisfaction ?? 0,
            @", ""deliveryModes"": [",
            deliveryModes,
            @"], ""website"": """,
            location.DeliveryLocation.Contact.Website,
            @""", ""address"": {""address1"":""",
            EscapeSpecialCharacters(location.DeliveryLocation.Address.Address1),
            @""", ""address2"": """,
            EscapeSpecialCharacters(location.DeliveryLocation.Address.Address2),
            @""", ""town"": """,
            EscapeSpecialCharacters(location.DeliveryLocation.Address.Town),
            @""", ""county"": """,
            EscapeSpecialCharacters(location.DeliveryLocation.Address.County),
            @""", ""postcode"": """,
            location.DeliveryLocation.Address.Postcode,
            @"""}, ""locationPoint"": [",
            location.DeliveryLocation.Address?.GeoPoint?.Longitude ?? 0, // TODO: LWA This needs to be handled better
            ", ",
            location.DeliveryLocation.Address?.GeoPoint?.Latitude ?? 0,
            @"],""location"": { ""type"": ""circle"", ""coordinates"": [",
            location.DeliveryLocation.Address?.GeoPoint?.Longitude ?? 0,
            ", ",
            location.DeliveryLocation.Address?.GeoPoint?.Latitude ?? 0,
            @"], ""radius"": """,
            location.Radius,
            @"mi"" }}");

            return rawProvider;
        }

        private string EscapeSpecialCharacters(string marketingInfo)
        {
            if (marketingInfo == null)
            {
                return null;
            }

            return marketingInfo.Replace(Environment.NewLine, "\\r\\n").Replace("\n", "\\n").Replace("\"", "\\\"");
        }
    }
}