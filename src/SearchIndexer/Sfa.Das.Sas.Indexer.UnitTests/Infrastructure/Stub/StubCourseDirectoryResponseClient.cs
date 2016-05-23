using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Rest;
using Newtonsoft.Json;
using Sfa.Das.Sas.Indexer.Infrastructure.CourseDirectory;
using Sfa.Das.Sas.Indexer.Infrastructure.CourseDirectory.Models;

namespace Sfa.Das.Sas.Indexer.UnitTests.Infrastructure.Stub
{
    public class StubCourseDirectoryResponseClient : ICourseDirectoryProviderDataService
    {
        public Uri BaseUri { get; set; }

        public ServiceClientCredentials Credentials { get; set; }

        public void Dispose()
        {
        }

        Task<HttpOperationResponse<IList<Provider>>> ICourseDirectoryProviderDataService.BulkprovidersWithOperationResponseAsync(int? version, CancellationToken cancellationToken)
        {
            var response = new HttpOperationResponse<IList<Provider>>();
            response.Body = new List<Provider>();
            var l = Retrieve();
            foreach (var provider in l)
            {
                response.Body.Add(provider);
            }

            return Task.FromResult(response);
        }

        private IEnumerable<Provider> Retrieve()
        {
            return JsonConvert.DeserializeObject<IEnumerable<Provider>>(StubCourseDirectoryResponse.Json);
        }
    }
}