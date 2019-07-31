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
            _mockBasket.Verify(x => x.UpdateAsync(It.Is<Guid>(a => a != Guid.Empty), It.IsAny<ApprenticeshipFavouritesBasket>()));
        }

        [Test]
        public async Task Handle_CreatesSameBasketId_IfPassedBasketIdNoLongerExistsInCache()
        {
            var expiredBasketId = Guid.NewGuid();

            var request = new AddFavouriteToBasketCommand
            {
                BasketId = expiredBasketId,
                ApprenticeshipId = "123"
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            response.Should().NotBeEmpty();
            _mockBasket.Verify(x => x.UpdateAsync(It.Is<Guid>(a => a != Guid.Empty && a == expiredBasketId), It.IsAny<ApprenticeshipFavouritesBasket>()));
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

            _mockBasket.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.Is<ApprenticeshipFavouritesBasket>(a => a[0].ApprenticeshipId == "123")));
        }

        [Test]
        public async Task Handle_AddsApprenticeshipToExistingBasket_ForNewApprenticeship()
        {
            var basketId = Guid.NewGuid();
            _mockBasket.Setup(x => x.GetAsync(basketId)).ReturnsAsync(new ApprenticeshipFavouritesBasket { new ApprenticeshipFavourite("111") });

            ApprenticeshipFavouritesBasket savedBasket = null; // Setup callback so we can check contents of basket easier.
            _mockBasket.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ApprenticeshipFavouritesBasket>()))
                .Returns(Task.CompletedTask)
                .Callback<Guid, ApprenticeshipFavouritesBasket>((a, b) => savedBasket = b);

            var request = new AddFavouriteToBasketCommand
            {
                BasketId = basketId,
                ApprenticeshipId = "123"
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            response.Should().Be(basketId);
            _mockBasket.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.Is<ApprenticeshipFavouritesBasket>(b => b.Count == 2)));

            savedBasket.SingleOrDefault(x => x.ApprenticeshipId == "123").Should().NotBeNull();
        }

        [Test]
        public async Task Handle_ShouldNotUpdateBasketForJustApprenticeship_IfApprenticeshipAlreadyInBasket()
        {
            var basketId = Guid.NewGuid();
            _mockBasket.Setup(x => x.GetAsync(basketId)).ReturnsAsync(new ApprenticeshipFavouritesBasket { new ApprenticeshipFavourite("123") });

            var request = new AddFavouriteToBasketCommand
            {
                BasketId = basketId,
                ApprenticeshipId = "123"
            };

            await _sut.Handle(request, default(CancellationToken));

            _mockBasket.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ApprenticeshipFavouritesBasket>()), Times.Never);
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

            _mockBasket.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.Is<ApprenticeshipFavouritesBasket>(a => a[0].ApprenticeshipId == "123" && a[0].Ukprns.Contains(12345678))));
        }

        [Test]
        public async Task Handle_AddsTrainingProviderToExistingBasket_ForNewApprenticeship()
        {
            Assert.Fail();
        }

        [Test]
        public async Task Handle_AddsTrainingProviderToExistingBasket_ForExistingApprenticeship()
        {
            Assert.Fail();
        }

        [Test]
        public async Task Handle_AddsTrainingProviderToExistingBasket_WhenProviderHasAlreadyBeenAddedForApprenticeship()
        {
            Assert.Fail();
        }
    }
}
