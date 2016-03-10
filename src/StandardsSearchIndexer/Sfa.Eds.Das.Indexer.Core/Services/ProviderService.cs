namespace Sfa.Eds.Das.Indexer.Core.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Models;
    public class ProviderService : IGetProviders
    {
        private readonly IGetApprenticeshipProviders _importer;

        public ProviderService(IGetApprenticeshipProviders importer)
        {
            _importer = importer;
        }

        public async Task<IEnumerable<ProviderOld>> GetProviders()
        {
            var incoming = _importer.GetApprenticeshipProviders();

            var result = incoming.Result.Select(providerImport => new ProviderOld
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