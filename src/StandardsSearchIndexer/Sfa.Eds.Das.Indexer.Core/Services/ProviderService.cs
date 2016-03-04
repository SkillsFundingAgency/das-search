namespace Sfa.Eds.Das.Indexer.Core.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Sfa.Eds.Das.Indexer.Common.Models;
    using Sfa.Eds.Das.Indexer.Core.Models.ProviderImport;
    using Sfa.Eds.Das.ProviderIndexer.Models;

    public class ProviderService : IGetProviders
    {
        private readonly IImportProviders _importer;

        public ProviderService(IImportProviders importer)
        {
            _importer = importer;
        }

        public async Task<IEnumerable<Provider>> GetProviders()
        {
            var incoming = _importer.GetProviders();
            return MapEach(incoming.Result);
        }

        private IEnumerable<Provider> MapEach(IEnumerable<ProviderImport> incoming)
        {
            foreach (var provider in incoming)
            {
                foreach (var standard in provider.Standards)
                {
                    foreach (var locationRef in standard.Locations)
                    {
                        var location = provider.Locations.FirstOrDefault(x => x.ID == locationRef.ID);
                        var address = location.Address ?? new Address();
                        yield return
                            new Provider
                            {
                                Coordinate = new Coordinate { Lat = address.Lat.Value, Lon = address.Long.Value },
                                PostCode = address.Postcode,
                                ProviderName = provider.Name,
                                Radius = locationRef.Radius,
                                StandardsId = new List<int>() {standard.StandardCode},
                                UkPrn = provider.Ukprn.ToString(),
                                VenueName = location.Name
                            };

                    }
                }

                foreach (var location in provider.Locations)
                {
                    var standards = provider.Standards.Where(x => x.Locations.Any(y => y.ID == location.ID.Value));
                    var address = location.Address ?? new Address();
                    yield return
                        new Provider
                            {
                                Coordinate = new Coordinate { Lat = address.Lat.Value, Lon = address.Long.Value },
                                PostCode = address.Postcode,
                                ProviderName = provider.Name,
                                Radius = 10,
                                ProviderId = 0,
                                StandardsId = standards.Select(x => x.StandardCode),
                                UkPrn = provider.Ukprn.ToString(),
                                VenueName = location.Name
                            };
                }

            }
        }
    }
}