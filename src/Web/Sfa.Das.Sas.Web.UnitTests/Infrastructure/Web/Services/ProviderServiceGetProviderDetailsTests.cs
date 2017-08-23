using FluentAssertions;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services
{
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;
    using Sas.Web.Services;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;
    using SFA.DAS.Providers.Api.Client;

    [TestFixture]
    public class ProviderServiceGetProviderDetailsTests
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
            mockProviderApiClient.Setup(x => x.Get(UkPrn)).Returns(provider);
            var providerService = new ProviderService(mockProviderApiClient.Object);
            _actualResult = providerService.GetProviderDetails(UkPrn);
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
                Aliases = new List<string> {"alias 5", "alias 1"},
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
