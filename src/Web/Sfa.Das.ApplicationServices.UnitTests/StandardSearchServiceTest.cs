namespace Sfa.Das.ApplicationServices.UnitTests
{
    using Models;
    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public sealed class StandardSearchServiceTest
    {
        [Test]
        public void ShouldCallWithProvidedParameters()
        {
            var search = new Mock<ISearchProvider>();
            search.Setup(m => m.SearchByKeyword(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ApprenticeshipSearchSortBy>()));

            var standardSearchProvider = new ApprenticeshipSearchService(search.Object);

            standardSearchProvider.SearchByKeyword("test", 1, 2);

            search.Verify(x => x.SearchByKeyword("test", 1, 2, It.IsAny<ApprenticeshipSearchSortBy>()));
        }

        [Test]
        public void ShouldRequestThatSearchIsOrderedByStandardsFirst()
        {
            var search = new Mock<ISearchProvider>();
            search.Setup(m => m.SearchByKeyword(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), ApprenticeshipSearchSortBy.StandardsFirst));

            var standardSearchProvider = new ApprenticeshipSearchService(search.Object);

            standardSearchProvider.SearchByKeyword("test", 1, 2);

            search.Verify(x => x.SearchByKeyword("test", 1, 2, ApprenticeshipSearchSortBy.StandardsFirst));
        }

        [Test]
        public void ShouldDefaultTakeTo1000When0()
        {
            var search = new Mock<ISearchProvider>();
            search.Setup(m => m.SearchByKeyword(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ApprenticeshipSearchSortBy>()));

            var standardSearchProvider = new ApprenticeshipSearchService(search.Object);

            standardSearchProvider.SearchByKeyword("test", 0, 0);

            search.Verify(x => x.SearchByKeyword("test", 0, 1000, It.IsAny<ApprenticeshipSearchSortBy>()));
        }
    }
}
