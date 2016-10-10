using System;
using System.IO;
using System.Net;
using System.Reflection;
using PactNet.Models;

namespace PactNet.TestExtensions
{
    public class PactBrokerClient : IDisposable
    {
        private readonly WebClient _client;

        public PactBrokerClient()
        {
            _client = new WebClient();
        }

        public void Upload(string pactBrokerUri, string providerName, string consumerName)
        {
            var details = new PactDetails { Provider = new Pacticipant { Name = providerName }, Consumer = new Pacticipant { Name = consumerName } };
            var filename = details.GeneratePactFileName();
            var dir = Directory.GetCurrentDirectory();

            if (!File.Exists("../../pacts/" + filename))
            {
                throw new FileNotFoundException("couldn't find the pact file", Path.Combine(dir, filename));
            }

            _client.Headers.Add("Content-Type", "application/json");
            var url = $"{pactBrokerUri}/pacts/provider/{providerName.Replace(" ", "%20")}/consumer/{consumerName.Replace(" ", "%20")}/version/{Assembly.GetExecutingAssembly().GetName().Version}";
            Console.WriteLine($"PUT {url}");
            _client.UploadFile(
                url,
                "PUT",
                $"../../pacts/{filename}");

        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}