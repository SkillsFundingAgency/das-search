namespace Sfa.Das.ApplicationServices.UnitTests
{
    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class StandardSearchServiceTest
    {
        [Test]
        public void ShouldCallWithProvidedParameters()
        {
            var search = new Mock<ISearchProvider>();
            search.Setup(m => m.SearchByKeyword(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));

            var standardSearchProvider = new StandardSearchService(search.Object);

            standardSearchProvider.SearchByKeyword("test", 1, 2);

            search.Verify(x => x.SearchByKeyword("test", 1, 2));
        }

        [Test]
        public void ShouldDefaultTakeTo1000When0()
        {
            var search = new Mock<ISearchProvider>();
            search.Setup(m => m.SearchByKeyword(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));

            var standardSearchProvider = new StandardSearchService(search.Object);

            standardSearchProvider.SearchByKeyword("test", 0, 0);

            search.Verify(x => x.SearchByKeyword("test", 0, 1000));
        }
    }
}
