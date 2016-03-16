using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sfa.Eds.Das.Indexer.ApplicationServices;
using Sfa.Eds.Das.Indexer.Core;
using Sfa.Eds.Das.Indexer.Core.Models.Provider;
using Sfa.Eds.Das.Indexer.Core.Services;
using Sfa.Infrastructure.Services;

namespace Sfa.Infrastructure.Elasticsearch
{
    public sealed class ElasticsearchProviderIndexMaintainer : ElasticsearchIndexMaintainerBase<Provider>
    {
        private readonly IGenerateIndexDefinitions<Provider> _indexDefinitionGenerator;

        public ElasticsearchProviderIndexMaintainer(IElasticsearchClientFactory factory, IGenerateIndexDefinitions<Provider> indexDefinitionGenerator, ILog log)
            : base(factory, log, "Provider")
        {
            _indexDefinitionGenerator = indexDefinitionGenerator;
        }

        public override void CreateIndex(string indexName)
        {
            Client.Raw.IndicesCreatePost(indexName, _indexDefinitionGenerator.Generate());
        }

        public override async Task IndexEntries(string indexName, ICollection<Provider> providers)
        {
            var documentCount = 0;

            foreach (var provider in providers)
            {
                try
                {
                    foreach (var standard in provider.Standards)
                    {
                        foreach (var location in standard.DeliveryLocations)
                        {
                            var query = CreateListRawFormat(provider, standard, location);
                            var response = await Client.Raw.IndexAsync(indexName, "provider", query);

                            if (!response.Success)
                            {
                                Log.Warn($"Unable to index provider document - UKPRN:{provider.Ukprn}, StandardCode:{standard.StandardCode}, LocationId:{location.DeliveryLocation.Id}");
                            }
                            else
                            {
                                documentCount++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error indexing provider: " + ex.Message, ex);
                    throw;
                }
            }

            Log.Debug($"Indexed a total of {documentCount} Provider documents");
        }

        private string CreateListRawFormat(Provider provider, StandardInformation standard, DeliveryInformation location)
        {
            var i = 0;
            var deliveryModes = new StringBuilder();

            foreach (var deliveryMode in location.DeliveryModes)
            {
                deliveryModes.Append(i == 0 ? string.Concat(@"""", EnumExtensions.GetDescription(deliveryMode), @"""") : string.Concat(", ", @"""", deliveryMode, @""""));

                i++;
            }

            var rawProvider = string.Concat(
            @"{ ""ukprn"": """,
            provider.Ukprn,
            @""", ""id"": """,
            $"{provider.Ukprn}{standard.StandardCode}{location.DeliveryLocation.Id}",
            @""", ""name"": """,
            provider.Name,
            @""", ""standardCode"": ",
            standard.StandardCode,
            @", ""locationId"": ",
            location.DeliveryLocation.Id,
            @", ""locationName"": """,
            location.DeliveryLocation.Name,
            @""", ""marketingInfo"": """,
            EscapeSpecialCharacters(standard.MarketingInfo),
            @""", ""phone"": """,
            standard.StandardContact.Phone,
            @""", ""email"": """,
            standard.StandardContact.Email,
            @""", ""contactUsUrl"": """,
            standard.StandardContact.Website,
            @""", ""standardInfoUrl"": """,
            standard.StandardInfoUrl,
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

            return marketingInfo.Replace(Environment.NewLine, "\\r\\n")
                .Replace("\n", "\\n")
                .Replace("\"", "\\\"");
        }
    }
}
