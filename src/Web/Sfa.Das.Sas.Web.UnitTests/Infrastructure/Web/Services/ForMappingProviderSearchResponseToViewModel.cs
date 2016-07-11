using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services
{
    using Sfa.Das.Sas.Core.Domain.Model;

    [TestFixture]
    public sealed class ForMappingProviderSearchResponseToViewModel
    {
        [Test]
        public void ShouldFullyPopulateTheMappedViewModel()
        {
            var mapper = new MappingService(null);

            var trainingLocations = new List<TrainingLocation> { new TrainingLocation { LocationId = 1, LocationName = "Location1", Address = new Address { Postcode = "N17" } } };

            var results = new ProviderStandardSearchResults
            {
                TrainingOptionsAggregation = new Dictionary<string, long?>
                {
                    ["dayrelease"] = 10,
                    ["blockrelease"] = 2
                },
                SelectedTrainingOptions = new List<string> { "dayrelease" },
                Hits = new List<IApprenticeshipProviderSearchResultsItem>
                {
                    new StandardProviderSearchResultsItem { TrainingLocations = trainingLocations, MatchingLocationId = 1 },
                    new StandardProviderSearchResultsItem { TrainingLocations = trainingLocations, MatchingLocationId = 1 },
                    new StandardProviderSearchResultsItem { TrainingLocations = trainingLocations, MatchingLocationId = 1 }
                },
                TotalResults = 105,
                ResultsToTake = 10,
                PostCode = "GU21 6DB",
                PostCodeMissing = true,
                StandardId = 1234,
                StandardName = "Test Name"
            };

            var source = new StandardProviderSearchResponse
            {
                Success = false,
                CurrentPage = 2,
                Results = results,
                SearchTerms = "a b c",
                ShowAllProviders = true,
                TotalResultsForCountry = 1000,
                StatusCode = StandardProviderSearchResponse.ResponseCodes.ApprenticeshipNotFound
            };

            var viewModel = mapper.Map<StandardProviderSearchResponse, ProviderStandardSearchResultViewModel>(source);

            viewModel.ActualPage.Should().Be(2);
            viewModel.DeliveryModes.Count().Should().Be(2);
            viewModel.DeliveryModes.Count(x => x.Checked).Should().Be(1);
            viewModel.Hits.Count().Should().Be(3);
            viewModel.LastPage.Should().Be(11);
            viewModel.SearchTerms.Should().Be("a b c");
            viewModel.PostCode.Should().Be("GU21 6DB");
            viewModel.PostCodeMissing.Should().BeTrue();
            viewModel.ResultsToTake.Should().Be(10);
            viewModel.ShowAll.Should().BeTrue();
            viewModel.StandardId.Should().Be(1234);
            viewModel.StandardName.Should().Be("Test Name");
            viewModel.TotalResultsForCountry.Should().Be(1000);
            viewModel.TotalResults.Should().Be(105);
            viewModel.HasError.Should().BeTrue();
            viewModel.Hits.First().LocationId.Should().Be(1);
            viewModel.Hits.First().LocationName.Should().Be("Location1");
            viewModel.Hits.First().Address.Postcode.Should().Be("N17");
        }
    }
}
