using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganisationServiceTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            MakeCallToOrgService();

            Console.ReadLine();
        }

        private static void MakeCallToOrgService()
        {
            var client = new RestClient("<add enpoint>");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("api/v1/organisations/ukprn/{ukprn}", Method.GET);
            //request.AddParameter("ukprn", "12345678"); // adds to POST or URL querystring based on Method
            request.AddUrlSegment("ukprn", "12345678"); // replaces matching token in request.Resource

            // easily add HTTP Headers
            //request.AddHeader("header", "value");

            // add files to upload (works with compatible verbs)
            //request.AddFile(path);

            // execute the request
            var response = client.Execute(request);
            var content = response.Content; // raw content as string

            // or automatically deserialize result
            // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            var response2 = client.Execute<Organisation>(request);
            var organisationId = response2.Data.organisationId;
            Console.WriteLine(organisationId);

            // easy async support
            //client.ExecuteAsync(request, res => {
            //    Console.WriteLine(res.Content);
            //});

            // async with deserialization
            //var asyncHandle = client.ExecuteAsync<Organisation>(request, response => {
            //    Console.WriteLine(response.Data.organisationId);
            //});

            // abort the request on demand
            //asyncHandle.Abort();
        }

        private class Organisation
        {
            public string organisationId { get; set; }
        }
    }
}
