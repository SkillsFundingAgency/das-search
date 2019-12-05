using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Commands;
using Sfa.Das.Sas.ApplicationServices.Handlers;
using Sfa.Das.Sas.Shared.Basket.Interfaces;
using Sfa.Das.Sas.Shared.Basket.Models;
using Assert = NUnit.Framework.Assert;

namespace Sfa.Das.Sas.Web.UnitTests.Application.Handlers
{
    [TestFixture]
    public class ClearBasketHandlerTests
    {
        private IRequestHandler<ClearBasketCommand, Guid> _sut;
        private Mock<IApprenticeshipFavouritesBasketStore> _mockBasket;

        [SetUp]
        public void Setup()
        {
            _mockBasket = new Mock<IApprenticeshipFavouritesBasketStore>();
            _sut = new ClearBasketCommandHandler(_mockBasket.Object);
        }

        [Test]
        public async Task Handle_ClearBasket()
        {
            var basketId = Guid.NewGuid();

            var basket = new ApprenticeshipFavouritesBasket { Id = basketId };

            basket.Add("123", 12345678);

            _mockBasket.Setup(x => x.GetAsync(basketId)).ReturnsAsync(basket);

            var request = new ClearBasketCommand
            {
                BasketId = basketId
            };

            //var response = await _sut.Handle(request, default);
            //Console.WriteLine("basket: " + basket[0].ApprenticeshipId);
            //Assert.AreEqual(new ApprenticeshipFavouritesBasket {}, basket);
        }
    }
}
