using NUnit.Framework;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Web.Factories;
using Assert = NUnit.Framework.Assert;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Factories
{
    [TestFixture]
    public class ShortlistViewModelFactoryTest
    {
        private ShortlistViewModelFactory _sut;

        [SetUp]
        public void Init()
        {
            _sut = new ShortlistViewModelFactory();
        }

        [Test]
        public void ShouldGetShortlistStandardViewModel()
        {
            // Assign
            var standard = new Standard
            {
                StandardId = 10,
                Title = "test standard",
                Level = 2
            };

            // Act
            var viewModel = _sut.GetShortlistViewModel(standard);

            // Assert
            Assert.AreEqual(standard.StandardId, viewModel.Id);
            Assert.AreEqual(standard.Title, viewModel.Title);
            Assert.AreEqual(standard.Level, viewModel.Level);
        }

        [Test]
        public void ShouldGetShortlistFrameworkViewModel()
        {
            // Assign
            var framework = new Framework
            {
                FrameworkId = 10,
                Title = "test framework",
                Level = 2
            };

            // Act
            var viewModel = _sut.GetShortlistViewModel(framework);

            // Assert
            Assert.AreEqual(framework.FrameworkId, viewModel.Id);
            Assert.AreEqual(framework.Title, viewModel.Title);
            Assert.AreEqual(framework.Level, viewModel.Level);
        }
    }
}
