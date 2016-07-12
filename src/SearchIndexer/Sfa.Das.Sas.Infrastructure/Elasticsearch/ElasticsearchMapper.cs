using System;
using System.Collections.Generic;
using System.Linq;
using Nest;
using Sfa.Das.Sas.Indexer.Core.Exceptions;
using Sfa.Das.Sas.Indexer.Core.Extensions;
using Sfa.Das.Sas.Indexer.Core.Logging;
using Sfa.Das.Sas.Indexer.Core.Models;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Indexer.Core.Models.Provider;
using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Helpers;
using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Models;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch
{
    using JobRoleItem = Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Models.JobRoleItem;

    public class ElasticsearchMapper : IElasticsearchMapper
    {
        private readonly ILog _logger;

        public ElasticsearchMapper(ILog logger)
        {
            _logger = logger;
        }

        public StandardDocument CreateStandardDocument(StandardMetaData standard)
        {
            var doc = new StandardDocument
            {
                StandardId = standard.Id,
                Title = standard.Title,
                JobRoles = standard.JobRoles,
                Keywords = standard.Keywords,
                Level = standard.NotionalEndLevel,
                StandardPdf = standard.StandardPdfUrl,
                AssessmentPlanPdf = standard.AssessmentPlanPdfUrl,
                TypicalLength = standard.TypicalLength,
                OverviewOfRole = standard.OverviewOfRole,
                EntryRequirements = standard.EntryRequirements,
                WhatApprenticesWillLearn = standard.WhatApprenticesWillLearn,
                Qualifications = standard.Qualifications,
                ProfessionalRegistration = standard.ProfessionalRegistration,
                SectorSubjectAreaTier1 = standard.SectorSubjectAreaTier1,
                SectorSubjectAreaTier2 = standard.SectorSubjectAreaTier2
            };

            return doc;
        }

        public FrameworkDocument CreateFrameworkDocument(FrameworkMetaData frameworkMetaData)
        {
            // Trim off any whitespaces in the title or the Pathway Name
            frameworkMetaData.NasTitle = frameworkMetaData.NasTitle?.Trim();
            frameworkMetaData.PathwayName = frameworkMetaData.PathwayName?.Trim();

            var doc = new FrameworkDocument
            {
                FrameworkId = $"{frameworkMetaData.FworkCode}{MapLevelProgType(frameworkMetaData.ProgType)}{frameworkMetaData.PwayCode}",
                Title = CreateFrameworkTitle(frameworkMetaData.NasTitle, frameworkMetaData.PathwayName),
                FrameworkCode = frameworkMetaData.FworkCode,
                FrameworkName = frameworkMetaData.NasTitle,
                PathwayCode = frameworkMetaData.PwayCode,
                PathwayName = frameworkMetaData.PathwayName,
                Level = MapLevelProgType(frameworkMetaData.ProgType),
                JobRoleItems = frameworkMetaData.JobRoleItems?.Select(m => new JobRoleItem { Title = m.Title, Description = m.Description }),
                TypicalLength = frameworkMetaData.TypicalLength,
                ExpiryDate = frameworkMetaData.EffectiveTo,
                SectorSubjectAreaTier1 = frameworkMetaData.SectorSubjectAreaTier1,
                SectorSubjectAreaTier2 = frameworkMetaData.SectorSubjectAreaTier2,
                CompletionQualifications = frameworkMetaData.CompletionQualifications,
                EntryRequirements = frameworkMetaData.EntryRequirements,
                ProfessionalRegistration = frameworkMetaData.ProfessionalRegistration,
                FrameworkOverview = frameworkMetaData.FrameworkOverview,
                CompetencyQualification = frameworkMetaData.CompetencyQualification,
                KnowledgeQualification = frameworkMetaData.KnowledgeQualification,
                CombinedQualification = frameworkMetaData.CombinedQualification
            };
            return doc;
        }

        public int MapLevelProgType(int level)
        {
            return ElasticLevelHelper.MapLevel(level);
        }

        public StandardProvider CreateStandardProviderDocument(Provider provider, StandardInformation standardInformation, DeliveryInformation deliveryInformation)
        {
            return CreateStandardProviderDocument(provider, standardInformation, new List<DeliveryInformation>() { deliveryInformation }, deliveryInformation.DeliveryLocation.Id.ToString());
        }

        public StandardProvider CreateStandardProviderDocument(Provider provider, StandardInformation standardInformation, IEnumerable<DeliveryInformation> deliveryInformation)
        {
            return CreateStandardProviderDocument(provider, standardInformation, deliveryInformation.ToList(), "s100PercentEmployer");
        }

        public FrameworkProvider CreateFrameworkProviderDocument(Provider provider, FrameworkInformation frameworkInformation, DeliveryInformation deliveryInformation)
        {
            return CreateFrameworkProviderDocument(provider, frameworkInformation, new List<DeliveryInformation> { deliveryInformation }, deliveryInformation.DeliveryLocation.Id.ToString());
        }

        public FrameworkProvider CreateFrameworkProviderDocument(Provider provider, FrameworkInformation frameworkInformation, IEnumerable<DeliveryInformation> deliveryInformation)
        {
            return CreateFrameworkProviderDocument(provider, frameworkInformation, deliveryInformation.ToList(), "f100PercentEmployer");
        }

        private StandardProvider CreateStandardProviderDocument(Provider provider, StandardInformation standardInformation, List<DeliveryInformation> deliveryInformation, string locationId)
        {
            try
            {
                var standardProvider = new StandardProvider
                {
                    Id = $"{provider.Ukprn}-{standardInformation.Code}-{locationId}",
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

        private FrameworkProvider CreateFrameworkProviderDocument(Provider provider, FrameworkInformation frameworkInformation, List<DeliveryInformation> deliveryInformation, string locationId)
        {
            try
            {
                var frameworkProvider = new FrameworkProvider
                {
                    Id = $"{provider.Ukprn}-{frameworkInformation.Code}{MapLevelProgType(frameworkInformation.ProgType)}{frameworkInformation.PathwayCode}-{locationId}",
                    FrameworkCode = frameworkInformation.Code,
                    PathwayCode = frameworkInformation.PathwayCode,
                    FrameworkId = string.Concat(frameworkInformation.Code, MapLevelProgType(frameworkInformation.ProgType), frameworkInformation.PathwayCode),
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

        private void PopulateDocumentSharedProperties(
            IProviderAppreticeshipDocument documentToPopulate,
            Provider provider,
            IApprenticeshipInformation apprenticeshipInformation,
            List<DeliveryInformation> deliveryLocations)
        {
            var locations = GetTrainingLocations(deliveryLocations);
            var firstLoc = deliveryLocations.FirstOrDefault();

            documentToPopulate.Ukprn = provider.Ukprn;
            documentToPopulate.ProviderName = provider.Name;
            documentToPopulate.ProviderMarketingInfo = EscapeSpecialCharacters(provider.MarketingInfo);
            documentToPopulate.ApprenticeshipMarketingInfo = EscapeSpecialCharacters(apprenticeshipInformation.MarketingInfo);
            documentToPopulate.Phone = apprenticeshipInformation.ContactInformation.Phone;
            documentToPopulate.Email = apprenticeshipInformation.ContactInformation.Email;
            documentToPopulate.ContactUsUrl = apprenticeshipInformation.ContactInformation.Website;
            documentToPopulate.ApprenticeshipInfoUrl = apprenticeshipInformation.InfoUrl;
            documentToPopulate.LearnerSatisfaction = provider.LearnerSatisfaction;
            documentToPopulate.EmployerSatisfaction = provider.EmployerSatisfaction;
            documentToPopulate.DeliveryModes = firstLoc == null ? new List<string>().ToArray() : GenerateListOfDeliveryModes(firstLoc.DeliveryModes);
            documentToPopulate.Website = firstLoc == null ? string.Empty : firstLoc.DeliveryLocation.Contact.Website;
            documentToPopulate.TrainingLocations = locations;
        }

        private List<TrainingLocation> GetTrainingLocations(IEnumerable<DeliveryInformation> deliveryLocations)
        {
            var locations = new List<TrainingLocation>();
            foreach (var loc in deliveryLocations)
            {
                locations.Add(
                    new TrainingLocation
                    {
                        LocationId = loc.DeliveryLocation.Id,
                        LocationName = loc.DeliveryLocation.Name,
                        Address =
                                new Models.Address
                                {
                                    Address1 = EscapeSpecialCharacters(loc.DeliveryLocation.Address.Address1),
                                    Address2 = EscapeSpecialCharacters(loc.DeliveryLocation.Address.Address2),
                                    Town = EscapeSpecialCharacters(loc.DeliveryLocation.Address.Town),
                                    County = EscapeSpecialCharacters(loc.DeliveryLocation.Address.County),
                                    PostCode = loc.DeliveryLocation.Address.Postcode,
                                },
                        Location =
                                new CircleGeoShape
                                {
                                    Coordinates =
                                            new GeoCoordinate(
                                            loc.DeliveryLocation.Address?.GeoPoint?.Latitude ?? 0,
                                            loc.DeliveryLocation.Address?.GeoPoint?.Longitude ?? 0),
                                    Radius = $"{loc.Radius}mi"
                                },
                        LocationPoint = new GeoCoordinate(
                                            loc.DeliveryLocation.Address?.GeoPoint?.Latitude ?? 0,
                                            loc.DeliveryLocation.Address?.GeoPoint?.Longitude ?? 0)
                    });
            }

            return locations;
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
