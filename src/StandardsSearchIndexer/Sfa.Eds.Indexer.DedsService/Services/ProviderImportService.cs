namespace Sfa.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using Sfa.Eds.Das.Indexer.Core.Models.ProviderImport;
    using Sfa.Eds.Das.Indexer.Core.Services;
    using Sfa.Infrastructure.Models;
    using Sfa.Infrastructure.Settings;

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
            return CreateMapper().Map<IEnumerable<ProviderImport>>(responseAsync.Result);
        }

        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<IEnumerable<Provider>, IEnumerable<ProviderImport>>(); });
            return config.CreateMapper();
        }
    }
}