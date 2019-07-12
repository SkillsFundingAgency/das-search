using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Shared.Components.Mapping;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;
using Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider;
using Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider.Search;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.Mapping.TrainingProvider
{
    [TestFixture]
    public class GivenSearchFilterIsMappedToViewModel
    {
        private TrainingProviderSearchFilterViewModelMapper _sut;
        private ProviderSearchResponse _resultsItemToMap;
        private TrainingProviderSearchViewModel _queryItemToMap;

        [SetUp]
        public void Setup()
        {

            _sut = new TrainingProviderSearchFilterViewModelMapper();

            _resultsItemToMap = new ProviderSearchResponse()
            {
                CurrentPage = 1,
                SearchTerms = "Terms",
                ShowAllProviders = true,
                ShowOnlyNationalProviders = true,
                StatusCode = ProviderSearchResponseCodes.Success,
                Results = new ProviderSearchResults()
                {
                    ActualPage = 1,
                    ApprenticeshipId = "157",
                    Hits = new List<ProviderSearchResultItem>()
                    {

                    },
                    PostCode = "Postcode",
                    PostCodeMissing = false,
                    ResultsToTake = 10,
                    NationalProviders = new Dictionary<string, long?>()
                    {
                        {"true",100 },
                        {"false",100 }
                    } ,
                    TrainingOptionsAggregation = new Dictionary<string, long?>()
                    {
                        { "blockrelease", 44},
                        { "100percentemployer", 31},
                        { "dayrelease", 28}
                    }


                }
            };
            _queryItemToMap = new TrainingProviderSearchViewModel()
            {
                ApprenticeshipId = "157",
                DeliveryModes = new List<string>() { "blockrelease", "100percentemployer", "dayrelease" },
                IsLevyPayer = false,
                Keywords = "words",
                NationalProvidersOnly = false,
                Page = 1,
                Postcode = "Postcode",
                ResultsToTake = 20,
                SortOrder = 1
            };
        }

        [Test]
        public void When_Mapping_Then_TrainingProviderSearchFilterViewModel_Is_Returned()
        {
            var result = _sut.Map(_resultsItemToMap, _queryItemToMap);

            result.Should().BeOfType<TrainingProviderSearchFilterViewModel>();
          result.Should().NotBeNull();
        }

        [Test]
        public void When_Mapping_Then_Items_Are_Mapped()
        {
            var result = _sut.Map(_resultsItemToMap, _queryItemToMap);
            result.ApprenticeshipId.Should().Be(_queryItemToMap.ApprenticeshipId);
            result.Page.Should().Be(_queryItemToMap.Page);
            result.Keywords.Should().Be(_queryItemToMap.Keywords);
            result.ResultsToTake.Should().Be(_queryItemToMap.ResultsToTake);
            result.DeliveryModes.Should().BeEquivalentTo(_queryItemToMap.DeliveryModes);
            result.SortOrder.Should().Be(_queryItemToMap.SortOrder);
            result.IsLevyPayer.Should().Be(_queryItemToMap.IsLevyPayer);
            result.Postcode.Should().Be(_queryItemToMap.Postcode);
            result.NationalProvidersOnly.Should().Be(_queryItemToMap.NationalProvidersOnly);

            result.NationalProvidersOnly.Should().Be(_queryItemToMap.NationalProvidersOnly);
            result.NationalProviders.Should().NotBeEmpty();
            result.NationalProviders.Should().HaveCount(2);
            result.TrainingOptions.Should().NotBeEmpty();
            result.TrainingOptions.Should().HaveCount(_queryItemToMap.DeliveryModes.Count());
        }

        [Test]
        public void When_Mapping_And_Results_Are_Null_Then_Items_Mapped_As_Null()
        {
            _resultsItemToMap.Results = null;

            var result = _sut.Map(_resultsItemToMap, _queryItemToMap);

            result.Page.Should().Be(_queryItemToMap.Page);
            result.Keywords.Should().Be(_queryItemToMap.Keywords);
            result.ResultsToTake.Should().Be(_queryItemToMap.ResultsToTake);
            result.NationalProviders.Should().BeEmpty();
            result.TrainingOptions.Should().BeEmpty();
        }
    }
}
