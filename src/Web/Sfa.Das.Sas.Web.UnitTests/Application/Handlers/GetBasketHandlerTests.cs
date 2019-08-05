using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Handlers;
using Sfa.Das.Sas.ApplicationServices.Interfaces;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Core.Domain.Services;

namespace Sfa.Das.Sas.Web.UnitTests.Application.Handlers
{
    [TestFixture]
    public class GetBasketHandlerTests
    {
        private IRequestHandler<GetBasketQuery, ApprenticeshipFavouritesBasketRead> _sut;
        private Mock<IApprenticeshipFavouritesBasketStore> _mockBasket;
        private Mock<IGetFrameworks> _mockFramework;
        private Mock<IGetStandards> _mockStandard;
        private Mock<IGetProviderDetails> _mockProviders;

        [SetUp]
        public void Setup()
        {
            _mockBasket = new Mock<IApprenticeshipFavouritesBasketStore>();
            _sut = new GetBasketHandler(new NullLogger<GetBasketHandler>(), _mockBasket.Object,_mockStandard.Object,_mockFramework.Object,_mockProviders.Object);

            _mockBasket.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(new ApprenticeshipFavouritesBasketRead { new ApprenticeshipFavouriteRead() { ApprenticeshipId = "23" } });
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
            _mockBasket.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((ApprenticeshipFavouritesBasketRead)null);

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
