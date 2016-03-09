using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Eds.Das.Indexer.Core.Models.ProviderImport;
using Sfa.Eds.Das.Indexer.Core.Services;
using Sfa.Infrastructure.Models;
using Sfa.Infrastructure.Settings;
using Address = Sfa.Eds.Das.Indexer.Core.Models.ProviderImport.Address;
using Contact = Sfa.Eds.Das.Indexer.Core.Models.ProviderImport.Contact;
using Framework = Sfa.Eds.Das.Indexer.Core.Models.ProviderImport.Framework;
using Location = Sfa.Eds.Das.Indexer.Core.Models.ProviderImport.Location;
using LocationRef = Sfa.Eds.Das.Indexer.Core.Models.ProviderImport.LocationRef;
using Standard = Sfa.Eds.Das.Indexer.Core.Models.ProviderImport.Standard;

namespace Sfa.Infrastructure.Services
{
    public class ProviderImportService : IImportProviders
    {
        private readonly ICourseDirectoryProviderDataService _courseDirectoryProvider;

        private readonly IInfrastructureSettings _settings;

        public ProviderImportService(IInfrastructureSettings settings)
        {
            _courseDirectoryProvider = new CourseDirectoryProviderDataService();
            _settings = settings;
        }

        public async Task<IEnumerable<ProviderImport>> GetProviders()
        {
            _courseDirectoryProvider.BaseUri = new Uri(_settings.CourseDirectoryUri);
            var responseAsync = _courseDirectoryProvider.BulkprovidersWithOperationResponseAsync();

            var providers = responseAsync.Result.Body;

            return providers.Select(MapFromProviderToProviderImport).ToList();
        }

        private IEnumerable<string> GetDeliveryModesFromIList(IList<string> deliveryModes)
        {
            return deliveryModes.ToList();
        }

        private IEnumerable<LocationRef> GetLocationRefFromIList(IList<Models.LocationRef> locations)
        {
            return locations.Select(location => new LocationRef
            {
                ID = location.ID, MarketingInfo = location.MarketingInfo, Radius = int.Parse(location.Radius.ToString()), StandardInfoUrl = location.StandardInfoUrl, DeliveryModes = GetDeliveryModesFromIList(location.DeliveryModes)
            }).ToList();
        }

        private IEnumerable<Standard> GetStandardsFromIList(IList<Models.Standard> standards)
        {
            return standards.Select(standard => new Standard
            {
                Contact = new Contact
                {
                    Email = standard.Contact.Email, Phone = standard.Contact.Phone, ContactUsUrl = standard.Contact.ContactUsUrl
                },
                MarketingInfo = standard.MarketingInfo, StandardCode = standard.StandardCode, StandardInfoUrl = standard.StandardInfoUrl, Locations = GetLocationRefFromIList(standard.Locations)
            }).ToList();
        }

        private IEnumerable<Framework> GetFrameworksFromIList(IList<Models.Framework> frameworks)
        {
            return frameworks.Select(framework => new Framework
            {
                Contact = new Contact
                {
                    Email = framework.Contact.Email, Phone = framework.Contact.Phone, ContactUsUrl = framework.Contact.ContactUsUrl
                },
                Level = framework.Level, FrameworkCode = framework.FrameworkCode, PathwayCode = framework.PathwayCode, Locations = GetLocationRefFromIList(framework.Locations)
            }).ToList();
        }

        private IEnumerable<Location> GetLocationFromIList(IList<Models.Location> locations)
        {
            return locations.Select(location => new Location
            {
                ID = location.ID, Name = location.Name, Email = location.Email, Phone = location.Phone, Website = location.Website, Address = new Address
                {
                    Address1 = location.Address.Address1, Address2 = location.Address.Address2, County = location.Address.County, Postcode = location.Address.Postcode, Town = location.Address.Postcode, Lat = location.Address.Lat, Long = location.Address.Long
                }
            }).ToList();
        }

        private ProviderImport MapFromProviderToProviderImport(Provider provider)
        {
            var providerImport = new ProviderImport
            {
                Id = provider.Id,
                Email = provider.Email,
                EmployerSatisfaction = provider.EmployerSatisfaction,
                LearnerSatisfaction = provider.LearnerSatisfaction,
                MarketingInfo = provider.MarketingInfo,
                Name = provider.Name,
                Phone = provider.Phone,
                Ukprn = provider.Ukprn,
                Website = provider.Website,
                Standards = GetStandardsFromIList(provider.Standards),
                Frameworks = GetFrameworksFromIList(provider.Frameworks),
                Locations = GetLocationFromIList(provider.Locations)
            };

            return providerImport;
        }
    }
}