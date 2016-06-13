using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Views.Services
{
    [TestFixture]
    public class ForMappingApprenticeDetailsToProviderViewModel
    {
        [Test]
        public void ShouldMapAllValauesCorrectly()
        {
            MappingService mappingService = new MappingService(null);

            var providerResult = new ApprenticeshipDetails
            {
                Product = new ApprenticeshipProduct
                {
                    Apprenticeship = new ApprenticeshipBasic
                    {
                        Code = 12,
                        ApprenticeshipInfoUrl = "apprenticeship test url",
                        ApprenticeshipMarketingInfo = "test marketing info"
                    },
                    DeliveryModes = new List<string>() { "On site", "Day trip" },
                    EmployerSatisfaction = 8.3,
                    LearnerSatisfaction = 2.1,
                    ProviderMarketingInfo = "provider marketing info"
                },
                Location = new Location
                {
                    LocationId = 23,
                    LocationName = "Location 1",
                    Address = new Address
                    {
                        Address1 = "12, test bay",
                        Address2 = "Market place",
                        Town = "Sea Town",
                        County = "Greensville",
                        Postcode = "AB12 3ED",
                        Lat = 123,
                        Long = 243
                    },
                    Distance = 2134
                },
                Provider = new Core.Domain.Model.Provider
                {
                    Id = 12345,
                    UkPrn = "9475987349857",
                    Name = "test provider",
                    ContactInformation = new ContactInformation
                    {
                        Email = "test@example.com",
                        Phone = "01234 567891",
                        Website = "www.test.co.uk",
                        ContactUsUrl = "www.contactus.co.uk"
                    }
                }
            };

            var viewModel = mappingService.Map<ApprenticeshipDetails, ApprenticeshipDetailsViewModel>(providerResult);

            viewModel.Address.Should().BeSameAs(providerResult.Location.Address);
            viewModel.Apprenticeship.Should().BeSameAs(providerResult.Product.Apprenticeship);
            viewModel.ContactInformation.Should().BeSameAs(providerResult.Provider.ContactInformation);
            viewModel.DeliveryModes.Should().BeEquivalentTo(providerResult.Product.DeliveryModes);

            viewModel.Location.LocationId.Should().Be(providerResult.Location.LocationId);
            viewModel.Location.LocationName.Should().BeSameAs(providerResult.Location.LocationName);
            viewModel.Location.Address.Should().BeSameAs(providerResult.Location.Address);
            viewModel.Location.Distance.Should().Be(providerResult.Location.Distance);
            viewModel.Location.Address.Should().BeSameAs(providerResult.Location.Address);

            viewModel.Name.Should().BeSameAs(providerResult.Provider.Name);
            viewModel.ProviderId.Should().Be(providerResult.Provider.Id.ToString());

            viewModel.EmployerSatisfactionMessage.Should().Be("8.3%");
            viewModel.LearnerSatisfactionMessage.Should().Be("2.1%");
        }

        [Test]
        public void WhenTypicalLengthIsEmpty()
        {
            MappingService mappingService = new MappingService(null);

            var providerResult = new ApprenticeshipDetails()
            {
                Product = new ApprenticeshipProduct()
                {
                    EmployerSatisfaction = 8.3,
                    LearnerSatisfaction = null
                }
            };

            var viewModel = mappingService.Map<ApprenticeshipDetails, ApprenticeshipDetailsViewModel>(providerResult);

            viewModel.EmployerSatisfactionMessage.Should().Be("8.3%");
            viewModel.LearnerSatisfactionMessage.Should().Be("No data available");
        }
    }
}
