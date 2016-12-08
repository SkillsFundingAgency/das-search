using System.Collections.Generic;
using System.Net.Http;
using NUnit.Framework;
using PactNet.Mocks.MockHttpService.Models;
using PactNet.TestExtensions;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types.Exceptions;

namespace Sfa.Das.Web.Pacts
{
    [TestFixture]
    [PactProvider("Apprenticeship Programs API")]
    public class ApprenticeshipApiClientTests : PactTestBase
    {
        [Test]
        public void ShouldGetaStandard()
        {
            //Arrange
            const string standardCode = "12";
            MockProviderService
                .UponReceiving($"a request to retrieve standard with id '{standardCode}'")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = $"/standards/{standardCode}",
                    Headers = new Dictionary<string, string>
                    {
                        {"Accept", "application/json"}
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

            var consumer = new StandardApiClient(MockProviderServiceBaseUri);

            //Act
            var result = consumer.Get(standardCode); // TODO is this needed?

            //Assert
            Assert.AreEqual(standardCode, result.StandardId);

            MockProviderService.VerifyInteractions();
        }

        [Test]
        public void ShouldSeeaStandardIsMissing()
        {
            //Arrange
            const string standardCode = "-1";
            MockProviderService
                .UponReceiving($"a request to retrieve standard with id '{standardCode}'")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = $"/standards/{standardCode}",
                    Headers = new Dictionary<string, string>
                    {
                        {"Accept", "application/json"}
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 404
                });

            var consumer = new StandardApiClient(MockProviderServiceBaseUri);

            //Act
            Assert.Throws<EntityNotFoundException>(() => consumer.Get(standardCode));

            MockProviderService.VerifyInteractions();
        }

        [Test]
        [Ignore("Dependant on Provider state")]
        public void ShouldGetFramework()
        {
            //Arrange
            const string frameworkId = "403-2-1";
            MockProviderService
                .UponReceiving($"a request to retrieve framework with id '{frameworkId}'")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = $"/frameworks/{frameworkId}",
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
                        FrameworkId = frameworkId
                    }
                });

            var consumer = new FrameworkApiClient(MockProviderServiceBaseUri);

            //Act
            var result = consumer.Get(frameworkId); // TODO is this needed?

            //Assert
            Assert.AreEqual(frameworkId, result.FrameworkId);

            MockProviderService.VerifyInteractions();
        }

        [Test]
        public void ShouldSeeAFrameworkIsMissing()
        {
            //Arrange
            const string frameworkId = "1-2-3";
            MockProviderService
                .UponReceiving($"a request to retrieve framework with id '{frameworkId}'")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = $"/frameworks/{frameworkId}",
                    Headers = new Dictionary<string, string>
                    {
                        { "Accept", "application/json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 404
                });

            var consumer = new FrameworkApiClient(MockProviderServiceBaseUri);

            //Act
            Assert.Throws<EntityNotFoundException>(() => consumer.Get(frameworkId));

            MockProviderService.VerifyInteractions();
        }
    }
}