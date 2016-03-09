namespace Sfa.Eds.Das.Indexer.Core.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ProviderIndexer.Models;

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

            var result = incoming.Result.Select(providerImport => new Provider
            {
                Id = providerImport.Id,
                UkPrn = providerImport.Ukprn.ToString(),
                Email = providerImport.Email,
                EmployerSatisfaction = providerImport.EmployerSatisfaction,
                LearnerSatisfaction = providerImport.LearnerSatisfaction,
                MarketingInfo = providerImport.MarketingInfo,
                Name = providerImport.Name,
                Phone = providerImport.Phone,
                Website = providerImport.Website,
                Locations = providerImport.Locations,
                Frameworks = providerImport.Frameworks,
                Standards = providerImport.Standards
            }).ToList();

            return result;
        }
    }
}