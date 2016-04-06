using Sfa.Eds.Das.Indexer.Core.Elasticsearch;

namespace Sfa.Infrastructure.Elasticsearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Eds.Das.Indexer.Core.Exceptions;
    using Eds.Das.Indexer.Core.Extensions;
    using Eds.Das.Indexer.Core.Models.Provider;
    using Nest;
    using Sfa.Eds.Das.Indexer.Core.Models;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;
    using Sfa.Eds.Das.Indexer.Core.Services;
    using Sfa.Infrastructure.Elasticsearch.Models;

    public class ElasticsearchMapper : IElasticsearchMapper
    {
        private readonly ILog _logger;

        public ElasticsearchMapper(ILog logger)
        {
            _logger = logger;
        }

        public StandardDocument CreateStandardDocument(StandardMetaData standard)
        {
            try
            {
                var doc = new StandardDocument
                {
                    StandardId = standard.Id,
                    Title = standard.Title,
                    JobRoles = standard.JobRoles,
                    Level = standard.NotionalEndLevel,
                    PdfFileName = standard.PdfFileName,
                    StandardPdf = standard.StandardPdfUrl,
                    AssessmentPlanPdf = standard.AssessmentPlanPdfUrl,
                    TypicalLength = standard.TypicalLength,
                    IntroductoryText = standard.IntroductoryText,
                    OverviewOfRole = standard.OverviewOfRole,
                    EntryRequirements = standard.EntryRequirements,
                    WhatApprenticesWillLearn = standard.WhatApprenticesWillLearn,
                    Qualifications = standard.Qualifications,
                    ProfessionalRegistration = standard.ProfessionalRegistration,
                };

                return doc;
            }
            catch (Exception ex)
            {
                _logger.Error("Error creating document", ex);

                throw;
            }
        }

        public FrameworkDocument CreateFrameworkDocument(FrameworkMetaData frameworkMetaData)
        {
            try
            {
                var doc = new FrameworkDocument
                {
                    FrameworkId = $"{frameworkMetaData.FworkCode}{frameworkMetaData.ProgType}{frameworkMetaData.PwayCode}",
                    Title = CreateFrameworkTitle(frameworkMetaData.NASTitle, frameworkMetaData.PathwayName),
                    FrameworkCode = frameworkMetaData.FworkCode,
                    FrameworkName = frameworkMetaData.NASTitle,
                    PathwayCode = frameworkMetaData.PwayCode,
                    PathwayName = frameworkMetaData.PathwayName,
                    Level = MapLevelProgType(frameworkMetaData.ProgType)
                };

                return doc;
            }
            catch (Exception ex)
            {
                _logger.Error("Error creating document", ex);

                throw;
            }
        }

        public int MapLevelProgType(int level)
        {
            var em = new ElasticMapper();
            return em.MapLevel(level);
        }

        public StandardProvider CreateStandardProviderDocument(Provider provider, StandardInformation standardInformation, DeliveryInformation deliveryInformation)
        {
            try
            {
                var standardProvider = new StandardProvider
                {
                    StandardCode = standardInformation.Code
                };

                PopulateDocumentSharedProperties(standardProvider, provider, standardInformation, deliveryInformation);

                return standardProvider;
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is NullReferenceException)
            {
                throw new MappingException("Unable to map to Standard Provider Document", ex);
            }
        }

        public FrameworkProvider CreateFrameworkProviderDocument(Provider provider, FrameworkInformation frameworkInformation, DeliveryInformation deliveryInformation)
        {
            try
            {
                var frameworkProvider = new FrameworkProvider
                {
                    FrameworkCode = frameworkInformation.Code,
                    PathwayCode = frameworkInformation.PathwayCode,
                    FrameworkId = string.Concat(frameworkInformation.Code, MapLevelProgType(frameworkInformation.Level), frameworkInformation.PathwayCode),
                    Level = frameworkInformation.Level
                };

                PopulateDocumentSharedProperties(frameworkProvider, provider, frameworkInformation, deliveryInformation);

                return frameworkProvider;
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is NullReferenceException)
            {
                throw new MappingException("Unable to map to Framework Provider Document", ex);
            }
        }

        private void PopulateDocumentSharedProperties(IProviderAppreticeshipDocument documentToPopulate, Provider provider, IApprenticeshipInformation apprenticeshipInformation, DeliveryInformation deliveryInformation)
        {
            documentToPopulate.Ukprn = provider.Ukprn;
            documentToPopulate.Name = provider.Name;
            documentToPopulate.Id = $"{provider.Ukprn}{apprenticeshipInformation.Code}{deliveryInformation.DeliveryLocation.Id}";
            documentToPopulate.LocationId = deliveryInformation.DeliveryLocation.Id;
            documentToPopulate.LocationName = deliveryInformation.DeliveryLocation.Name;
            documentToPopulate.ProviderMarketingInfo = EscapeSpecialCharacters(provider.MarketingInfo);
            documentToPopulate.ApprenticeshipMarketingInfo = EscapeSpecialCharacters(apprenticeshipInformation.MarketingInfo);
            documentToPopulate.Phone = apprenticeshipInformation.ContactInformation.Phone;
            documentToPopulate.Email = apprenticeshipInformation.ContactInformation.Email;
            documentToPopulate.ContactUsUrl = apprenticeshipInformation.ContactInformation.Website;
            documentToPopulate.StandardInfoUrl = apprenticeshipInformation.InfoUrl;
            documentToPopulate.LearnerSatisfaction = provider.LearnerSatisfaction;
            documentToPopulate.EmployerSatisfaction = provider.EmployerSatisfaction;
            documentToPopulate.DeliveryModes = GenerateListOfDeliveryModes(deliveryInformation.DeliveryModes);
            documentToPopulate.Website = deliveryInformation.DeliveryLocation.Contact.Website;
            documentToPopulate.Address = new Models.Address
            {
                Address1 = EscapeSpecialCharacters(deliveryInformation.DeliveryLocation.Address.Address1),
                Address2 = EscapeSpecialCharacters(deliveryInformation.DeliveryLocation.Address.Address2),
                Town = EscapeSpecialCharacters(deliveryInformation.DeliveryLocation.Address.Town),
                County = EscapeSpecialCharacters(deliveryInformation.DeliveryLocation.Address.County),
                PostCode = deliveryInformation.DeliveryLocation.Address.Postcode,
            };
            documentToPopulate.LocationPoint = new GeoCoordinate(deliveryInformation.DeliveryLocation.Address?.GeoPoint?.Latitude ?? 0, deliveryInformation.DeliveryLocation.Address?.GeoPoint?.Longitude ?? 0);
            documentToPopulate.Location = new CircleGeoShape { Coordinates = new GeoCoordinate(deliveryInformation.DeliveryLocation.Address?.GeoPoint?.Latitude ?? 0, deliveryInformation.DeliveryLocation.Address?.GeoPoint?.Longitude ?? 0), Radius = $"{deliveryInformation.Radius}mi" };
        }

        private string[] GenerateListOfDeliveryModes(IEnumerable<ModesOfDelivery> deliveryModes)
        {
            return deliveryModes.Select(x => x.GetDescription()).ToArray();
        }

        private string EscapeSpecialCharacters(string marketingInfo)
        {
            if (marketingInfo == null)
            {
                return null;
            }

            return marketingInfo.Replace(Environment.NewLine, "\\r\\n").Replace("\n", "\\n").Replace("\"", "\\\"");
        }

        private string CreateFrameworkTitle(string framworkname, string pathwayName)
        {
            if (framworkname.Equals(pathwayName) || string.IsNullOrWhiteSpace(pathwayName))
            {
                return framworkname;
            }

            return $"{framworkname}: {pathwayName}";
        }
    }
}
