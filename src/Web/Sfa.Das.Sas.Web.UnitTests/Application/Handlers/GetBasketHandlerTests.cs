using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Handlers;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Shared.Basket.Interfaces;
using Sfa.Das.Sas.Shared.Basket.Models;

namespace Sfa.Das.Sas.Web.UnitTests.Application.Handlers
{
    [TestFixture]
    public class GetBasketHandlerTests
    {
        private IRequestHandler<GetBasketQuery, ApprenticeshipFavouritesBasket> _sut;
        private Mock<IApprenticeshipFavouritesBasketStore> _mockBasket;

        [SetUp]
        public void Setup()
        {
            _mockBasket = new Mock<IApprenticeshipFavouritesBasketStore>();
            _sut = new GetBasketHandler(new NullLogger<GetBasketHandler>(), _mockBasket.Object);

            _mockBasket.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(new ApprenticeshipFavouritesBasket { new ApprenticeshipFavourite { ApprenticeshipId = "23" } });
        }

        [Test]
        public async Task Handle_ReturnsBasketFromCache_ForGivenBasketId()
        {
            var basketId = Guid.NewGuid();

            var request = new GetBasketQuery
            {
                BasketId = basketId
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            response.Should().NotBeNull();
            response.Should().NotBeEmpty();
            _mockBasket.Verify(x => x.GetAsync(basketId));
        }

        [Test]
        public async Task Handle_ReturnsEmptyBasket_IfBasketDoesNotExist()
        {
            _mockBasket.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((ApprenticeshipFavouritesBasket)null);

            var basketId = Guid.NewGuid();

            var request = new GetBasketQuery
            {
                BasketId = basketId
            };

            var response = await _sut.Handle(request, default(CancellationToken));

            response.Should().NotBeNull();
            response.Should().BeEmpty();
            _mockBasket.Verify(x => x.GetAsync(basketId));
        }
    }
}
