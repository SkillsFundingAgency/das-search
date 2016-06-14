using NUnit.Framework;
using Sfa.Das.Sas.Web.Factories;
using Assert = NUnit.Framework.Assert;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Factories
{
    [TestFixture]
    public class ShortlistStandardViewModelFactoryTest
    {
        private ShortlistStandardViewModelFactory _sut;

        [SetUp]
        public void Init()
        {
            _sut = new ShortlistStandardViewModelFactory();
        }

        [Test]
        public void ShouldGetShortlistFrameworkViewModel()
        {
            // Assign
            const int id = 10;
            const string title = "test standard";
            const int level = 2;

            // Act
            var viewModel = _sut.GetShortlistStandardViewModel(id, title, level);

            // Assert
            Assert.AreEqual(id, viewModel.Id);
            Assert.AreEqual(title, viewModel.Title);
            Assert.AreEqual(level, viewModel.Level);
        }
    }
}
