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
                        var queryList = CreateListRawFormat(provider, standard);

                        foreach (var query in queryList)
                        {
                            await Client.Raw.IndexAsync(indexName, "provider", query);
                            documentCount++;
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

        private List<string> CreateListRawFormat(Provider provider, StandardInformation standard)
        {
            var list = new List<string>();

            foreach (var standardLocation in standard.DeliveryLocations)
            {
                var i = 0;
                var deliveryModes = new StringBuilder();

                foreach (var deliveryMode in standardLocation.DeliveryModes)
                {
                    deliveryModes.Append(i == 0 ? string.Concat(@"""", EnumHelper.GetDescription(deliveryMode), @"""") : string.Concat(", ", @"""", deliveryMode, @""""));

                    i++;
                }

                if (standard == null || standardLocation == null || standardLocation.DeliveryLocation == null)
                {
                    throw new Exception("Test");
                }

                var rawProvider = string.Concat(
                @"{ ""ukprn"": """,
                provider.Ukprn,
                @""", ""id"": """,
                $"{provider.Ukprn}{standard.StandardCode}{standardLocation.DeliveryLocation.Id}",
                @""", ""name"": """,
                provider.Name,
                @""", ""standardCode"": ",
                standard.StandardCode,
                @", ""locationId"": ",
                standardLocation.DeliveryLocation.Id,
                @", ""locationName"": """,
                standardLocation.DeliveryLocation.Name,
                @""", ""marketingInfo"": """,
                standard.MarketingInfo,
                @""", ""phone"": """,
                standard.StandardContact.Phone,
                @""", ""email"": """,
                standard.StandardContact.Email,
                @""", ""contactUsUrl"": """,
                standard.StandardContact.Website,
                @""", ""standardInfoUrl"": """,
                standard.StandardInfoUrl,
                @""", ""learnerSatisfaction"": ",
                provider.LearnerSatisfaction,
                @", ""employerSatisfaction"": ",
                provider.EmployerSatisfaction,
                @", ""deliveryModes"": [",
                deliveryModes,
                @"], ""website"": """,
                standardLocation.DeliveryLocation.Contact.Website,
                @""", ""address"": {""address1"":""",
                standardLocation.DeliveryLocation.Address.Address1,
                @""", ""address2"": """,
                standardLocation.DeliveryLocation.Address.Address2,
                @""", ""town"": """,
                standardLocation.DeliveryLocation.Address.Town,
                @""", ""county"": """,
                standardLocation.DeliveryLocation.Address.County,
                @""", ""postcode"": """,
                standardLocation.DeliveryLocation.Address.Postcode,
                @"""}, ""locationPoint"": [",
                standardLocation.DeliveryLocation.Address?.GeoPoint?.Longitude ?? 0, // TODO: LWA This needs to be handled better
                ", ",
                standardLocation.DeliveryLocation.Address?.GeoPoint?.Latitude ?? 0,
                @"],""location"": { ""type"": ""circle"", ""coordinates"": [",
                standardLocation.DeliveryLocation.Address?.GeoPoint?.Longitude ?? 0,
                ", ",
                standardLocation.DeliveryLocation.Address?.GeoPoint?.Latitude ?? 0,
                @"], ""radius"": """,
                standardLocation.Radius,
                @"mi"" }}");

                list.Add(rawProvider);
            }

            return list;
        }
    }
}
