using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using PactNet;
using PactNet.Mocks.MockHttpService;
using PactNet.Models;

namespace Sfa.Das.Web.ApprenticeshipApiTests
{
    public class ConsumerApprenticeshipApiPact : IDisposable
    {
        private readonly string _consumerName;
        private readonly string providerName = "Apprenticeship Api";
        private string pactBrokerUri => ConfigurationManager.AppSettings["PactBrokerUri"];
        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }

        public int MockServerPort { get { return 1234; } }
        public string MockProviderServiceBaseUri { get { return String.Format("http://localhost:{0}", MockServerPort); } }

        public ConsumerApprenticeshipApiPact(string consumerName)
        {
            _consumerName = consumerName;
            PactBuilder = new PactBuilder()
                .ServiceConsumer(consumerName)
                .HasPactWith(providerName);

            MockProviderService = PactBuilder.MockService(MockServerPort);
        }

        public void Dispose()
        {
            PactDetails details = new PactDetails { Provider = new Pacticipant { Name = providerName }, Consumer = new Pacticipant { Name = _consumerName } };
            var filename = details.GeneratePactFileName();
            var dir = Directory.GetCurrentDirectory();

            PactBuilder.Build();
            Console.WriteLine($"Saving Pact {Path.Combine(dir, filename)}");

            if (!File.Exists("../../pacts/" + filename))
            {
                throw new FileNotFoundException("couldn't find the pact file", filename);
            }

            if (!string.IsNullOrEmpty(pactBrokerUri))
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add("Content-Type", "application/json");
                    client.UploadFile(
                        $"{pactBrokerUri}/pacts/provider/{providerName.Replace(" ", "%20")}/consumer/{_consumerName.Replace(" ", "%20")}/version/{Assembly.GetExecutingAssembly().GetName().Version}",
                        "PUT",
                        $"../../pacts/{filename}");
                }
            }
        }
    }
}