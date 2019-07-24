using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services
{
    using Core.Domain.Model;

    [TestFixture]
    public sealed class ForMappingProviderSearchResponseToViewModel
    {
        [Test]
        public void ShouldFullyPopulateTheMappedViewModel()
        {
            var mapper = new MappingService(null);

            var trainingLocations = new List<TrainingLocation> { new TrainingLocation { LocationId = 1, LocationName = "Location1", Address = new Address { Postcode = "N17" } } };

            var results = new ProviderSearchResults
            {
                TrainingOptionsAggregation = new Dictionary<string, long?>
                {
                    ["dayrelease"] = 10,
                    ["blockrelease"] = 2
                },
                SelectedTrainingOptions = new List<string> { "dayrelease" },
                Hits = new List<ProviderSearchResultItem>
                {
                    new ProviderSearchResultItem { LocationId = 1, LocationName = "Location1", Address = new Address { Postcode = "N17" }, OverallAchievementRate = 42.5 },
                    new ProviderSearchResultItem { LocationId = 1, LocationName = "Location1", Address = new Address { Postcode = "N17" } },
                    new ProviderSearchResultItem { LocationId = 1, LocationName = "Location1", Address = new Address { Postcode = "N17" } }
                },
                TotalResults = 105,
                ResultsToTake = 10,
                PostCode = "GU21 6DB",
                PostCodeMissing = true,
                ApprenticeshipId = "1234",
                Title = "Test Name"
            };

            var source = new ProviderSearchResponse
            {
                Success = false,
                CurrentPage = 2,
                Results = results,
                SearchTerms = "a b c",
                ShowAllProviders = true,
                TotalResultsForCountry = 1000,
                StatusCode = ProviderSearchResponseCodes.ApprenticeshipNotFound
            };

            var viewModel = mapper.Map<ProviderSearchResponse, ProviderStandardSearchResultViewModel>(source);

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
            viewModel.StandardId.Should().Be("1234");
            viewModel.StandardName.Should().Be("Test Name");
            viewModel.TotalResultsForCountry.Should().Be(1000);
            viewModel.TotalResults.Should().Be(105);
            viewModel.HasError.Should().BeTrue();
            viewModel.Hits.First().LocationId.Should().Be(1);
            viewModel.Hits.First().LocationName.Should().Be("Location1");
            viewModel.Hits.First().Address.Postcode.Should().Be("N17");
            viewModel.Hits.First().AchievementRateMessage.Should().Be("42.5%");
            viewModel.Hits.ElementAt(2).AchievementRateMessage.Should().Be("no data available");
        }

        [Test]
        public void ShouldFullyPopulateTheMappedFrameworkViewModelFor()
        {
            var mapper = new MappingService(null);

            var trainingLocation =  new TrainingLocation { LocationId = 1, LocationName = "Location1", Address = new Address { Postcode = "N17" } };

            var results = new ProviderSearchResults
            {
                TrainingOptionsAggregation = new Dictionary<string, long?>
                {
                    ["dayrelease"] = 10,
                    ["blockrelease"] = 2
                },
                SelectedTrainingOptions = new List<string> { "dayrelease" },
                Hits = new List<ProviderSearchResultItem>
                {
                    new ProviderSearchResultItem { LocationId = trainingLocation.LocationId, LocationName = trainingLocation.LocationName,Address = new Address { Postcode = "N17" },OverallAchievementRate = 42.5 },
                    new ProviderSearchResultItem { LocationId = trainingLocation.LocationId, LocationName = trainingLocation.LocationName,Address = new Address { Postcode = "N17" } },
                    new ProviderSearchResultItem { LocationId = trainingLocation.LocationId, LocationName = trainingLocation.LocationName,Address = new Address { Postcode = "N17" }, EmployerSatisfaction = 1.1, LearnerSatisfaction = 2.2 }
                },
                TotalResults = 105,
                ResultsToTake = 10,
                PostCode = "GU21 6DB",
                PostCodeMissing = true,
                ApprenticeshipId = "4321",
                Title = "Test Name"
            };

            var source = new ProviderSearchResponse
            {
                Success = false,
                CurrentPage = 2,
                Results = results,
                SearchTerms = "a b c",
                ShowAllProviders = true,
                TotalResultsForCountry = 1000,
                StatusCode = ProviderSearchResponseCodes.ApprenticeshipNotFound
            };

            var viewModel = mapper.Map<ProviderSearchResponse, ProviderFrameworkSearchResultViewModel>(source);

            viewModel.Title.Should().Be("Test Name");
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
            viewModel.FrameworkId.Should().Be("4321");
            viewModel.FrameworkName.Should().Be("Test Name");
            viewModel.TotalProvidersCountry.Should().Be(1000);
            viewModel.TotalResults.Should().Be(105);
            viewModel.HasError.Should().BeTrue();
            viewModel.Hits.First().LocationId.Should().Be(1);
            viewModel.Hits.First().LocationName.Should().Be("Location1");
            viewModel.Hits.First().Address.Postcode.Should().Be("N17");
            viewModel.Hits.First().AchievementRateMessage.Should().Be("42.5%");
            viewModel.Hits.ElementAt(2).AchievementRateMessage.Should().Be("no data available");
            viewModel.Hits.ElementAt(1).EmployerSatisfactionMessage.Should().Be("no data available");
            viewModel.Hits.ElementAt(1).LearnerSatisfactionMessage.Should().Be("no data available");
            viewModel.Hits.ElementAt(2).EmployerSatisfactionMessage.Should().Be("1.1%");
            viewModel.Hits.ElementAt(2).LearnerSatisfactionMessage.Should().Be("2.2%");
        }
    }
}
