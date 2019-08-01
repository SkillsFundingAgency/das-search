using System.Collections.Generic;
using System.Linq;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider.Search;
using Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider;

namespace Sfa.Das.Sas.Shared.Components.Mapping
{
    public class TrainingProviderSearchFilterViewModelMapper : ITrainingProviderSearchFilterViewModelMapper
    {

        public TrainingProviderSearchFilterViewModel Map(ProviderSearchResponse item, TrainingProviderSearchViewModel searchQueryModel)
        {
            var result = new TrainingProviderSearchFilterViewModel();

            result.NationalProviders = new List<FilterViewModel>();

            foreach (var resultsNationalProvider in item.Results.NationalProviders)
            {
                var filter = new FilterViewModel();

                filter.Value = resultsNationalProvider.Key;
                filter.Count = resultsNationalProvider.Value;

                if (filter.Value == "true")
                {
                    filter.Checked = item.ShowOnlyNationalProviders;
                }
                else
                {
                    filter.Checked = item.ShowOnlyNationalProviders == false;
                    filter.Count = item.Results.NationalProviders.Sum(s => s.Value.Value);
                }

                result.NationalProviders.Add(filter);
            }

            result.TrainingOptions = new List<FilterViewModel>();

            foreach (var trainingOption in item.Results.TrainingOptionsAggregation)
            {
                var filter = new FilterViewModel();

                filter.Count = trainingOption.Value;
             
                switch (trainingOption.Key.ToLower())
                {
                    case "dayrelease":
                        filter.Value = "0";

                        break;
                    case "blockrelease":
                        filter.Value = "1";
                        break;
                    case "100percentemployer":
                        filter.Value = "2";
                        break;
                }
                filter.Checked = searchQueryModel.DeliveryModes.Contains(filter.Value);
                
                result.TrainingOptions.Add(filter);
            }

            return result;
        }
    }
}
