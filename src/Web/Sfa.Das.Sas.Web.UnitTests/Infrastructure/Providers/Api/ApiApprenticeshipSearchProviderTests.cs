namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Providers.Api
{
    using Moq;
    using NUnit.Framework;
    using SFA.DAS.Apprenticeships.Api.Client;
    using Sfa.Das.Sas.Infrastructure.Mapping;
    using Sfa.Das.Sas.Infrastructure.Providers;

    public class ApiApprenticeshipSearchProviderTests
    {
        private Mock<IApprenticeshipProgrammeApiClient> _apprenticeshipProgrammeApiClientMock;
        private Mock<IApprenticeshipSearchResultsMapping> _apprenticeshipSearchResultsMappingMock;

        private ApprenticeshipsSearchApiProvider _sut;

        [SetUp]
        public void Setup()
        {
            _apprenticeshipProgrammeApiClientMock = new Mock<IApprenticeshipProgrammeApiClient>();
            _apprenticeshipSearchResultsMappingMock = new Mock<IApprenticeshipSearchResultsMapping>();

            _sut = new ApprenticeshipsSearchApiProvider(_apprenticeshipProgrammeApiClientMock.Object,_apprenticeshipSearchResultsMappingMock.Object);
        }
    }
}