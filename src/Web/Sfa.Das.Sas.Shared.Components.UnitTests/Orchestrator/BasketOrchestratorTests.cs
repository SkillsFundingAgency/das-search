using System;
using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Shared.Components.Cookies;
using Sfa.Das.Sas.Shared.Components.Mapping;
using Sfa.Das.Sas.Shared.Components.Orchestrators;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;
using Sfa.Das.Sas.Shared.Components.ViewModels;
using Sfa.Das.Sas.Shared.Components.ViewModels.Basket;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.Orchestrator
{
    [TestFixture]
    public class BasketOrchestratorTests
    {
        private BasketOrchestrator _sut;
        private Mock<IMediator> _mediatorMock;
        private Mock<IBasketViewModelMapper> _basketViewModelMapperMock;
        private Mock<ICookieManager> _cookieManagerMock;

        private string _basketId = "12345678-abcd-1234-abcd-0123456789ab";

        private ApprenticeshipFavouritesBasket _apprenticeshipFavouritesBasket = new ApprenticeshipFavouritesBasket()
        {
            new ApprenticeshipFavourite()
            {
                ApprenticeshipId = "123"
            },
            new ApprenticeshipFavourite()
            {
                ApprenticeshipId = "123-12-12"
            }
        };

        private BasketViewModel _basketViewModel;

        [SetUp]
        public void Setup()
        {
            _basketViewModel = new BasketViewModel()
            {
                BasketId = Guid.Parse(_basketId)
            };

            _mediatorMock = new Mock<IMediator>();
            _basketViewModelMapperMock = new Mock<IBasketViewModelMapper>();
            _cookieManagerMock = new Mock<ICookieManager>();

            _basketViewModelMapperMock.Setup(s => s.Map(new ApprenticeshipFavouritesBasket())).Returns(new BasketViewModel());
            _basketViewModelMapperMock.Setup(s => s.Map(_apprenticeshipFavouritesBasket)).Returns(_basketViewModel);

            _sut = new BasketOrchestrator(_mediatorMock.Object,_cookieManagerMock.Object,_basketViewModelMapperMock.Object);
        }


        [Test]
        public void When_getting_basket_without_cookie_Then_Empty_Model_Returned()
        {
            _cookieManagerMock.Setup(s=> s.Get(It.IsAny<string>())).Returns((string)null);

            var result = _sut.GetBasket().Result;

            result.BasketId.Should().NotHaveValue();
        }

        [Test]
        public void When_getting_valid_basket_with_cookie_Then_basket_returned()
        {
            _cookieManagerMock.Setup(s => s.Get(It.IsAny<string>())).Returns(_basketId);
            _mediatorMock.Setup(s => s.Send(It.IsAny<GetBasketQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(_apprenticeshipFavouritesBasket);

            _sut = new BasketOrchestrator(_mediatorMock.Object, _cookieManagerMock.Object, _basketViewModelMapperMock.Object);

            var result = _sut.GetBasket().Result;

            _basketViewModelMapperMock.Verify(s => s.Map(_apprenticeshipFavouritesBasket), Times.Once);

            result.BasketId.Should().HaveValue();
            result.Should().BeEquivalentTo(_basketViewModel);
        }

        [Test]
        public void When_getting_invalid_basket_with_cookie_Then_model_with()
        {
            _cookieManagerMock.Setup(s => s.Get(It.IsAny<string>())).Returns(_basketId);
            _mediatorMock.Setup(s => s.Send(It.IsAny<GetBasketQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ApprenticeshipFavouritesBasket());

            _sut = new BasketOrchestrator(_mediatorMock.Object, _cookieManagerMock.Object, _basketViewModelMapperMock.Object);

            var result = _sut.GetBasket().Result;

            _basketViewModelMapperMock.Verify(s => s.Map(new ApprenticeshipFavouritesBasket()), Times.Once);


            result.BasketId.Should().NotHaveValue();


        }


    }
}
