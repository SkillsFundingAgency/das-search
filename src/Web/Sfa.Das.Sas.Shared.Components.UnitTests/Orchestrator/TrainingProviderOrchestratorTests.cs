using System.Threading;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Shared.Components.Mapping;
using Sfa.Das.Sas.Shared.Components.Orchestrators;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;
using Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider;
using Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider.Search;
using Sfa.Das.Sas.Shared.Components.ViewModels;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.Orchestrator
{
    [TestFixture]
    public class TrainingProviderOrchestratorTests
    {
        private TrainingProviderOrchestrator _sut;
        private Mock<IMediator> _mockMediator;
        private Mock<ISearchResultsViewModelMapper> _mockSearchResultsViewModelMapper;
        private Mock<ITrainingProviderDetailsViewModelMapper> _mockTrainingProviderDetailsViewModelMapper;
        private Mock<ITrainingProviderSearchFilterViewModelMapper> _mockTrainingProviderFilterViewModelMapper;
        private Mock<ITrainingProviderClosestLocationsViewModelMapper> _mockTrainingProviderClosestLocationsViewModelMapper;
        private Mock<ILog> _mockLogger;

        private  TrainingProviderSearchViewModel _searchQueryViewModel = new TrainingProviderSearchViewModel();

        private GroupedProviderSearchResponse _searchResults = new GroupedProviderSearchResponse() { Success = true };
        private ProviderSearchResponse _searchResultsError = new ProviderSearchResponse(){Success = false,StatusCode = ProviderSearchResponseCodes.PostCodeInvalidFormat};
        private SearchResultsViewModel<TrainingProviderSearchResultsItem, TrainingProviderSearchViewModel> _searchResultsViewModel = new SearchResultsViewModel<TrainingProviderSearchResultsItem, TrainingProviderSearchViewModel>();
        private TrainingProviderSearchFilterViewModel _searchFilterViewModel = new TrainingProviderSearchFilterViewModel();
        private ClosestLocationsViewModel _closestLocationsViewModel = new ClosestLocationsViewModel();

        [SetUp]
        public void Setup()
        {

            _mockMediator = new Mock<IMediator>();
            _mockSearchResultsViewModelMapper = new Mock<ISearchResultsViewModelMapper>();
            _mockTrainingProviderDetailsViewModelMapper = new Mock<ITrainingProviderDetailsViewModelMapper>();
            _mockTrainingProviderFilterViewModelMapper = new Mock<ITrainingProviderSearchFilterViewModelMapper>();
            _mockTrainingProviderClosestLocationsViewModelMapper = new Mock<ITrainingProviderClosestLocationsViewModelMapper>();
            _mockLogger = new Mock<ILog>();

            _mockMediator.Setup(s => s.Send<GroupedProviderSearchResponse>(It.IsAny<GroupedProviderSearchQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(_searchResults);
            _mockSearchResultsViewModelMapper.Setup(s => s.Map(It.IsAny<GroupedProviderSearchResponse>(), It.IsAny<TrainingProviderSearchViewModel>())).Returns(_searchResultsViewModel);
            _mockTrainingProviderFilterViewModelMapper.Setup(s => s.Map(It.IsAny<GroupedProviderSearchResponse>(), It.IsAny<TrainingProviderSearchViewModel>())).Returns(_searchFilterViewModel);
            _mockTrainingProviderClosestLocationsViewModelMapper.Setup(s => s.Map(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<GetClosestLocationsResponse>())).Returns(_closestLocationsViewModel);

            _sut = new TrainingProviderOrchestrator(_mockMediator.Object, _mockSearchResultsViewModelMapper.Object,_mockLogger.Object,_mockTrainingProviderDetailsViewModelMapper.Object,_mockTrainingProviderFilterViewModelMapper.Object, _mockTrainingProviderClosestLocationsViewModelMapper.Object);
        }

        [Test]
        public void When_SearchResultsRequested_Then_TrainingProiderSearchResultsViewModel_Is_Returned()
        {
            _searchQueryViewModel.Keywords = "keyword";
            _searchQueryViewModel.ApprenticeshipId = "123";
            _searchQueryViewModel.Postcode = "NN123NN";

            var result = _sut.GetSearchResults(_searchQueryViewModel).Result;

            result.Should().BeOfType<SearchResultsViewModel<TrainingProviderSearchResultsItem, TrainingProviderSearchViewModel>>();
        }

        [Test]
        public void When_SearchResultsRequested_Then_TrainingProvider_Are_Searched_By_Apprenticeship_And_Location()
        {
            _searchQueryViewModel.Keywords = "keyword";

            var result = _sut.GetSearchResults(_searchQueryViewModel);

            _mockMediator.Verify(s => s.Send<GroupedProviderSearchResponse>(It.IsAny<GroupedProviderSearchQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public void When_SearchResultsRequested_Then_Search_Results_Are_Mapped_To_ViewModel()
        {
            _searchQueryViewModel.Keywords = "keyword";

            var result = _sut.GetSearchResults(_searchQueryViewModel).Result;

            result.Should().BeOfType<SearchResultsViewModel<TrainingProviderSearchResultsItem, TrainingProviderSearchViewModel>>();

            _mockSearchResultsViewModelMapper.Verify(v => v.Map(_searchResults, _searchQueryViewModel));
        }

        [Test]
        public void When_SearchFilterRequested_Then_TrainingProiderSearchResultsViewModel_Is_Returned()
        {
            _searchQueryViewModel.Keywords = "keyword";
            _searchQueryViewModel.ApprenticeshipId = "123";
            _searchQueryViewModel.Postcode = "NN123NN";

            var result = _sut.GetSearchFilter(_searchQueryViewModel).Result;

            result.Should().BeOfType<TrainingProviderSearchFilterViewModel>();
        }

        [Test]
        public void When_SearchFilterRequested_Then_TrainingProvider_Are_Searched_By_Apprenticeship_And_Location()
        {
            _searchQueryViewModel.Keywords = "keyword";

            var result = _sut.GetSearchFilter(_searchQueryViewModel);

            _mockMediator.Verify(s => s.Send<GroupedProviderSearchResponse>(It.IsAny<GroupedProviderSearchQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public void When_SearchFilterRequested_Then_Search_Results_Are_Mapped_To_ViewModel()
        {
            _searchQueryViewModel.Keywords = "keyword";

            var result = _sut.GetSearchFilter(_searchQueryViewModel).Result;

            result.Should().BeOfType<TrainingProviderSearchFilterViewModel>();

            _mockTrainingProviderFilterViewModelMapper.Verify(v => v.Map(_searchResults, _searchQueryViewModel));
        }

        [Test]
        public async Task When_ClosestLocationsRequested_Then_ClosestLocationsViewModel_Is_Returned()
        {
            var result = await _sut.GetClosestLocations("123", 12345678, "AB12 3DF");

            result.Should().BeOfType<ClosestLocationsViewModel>();
        }

        [Test]
        public void When_ClosestLocationsRequested_Then_Mediator_Is_Called()
        {
            var result = _sut.GetClosestLocations("123", 12345678, "AB12 3DF");

            _mockMediator.Verify(s => s.Send<GetClosestLocationsResponse>(It.IsAny<GetClosestLocationsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task When_ClosestLocationsRequested_Then_Search_Results_Are_Mapped_To_ViewModel()
        {
            var result = await _sut.GetClosestLocations("123", 12345678, "AB12 3DF");

            _mockTrainingProviderClosestLocationsViewModelMapper.Verify(v => v.Map("123", 12345678, "AB12 3DF", It.IsAny<GetClosestLocationsResponse>()));
        }
    }
}
