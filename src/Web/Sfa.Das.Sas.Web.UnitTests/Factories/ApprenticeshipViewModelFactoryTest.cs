namespace Sfa.Das.Sas.Web.UnitTests.Factories
{
    using System.Web.Mvc;
    using Core.Domain.Model;
    using Core.Domain.Services;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    using Web.Factories;

    [TestFixture]
    public class ApprenticeshipViewModelFactoryTest
    {
        private Mock<UrlHelper> _mockUrlHelper;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _mockUrlHelper = new Mock<UrlHelper>();

            _mockUrlHelper.Setup(m => m.Action("StandardResults", "Provider")).Returns("/hello/standard");
            _mockUrlHelper.Setup(m => m.Action("Standard", "Apprenticeship", It.IsAny<object>())).Returns("/hello/StandardPrevLink/id");

            _mockUrlHelper.Setup(m => m.Action("FrameworkResults", "Provider")).Returns("/hello/framework");
            _mockUrlHelper.Setup(m => m.Action("Framework", "Apprenticeship", It.IsAny<object>())).Returns("/hello/FrameworkPrevLink/id");
        }

        [Test]
        public void WhenCreatingAStandardViewModel()
        {
            var mockGetStandards = new Mock<IGetStandards>();
            mockGetStandards.Setup(m => m.GetStandardById(1)).Returns(new Standard { Title = "Standard1", StandardId = 1, NotionalEndLevel = 4 });

            var factory = new ApprenticeshipViewModelFactory(mockGetStandards.Object, null, null);

            var viewModel = factory.GetProviderSearchViewModelForStandard(1, _mockUrlHelper.Object);

            viewModel.PostUrl.Should().Be("/hello/standard");
            viewModel.PreviousPageLink.Title.Should().Be("Go back to apprenticeship");
            viewModel.PreviousPageLink.Url.Should().Be("/hello/StandardPrevLink/id");

            viewModel.ApprenticeshipId.Should().Be(1);
            viewModel.HasError.Should().BeFalse();
            viewModel.Title.Should().Be("Standard1, level 4");
        }

        [Test]
        public void WhenCreatingAFrameworkViewModel()
        {
            var mockGetFrameworks = new Mock<IGetFrameworks>();

            mockGetFrameworks.Setup(m => m.GetFrameworkById(1254)).Returns(new Framework { Title = "Framework 1254", FrameworkId = 1254, Level = 6 });

            var factory = new ApprenticeshipViewModelFactory(null, mockGetFrameworks.Object, null);

            var viewModel = factory.GetFrameworkProvidersViewModel(1254, _mockUrlHelper.Object);

            viewModel.PostUrl.Should().Be("/hello/framework");
            viewModel.PreviousPageLink.Title.Should().Be("Go back to apprenticeship");
            viewModel.PreviousPageLink.Url.Should().Be("/hello/FrameworkPrevLink/id");

            viewModel.ApprenticeshipId.Should().Be(1254);
            viewModel.HasError.Should().BeFalse();
            viewModel.Title.Should().Be("Framework 1254, level 6");
        }

        [Test]
        public void WhenNoFrameworkIsFound()
        {
            var mockGetFrameworks = new Mock<IGetFrameworks>();
            var factory = new ApprenticeshipViewModelFactory(null, mockGetFrameworks.Object, null);

            var result = factory.GetFrameworkViewModel(1);
            result.Should().BeNull();
        }

        [Test]
        public void WhenNoStandardIsFound()
        {
            var mockGetStandards = new Mock<IGetStandards>();
            var factory = new ApprenticeshipViewModelFactory(mockGetStandards.Object, null, null);

            var result = factory.GetStandardViewModel(1);
            result.Should().BeNull();
        }
    }
}