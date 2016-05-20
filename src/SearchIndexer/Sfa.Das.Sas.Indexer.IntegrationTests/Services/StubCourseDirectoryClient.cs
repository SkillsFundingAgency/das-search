namespace Sfa.Das.Sas.Indexer.IntegrationTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Infrastructure.CourseDirectory.Models;
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using Sfa.Das.Sas.Indexer.Infrastructure.CourseDirectory;

    public class StubCourseDirectoryClient : ICourseDirectoryProviderDataService
    {
        public ServiceClientCredentials Credentials { get; set; }

        public Uri BaseUri { get; set; }

        public void Dispose()
        {
        }

        Task<HttpOperationResponse<IList<Provider>>> ICourseDirectoryProviderDataService.BulkprovidersWithOperationResponseAsync(int? version, CancellationToken cancellationToken)
        {
            var response = new HttpOperationResponse<IList<Provider>>();
            var l = Retrieve();
            foreach (var provider in l)
            {
                response.Body.Add(provider);
            }

            return Task.FromResult(response);
        }

        private IEnumerable<Provider> Retrieve()
        {
            return JsonConvert.DeserializeObject<IEnumerable<Provider>>(StubCourseDirectoryData.Json);
        }
    }
}