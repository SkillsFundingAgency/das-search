namespace Sfa.Infrastructure.Elasticsearch
{
    public class ApprenticeshipIndexDefinitions : IApprenticeshipIndexDefinitions
    {
        public string Generate()
        {
            return @"
            {
                ""mappings"": {
                    ""standarddocument"": {
                        ""properties"": {
                            ""title"": {
                                ""type"": ""string"",
                                ""analyzer"": ""english""
                            },
                            ""jobRoles"": {
                                ""type"": ""string"",
                                ""analyzer"": ""english""
                            },
                            ""assessmentPlanPdf"": {
                                ""type"": ""string""
                            },
                            ""entryRequirements"": {
                                ""type"": ""string""
                            },
                            ""typicalLength"": {
                                ""type"": ""object"",
                                ""properties"": {
                                    ""from"": {
                                        ""type"": ""integer""
                                    },
                                    ""to"": {
                                        ""type"": ""integer""
                                    },
                                    ""unit"": {
                                        ""type"": ""string""
                                    }
                                }
                            }
                        }
                    },
                    ""frameworkdocument"": {
                        ""properties"": {
                            ""frameworkName"": {
                                ""type"": ""string"",
                                ""analyzer"": ""english""
                            },
                            ""pathwayName"": {
                                ""type"": ""string"",
                                ""analyzer"": ""english""
                            }
                        }
                    }
                }
            }";
        }
    }
}