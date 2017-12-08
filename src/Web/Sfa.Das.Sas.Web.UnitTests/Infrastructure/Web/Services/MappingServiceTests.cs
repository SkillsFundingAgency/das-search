namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ApplicationServices.Models;
    using ApplicationServices.Responses;
    using Core.Domain.Model;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Sas.Web.Services;
    using SFA.DAS.NLog.Logger;
    using ViewModels;

    [TestFixture]
    public sealed class MappingServiceTests
    {
        [Test]
        public void MappingConfigurationShouldBeValid()
        {
            var service = new MappingService(Mock.Of<ILog>());

            service.Configuration.AssertConfigurationIsValid();
        }

        [Test]
        public void ShouldMappFromFrameworkFrameworkViewModel()
        {
            var service = new MappingService(Mock.Of<ILog>());
            var framework = new GetFrameworkResponse
            {
                Framework = new Framework
                {
                    Title = "Abba: Abba",
                    ExpiryDate = new DateTime(1882, 09, 04),
                    JobRoleItems = new List<JobRoleItem> { new JobRoleItem { Description = "Description 1", Title = "Title1" } }
                }
            };

            var viewModel = service.Map<GetFrameworkResponse, FrameworkViewModel>(framework);

            viewModel.ExpiryDateString.Should().Be("5 September 1882");
            viewModel.JobRoles.FirstOrDefault().Should().Be("Title1");
            viewModel.Title.ShouldBeEquivalentTo("Abba");
        }

        [Test]
        public void ShouldMappFromFrameworkFrameworkViewModelWhenFrameworkIsEmpty()
        {
            var service = new MappingService(Mock.Of<ILog>());
            var framework = new GetFrameworkResponse
            {
                Framework = new Framework
                {
                    Title = "title1",
                }
            };

            var viewModel = service.Map<GetFrameworkResponse, FrameworkViewModel>(framework);

            viewModel.ExpiryDateString.Should().BeNull();
            viewModel.Should().NotBeNull();
        }

        [Test]
        public void ShouldMapGetFrameworkProvidersResponseToViewModel()
        {
            var service = new MappingService(Mock.Of<ILog>());

            var response = new GetFrameworkProvidersResponse
            {
                FrameworkId = "2",
                Title = "test title",
                Level = 3,
                Keywords = "test words",
                Postcode = "AS1 2DF"
            };

            var viewModel = service.Map<GetFrameworkProvidersResponse, ProviderSearchViewModel>(response);

            viewModel.Should().NotBeNull();
            viewModel.ApprenticeshipId.Should().Be(response.FrameworkId);
            viewModel.Title.Should().Be($"{response.Title}, level {response.Level}");
            viewModel.PostCode.Should().Be(response.Postcode);
            viewModel.SearchTerms.Should().Be(response.Keywords);
        }

        [Test]
        public void ShouldMapGetStandardProvidersResponseToViewModel()
        {
            var service = new MappingService(Mock.Of<ILog>());

            var response = new GetStandardProvidersResponse
            {
                StandardId = "2",
                Title = "test title",
                Level = 3,
                Keywords = "test words",
                Postcode = "AS1 2DF"
            };

            var viewModel = service.Map<GetStandardProvidersResponse, ProviderSearchViewModel>(response);

            viewModel.Should().NotBeNull();
            viewModel.ApprenticeshipId.Should().Be(response.StandardId);
            viewModel.Title.Should().Be(response.Title);
            viewModel.PostCode.Should().Be(response.Postcode);
            viewModel.SearchTerms.Should().Be(response.Keywords);
        }

        [Test]
        public void ShouldMapStandardProvidersItemToViewModel()
        {
            var service = new MappingService(Mock.Of<ILog>());

            var response = new StandardProviderSearchResultsItem
            {
                StandardCode = 2,
                TrainingLocations = new List<TrainingLocation>
                {
                    new TrainingLocation
                    {
                        Address = new Address { Address1 = "Address 1", Address2 = "Address 2" },
                        LocationName = "Location Name",
                        LocationId = 12345
                    }
                },
                MatchingLocationId = 12345
            };

            var viewModel = service.Map<StandardProviderSearchResultsItem, StandardProviderResultItemViewModel>(response);

            viewModel.Should().NotBeNull();
            viewModel.LocationAddressLine.Should().Be("Location Name, Address 1, Address 2");
            viewModel.DeliveryModes.Count.Should().Be(0);
        }

        [Test]
        public void ShouldMapFrameworkProvidersItemToViewModel()
        {
            var service = new MappingService(Mock.Of<ILog>());

            var response = new FrameworkProviderSearchResultsItem
            {
                FrameworkId = "123-45-6",
                TrainingLocations = new List<TrainingLocation>
                {
                    new TrainingLocation
                    {
                        Address = new Address { Address1 = "Address 1", County = "Angleterre" },
                        LocationName = "Location Name",
                        LocationId = 12345
                    }
                },
                DeliveryModes = new List<string> { "100PercentEmployer", "DayRelease"},
                MatchingLocationId = 12345
            };

            var viewModel = service.Map<FrameworkProviderSearchResultsItem, FrameworkProviderResultItemViewModel>(response);

            viewModel.Should().NotBeNull();
            viewModel.LocationAddressLine.Should().Be("Location Name, Address 1, Angleterre");
            viewModel.DeliveryModes.Count(x => x.Contains("100PercentEmployer")).Should().Be(1);
            viewModel.DeliveryModes.Count(x => x.Contains("DayRelease")).Should().Be(1);
            viewModel.DeliveryModes.Count.Should().Be(2);
        }
    }
}