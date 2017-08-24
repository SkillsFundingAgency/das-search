using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Infrastructure.Repositories;
using SFA.DAS.Apprenticeships.Api.Types.Providers;
using SFA.DAS.Providers.Api.Client;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Repositories
{
    [TestFixture]
    public class ProviderRepositoryGetProviderListTests
    {
        private Dictionary<long, string> _actualResult;
        private Dictionary<long, string> _expectedResult;
 
        [SetUp]
        public void Init()
        {
            var mockProviderApiClient = new Mock<IProviderApiClient>();
            const long ukprn = 11000;
            const long ukprn2 = 20;
            const long ukprn3 = 1;

            const string providerName1 = "first item added";
            const string providerName2 = "a second item";
            const string providerName3 = "third company";

            var providerSummaries = new List<ProviderSummary>
            {
                new ProviderSummary { Ukprn = ukprn, ProviderName = providerName1 },
                new ProviderSummary { Ukprn = ukprn2, ProviderName = providerName2 },
                new ProviderSummary { Ukprn = ukprn3, ProviderName = providerName3 }
            };

            _expectedResult = new Dictionary<long, string>
            {
                { ukprn, providerName1 },
                { ukprn2, providerName2 },
                { ukprn3, providerName3 }
            };

            mockProviderApiClient.Setup(x => x.FindAll()).Returns(providerSummaries);
            var providerRepository = new ProviderRepository(mockProviderApiClient.Object);
            _actualResult = providerRepository.GetProviderList();
        }

        [Test]
        public void ShouldProvideTheMatchingNumberOfProviderSummaries()
        {
            Assert.AreEqual(_actualResult, _expectedResult);
        }
    }
}
