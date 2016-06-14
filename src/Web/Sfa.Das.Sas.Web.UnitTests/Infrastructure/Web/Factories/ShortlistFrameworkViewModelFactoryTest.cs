using NUnit.Framework;
using Sfa.Das.Sas.Web.Factories;
using Assert = NUnit.Framework.Assert;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Factories
{
    [TestFixture]
    public class ShortlistFrameworkViewModelFactoryTest
    {
        private ShortlistFrameworkViewModelFactory _sut;

        [SetUp]
        public void Init()
        {
            _sut = new ShortlistFrameworkViewModelFactory();
        }

        [Test]
        public void ShouldGetShortlistFrameworkViewModel()
        {
            // Assign
            const int id = 10;
            const string title = "test framework";
            const int level = 2;

            // Act
            var viewModel = _sut.GetShortlistFrameworkViewModel(id, title, level);

            // Assert
            Assert.AreEqual(id, viewModel.Id);
            Assert.AreEqual(title, viewModel.Title);
            Assert.AreEqual(level, viewModel.Level);
        }
    }
}
