using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Shared.Components.Extensions.Domain;
using Sfa.Das.Sas.Shared.Components.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Das.Sas.Shared.Components.Mapping
{
    public class TrainingProviderClosestLocationsViewModelMapper : ITrainingProviderClosestLocationsViewModelMapper
    {
        public ClosestLocationsViewModel Map(string apprenticeshipId, int ukprn, string postcode, GetClosestLocationsResponse source)
        {
            var model = new ClosestLocationsViewModel
            {
                ApprenticeshipId = apprenticeshipId,
                ProviderName = source.ProviderName,
                Ukprn = ukprn,
                Locations = source.Results.Hits.Select(x => new CloseLocationViewModel { Distance = x.Distance, PostCode = x.Address.Postcode, AddressWithoutPostCode = x.Address.GetCommaDelimitedAddress() }).ToList()
            };

            return model;
        }
    }
}
