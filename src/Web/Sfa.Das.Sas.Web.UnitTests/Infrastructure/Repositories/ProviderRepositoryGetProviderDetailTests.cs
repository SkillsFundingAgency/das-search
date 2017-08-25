namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Moq;
    using NUnit.Framework;
    using Sas.Infrastructure.Repositories;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;
    using SFA.DAS.Providers.Api.Client;

    [TestFixture]
    public class ProviderRepositoryGetProviderDetailTests
    {
        private const long UkPrn = 11000;
        private Provider _actualResult;
        private Provider _expectedResult;

        [SetUp]
        public void Init()
        {

            var provider = GetProvider();
            var mockProviderApiClient = new Mock<IProviderApiClient>();
            _expectedResult = provider;
            mockProviderApiClient.Setup(x => x.GetAsync(UkPrn)).Returns(Task.FromResult(provider));
            var providerRepository = new ProviderDetailRepository(mockProviderApiClient.Object);
            _actualResult = providerRepository.GetProviderDetails(UkPrn).Result;
        }

        [Test]
        public void ShouldProvideTheMatchingNumberOfProviderSummaries()
        {
            Assert.AreEqual(_actualResult, _expectedResult);
        }

        private static Provider GetProvider()
        {
            return new Provider
            {
                Aliases = new List<string> { "alias 5", "alias 1" },
                EmployerSatisfaction = 0,
                LearnerSatisfaction = 0,
                Email = "test@test.co.uk",
                IsEmployerProvider = true,
                IsHigherEducationInstitute = true,
                NationalProvider = true,
                Phone = "553434",
                Ukprn = UkPrn,
                ProviderName = "The Fire Brigade",
                Uri = "http://www.tester.com/stuff",
                Website = "http://www.tester.com"
            };
        }

    }
}
