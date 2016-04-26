using System;
using System.Collections.Generic;
using System.Linq;
using Nest;
using Sfa.Das.Sas.Indexer.Core.Exceptions;
using Sfa.Das.Sas.Indexer.Core.Extensions;
using Sfa.Das.Sas.Indexer.Core.Models;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Indexer.Core.Models.Provider;
using Sfa.Das.Sas.Indexer.Core.Services;
using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Helpers;
using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Models;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch
{
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
                    Keywords = standard.Keywords,
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
                // Trim off any whitespaces in the title or the Pathway Name
                frameworkMetaData.NASTitle = frameworkMetaData.NASTitle?.Trim();
                frameworkMetaData.PathwayName = frameworkMetaData.PathwayName?.Trim();

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
            return ElasticLevelHelper.MapLevel(level);
        }

        public StandardProvider CreateStandardProviderDocument(Provider provider, StandardInformation standardInformation, DeliveryInformation deliveryInformation)
        {
            try
            {
                var standardProvider = new StandardProvider
                {
                    Id = $"{provider.Ukprn}-{standardInformation.Code}-{deliveryInformation.DeliveryLocation.Id}",
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
                    Id = $"{provider.Ukprn}-{frameworkInformation.Code}{MapLevelProgType(frameworkInformation.ProgType)}{frameworkInformation.PathwayCode}-{deliveryInformation.DeliveryLocation.Id}",
                    FrameworkCode = frameworkInformation.Code,
                    PathwayCode = frameworkInformation.PathwayCode,
                    FrameworkId = string.Concat(frameworkInformation.Code, frameworkInformation.ProgType, frameworkInformation.PathwayCode),
                    Level = MapLevelProgType(frameworkInformation.ProgType)
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
            documentToPopulate.LocationId = deliveryInformation.DeliveryLocation.Id;
            documentToPopulate.LocationName = deliveryInformation.DeliveryLocation.Name;
            documentToPopulate.ProviderMarketingInfo = EscapeSpecialCharacters(provider.MarketingInfo);
            documentToPopulate.ApprenticeshipMarketingInfo = EscapeSpecialCharacters(apprenticeshipInformation.MarketingInfo);
            documentToPopulate.Phone = apprenticeshipInformation.ContactInformation.Phone;
            documentToPopulate.Email = apprenticeshipInformation.ContactInformation.Email;
            documentToPopulate.ContactUsUrl = apprenticeshipInformation.ContactInformation.Website;
            documentToPopulate.ApprenticeshipInfoUrl = apprenticeshipInformation.InfoUrl;
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
            if (string.IsNullOrWhiteSpace(pathwayName) || framworkname.Equals(pathwayName))
            {
                return framworkname;
            }

            return $"{framworkname}: {pathwayName}";
        }
    }
}
