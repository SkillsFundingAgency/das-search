using System;
using System.Configuration;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService;

namespace PactNet.TestExtensions
{
    public abstract class PactTestBase
    {
        private string _providerName => PactProviderAttribute.GetProviderName(this);
        private string _pactBrokerUri => ConfigurationFinder.OptionalSetting("PactBrokerUri");
        private string _consumerName => ConfigurationFinder.RequiredSetting("PactConsumerName");

        private IPactBuilder PactBuilder { get; set; }
        public IMockProviderService MockProviderService { get; private set; }
        public int MockServerPort => 88;
        public string MockProviderServiceBaseUri => $"http://localhost:{MockServerPort}";

        [OneTimeSetUp]
        public void Setup()
        {
            PactBuilder = new PactBuilder()
                .ServiceConsumer(_consumerName)
                .HasPactWith(_providerName);

            MockProviderService = PactBuilder.MockService(MockServerPort);
            MockProviderService.ClearInteractions();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            PactBuilder.Build();

            if (!string.IsNullOrEmpty(_pactBrokerUri))
            {
                using (var client = new PactBrokerClient())
                {
                    client.Upload(_pactBrokerUri, _providerName, _consumerName);
                }
            }
        }
    }
}