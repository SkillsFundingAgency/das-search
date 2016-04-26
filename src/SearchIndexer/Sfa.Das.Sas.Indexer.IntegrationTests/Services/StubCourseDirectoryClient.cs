using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sfa.Das.Sas.Indexer.Core.Models.Provider;
using Sfa.Das.Sas.Indexer.Core.Services;

namespace Sfa.Das.Sas.Indexer.IntegrationTests.Services
{
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