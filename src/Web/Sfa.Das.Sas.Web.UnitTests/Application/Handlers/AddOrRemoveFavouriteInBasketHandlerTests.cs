using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Types.Providers;
using SFA.DAS.Providers.Api.Client;
using Sfa.Das.Sas.ApplicationServices.Commands;
using Sfa.Das.Sas.ApplicationServices.Handlers;
using Sfa.Das.Sas.ApplicationServices.Interfaces;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Shared.Basket.Interfaces;
using Sfa.Das.Sas.Shared.Basket.Models;

namespace Sfa.Das.Sas.Web.UnitTests.Application.Handlers
{
    [TestFixture]
    public class AddOrRemoveFavouriteInBasketHandlerTests
    {
        private IRequestHandler<AddOrRemoveFavouriteInBasketCommand, AddOrRemoveFavouriteInBasketResponse> _sut;
        private Mock<IApprenticeshipFavouritesBasketStore> _mockBasket;
        private Mock<IProviderApiClient> _mockProviderApiClient;

        [SetUp]
        public void Setup()
        {
            _mockBasket = new Mock<IApprenticeshipFavouritesBasketStore>();
            _mockProviderApiClient = new Mock<IProviderApiClient>();

            _mockProviderApiClient.Setup(s => s.Get(It.IsAny<int>())).Returns(new SFA.DAS.Apprenticeships.Api.Types.Providers.Provider() {ProviderName = "TestProvider"});

            var mediator = Substitute.For<IMediator>();

            mediator.Send(Arg.Any<GetStandardQuery>(), Arg.Any<CancellationToken>()).Returns(new GetStandardResponse()
            {
                Standard = new Standard { Title = "Standard 1" }
            });

            _sut = new AddorRemoveFavouriteInBasketCommandHandler(new NullLogger<AddorRemoveFavouriteInBasketCommandHandler>(), _mockBasket.Object,_mockProviderApiClient.Object, mediator);
        }

