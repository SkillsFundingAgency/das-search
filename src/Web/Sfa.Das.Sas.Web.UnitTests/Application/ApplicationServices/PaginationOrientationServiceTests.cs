using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Services;

namespace Sfa.Das.Sas.Web.UnitTests.Application.ApplicationServices
{
    [TestFixture]
    public class PaginationOrientationServiceTests
    {
        [TestCase(1, 20, 30, 0, 1, 2)]
        [TestCase(0, 20, 30, 0, 1, 2)]
        [TestCase(1, 20, 15, 0, 1, 1)]
        [TestCase(2, 20, 30, 20, 2, 2)]
        [TestCase(2, 20, 50, 20, 2, 3)]
        [TestCase(2, 20, 40, 20, 2, 2)]
        [TestCase(2, 20, 41, 20, 2, 3)]
        [TestCase(3, 20, 41, 40, 3, 3)]
        [TestCase(3, 20, 40, 20, 2, 2)]

        public void ShouldReturnExpectedPaginationResultsFromParameters(int page, int take, long totalHits, int skip, int currentPage, int lastPage)
        {
            var res = new PaginationOrientationService().GeneratePaginationDetails(page, take, totalHits);
            Assert.AreEqual(skip, res.Skip);
            Assert.AreEqual(lastPage, res.LastPage);
            Assert.AreEqual(currentPage, res.CurrentPage);
        }
    }
}
