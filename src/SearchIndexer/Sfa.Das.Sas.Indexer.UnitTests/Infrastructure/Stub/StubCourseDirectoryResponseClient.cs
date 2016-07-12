using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Rest;
using Sfa.Das.Sas.Indexer.Infrastructure.CourseDirectory;
using Sfa.Das.Sas.Indexer.Infrastructure.CourseDirectory.Models;

namespace Sfa.Das.Sas.Indexer.UnitTests.Infrastructure.Stub
{
    using Newtonsoft.Json.Linq;

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
            var inputObject = JToken.Parse(StubCourseDirectoryResponse.Json);
            IList<Provider> deserializedObject = new List<Provider>();
            foreach (var iListValue in (JArray)inputObject)
            {
                var provider = new Provider();
                provider.DeserializeJson(iListValue);
                deserializedObject.Add(provider);
            }

            return deserializedObject;
        }
    }
}