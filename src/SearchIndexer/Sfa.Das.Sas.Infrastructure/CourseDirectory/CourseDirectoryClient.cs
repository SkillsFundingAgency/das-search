using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Das.Sas.Indexer.Core.Logging;
using Sfa.Das.Sas.Indexer.Core.Logging.Models;
using Sfa.Das.Sas.Indexer.Core.Models;
using Sfa.Das.Sas.Indexer.Core.Models.Provider;
using Sfa.Das.Sas.Indexer.Core.Services;
using Sfa.Das.Sas.Indexer.Infrastructure.CourseDirectory.Models;
using Sfa.Das.Sas.Indexer.Infrastructure.Settings;

namespace Sfa.Das.Sas.Indexer.Infrastructure.CourseDirectory
{
    public sealed class CourseDirectoryClient : IGetApprenticeshipProviders
    {
        private readonly IInfrastructureSettings _settings;

        private readonly ICourseDirectoryProviderDataService _courseDirectoryProviderDataService;

        private readonly ILog _logger;

        public CourseDirectoryClient(IInfrastructureSettings settings, ICourseDirectoryProviderDataService courseDirectoryProviderDataService, ILog logger)
        {
            _settings = settings;
            _courseDirectoryProviderDataService = courseDirectoryProviderDataService;
            _logger = logger;
        }

        public async Task<IEnumerable<Das.Sas.Indexer.Core.Models.Provider.Provider>> GetApprenticeshipProvidersAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            _courseDirectoryProviderDataService.BaseUri = new Uri(_settings.CourseDirectoryUri);
            var responseAsync = await _courseDirectoryProviderDataService.BulkprovidersWithOperationResponseAsync();

            var providers = responseAsync.Body;

            var selectedProviders = providers.Select(MapFromProviderToProviderImport).ToList();

            _logger.Debug(
                "CourseDirectory.GetApprenticeshipProvidersAsync",
                new TimingLogEntry { ElaspedMilliseconds = stopwatch.Elapsed.TotalMilliseconds });

            _courseDirectoryProviderDataService.Dispose();

            return selectedProviders;
        }

        private static Coordinate SetGeoPoint(Models.Location matchingLocation)
        {
            if (!matchingLocation.Address.Latitude.HasValue || !matchingLocation.Address.Longitude.HasValue)
            {
                return null;
            }

            return new Coordinate { Latitude = matchingLocation.Address.Latitude.Value, Longitude = matchingLocation.Address.Longitude.Value };
        }

        private IEnumerable<StandardInformation> GetStandardsFromIList(IList<Models.Standard> standards, IEnumerable<Das.Sas.Indexer.Core.Models.Provider.Location> providerLocations)
        {
            return
                standards.Select(
                    standard =>
                    new StandardInformation
                        {
                            Code = standard.StandardCode,
                            InfoUrl = standard.StandardInfoUrl,
                            ContactInformation = new ContactInformation { Email = standard.Contact.Email, Phone = standard.Contact.Phone, Website = standard.Contact.ContactUsUrl },
                            MarketingInfo = standard.MarketingInfo,
                            DeliveryLocations = GetDeliveryLocations(standard.Locations, providerLocations)
                        }).ToList();
        }

        private IEnumerable<FrameworkInformation> GetFrameworksFromIList(IList<Models.Framework> frameworks, IEnumerable<Das.Sas.Indexer.Core.Models.Provider.Location> providerLocations)
        {
            return
                frameworks.Select(
                    framework =>
                    new FrameworkInformation
                        {
                            Code = framework.FrameworkCode,
                            ProgType = framework.GetProgType,
                            PathwayCode = framework.PathwayCode,
                            InfoUrl = framework.FrameworkInfoUrl,
                            ContactInformation = new ContactInformation { Email = framework.Contact.Email, Phone = framework.Contact.Phone, Website = framework.Contact.ContactUsUrl },
                            MarketingInfo = framework.MarketingInfo,
                            DeliveryLocations = GetDeliveryLocations(framework.Locations, providerLocations)
                        }).ToList();
        }

        private IEnumerable<DeliveryInformation> GetDeliveryLocations(IList<LocationRef> apprenticshipLocations, IEnumerable<Das.Sas.Indexer.Core.Models.Provider.Location> providerLocations)
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

        private Das.Sas.Indexer.Core.Models.Provider.Location MapToLocationEntity(Models.Location matchingLocation)
        {
            return new Das.Sas.Indexer.Core.Models.Provider.Location
                       {
                           Id = matchingLocation.ID.Value,
                           Name = matchingLocation.Name,
                           Contact =
                               new ContactInformation { Email = matchingLocation.Email, Phone = matchingLocation.Phone, Website = matchingLocation.Website },
                           Address =
                               new Core.Models.Provider.Address
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

        private IEnumerable<Core.Models.Provider.Location> GetLocationFromIList(IList<Models.Location> locations)
        {
            return locations.Select(MapToLocationEntity).ToList();
        }

        private Core.Models.Provider.Provider MapFromProviderToProviderImport(Models.Provider provider)
        {
            var providerLocations = GetLocationFromIList(provider.Locations);

            var providerImport = new Core.Models.Provider.Provider
                                     {
                                         Id = provider.Id.ToString(),
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