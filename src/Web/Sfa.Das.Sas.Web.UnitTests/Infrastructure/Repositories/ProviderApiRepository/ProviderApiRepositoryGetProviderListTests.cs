namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;

    [TestFixture]
    public class ProviderApiRepositoryGetProviderListTests : ProviderApiRepositoryBase
    {
        private List<ProviderSummary> _actualResult;
        private List<ProviderSummary> _expectedResult;


        [SetUp]
        public void Init()
        {
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

            _expectedResult = providerSummaries;

            _mockProviderApiClient.Setup(x => x.FindAll()).Returns((IEnumerable<ProviderSummary>)providerSummaries);
        }

        [Test]
        public void ShouldProvideTheMatchingNumberOfProviderSummaries()
        {
            var res = _sut.GetAllProviders();
            _actualResult = res.ToList();
            Assert.AreEqual(_actualResult, _expectedResult);
        }
    }
}
