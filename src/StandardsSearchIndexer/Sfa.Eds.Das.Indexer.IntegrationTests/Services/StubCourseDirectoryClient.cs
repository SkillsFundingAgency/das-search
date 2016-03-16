namespace Sfa.Eds.Das.Indexer.IntegrationTests.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Sfa.Eds.Das.Indexer.Core.Models.Provider;
    using Sfa.Eds.Das.Indexer.Core.Services;

    public class StubCourseDirectoryClient : IGetApprenticeshipProviders
    {
        public Task<IEnumerable<Provider>> GetApprenticeshipProvidersAsync()
        {
            return Task.FromResult(Retrieve());
        }

        private IEnumerable<Provider> Retrieve()
        {
            return JsonConvert.DeserializeObject<IEnumerable<Provider>>(StubCourseDirectoryData.Json);
        }
    }
}