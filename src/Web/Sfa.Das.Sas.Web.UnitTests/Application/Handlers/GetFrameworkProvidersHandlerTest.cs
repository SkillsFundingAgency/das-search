using System.Threading;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Handlers;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;

namespace Sfa.Das.Sas.Web.UnitTests.Application.Handlers
{
    [TestFixture]
    public class GetFrameworkProvidersHandlerTest
    {
        private GetFrameworkProvidersHandler _sut;
        private Mock<IGetFrameworks> _mockGetFrameworks;

        [SetUp]
        public void Init()
        {
            _mockGetFrameworks = new Mock<IGetFrameworks>();

            _sut = new GetFrameworkProvidersHandler(_mockGetFrameworks.Object);
        }

        [Test]
        public void ShouldReturnStandardDetails()
        {
            var framework = new Framework
            {
                FrameworkId = "2",
                Title = "test framework",
                Level = 3
            };

            var query = new GetFrameworkProvidersQuery
            {
                FrameworkId = framework.FrameworkId,
                Keywords = "Test keywords",
                Postcode = "AB1 2CD"
            };

            _mockGetFrameworks.Setup(x => x.GetFrameworkById(framework.FrameworkId)).Returns(framework);

            var response = _sut.Handle(query, default(CancellationToken)).Result;

            response.FrameworkId.Should().Be(framework.FrameworkId);
            response.Keywords.Should().Be(query.Keywords);
            response.Postcode.Should().Be(query.Postcode);
            response.Title.Should().Be(framework.Title);
        }

        [Test]
        public void ShouldReturnNotFoundIsStandardCannotBeFound()
        {
            _mockGetFrameworks.Setup(x => x.GetFrameworkById(It.IsAny<string>()));

            var response = _sut.Handle(new GetFrameworkProvidersQuery(), default(CancellationToken)).Result;

            response.StatusCode.Should().Be(GetFrameworkProvidersResponse.ResponseCodes.NoFrameworkFound);
        }
    }
}
