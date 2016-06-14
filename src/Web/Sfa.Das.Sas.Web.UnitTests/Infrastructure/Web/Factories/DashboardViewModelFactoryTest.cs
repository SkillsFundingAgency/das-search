using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sfa.Das.Sas.Web.Factories;
using Sfa.Das.Sas.Web.ViewModels;
using Assert = NUnit.Framework.Assert;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Factories
{
    [TestFixture]
    public class DashboardViewModelFactoryTest
    {
        private DashboardViewModelFactory _sut;

        [SetUp]
        public void Init()
        {
            _sut = new DashboardViewModelFactory();
        }

        [Test]
        public void ShouldGetShortlistFrameworkViewModel()
        {
            // Assign
            var standardViewModel = new ShortlistStandardViewModel();
            var frameworkViewModel = new ShortlistFrameworkViewModel();
            var standardViewModels = new List<ShortlistStandardViewModel> { standardViewModel };
            var frameworkViewModels = new List<ShortlistFrameworkViewModel> { frameworkViewModel };

            // Act
            var viewModel = _sut.GetDashboardViewModel(standardViewModels, frameworkViewModels);

            // Assert
            Assert.Contains(standardViewModel, viewModel.Apprenticeships.ToList());
            Assert.Contains(frameworkViewModel, viewModel.Apprenticeships.ToList());
        }
    }
}
