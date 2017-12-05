using Sfa.Das.Sas.ApplicationServices.Models;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Controllers
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using ApplicationServices.Queries;
    using ApplicationServices.Responses;
    using FluentAssertions;
    using MediatR;
    using Moq;
    using NUnit.Framework;
    using Sas.Web.Controllers;
    using Sas.Web.Services;
    using SFA.DAS.NLog.Logger;
    using ViewModels;

    [TestFixture]
    public sealed class ApprenticeshipControllerTests
    {
        private ApprenticeshipController _sut;
        private Mock<ILog> _mockLogger;
        private Mock<IMappingService> _mockMappingService;
        private Mock<IMediator> _mockMediator;

        [SetUp]
        public void Init()
        {
            _mockLogger = new Mock<ILog>();
            _mockMappingService = new Mock<IMappingService>();
            _mockMediator = new Mock<IMediator>();

            _sut = new ApprenticeshipController(
                _mockLogger.Object,
                _mockMappingService.Object,
                _mockMediator.Object);
        }

        [Test]
        public void ShouldRedirectIfSearchResultsPageTooHigh()
        {
            _mockMediator.Setup(x => x.Send(It.IsAny<ApprenticeshipSearchQuery>()))
                .Returns(new ApprenticeshipSearchResponse
                {
                    StatusCode = ApprenticeshipSearchResponse.ResponseCodes.PageNumberOutOfUpperBound
                });

            var urlHelper = new Mock<UrlHelper>();

            urlHelper.Setup(x => x.Action(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<RouteValueDictionary>()))
                     .Returns("www.google.co.uk");

            _sut.Url = urlHelper.Object;

            var result = _sut.SearchResults(new ApprenticeshipSearchQuery()) as RedirectResult;

            result.Should().NotBeNull();
        }

        [Test]
        public void ShouldReturnSearchResults()
        {
            var viewModel = new ApprenticeshipSearchResultViewModel();

            _mockMediator.Setup(x => x.Send(It.IsAny<ApprenticeshipSearchQuery>()))
                         .Returns(new ApprenticeshipSearchResponse
                        {
                            StatusCode = ApprenticeshipSearchResponse.ResponseCodes.Success
                        });

            _mockMappingService.Setup(x =>
                x.Map<ApprenticeshipSearchResponse, ApprenticeshipSearchResultViewModel>(It.IsAny<ApprenticeshipSearchResponse>()))
                .Returns(viewModel);

            var result = _sut.SearchResults(new ApprenticeshipSearchQuery()) as ViewResult;

            _mockMediator.Verify(x => x.Send(It.IsAny<ApprenticeshipSearchQuery>()));
            _mockMappingService.Verify(
                x =>
                x.Map<ApprenticeshipSearchResponse, ApprenticeshipSearchResultViewModel>(It.IsAny<ApprenticeshipSearchResponse>()),
                Times.Once);

            result.Model.Should().Be(viewModel);
        }

        [Test]
        public void ShouldReturnResultWhenSearching()
        {
            // act
            var result = _sut.Search() as ViewResult;

            // assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void ShouldReturnStandardNotFound()
        {
            _mockMediator.Setup(x => x.Send(It.IsAny<GetStandardQuery>()))
                .Returns(new GetStandardResponse
                {
                    StatusCode = GetStandardResponse.ResponseCodes.StandardNotFound
                });

            var result = _sut.Standard("2", "test") as HttpNotFoundResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
        }

        [Test]
        public void ShouldReturnViewModelWhenAssessmentOrgsEntityNotFound()
        {
            var viewModel = new StandardViewModel();
            var response = new GetStandardResponse { StatusCode = GetStandardResponse.ResponseCodes.AssessmentOrgsEntityNotFound };
            _mockMediator.Setup(x => x.Send(It.IsAny<GetStandardQuery>()))
                .Returns(response);
            _mockMappingService.Setup(m => m.Map<GetStandardResponse, StandardViewModel>(response))
                .Returns(viewModel);

            var result = _sut.Standard("2", "test") as ViewResult;

            result.Should().NotBeNull();
            result.Model.Should().Be(viewModel);
        }

        [Test]
        public void ShouldReturnStandard()
        {
            // Assign
            var response = new GetStandardResponse();
            var viewModel = new StandardViewModel();

            _mockMediator.Setup(m => m.Send(It.IsAny<GetStandardQuery>()))
                .Returns(response);

            _mockMappingService.Setup(m => m.Map<GetStandardResponse, StandardViewModel>(response))
                .Returns(viewModel);

            // Act
            var result = _sut.Standard("1", "test") as ViewResult;

            // Assert
            _mockMediator.Verify(m => m.Send(It.IsAny<GetStandardQuery>()));
            _mockMappingService.Verify(m => m.Map<GetStandardResponse, StandardViewModel>(response));

            result.Model.Should().Be(viewModel);
        }

        [Test]
        public void ShouldReturnNotFoundIfStandardIdIsBelowZero()
        {
            _mockMediator.Setup(x => x.Send(It.IsAny<GetStandardQuery>()))
                .Returns(new GetStandardResponse
                {
                    StatusCode = GetStandardResponse.ResponseCodes.InvalidStandardId
                });

            var result = _sut.Standard("-2", "test") as HttpNotFoundResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
        }

        [Test]
        public void ShouldReturnFramework()
        {
            // Assign
            var response = new GetFrameworkResponse();
            var viewModel = new FrameworkViewModel();

            _mockMediator.Setup(m => m.Send(It.IsAny<GetFrameworkQuery>()))
                .Returns(response);

            _mockMappingService.Setup(m => m.Map<GetFrameworkResponse, FrameworkViewModel>(response))
                .Returns(viewModel);

            // Act
            var result = _sut.Framework("1", "test") as ViewResult;

            // Assert
            _mockMediator.Verify(m => m.Send(It.IsAny<GetFrameworkQuery>()));
            _mockMappingService.Verify(m => m.Map<GetFrameworkResponse, FrameworkViewModel>(response));

            result.Model.Should().Be(viewModel);
        }

        [Test]
        public void ShouldReturnFrameworkNotFound()
        {
            _mockMediator.Setup(x => x.Send(It.IsAny<GetFrameworkQuery>()))
                .Returns(new GetFrameworkResponse
                {
                    StatusCode = GetFrameworkResponse.ResponseCodes.FrameworkNotFound
                });

            var result = _sut.Framework("2", "test") as HttpNotFoundResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
        }

        [Test]
        public void ShouldReturnNotFoundIfFrameworkIdBelowZero()
        {
            _mockMediator.Setup(x => x.Send(It.IsAny<GetFrameworkQuery>()))
                .Returns(new GetFrameworkResponse
                {
                    StatusCode = GetFrameworkResponse.ResponseCodes.InvalidFrameworkId
                });

            var result = _sut.Framework("2", "test") as HttpNotFoundResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
        }

        [Test]
        public void ShouldReturnStandardProviders()
        {
            var response = new GetStandardProvidersResponse
            {
                StatusCode = GetStandardProvidersResponse.ResponseCodes.Success
            };

            _mockMediator.Setup(x => x.Send(It.IsAny<GetStandardProvidersQuery>())).Returns(response);

            _mockMappingService.Setup(x =>
                x.Map<GetStandardProvidersResponse, ProviderSearchViewModel>(response))
                .Returns(new ProviderSearchViewModel());

            _sut.SearchForStandardProviders("2", ProviderSearchResponseCodes.Success, "AB12 3CD", "test", string.Empty, null);

            _mockMediator.Verify(x => x.Send(It.IsAny<GetStandardProvidersQuery>()), Times.Once);

            _mockMappingService.Verify(
                x => x.Map<GetStandardProvidersResponse, ProviderSearchViewModel>(response), Times.Once);
        }

        [Test]
        public void ShouldReturnStandardProvidersNotFound()
        {
            var response = new GetStandardProvidersResponse
            {
                StatusCode = GetStandardProvidersResponse.ResponseCodes.NoStandardFound
            };

            _mockMediator.Setup(x => x.Send(It.IsAny<GetStandardProvidersQuery>())).Returns(response);

            _mockMappingService.Setup(x => x.Map<GetStandardProvidersResponse, ProviderSearchViewModel>(
                It.IsAny<GetStandardProvidersResponse>()));

            var result = _sut.SearchForStandardProviders("2", ProviderSearchResponseCodes.Success, "AB12 3CD", "test", string.Empty, null) as HttpNotFoundResult;

            _mockMediator.Verify(x => x.Send(It.IsAny<GetStandardProvidersQuery>()), Times.Once);

            _mockMappingService.Verify(
                x => x.Map<GetStandardProvidersResponse, ProviderSearchViewModel>(
                    It.IsAny<GetStandardProvidersResponse>()), Times.Never);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
        }

        [Test]
        public void ShouldReturnFrameworkProviders()
        {
            // Assign
            var response = new GetFrameworkProvidersResponse
            {
                StatusCode = GetFrameworkProvidersResponse.ResponseCodes.Success
            };

            _mockMediator.Setup(x => x.Send(It.IsAny<GetFrameworkProvidersQuery>()))
                .Returns(response);

            _mockMappingService.Setup(x =>
                x.Map<GetFrameworkProvidersResponse, ProviderSearchViewModel>(response))
                .Returns(new ProviderSearchViewModel());

            // Act
            _sut.SearchForFrameworkProviders("2", ProviderSearchResponseCodes.Success, "AB12 3CD", "test", string.Empty, null);

            // Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<GetFrameworkProvidersQuery>()), Times.Once);

            _mockMappingService.Verify(
                x => x.Map<GetFrameworkProvidersResponse, ProviderSearchViewModel>(response), Times.Once);
        }

        [Test]
        public void ShouldReturnFrameworkProvidersNotFound()
        {
            var response = new GetFrameworkProvidersResponse
            {
                StatusCode = GetFrameworkProvidersResponse.ResponseCodes.NoFrameworkFound
            };

            _mockMediator.Setup(x => x.Send(It.IsAny<GetFrameworkProvidersQuery>())).Returns(response);

            _mockMappingService.Setup(x => x.Map<GetFrameworkProvidersResponse, ProviderSearchViewModel>(
                It.IsAny<GetFrameworkProvidersResponse>()));

            var result = _sut.SearchForFrameworkProviders("2", ProviderSearchResponseCodes.Success, "AB12 3CD", "test", string.Empty, null) as HttpNotFoundResult;

            _mockMediator.Verify(x => x.Send(It.IsAny<GetFrameworkProvidersQuery>()), Times.Once);

            _mockMappingService.Verify(
                x => x.Map<GetFrameworkProvidersResponse, ProviderSearchViewModel>(
                    It.IsAny<GetFrameworkProvidersResponse>()), Times.Never);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
        }
    }
}
