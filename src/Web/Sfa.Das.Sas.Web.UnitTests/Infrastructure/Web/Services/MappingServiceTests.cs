using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services
{
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
                    TypicalLength = new TypicalLength { From = 12, To = 18, Unit = "m" },
                    JobRoleItems = new List<JobRoleItem> { new JobRoleItem { Description = "Description 1", Title = "Title1" } }
                }
            };

            var viewModel = service.Map<GetFrameworkResponse, FrameworkViewModel>(framework);

            viewModel.ExpiryDateString.Should().Be("5 September 1882");
            viewModel.TypicalLengthMessage.Should().Be("12 to 18 months");
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
                FrameworkId = 2,
                Title = "test title",
                Level = 3,
                Keywords = "test words",
                Postcode = "AS1 2DF",
                HasErrors = true
            };

            var viewModel = service.Map<GetFrameworkProvidersResponse, ProviderSearchViewModel>(response);

            viewModel.Should().NotBeNull();
            viewModel.ApprenticeshipId.Should().Be(response.FrameworkId);
            viewModel.Title.Should().Be($"{response.Title}, level {response.Level}");
            viewModel.PostCode.Should().Be(response.Postcode);
            viewModel.SearchTerms.Should().Be(response.Keywords);
            viewModel.HasError.Should().Be(response.HasErrors);
        }

        [Test]
        public void ShouldMapGetStandardProvidersResponseToViewModel()
        {
            var service = new MappingService(Mock.Of<ILog>());

            var response = new GetStandardProvidersResponse
            {
                StandardId = 2,
                Title = "test title",
                Level = 3,
                Keywords = "test words",
                Postcode = "AS1 2DF",
                HasErrors = true
            };

            var viewModel = service.Map<GetStandardProvidersResponse, ProviderSearchViewModel>(response);

            viewModel.Should().NotBeNull();
            viewModel.ApprenticeshipId.Should().Be(response.StandardId);
            viewModel.Title.Should().Be(response.Title);
            viewModel.PostCode.Should().Be(response.Postcode);
            viewModel.SearchTerms.Should().Be(response.Keywords);
            viewModel.HasError.Should().Be(response.HasErrors);
        }
    }
}