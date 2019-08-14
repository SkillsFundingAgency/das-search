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
        private Mock<ILog> _mockLogger;

        private  TrainingProviderSearchViewModel _searchQueryViewModel = new TrainingProviderSearchViewModel();

        private ProviderSearchResponse _searchResults = new ProviderSearchResponse() { Success = true };
        private ProviderSearchResponse _searchResultsError = new ProviderSearchResponse(){Success = false,StatusCode = ProviderSearchResponseCodes.PostCodeInvalidFormat};
        private SearchResultsViewModel<TrainingProviderSearchResultsItem, TrainingProviderSearchViewModel> _searchResultsViewModel = new SearchResultsViewModel<TrainingProviderSearchResultsItem, TrainingProviderSearchViewModel>();

        [SetUp]
        public void Setup()
        {

            _mockMediator = new Mock<IMediator>();
            _mockSearchResultsViewModelMapper = new Mock<ISearchResultsViewModelMapper>();
            _mockTrainingProviderDetailsViewModelMapper = new Mock<ITrainingProviderDetailsViewModelMapper>();
            _mockTrainingProviderFilterViewModelMapper = new Mock<ITrainingProviderSearchFilterViewModelMapper>();
            _mockLogger = new Mock<ILog>();

            _mockMediator.Setup(s => s.Send<ProviderSearchResponse>(It.IsAny<ProviderSearchQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(_searchResults);
            _mockSearchResultsViewModelMapper.Setup(s => s.Map(It.IsAny<ProviderSearchResponse>(), It.IsAny<TrainingProviderSearchViewModel>())).Returns(_searchResultsViewModel);

            _sut = new TrainingProviderOrchestrator(_mockMediator.Object, _mockSearchResultsViewModelMapper.Object,_mockLogger.Object,_mockTrainingProviderDetailsViewModelMapper.Object,_mockTrainingProviderFilterViewModelMapper.Object);
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

            _mockMediator.Verify(s => s.Send<ProviderSearchResponse>(It.IsAny<ProviderSearchQuery>(), It.IsAny<CancellationToken>()), Times.Once);
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

            _mockMediator.Verify(s => s.Send<ProviderSearchResponse>(It.IsAny<ProviderSearchQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public void When_SearchFilterRequested_Then_Search_Results_Are_Mapped_To_ViewModel()
        {
            _searchQueryViewModel.Keywords = "keyword";

            var result = _sut.GetSearchFilter(_searchQueryViewModel).Result;

            result.Should().BeOfType<TrainingProviderSearchFilterViewModel>();

            _mockSearchResultsViewModelMapper.Verify(v => v.Map(_searchResults, _searchQueryViewModel));
        }
    }
}
