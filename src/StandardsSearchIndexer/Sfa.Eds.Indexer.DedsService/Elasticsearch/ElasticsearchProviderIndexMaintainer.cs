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
            foreach (var provider in providers)
            {
                try
                {
                    foreach (var standard in provider.Standards)
                    {
                        var queryList = CreateListRawFormat(provider, standard);
                        foreach (var query in queryList)
                        {
                            await Client.Raw.IndexAsync(indexName, "provider", query);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error indexing provider: " + ex.Message, ex);
                    throw;
                }
            }
        }

        private List<string> CreateListRawFormat(Provider provider, StandardInformation standard)
        {
            var list = new List<string>();

            foreach (var standardLocation in standard.DeliveryLocations)
            {
                var location = standardLocation.DeliveryLocation;

                var i = 0;
                var deliveryModes = new StringBuilder();

                foreach (var deliveryMode in standardLocation.DeliveryModes)
                {
                    deliveryModes.Append(i == 0 ? string.Concat(@"""", EnumHelper.GetDescription(deliveryMode), @"""") : string.Concat(", ", @"""", deliveryMode, @""""));

                    i++;
                }

                var rawProvider = string.Concat(
                @"{ ""ukprn"": """,
                provider.Ukprn,
                @""", ""id"": """,
                provider.Id,
                @""", ""name"": """,
                provider.Name ?? "Unspecified",
                @""", ""standardcode"": ",
                standard.StandardCode,
                @", ""locationId"": ",
                location.Id,
                @", ""locationName"": """,
                location.Name ?? "Unspecified",
                @""", ""marketingInfo"": """,
                standard.MarketingInfo ?? "Unspecified",
                @""", ""phone"": """,
                standard.StandardContact.Phone ?? "Unspecified",
                @""", ""email"": """,
                standard.StandardContact.Email ?? "Unspecified",
                @""", ""contactUsUrl"": """,
                standard.StandardContact.Website ?? "Unspecified",
                @""", ""standardInfoUrl"": """,
                standard.StandardInfoUrl ?? "Unspecified",
                @""", ""deliveryModes"": [",
                deliveryModes,
                @"], ""website"": """,
                location.Contact.Website ?? "Unspecified",
                @""", ""address"": {""address1"":""",
                location.Address.Address1 ?? "Unspecified",
                @""", ""address2"": """,
                location.Address.Address2 ?? "Unspecified",
                @""", ""town"": """,
                location.Address.Town ?? "Unspecified",
                @""", ""county"": """,
                location.Address.County ?? "Unspecified",
                @""", ""postcode"": """,
                location.Address.Postcode ?? "Unspecified",
                @"""}, ""locationPoint"": [",
                location.Address.GeoPoint.Longitude,
                ", ",
                location.Address.GeoPoint.Latitude,
                @"],""location"": { ""type"": ""circle"", ""coordinates"": [",
                location.Address.GeoPoint.Longitude,
                ", ",
                location.Address.GeoPoint.Latitude,
                @"], ""radius"": """,
                standardLocation.Radius,
                @"mi"" }}");

                list.Add(rawProvider);
            }

            return list;
        }
    }
}
