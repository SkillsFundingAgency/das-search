using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Shared.Components.Controllers;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.Controller
{
    [TestFixture]
    public class BasketControllerTests
    {
        private const string APPRENTICESHIP_ID = "123";
        private Mock<IMediator> _mockMediator;
        private BasketController _sut;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _sut = new BasketController(_mockMediator.Object);
        }

        //TODO: LWA - Need to redirect back to origin of request.
        [Test]
        public async Task Add_ReturnsRedirectResult()
        {
            var result = await _sut.Add(APPRENTICESHIP_ID, null);

            result.Should().BeAssignableTo<RedirectResult>();
        }
    }
}
