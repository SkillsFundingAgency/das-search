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

namespace Sfa.Das.Sas.Shared.Components.Orchestrators
{
    public class TrainingProviderOrchestrator : ITrainingProviderOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ISearchResultsViewModelMapper _searchResultsViewModelMapper;
        private readonly ITrainingProviderDetailsViewModelMapper _trainingProviderDetailsViewModelMapper;
        private readonly ITrainingProviderSearchFilterViewModelMapper _trainingProviderSearchFilterViewModelMapper;
        private readonly ILog _logger;

        public TrainingProviderOrchestrator(IMediator mediator, ISearchResultsViewModelMapper searchResultsViewModelMapper, ILog logger, ITrainingProviderDetailsViewModelMapper trainingProviderDetailsViewModelMapper, ITrainingProviderSearchFilterViewModelMapper trainingProviderSearchFilterViewModelMapper)
        {
            _mediator = mediator;
            _searchResultsViewModelMapper = searchResultsViewModelMapper;
            _logger = logger;
            _trainingProviderDetailsViewModelMapper = trainingProviderDetailsViewModelMapper;
            _trainingProviderSearchFilterViewModelMapper = trainingProviderSearchFilterViewModelMapper;
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

            var response = await _mediator.Send(new ApprenticeshipProviderDetailQuery() { UkPrn = Convert.ToInt32(detailsQueryModel.Ukprn), ApprenticeshipId = detailsQueryModel.ApprenticeshipId, ApprenticeshipType = detailsQueryModel.ApprenticeshipType, LocationId = detailsQueryModel.LocationId });

            if (response.StatusCode == ApprenticeshipProviderDetailResponse.ResponseCodes.ApprenticeshipProviderNotFound)
            {
                var message = $"Cannot find provider: {detailsQueryModel.Ukprn}";
                _logger.Warn($"404 - {message}");
                throw new HttpRequestException(message);
            }

            if (response.StatusCode == ApprenticeshipProviderDetailResponse.ResponseCodes.InvalidInput)
            {
                var message = $"Not able to call the apprenticeship service.";
                _logger.Warn($"{response.StatusCode} - {message}");

                throw new HttpRequestException(message);
            }

            var model = _trainingProviderDetailsViewModelMapper.Map(response);

            return model;
        }
    }
}