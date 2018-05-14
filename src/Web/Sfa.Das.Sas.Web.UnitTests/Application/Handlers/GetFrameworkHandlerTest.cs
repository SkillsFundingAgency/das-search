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
    using Sas.ApplicationServices.Validators;
    using SFA.DAS.NLog.Logger;

    [TestFixture]
    public sealed class GetFrameworkHandlerTest
    {
        private GetFrameworkHandler _sut;
        private Mock<IGetFrameworks> _mockGetFrameworks;

        [SetUp]
        public void Init()
        {
            _mockGetFrameworks = new Mock<IGetFrameworks>();

            _sut = new GetFrameworkHandler(
                _mockGetFrameworks.Object,
                new FrameworkQueryValidator(new Validation()),
                Mock.Of<ILog>());
        }

        [TestCase("1-2-a")]
        [TestCase("a-2-3")]
        [TestCase("1-a-3")]
        [TestCase("1-2-3a")]
        [TestCase("a1-2-3")]
        [TestCase("1-2a-3")]
        [TestCase("--")]
        [TestCase("1--")]
        [TestCase("-2-")]
        [TestCase("--3")]
        [TestCase("1--3")]
        [TestCase("-2-3")]
        [TestCase("1-2-")]
        [TestCase("")]
        public void ShouldReturnInvalidFrameworkIdStatus(string frameworkId)
        {
            var response = _sut.Handle(new GetFrameworkQuery { Id = frameworkId, Keywords = "Test" });

            response.StatusCode.Should().Be(GetFrameworkResponse.ResponseCodes.InvalidFrameworkId);
        }

        [Test]
        public void ShouldReturnInvalidFrameworkIdStatusIfIdIsBelowZero()
        {
            var response = _sut.Handle(new GetFrameworkQuery() { Id = "-1", Keywords = "Test" });

            response.StatusCode.Should().Be(GetFrameworkResponse.ResponseCodes.InvalidFrameworkId);
        }

        [Test]
        public void ShouldReturnFrameworkNotFoundStatusIfFrameworkCannotBeFound()
        {
            var response = _sut.Handle(new GetFrameworkQuery { Id = "4-1-2", Keywords = "Test" });

            response.StatusCode.Should().Be(GetFrameworkResponse.ResponseCodes.FrameworkNotFound);
        }

        [Test]
        public void ShouldReturnFrameworkGoneStatusIfFrameworkIsNotActive()
        {
            var query = new GetFrameworkQuery { Id = "1-2-3", Keywords = "Test" };
            var framework = new Framework { FrameworkId = query.Id, IsActiveFramework = false };

            _mockGetFrameworks.Setup(x => x.GetFrameworkById(query.Id)).Returns(framework);

            var response = _sut.Handle(query);

            response.StatusCode.Should().Be(GetFrameworkResponse.ResponseCodes.Gone);
        }

        [Test]
        public void ShouldGetFrameworkFromGetFrameworkRepository()
        {
            var query = new GetFrameworkQuery() { Id = "1-2-3", Keywords = "Test" };

            _mockGetFrameworks.Setup(x => x.GetFrameworkById(query.Id));

            var response = _sut.Handle(query);

            _mockGetFrameworks.Verify(x => x.GetFrameworkById(query.Id), Times.Once);
        }

        [Test]
        public void ShouldReturnFoundFrameworkInResponse()
        {
            var query = new GetFrameworkQuery { Id = "1-2-3", Keywords = "Test" };
            var framework = new Framework { FrameworkId = query.Id, IsActiveFramework = true};

            _mockGetFrameworks.Setup(x => x.GetFrameworkById(query.Id)).Returns(framework);

            var response = _sut.Handle(query);

            response.Framework.Should().Be(framework);
        }

        [Test]
        public void ShouldReturnSearchTerms()
        {
            var query = new GetFrameworkQuery { Id = "1-2-3", Keywords = "Test" };
            var framework = new Framework { FrameworkId = query.Id, IsActiveFramework = true};

            _mockGetFrameworks.Setup(x => x.GetFrameworkById(query.Id)).Returns(framework);

            var response = _sut.Handle(query);

            response.SearchTerms.Should().Be(query.Keywords);
        }
    }
}
