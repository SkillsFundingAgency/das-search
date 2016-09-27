using System;
using System.Collections.Generic;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Sfa.Das.ApprenticeshipInfoService.Client;

namespace Sfa.Das.Web.ApprenticeshipApiTests
{
    [TestFixture]
    public class ApprenticeshipApiConsumerTests
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;
        private ConsumerApprenticeshipApiPact _pact;

        [OneTimeSetUp]
        public void Setup()
        {
            _pact = new ConsumerApprenticeshipApiPact("Find Apprenticeship Training");
            _mockProviderService = _pact.MockProviderService;
            _mockProviderServiceBaseUri = _pact.MockProviderServiceBaseUri;
            _mockProviderService.ClearInteractions();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _pact.Dispose();
        }

        [Test]
        public void ShouldGetaStandard()
        {
            //Arrange
            const int standardCode = 12;
            _mockProviderService.Given($"there is an standard with id '{standardCode}'")
                .UponReceiving($"a request to retrieve standard with id '{standardCode}'")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/standards/" + standardCode,
                    Headers = new Dictionary<string, string>
                    {
                        { "Accept", "application/json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        StandardId = standardCode
                    }
                });

            var consumer = new StandardApiClient(_mockProviderServiceBaseUri);

            //Act
            var result = consumer.Get(standardCode);

            //Assert
            Assert.AreEqual(standardCode, result.StandardId);

            _mockProviderService.VerifyInteractions();
        }
    }
}