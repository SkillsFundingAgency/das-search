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
using Sfa.Das.Sas.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;

using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;

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
        private Mock<ICacheStorageService> _mockCacheService;
        private Mock<TrainingProviderDetailQueryViewModel> _mockTrainingProviderDetailQueryViewModel;

        private Mock<IMemoryCache> _mockMemoryCache;
        private Mock<IDistributedCache> _mockDistributedCache;

        private TrainingProviderSearchViewModel _searchQueryViewModel = new TrainingProviderSearchViewModel();
        private TrainingProviderDetailQueryViewModel _detailsQueryViewModel = new TrainingProviderDetailQueryViewModel();

        private ProviderSearchResponse _searchResults = new ProviderSearchResponse() { Success = true };
        private ProviderSearchResponse _searchResultsError = new ProviderSearchResponse(){Success = false,StatusCode = ProviderSearchResponseCodes.PostCodeInvalidFormat};
        private SearchResultsViewModel<TrainingProviderSearchResultsItem, TrainingProviderSearchViewModel> _searchResultsViewModel = new SearchResultsViewModel<TrainingProviderSearchResultsItem, TrainingProviderSearchViewModel>();
        private TrainingProviderSearchFilterViewModel _searchFilterViewModel = new TrainingProviderSearchFilterViewModel();
        private ApprenticeshipProviderDetailResponse _providerDetailResponse = new ApprenticeshipProviderDetailResponse() { StatusCode = ApprenticeshipProviderDetailResponse.ResponseCodes.Success };
        [SetUp]
        public void Setup()
        {

            _mockMediator = new Mock<IMediator>();
            _mockSearchResultsViewModelMapper = new Mock<ISearchResultsViewModelMapper>();
            _mockTrainingProviderDetailsViewModelMapper = new Mock<ITrainingProviderDetailsViewModelMapper>();
            _mockTrainingProviderFilterViewModelMapper = new Mock<ITrainingProviderSearchFilterViewModelMapper>();
            _mockLogger = new Mock<ILog>();


            _mockCacheService = new Mock<ICacheStorageService>();
            _mockTrainingProviderDetailQueryViewModel = new Mock<TrainingProviderDetailQueryViewModel>();

            _mockMediator.Setup(s => s.Send<ProviderSearchResponse>(It.IsAny<ProviderSearchQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(_searchResults);
            _mockSearchResultsViewModelMapper.Setup(s => s.Map(It.IsAny<ProviderSearchResponse>(), It.IsAny<TrainingProviderSearchViewModel>())).Returns(_searchResultsViewModel);
            _mockTrainingProviderFilterViewModelMapper.Setup(s => s.Map(It.IsAny<ProviderSearchResponse>(), It.IsAny<TrainingProviderSearchViewModel>())).Returns(_searchFilterViewModel);

            _detailsQueryViewModel.ApprenticeshipId = "123";
            _detailsQueryViewModel.Ukprn = 10000020;
            _detailsQueryViewModel.LocationId = 100;

            var cacheKey = _detailsQueryViewModel.Ukprn + _detailsQueryViewModel.LocationId + _detailsQueryViewModel.ApprenticeshipId;

            _sut = new TrainingProviderOrchestrator(_mockMediator.Object, _mockSearchResultsViewModelMapper.Object,_mockLogger.Object,_mockTrainingProviderDetailsViewModelMapper.Object,_mockTrainingProviderFilterViewModelMapper.Object, _mockCacheService.Object);
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

            _mockTrainingProviderFilterViewModelMapper.Verify(v => v.Map(_searchResults, _searchQueryViewModel));
        }

        [Test]
        public void When_SearchResultsRequested_Then_CallsMediatrWhenCacheIsEmpty()
        {

            var cacheKey = _detailsQueryViewModel.Ukprn + _detailsQueryViewModel.LocationId + _detailsQueryViewModel.ApprenticeshipId;

            _mockCacheService.Setup(s => s.RetrieveFromCache<ApprenticeshipProviderDetailResponse>(cacheKey)).Returns(GenerateNullCacheObject());

            var result = _sut.GetDetails(_detailsQueryViewModel);

            _mockMediator.Verify(s => s.Send<ApprenticeshipProviderDetailResponse>(It.IsAny<ApprenticeshipProviderDetailQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            
        }

        [Test]
        public void When_SearchResultsRequested_Then_GetResultsFromCache()
        {
            var cacheKey = _detailsQueryViewModel.Ukprn + _detailsQueryViewModel.LocationId + _detailsQueryViewModel.ApprenticeshipId;

            _mockCacheService.Setup(s => s.RetrieveFromCache<ApprenticeshipProviderDetailResponse>(cacheKey)).Returns(GenerateMockCacheObject());

            var result = _sut.GetDetails(_detailsQueryViewModel);

            _mockMediator.Verify(s => s.Send<ApprenticeshipProviderDetailResponse>(It.IsAny<ApprenticeshipProviderDetailQuery>(), It.IsAny<CancellationToken>()), Times.Never());

        }

        //[Test]
        //public void When_SearchResultsRequested_Then_SaveResultsFromRedisToInMemoryCache()
        //{
        //    var cacheKey = _detailsQueryViewModel.Ukprn + _detailsQueryViewModel.LocationId + _detailsQueryViewModel.ApprenticeshipId;
        //    string json;

        //    _mockMediator.Setup(s => s.Send<ApprenticeshipProviderDetailResponse>(It.IsAny<ApprenticeshipProviderDetailQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(_providerDetailResponse);
        //  //  _mockCacheService.Setup(s => s.RetrieveFromCache<ApprenticeshipProviderDetailResponse>(cacheKey)).Returns(GenerateMockCacheObject());
        //    _mockMemoryCache.Setup(s => s.TryGetValue<string>(It.IsAny<string>(), out json)).Returns(false);

        //    var result = _sut.GetDetails(_detailsQueryViewModel);

        //    _mockMediator.Verify(s => s.Send<ApprenticeshipProviderDetailResponse>(It.IsAny<ApprenticeshipProviderDetailQuery>(), It.IsAny<CancellationToken>()), Times.Never());
        //   // _mockCacheService.Verify(s => s.SaveToCache(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()), Times.Once());
        ////    _mockDistributedCache.Verify(s => s.GetStringAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        //}

        private string GenerateMockCacheString()
        {
            var response = new ApprenticeshipProviderDetailResponse()
            {
                ApprenticeshipName = "Test Apprenticeship",
                ApprenticeshipDetails = new Core.Domain.Model.ApprenticeshipDetails {
                    Provider = new Core.Domain.Model.Provider { UkPrn = 10000020 },
                    Location = new Core.Domain.Model.Location { LocationId = 100},
                }
            };
            
            return JsonConvert.SerializeObject(response);
        }
        private async Task<ApprenticeshipProviderDetailResponse> GenerateMockCacheObject()
        {
            return JsonConvert.DeserializeObject<ApprenticeshipProviderDetailResponse>(GenerateMockCacheString());
        }

        private async Task<ApprenticeshipProviderDetailResponse> GenerateNullCacheObject()
        {
            return null;
        }
       

        //Test Scenarios
        // completely new search - not in memory or redis cache
        // calls the main get
        // new search - not in memory but is in redis
        // retrieves from redis and saves to inmemory
        // existing inmemory and in redis
        // retrieves from inmemory and doesn't call the main get
    }
}
