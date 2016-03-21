namespace Sfa.Infrastructure.CourseDirectory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Sfa.Eds.Das.Indexer.Core.Models;
    using Sfa.Eds.Das.Indexer.Core.Models.Provider;
    using Sfa.Eds.Das.Indexer.Core.Services;
    using Sfa.Infrastructure.CourseDirectory.Models;
    using Sfa.Infrastructure.Settings;

    public sealed class CourseDirectoryClient : IGetApprenticeshipProviders
    {
        private readonly IInfrastructureSettings _settings;

        public CourseDirectoryClient(IInfrastructureSettings settings)
        {
            _settings = settings;
        }

        public async Task<IEnumerable<Eds.Das.Indexer.Core.Models.Provider.Provider>> GetApprenticeshipProvidersAsync()
        {
            using (var courseDirectoryProvider = new CourseDirectoryProviderDataService())
            {
                courseDirectoryProvider.BaseUri = new Uri(_settings.CourseDirectoryUri);
                var responseAsync = await courseDirectoryProvider.BulkprovidersWithOperationResponseAsync();

                var providers = responseAsync.Body;

                return providers.Select(MapFromProviderToProviderImport).ToList();
            }
        }

        private static Coordinate SetGeoPoint(Models.Location matchingLocation)
        {
            if (!matchingLocation.Address.Latitude.HasValue || !matchingLocation.Address.Longitude.HasValue)
            {
                return null;
            }

            return new Coordinate { Latitude = matchingLocation.Address.Latitude.Value, Longitude = matchingLocation.Address.Longitude.Value };
        }

        private IEnumerable<StandardInformation> GetStandardsFromIList(IList<Models.Standard> standards, IEnumerable<Eds.Das.Indexer.Core.Models.Provider.Location> providerLocations)
        {
            return
                standards.Select(
                    standard =>
                    new StandardInformation
                        {
                            StandardCode = standard.StandardCode,
                            StandardInfoUrl = standard.StandardInfoUrl,
                            StandardContact = new ContactInformation { Email = standard.Contact.Email, Phone = standard.Contact.Phone, Website = standard.Contact.ContactUsUrl },
                            MarketingInfo = standard.MarketingInfo,
                            DeliveryLocations = GetDeliveryLocations(standard.Locations, providerLocations)
                        }).ToList();
        }

        private IEnumerable<FrameworkInformation> GetFrameworksFromIList(IList<Models.Framework> frameworks, IEnumerable<Eds.Das.Indexer.Core.Models.Provider.Location> providerLocations)
        {
            return
                frameworks.Select(
                    framework =>
                    new FrameworkInformation
                        {
                            FrameworkCode = framework.FrameworkCode,
                            Level = framework.Level,
                            PathwayCode = framework.PathwayCode,
                            FrameworkInfoUrl = framework.FrameworkInfoUrl,
                            FrameworkContact = new ContactInformation { Email = framework.Contact.Email, Phone = framework.Contact.Phone, Website = framework.Contact.ContactUsUrl },
                            MarketingInfo = framework.MarketingInfo,
                            DeliveryLocations = GetDeliveryLocations(framework.Locations, providerLocations)
                        }).ToList();
        }

        private IEnumerable<DeliveryInformation> GetDeliveryLocations(IList<LocationRef> apprenticshipLocations, IEnumerable<Eds.Das.Indexer.Core.Models.Provider.Location> providerLocations)
        {
            var deliveryLocations = new List<DeliveryInformation>(apprenticshipLocations.Count);

            foreach (var apprenticeshipLocation in apprenticshipLocations)
            {
                var matchingLocation = providerLocations.Single(x => x.Id == apprenticeshipLocation.ID);

                deliveryLocations.Add(
                    new DeliveryInformation { DeliveryLocation = matchingLocation, DeliveryModes = MapToDeliveryModes(apprenticeshipLocation.DeliveryModes), Radius = apprenticeshipLocation.Radius });
            }

            return deliveryLocations;
        }

        private IEnumerable<ModesOfDelivery> MapToDeliveryModes(IList<string> deliveryModes)
        {
            var modes = new List<ModesOfDelivery>(deliveryModes?.Count ?? 0);

            if (deliveryModes != null)
            {
                foreach (var mode in deliveryModes)
                {
                    switch (mode)
                    {
                        case "100PercentEmployer":
                            modes.Add(ModesOfDelivery.OneHundredPercentEmployer);
                            break;
                        case "BlockRelease":
                            modes.Add(ModesOfDelivery.BlockRelease);
                            break;
                        case "DayRelease":
                            modes.Add(ModesOfDelivery.DayRelease);
                            break;
                        default:
                            break;
                    }
                }
            }

            return modes;
        }

        private Eds.Das.Indexer.Core.Models.Provider.Location MapToLocationEntity(Models.Location matchingLocation)
        {
            return new Eds.Das.Indexer.Core.Models.Provider.Location
                       {
                           Id = matchingLocation.ID.Value,
                           Name = matchingLocation.Name,
                           Contact =
                               new ContactInformation { Email = matchingLocation.Email, Phone = matchingLocation.Phone, Website = matchingLocation.Website },
                           Address =
                               new Eds.Das.Indexer.Core.Models.Provider.Address
                                   {
                                       Address1 = matchingLocation.Address.Address1,
                                       Address2 = matchingLocation.Address.Address2,
                                       Town = matchingLocation.Address.Town,
                                       County = matchingLocation.Address.County,
                                       Postcode = matchingLocation.Address.Postcode,
                                       GeoPoint = SetGeoPoint(matchingLocation)
                                   }
                       };
        }

        private IEnumerable<Eds.Das.Indexer.Core.Models.Provider.Location> GetLocationFromIList(IList<Models.Location> locations)
        {
            return locations.Select(MapToLocationEntity).ToList();
        }

        private Eds.Das.Indexer.Core.Models.Provider.Provider MapFromProviderToProviderImport(Models.Provider provider)
        {
            var providerLocations = GetLocationFromIList(provider.Locations);

            var providerImport = new Eds.Das.Indexer.Core.Models.Provider.Provider
                                     {
                                         Id = provider.Id,
                                         Ukprn = provider.Ukprn,
                                         Name = provider.Name,
                                         ContactDetails =
                                             new ContactInformation { Email = provider.Email, Phone = provider.Phone, Website = provider.Website },
                                         EmployerSatisfaction = provider.EmployerSatisfaction,
                                         LearnerSatisfaction = provider.LearnerSatisfaction,
                                         MarketingInfo = provider.MarketingInfo,
                                         Standards = GetStandardsFromIList(provider.Standards, providerLocations),
                                         Frameworks = GetFrameworksFromIList(provider.Frameworks, providerLocations),
                                         Locations = GetLocationFromIList(provider.Locations)
                                     };

            return providerImport;
        }
    }
}