        [Test]
        public async Task Handle_CreatesBasketId_WhenNoBasketSpecified()
        {
            var request = new AddOrRemoveFavouriteInBasketCommand
            {
                BasketId = null,
                ApprenticeshipId = "123"
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            response.BasketId.Should().NotBeEmpty();
            _mockBasket.Verify(x => x.UpdateAsync(It.Is<ApprenticeshipFavouritesBasket>(a => a.Id != Guid.Empty)));
        }

        [Test]
        public async Task Handle_CreatesNewBasketId_IfPassedBasketIdNoLongerExistsInCache()
        {
            var expiredBasketId = Guid.NewGuid();

            var request = new AddOrRemoveFavouriteInBasketCommand
            {
                BasketId = expiredBasketId,
                ApprenticeshipId = "123"
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            response.BasketId.Should().NotBeEmpty();
            _mockBasket.Verify(x => x.UpdateAsync(It.Is<ApprenticeshipFavouritesBasket>(a => a.Id != Guid.Empty && a.Id != expiredBasketId)));
        }

        [Test]
        public async Task Handle_AddsApprenticeshipToBasket_ForNewBasket()
        {
            var basketId = Guid.NewGuid();

            var request = new AddOrRemoveFavouriteInBasketCommand
            {
                BasketId = null,
                ApprenticeshipId = "123"
            };

            await _sut.Handle(request, default(CancellationToken));

            _mockBasket.Verify(x => x.UpdateAsync(It.Is<ApprenticeshipFavouritesBasket>(a => a.First().ApprenticeshipId == "123")));
        }

        [Test]
        public async Task Handle_AddsApprenticeshipToExistingBasket_ForNewApprenticeship()
        {
            var basketId = Guid.NewGuid();
            var basket = new ApprenticeshipFavouritesBasket { Id = basketId };
            basket.Add("111");

            _mockBasket.Setup(x => x.GetAsync(basketId)).ReturnsAsync(basket);

            ApprenticeshipFavouritesBasket savedBasket = null; // Setup callback so we can check contents of basket easier.
            _mockBasket.Setup(x => x.UpdateAsync(It.IsAny<ApprenticeshipFavouritesBasket>()))
                .Returns(Task.CompletedTask)
                .Callback<ApprenticeshipFavouritesBasket>((a) => savedBasket = a);

            var request = new AddOrRemoveFavouriteInBasketCommand
            {
                BasketId = basketId,
                ApprenticeshipId = "123"
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            response.Should().ShouldBeEquivalentTo(new AddOrRemoveFavouriteInBasketResponse(){ BasketId = basketId, ApprenticeshipName = "Standard 1", BasketOperation = BasketOperation.Added });
            _mockBasket.Verify(x => x.UpdateAsync(It.Is<ApprenticeshipFavouritesBasket>(b => b.Count() == 2)));

            savedBasket.SingleOrDefault(x => x.ApprenticeshipId == "123").Should().NotBeNull();
        }

        [Test]
        public async Task Handle_ShouldRemoveApprenticeFromBasket_IfApprenticeshipAlreadyInBasket()
        {
            var basketId = Guid.NewGuid();
            var basket = new ApprenticeshipFavouritesBasket { Id = basketId };

            basket.Add("123", 23456, "TestProvider");
            basket.Add("123", 12345, "TestProvider");

            _mockBasket.Setup(x => x.GetAsync(basketId)).ReturnsAsync(basket);

            var request = new AddOrRemoveFavouriteInBasketCommand
            {
                BasketId = basketId,
                ApprenticeshipId = "123"
            };

            await _sut.Handle(request, default(CancellationToken));

            _mockBasket.Verify(x => x.UpdateAsync(It.IsAny<ApprenticeshipFavouritesBasket>()), Times.Once);

            basket.Should().NotBeNull();
            basket.Should().BeEmpty();
        }

        [Test]
        public async Task Handle_AddsProviderLocationToBasket_ForNewBasket()
        {
            var basketId = Guid.NewGuid();

            var request = new AddOrRemoveFavouriteInBasketCommand
            {
                BasketId = null,
                ApprenticeshipId = "123",
                LocationId = 12345,
                Ukprn = 12345678
            };

            await _sut.Handle(request, default(CancellationToken));

            _mockBasket.Verify(x => x.UpdateAsync(It.Is<ApprenticeshipFavouritesBasket>(a => a[0].ApprenticeshipId == "123" && a[0].Providers.Select(w => w.Ukprn).Contains(12345678) && a[0].Providers.SingleOrDefault(w => w.Ukprn == 12345678).Locations.Contains(12345))));
        }

        [Test]
        public async Task Handle_AddsTrainingProviderToExistingBasket_ForExistingApprenticeship()
        {
            var basketId = Guid.NewGuid();
            var basket = new ApprenticeshipFavouritesBasket { Id = basketId };
            basket.Add("123");

            _mockBasket.Setup(x => x.GetAsync(basketId)).ReturnsAsync(basket);

            ApprenticeshipFavouritesBasket savedBasket = null; // Setup callback so we can check contents of basket easier.
            _mockBasket.Setup(x => x.UpdateAsync(It.IsAny<ApprenticeshipFavouritesBasket>()))
                .Returns(Task.CompletedTask)
                .Callback<ApprenticeshipFavouritesBasket>((a) => savedBasket = a);

            var request = new AddOrRemoveFavouriteInBasketCommand
            {
                BasketId = basketId,
                ApprenticeshipId = "123",
                Ukprn = 12345678
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            response.Should().ShouldBeEquivalentTo(new AddOrRemoveFavouriteInBasketResponse(){ BasketId = basketId, ApprenticeshipName = "Standard 1", BasketOperation = BasketOperation.Added });
            _mockBasket.Verify(x => x.UpdateAsync(It.Is<ApprenticeshipFavouritesBasket>(b => b.Count() == 1)));

            var favourite = savedBasket.SingleOrDefault(x => x.ApprenticeshipId == "123");
            favourite.Should().NotBeNull();
            favourite.Providers.Count.Should().Be(1);
            favourite.Providers.First().Ukprn.Should().Be(12345678);
        }

        [Test]
        public async Task Handle_AddsTrainingProviderToExistingBasket_ForNewApprenticeship()
        {
            var basketId = Guid.NewGuid();
            var basket = new ApprenticeshipFavouritesBasket { Id = basketId };
            basket.Add("111");

            _mockBasket.Setup(x => x.GetAsync(basketId)).ReturnsAsync(basket);

            ApprenticeshipFavouritesBasket savedBasket = null; // Setup callback so we can check contents of basket easier.
            _mockBasket.Setup(x => x.UpdateAsync(It.IsAny<ApprenticeshipFavouritesBasket>()))
                .Returns(Task.CompletedTask)
                .Callback<ApprenticeshipFavouritesBasket>((a) => savedBasket = a);

            var request = new AddOrRemoveFavouriteInBasketCommand
            {
                BasketId = basketId,
                ApprenticeshipId = "123",
                Ukprn = 12345678
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            response.Should().ShouldBeEquivalentTo(new AddOrRemoveFavouriteInBasketResponse(){ BasketId = basketId, ApprenticeshipName = "Standard 1", BasketOperation = BasketOperation.Added });
            _mockBasket.Verify(x => x.UpdateAsync(It.Is<ApprenticeshipFavouritesBasket>(b => b.Count() == 2)));

            var favourite = savedBasket.SingleOrDefault(x => x.ApprenticeshipId == "123");
            favourite.Should().NotBeNull();
            favourite.Providers.Count.Should().Be(1);
            favourite.Providers.First().Ukprn.Should().Be(12345678);
        }

        [Test]
        public async Task Handle_ShouldDeleteTrainingProviderFromExistingBasket_WhenProviderHasAlreadyBeenAddedForApprenticeship()
        {
            var basketId = Guid.NewGuid();
            var basket = new ApprenticeshipFavouritesBasket { Id = basketId };
            basket.Add("123", 12345678, "TestProvider");
            basket.Add("123", 23456789, "TestProvider");

            _mockBasket.Setup(x => x.GetAsync(basketId)).ReturnsAsync(basket);

            ApprenticeshipFavouritesBasket savedBasket = null; // Setup callback so we can check contents of basket easier.
            _mockBasket.Setup(x => x.UpdateAsync(It.IsAny<ApprenticeshipFavouritesBasket>()))
                .Returns(Task.CompletedTask)
                .Callback<ApprenticeshipFavouritesBasket>((a) => savedBasket = a);

            var request = new AddOrRemoveFavouriteInBasketCommand
            {
                BasketId = basketId,
                ApprenticeshipId = "123",
                Ukprn = 12345678
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            _mockBasket.Verify(x => x.UpdateAsync(It.IsAny<ApprenticeshipFavouritesBasket>()), Times.Once);

            var favourite = savedBasket.SingleOrDefault(x => x.ApprenticeshipId == "123");
            favourite.Should().NotBeNull();
            favourite.Providers.Count.Should().Be(1);
            favourite.Providers.Select(s => s.Ukprn).Should().NotContain(12345678);
        }




        [Test]
        public async Task Handle_AddsTrainingProviderLocationToExistingBasket_ForExistingApprenticeship()
        {
            var basketId = Guid.NewGuid();
            var basket = new ApprenticeshipFavouritesBasket { Id = basketId };
            basket.Add("123");

            _mockBasket.Setup(x => x.GetAsync(basketId)).ReturnsAsync(basket);

            ApprenticeshipFavouritesBasket savedBasket = null; // Setup callback so we can check contents of basket easier.
            _mockBasket.Setup(x => x.UpdateAsync(It.IsAny<ApprenticeshipFavouritesBasket>()))
                .Returns(Task.CompletedTask)
                .Callback<ApprenticeshipFavouritesBasket>((a) => savedBasket = a);

            var request = new AddOrRemoveFavouriteInBasketCommand
            {
                BasketId = basketId,
                ApprenticeshipId = "123",
                Ukprn = 12345678,
                LocationId = 10000020
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            response.Should().Be(basketId);
            _mockBasket.Verify(x => x.UpdateAsync(It.Is<ApprenticeshipFavouritesBasket>(b => b.Count() == 1)));

            var favourite = savedBasket.SingleOrDefault(x => x.ApprenticeshipId == "123");
            favourite.Should().NotBeNull();
            favourite.Providers.Should().NotBeNull();
            favourite.Providers.Count.Should().Be(1);
            favourite.Providers.First().Ukprn.Should().Be(12345678);
            favourite.Providers.First().Locations.Count().Should().Be(1);
            favourite.Providers.First().Locations.Contains(10000020);
        }

        [Test]
        public async Task Handle_AddsTrainingProviderLocationToExistingBasket_ForExistingApprenticeshipAndProviderWithNoLocations()
        {
            var basketId = Guid.NewGuid();
            var basket = new ApprenticeshipFavouritesBasket { Id = basketId };
            basket.Add("123", 12345678, "TestProvider");

            _mockBasket.Setup(x => x.GetAsync(basketId)).ReturnsAsync(basket);

            ApprenticeshipFavouritesBasket savedBasket = null; // Setup callback so we can check contents of basket easier.
            _mockBasket.Setup(x => x.UpdateAsync(It.IsAny<ApprenticeshipFavouritesBasket>()))
                .Returns(Task.CompletedTask)
                .Callback<ApprenticeshipFavouritesBasket>((a) => savedBasket = a);

            var request = new AddOrRemoveFavouriteInBasketCommand
            {
                BasketId = basketId,
                ApprenticeshipId = "123",
                Ukprn = 12345678,
                LocationId = 10000020
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            response.Should().ShouldBeEquivalentTo(new AddOrRemoveFavouriteInBasketResponse(){ BasketId = basketId, ApprenticeshipName = "Standard 1", BasketOperation = BasketOperation.Added });
            _mockBasket.Verify(x => x.UpdateAsync(It.Is<ApprenticeshipFavouritesBasket>(b => b.Count() == 1)));

            var favourite = savedBasket.SingleOrDefault(x => x.ApprenticeshipId == "123");
            favourite.Should().NotBeNull();
            favourite.Providers.Should().NotBeNull();
            favourite.Providers.Count.Should().Be(1);
            favourite.Providers.First().Ukprn.Should().Be(12345678);
            favourite.Providers.First().Locations.Count().Should().Be(1);
            favourite.Providers.First().Locations.Contains(10000020);
            favourite.Providers.First().Name.Should().Be("TestProvider");
        }

        [Test]
        public async Task Handle_AddsTrainingProviderLocationToExistingBasket_ForExistingApprenticeshipAndProviderWithExistingLocations()
        {
            var basketId = Guid.NewGuid();
            var basket = new ApprenticeshipFavouritesBasket { Id = basketId };
            basket.Add("123", 12345678,"TestProvider", 10000030);

            _mockBasket.Setup(x => x.GetAsync(basketId)).ReturnsAsync(basket);

            ApprenticeshipFavouritesBasket savedBasket = null; // Setup callback so we can check contents of basket easier.
            _mockBasket.Setup(x => x.UpdateAsync(It.IsAny<ApprenticeshipFavouritesBasket>()))
                .Returns(Task.CompletedTask)
                .Callback<ApprenticeshipFavouritesBasket>((a) => savedBasket = a);

            var request = new AddOrRemoveFavouriteInBasketCommand
            {
                BasketId = basketId,
                ApprenticeshipId = "123",
                Ukprn = 12345678,
                LocationId = 10000020
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            response.Should().ShouldBeEquivalentTo(new AddOrRemoveFavouriteInBasketResponse(){ BasketId = basketId, ApprenticeshipName = "Standard 1", BasketOperation = BasketOperation.Added });
            _mockBasket.Verify(x => x.UpdateAsync(It.Is<ApprenticeshipFavouritesBasket>(b => b.Count() == 1)));

            var favourite = savedBasket.SingleOrDefault(x => x.ApprenticeshipId == "123");
            favourite.Should().NotBeNull();
            favourite.Providers.Should().NotBeNull();
            favourite.Providers.Count.Should().Be(1);
            favourite.Providers.First().Ukprn.Should().Be(12345678);
            favourite.Providers.First().Locations.Count().Should().Be(2);
            favourite.Providers.First().Locations.Contains(10000020);
            favourite.Providers.First().Name.Should().Be("TestProvider");
        }

        [Test]
        public async Task Handle_AddsTrainingProviderLocationToExistingBasket_ForNewApprenticeship()
        {
            var basketId = Guid.NewGuid();
            var basket = new ApprenticeshipFavouritesBasket { Id = basketId };
            basket.Add("111");

            _mockBasket.Setup(x => x.GetAsync(basketId)).ReturnsAsync(basket);

            ApprenticeshipFavouritesBasket savedBasket = null; // Setup callback so we can check contents of basket easier.
            _mockBasket.Setup(x => x.UpdateAsync(It.IsAny<ApprenticeshipFavouritesBasket>()))
                .Returns(Task.CompletedTask)
                .Callback<ApprenticeshipFavouritesBasket>((a) => savedBasket = a);

            var request = new AddOrRemoveFavouriteInBasketCommand
            {
                BasketId = basketId,
                ApprenticeshipId = "123",
                Ukprn = 12345678
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            response.Should().Be(basketId);
            _mockBasket.Verify(x => x.UpdateAsync(It.Is<ApprenticeshipFavouritesBasket>(b => b.Count() == 2)));

            var favourite = savedBasket.SingleOrDefault(x => x.ApprenticeshipId == "123");
            favourite.Should().NotBeNull();
            favourite.Providers.Count.Should().Be(1);
            favourite.Providers.First().Ukprn.Should().Be(12345678);
        }

        [Test]
        public async Task Handle_ShouldDeleteTrainingProviderLocationFromExistingBasket_WhenProviderLocationHasAlreadyBeenAddedForApprenticeship()
        {
            var basketId = Guid.NewGuid();
            var basket = new ApprenticeshipFavouritesBasket { Id = basketId };
            basket.Add("123", 12345678,"TestProvider", 10000020);
            basket.Add("123", 23456789, "TestProvider", 10000030);

            _mockBasket.Setup(x => x.GetAsync(basketId)).ReturnsAsync(basket);

            ApprenticeshipFavouritesBasket savedBasket = null; // Setup callback so we can check contents of basket easier.
            _mockBasket.Setup(x => x.UpdateAsync(It.IsAny<ApprenticeshipFavouritesBasket>()))
                .Returns(Task.CompletedTask)
                .Callback<ApprenticeshipFavouritesBasket>((a) => savedBasket = a);

            var request = new AddOrRemoveFavouriteInBasketCommand
            {
                BasketId = basketId,
                ApprenticeshipId = "123",
                Ukprn = 12345678,
                LocationId = 10000020
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            _mockBasket.Verify(x => x.UpdateAsync(It.IsAny<ApprenticeshipFavouritesBasket>()), Times.Once);

            var favourite = savedBasket.SingleOrDefault(x => x.ApprenticeshipId == "123");
            favourite.Should().NotBeNull();
            favourite.Providers.Count.Should().Be(1);
        }
    }
}
