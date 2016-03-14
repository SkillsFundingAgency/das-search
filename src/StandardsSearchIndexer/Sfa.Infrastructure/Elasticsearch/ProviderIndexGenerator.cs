using Sfa.Eds.Das.Indexer.Core.Models.Provider;

namespace Sfa.Infrastructure.Elasticsearch
{
    public sealed class ProviderIndexGenerator : IGenerateIndexDefinitions<Provider>
    {
        public string Generate()
        {
            return @"
                {
                    ""mappings"": 
                    {
                        ""provider"": 
                        { 
                            ""properties"": 
                            {
                                ""ukprn"":
                                {
                                    ""type"": ""long""
                                },
                                ""id"":
                                {
                                    ""type"": ""long""
                                },
                                ""name"":
                                {
                                    ""type"": ""string""
                                },
                                ""standardCode"":
                                {
                                    ""type"": ""long""
                                },
                                ""locationId"":
                                {
                                    ""type"": ""long""
                                },
                                ""locationName"":
                                {
                                    ""type"": ""string""
                                },
                                ""marketingInfo"":
                                {
                                    ""type"": ""string""
                                },
                                ""standardInfoUrl"":
                                {
                                    ""type"": ""string""
                                },
                                ""deliveryModes"":
                                {
                                    ""type"": ""string""
                                },
                                ""website"":
                                {
                                    ""type"": ""string""
                                },
                                ""phone"":
                                {
                                    ""type"": ""string""
                                },
                                ""email"":
                                {
                                    ""type"": ""string""
                                },
                                ""contactUsUrl"":
                                {
                                    ""type"": ""string""
                                },
                                ""standardsId"":
                                {
                                    ""type"": ""long""
                                },
                                ""locationPoint"":
                                {
                                    ""type"": ""geo_point""
                                },
                                ""location"": 
                                {
                                    ""type"": ""geo_shape""
                                },
                                ""address"": 
                                {
                                    ""type"": ""nested"",
                                    ""properties"": 
                                    {
                                        ""address1"":       
                                        { 
                                            ""type"": ""string""  
                                        },
                                        ""address2"":       
                                        { 
                                            ""type"": ""string""  
                                        },  
                                        ""town"":       
                                        { 
                                            ""type"": ""string""  
                                        },
                                        ""county"":       
                                        { 
                                            ""type"": ""string""  
                                        },
                                        ""postcode"":       
                                        { 
                                            ""type"": ""string""  
                                        }
                                    }
                                }
                            }
                        }
                    }
                }";
        }
    }
}
