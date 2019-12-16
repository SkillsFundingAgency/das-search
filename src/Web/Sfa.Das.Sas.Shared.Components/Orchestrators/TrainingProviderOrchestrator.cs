using System;
using System.Net.Http;
using System.Threading.Tasks;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Shared.Components.Mapping;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;
using Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider.Search;
using Sfa.Das.Sas.Shared.Components.ViewModels;
using SFA.DAS.NLog.Logger;
using Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider;
using Sfa.Das.Sas.Infrastructure.Services;

namespace Sfa.Das.Sas.Shared.Components.Orchestrators
{
    public class TrainingProviderOrchestrator : ITrainingProviderOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ISearchResultsViewModelMapper _searchResultsViewModelMapper;
        private readonly ITrainingProviderDetailsViewModelMapper _trainingProviderDetailsViewModelMapper;
        private readonly ITrainingProviderSearchFilterViewModelMapper _trainingProviderSearchFilterViewModelMapper;
        private readonly ILog _logger;
        private readonly ICacheStorageService _cacheService;
        
        public TrainingProviderOrchestrator(IMediator mediator, ISearchResultsViewModelMapper searchResultsViewModelMapper, ILog logger, ITrainingProviderDetailsViewModelMapper trainingProviderDetailsViewModelMapper, ITrainingProviderSearchFilterViewModelMapper trainingProviderSearchFilterViewModelMapper, ICacheStorageService cacheService)
        {
            _mediator = mediator;
            _searchResultsViewModelMapper = searchResultsViewModelMapper;
            _logger = logger;
            _trainingProviderDetailsViewModelMapper = trainingProviderDetailsViewModelMapper;
            _trainingProviderSearchFilterViewModelMapper = trainingProviderSearchFilterViewModelMapper;
            _cacheService = cacheService;
        }

        public async Task<SearchResultsViewModel<TrainingProviderSearchResultsItem, TrainingProviderSearchViewModel>> GetSearchResults(TrainingProviderSearchViewModel searchQueryModel)
        {

            var results = await _mediator.Send(new ProviderSearchQuery()
            {
                ApprenticeshipId = searchQueryModel.ApprenticeshipId,
                PostCode = searchQueryModel.Postcode,
                DeliveryModes = searchQueryModel.DeliveryModes,
                NationalProvidersOnly = searchQueryModel.NationalProvidersOnly,
                Page = searchQueryModel.Page
            });

            var model = _searchResultsViewModelMapper.Map(results, searchQueryModel);

            if (results.Success == false)
            {
                throw new Exception($"Unable to get provider search response: {results.StatusCode}");
               
            }
            return _searchResultsViewModelMapper.Map(results, searchQueryModel);
        }

        public async Task<TrainingProviderSearchFilterViewModel> GetSearchFilter(TrainingProviderSearchViewModel searchQueryModel)
        {
            var results = await _mediator.Send(new ProviderSearchQuery()
            {
                ApprenticeshipId = searchQueryModel.ApprenticeshipId,
                PostCode = searchQueryModel.Postcode,
                DeliveryModes =searchQueryModel.DeliveryModes,
                NationalProvidersOnly = searchQueryModel.NationalProvidersOnly
            });

            var model = _trainingProviderSearchFilterViewModelMapper.Map(results, searchQueryModel);

            if (results.Success == false)
            {
                throw new Exception($"Unable to get provider search response: {results.StatusCode}");

            }

            return model;
        }

        public async Task<TrainingProviderDetailsViewModel> GetDetails(TrainingProviderDetailQueryViewModel detailsQueryModel)
        {

            var cacheKey = detailsQueryModel.Ukprn + detailsQueryModel.LocationId + detailsQueryModel.ApprenticeshipId;

            var cacheEntry = await _cacheService.RetrieveFromCache<ApprenticeshipProviderDetailResponse>(cacheKey);

            if (cacheEntry == null)
            {
                cacheEntry = await _mediator.Send(new ApprenticeshipProviderDetailQuery() { UkPrn = Convert.ToInt32(detailsQueryModel.Ukprn), ApprenticeshipId = detailsQueryModel.ApprenticeshipId, ApprenticeshipType = detailsQueryModel.ApprenticeshipType, LocationId = detailsQueryModel.LocationId });

                if (cacheEntry.StatusCode == ApprenticeshipProviderDetailResponse.ResponseCodes.ApprenticeshipProviderNotFound)
                {
                    var message = $"Cannot find provider: {detailsQueryModel.Ukprn}";
                    _logger.Warn($"404 - {message}");
                    throw new HttpRequestException(message);
                }

                if (cacheEntry.StatusCode == ApprenticeshipProviderDetailResponse.ResponseCodes.InvalidInput)
                {
                    var message = $"Not able to call the apprenticeship service.";
                    _logger.Warn($"{cacheEntry.StatusCode} - {message}");

                    throw new HttpRequestException(message);
                }

                await _cacheService.SaveToCache(cacheKey, cacheEntry, new TimeSpan(30, 0, 0, 0), new TimeSpan(1, 0, 0, 0));
            }
            
            var model = _trainingProviderDetailsViewModelMapper.Map(cacheEntry);

            return model;
        }
    }
}