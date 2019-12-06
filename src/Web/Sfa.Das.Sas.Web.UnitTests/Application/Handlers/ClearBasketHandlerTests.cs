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

        //[Test]
        //public async Task Handle_ClearBasket()
        //{
        //    var basketId = Guid.NewGuid();

        //    var basket = new ApprenticeshipFavouritesBasket { Id = basketId };

        //    basket.Add("123", 12345678);

        //    _mockBasket.Setup(x => x.GetAsync(basketId)).ReturnsAsync(basket);

        //    ApprenticeshipFavouritesBasket savedBasket = null; // Setup callback so we can check contents of basket easier.

        //    _mockBasket.Setup(x => x.ClearBasketAsync(It.IsAny<ApprenticeshipFavouritesBasket>()))
        //        .Returns(Task.CompletedTask)
        //        .Callback<ApprenticeshipFavouritesBasket>((a) => savedBasket = a);

        //    var request = new ClearBasketCommand
        //    {
        //        BasketId = basketId
        //    };

        //    await _sut.Handle(request, default);
            
        //    var newBasket = await _mockBasket.Object.GetAsync(basketId);
        //    //_mockBasket.Verify(x => x.ClearBasketAsync(It.IsAny<ApprenticeshipFavouritesBasket>()), Times.Once);

        //    //var apprenticeshipsCount = savedBasket._items.Count;
            
        //    Assert.AreEqual(0, newBasket._items.Count);
        //}
    }
}
