using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Commands;
using Sfa.Das.Sas.ApplicationServices.Handlers;
using Sfa.Das.Sas.ApplicationServices.Interfaces;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Shared.Basket.Interfaces;
using Sfa.Das.Sas.Shared.Basket.Models;

namespace Sfa.Das.Sas.Web.UnitTests.Application.Handlers
{
    [TestFixture]
    public class AddFavouriteToBasketHandlerTests
    {
        private IRequestHandler<AddFavouriteToBasketCommand, Guid> _sut;
        private Mock<IApprenticeshipFavouritesBasketStore> _mockBasket;

        [SetUp]
        public void Setup()
        {
            _mockBasket = new Mock<IApprenticeshipFavouritesBasketStore>();
            _sut = new AddFavouriteToBasketCommandHandler(new NullLogger<AddFavouriteToBasketCommandHandler>(), _mockBasket.Object);
        }

        [Test]
        public async Task Handle_CreatesBasketId_WhenNoBasketSpecified()
        {
            var request = new AddFavouriteToBasketCommand
            {
                BasketId = null,
                ApprenticeshipId = "123"
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            response.Should().NotBeEmpty();
            _mockBasket.Verify(x => x.UpdateAsync(It.Is<ApprenticeshipFavouritesBasket>(a => a.Id != Guid.Empty)));
        }

        [Test]
        public async Task Handle_CreatesNewBasketId_IfPassedBasketIdNoLongerExistsInCache()
        {
            var expiredBasketId = Guid.NewGuid();

            var request = new AddFavouriteToBasketCommand
            {
                BasketId = expiredBasketId,
                ApprenticeshipId = "123"
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            response.Should().NotBeEmpty();
            _mockBasket.Verify(x => x.UpdateAsync(It.Is<ApprenticeshipFavouritesBasket>(a => a.Id != Guid.Empty && a.Id != expiredBasketId)));
        }

        [Test]
        public async Task Handle_AddsApprenticeshipToBasket_ForNewBasket()
        {
            var basketId = Guid.NewGuid();

            var request = new AddFavouriteToBasketCommand
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

            var request = new AddFavouriteToBasketCommand
            {
                BasketId = basketId,
                ApprenticeshipId = "123"
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            response.Should().Be(basketId);
            _mockBasket.Verify(x => x.UpdateAsync(It.Is<ApprenticeshipFavouritesBasket>(b => b.Count() == 2)));

            savedBasket.SingleOrDefault(x => x.ApprenticeshipId == "123").Should().NotBeNull();
        }

        [Test]
        public async Task Handle_ShouldRemoveApprenticeFromBasket_IfApprenticeshipAlreadyInBasket()
        {
            var basketId = Guid.NewGuid();
            var basket = new ApprenticeshipFavouritesBasket { Id = basketId };

            basket.Add("123", 23456);
            basket.Add("123", 12345);

            _mockBasket.Setup(x => x.GetAsync(basketId)).ReturnsAsync(basket);

            var request = new AddFavouriteToBasketCommand
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
        public async Task Handle_AddsProviderToBasket_ForNewBasket()
        {
            var basketId = Guid.NewGuid();

            var request = new AddFavouriteToBasketCommand
            {
                BasketId = null,
                ApprenticeshipId = "123",
                Ukprn = 12345678
            };

            await _sut.Handle(request, default(CancellationToken));

            _mockBasket.Verify(x => x.UpdateAsync(It.Is<ApprenticeshipFavouritesBasket>(a => a[0].ApprenticeshipId == "123" && a[0].Ukprns.Contains(12345678))));
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

            var request = new AddFavouriteToBasketCommand
            {
                BasketId = basketId,
                ApprenticeshipId = "123",
                Ukprn = 12345678
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            response.Should().Be(basketId);
            _mockBasket.Verify(x => x.UpdateAsync(It.Is<ApprenticeshipFavouritesBasket>(b => b.Count() == 1)));

            var favourite = savedBasket.SingleOrDefault(x => x.ApprenticeshipId == "123");
            favourite.Should().NotBeNull();
            favourite.Ukprns.Count.Should().Be(1);
            favourite.Ukprns.First().Should().Be(12345678);
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

            var request = new AddFavouriteToBasketCommand
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
            favourite.Ukprns.Count.Should().Be(1);
            favourite.Ukprns.First().Should().Be(12345678);
        }

        [Test]
        public async Task Handle_ShouldDeleteTrainingProviderFromExistingBasket_WhenProviderHasAlreadyBeenAddedForApprenticeship()
        {
            var basketId = Guid.NewGuid();
            var basket = new ApprenticeshipFavouritesBasket { Id = basketId };
            basket.Add("123", 12345678);
            basket.Add("123", 23456789);

            _mockBasket.Setup(x => x.GetAsync(basketId)).ReturnsAsync(basket);

            ApprenticeshipFavouritesBasket savedBasket = null; // Setup callback so we can check contents of basket easier.
            _mockBasket.Setup(x => x.UpdateAsync(It.IsAny<ApprenticeshipFavouritesBasket>()))
                .Returns(Task.CompletedTask)
                .Callback<ApprenticeshipFavouritesBasket>((a) => savedBasket = a);

            var request = new AddFavouriteToBasketCommand
            {
                BasketId = basketId,
                ApprenticeshipId = "123",
                Ukprn = 12345678
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            _mockBasket.Verify(x => x.UpdateAsync(It.IsAny<ApprenticeshipFavouritesBasket>()), Times.Once);

            var favourite = savedBasket.SingleOrDefault(x => x.ApprenticeshipId == "123");
            favourite.Should().NotBeNull();
            favourite.Ukprns.Count.Should().Be(1);
            favourite.Ukprns.Should().NotContain(12345678);
        }
    }
}
