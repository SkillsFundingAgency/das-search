using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Eds.Das.Indexer.Core.Models.Provider;
using Sfa.Eds.Das.Indexer.Core.Services;
using Sfa.Infrastructure.Models;
using Sfa.Infrastructure.Settings;

namespace Sfa.Infrastructure.CourseDirectory
{
    public class CourseDirectoryClient : IGetApprenticeshipProviders
    {
        private readonly ICourseDirectoryProviderDataService _courseDirectoryProvider;

        private readonly IInfrastructureSettings _settings;

        public CourseDirectoryClient(IInfrastructureSettings settings)
        {
            _courseDirectoryProvider = new CourseDirectoryProviderDataService();
            _settings = settings;
        }

        public async Task<IEnumerable<Eds.Das.Indexer.Core.Models.Provider.Provider>> GetApprenticeshipProvidersAsync()
        {
            _courseDirectoryProvider.BaseUri = new Uri(_settings.CourseDirectoryUri);
            var responseAsync = await _courseDirectoryProvider.BulkprovidersWithOperationResponseAsync();

            var providers = responseAsync.Body;

            // TODO: LWA Extract mapping logic out of this class.
            return providers.Select(MapFromProviderToProviderImport).ToList();
        }

        private IEnumerable<StandardInformation> GetStandardsFromIList(IList<Models.Standard> standards, IEnumerable<Eds.Das.Indexer.Core.Models.Provider.Location> providerLocations)
        {
            return standards.Select(standard => new StandardInformation
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
            return frameworks.Select(framework => new FrameworkInformation
            {
                FrameworkCode = framework.FrameworkCode,
                Level = framework.Level,
                PathwayCode = framework.PathwayCode,
                FrameworkInfoUrl = string.Empty, //TODO: LWA -  framework.FrameworkInfoUrl missing
                FrameworkContact = new ContactInformation { Email = framework.Contact.Email, Phone = framework.Contact.Phone, Website = framework.Contact.ContactUsUrl },
                MarketingInfo = string.Empty, // TODO: LWA marketing info is missing from the Infrastucture Model -- framework.MarketingInfo,
                DeliveryLocations = GetDeliveryLocations(framework.Locations, providerLocations)
            }).ToList();
        }

        private IEnumerable<DeliveryInformation> GetDeliveryLocations(IList<LocationRef> apprenticshipLocations, IEnumerable<Eds.Das.Indexer.Core.Models.Provider.Location> providerLocations)
        {
            var deliveryLocations = new List<DeliveryInformation>(apprenticshipLocations.Count);

            foreach (var apprenticeshipLocation in apprenticshipLocations)
            {
                var matchingLocation = providerLocations.Single(x => x.Id == apprenticeshipLocation.ID);

                // TODO: LWA populate the location id & sort radius types out.
                deliveryLocations.Add(new DeliveryInformation
                {
                    Id = string.Empty,
                    DeliveryLocation = matchingLocation,
                    DeliveryModes = MapToDeliveryModes(apprenticeshipLocation.DeliveryModes),
                    Radius = (int)apprenticeshipLocation.Radius.Value
                });
            }

            return deliveryLocations;
        }

        private IEnumerable<ModesOfDelivery> MapToDeliveryModes(IList<string> deliveryModes)
        {
            // TODO: LWA handle when delivery modes is null
            var modes = new List<ModesOfDelivery>(deliveryModes.Count);

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

            return modes;
        }

        private Eds.Das.Indexer.Core.Models.Provider.Location MapToLocationEntity(Models.Location matchingLocation)
        {
            return new Eds.Das.Indexer.Core.Models.Provider.Location
            {
                Id = matchingLocation.ID.Value,
                Name = matchingLocation.Name,
                Contact = new ContactInformation { Email = matchingLocation.Email, Phone = matchingLocation.Phone, Website = matchingLocation.Website },
                Address = new Eds.Das.Indexer.Core.Models.Provider.Address
                {
                    Address1 = matchingLocation.Address.Address1,
                    Address2 = matchingLocation.Address.Address2,
                    Town = matchingLocation.Address.Town,
                    County = matchingLocation.Address.County,
                    Postcode = matchingLocation.Address.Postcode,
                    GeoPoint = new Eds.Das.Indexer.Common.Models.Coordinate
                    {
                        Lat = matchingLocation.Address.Lat.Value, // TODO: LWA - Need to handle when null is being passed.
                        Lon = matchingLocation.Address.Long.Value
                    }
                }
            };
        }

        private IEnumerable<Eds.Das.Indexer.Core.Models.Provider.Location> GetLocationFromIList(IList<Models.Location> locations)
        {
            return locations.Select(location => MapToLocationEntity(location)).ToList();
        }

        private Eds.Das.Indexer.Core.Models.Provider.Provider MapFromProviderToProviderImport(Models.Provider provider)
        {
            var providerLocations = GetLocationFromIList(provider.Locations);

            var providerImport = new Eds.Das.Indexer.Core.Models.Provider.Provider
            {
                Id = provider.Id,
                Ukprn = provider.Ukprn,
                Name = provider.Name,
                ContactDetails = new ContactInformation { Email = provider.Email, Phone = provider.Phone, Website = provider.Website },
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