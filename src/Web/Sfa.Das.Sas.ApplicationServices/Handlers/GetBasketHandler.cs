using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apprenticeships.Api.Types;
using Sfa.Das.Sas.ApplicationServices.Interfaces;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Core.Domain.Services;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public class GetBasketHandler : IRequestHandler<GetBasketQuery, ApprenticeshipFavouritesBasketRead>
    {
        private readonly ILogger<GetBasketHandler> _logger;
        private readonly IApprenticeshipFavouritesBasketStore _basketStore;
        private readonly IGetStandards _getStandards;
        private readonly IGetFrameworks _getFrameworks;
        private readonly IGetProviderDetails _getProvider;

         public GetBasketHandler(
            ILogger<GetBasketHandler> logger,
            IApprenticeshipFavouritesBasketStore basketStore, IGetStandards getStandards, IGetFrameworks getFrameworks, IGetProviderDetails getProvider)
        {
            _logger = logger;
            _basketStore = basketStore;
            _getStandards = getStandards;
            _getFrameworks = getFrameworks;
            _getProvider = getProvider;
        }

        public async Task<ApprenticeshipFavouritesBasketRead> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting basket for {basketId}", request.BasketId);

            var basket = await _basketStore.GetAsync(request.BasketId);


            if(basket != null)
            {
                Parallel.ForEach(basket, (basketItem) =>
                {
                    EnrichApprenticeshipInfo(basketItem);
                });
            }


            return basket ?? new ApprenticeshipFavouritesBasketRead();
        }

        private void EnrichApprenticeshipInfo(ApprenticeshipFavouriteRead apprenticeship)
        {
            if (IsFramework(apprenticeship.ApprenticeshipId))
            {
                EnrichFramework(apprenticeship);
            }
            else
            {
                EnrichStandard(apprenticeship);
            }

            EnrichTrainingProvider(apprenticeship);
        }

        private void EnrichTrainingProvider(ApprenticeshipFavouriteRead apprenticeship)
        {
            Parallel.ForEach(apprenticeship.Providers, async item =>
            {
                var providerResult = await _getProvider.GetProviderDetails(item.Ukprn);

                if (providerResult != null)
                {
                    item.Name = providerResult.ProviderName;
                }
            });
        }

        private void EnrichFramework(ApprenticeshipFavouriteRead apprenticeship)
        {
            var framework = _getFrameworks.GetFrameworkById(apprenticeship.ApprenticeshipId);

            apprenticeship.Title = framework.Title;
            apprenticeship.Duration = framework.Duration;
            apprenticeship.Level = framework.Level;
        }

        private void EnrichStandard(ApprenticeshipFavouriteRead apprenticeship)
        {
            var standard = _getStandards.GetStandardById(apprenticeship.ApprenticeshipId);

            apprenticeship.Title = standard.Title;
            apprenticeship.Duration = standard.Duration;
            apprenticeship.Level = standard.Level;

        }

        public bool IsFramework(string id)
        {
            return id.Contains("-");
        }
    }
}
