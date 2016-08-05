using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Handlers;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;

namespace Sfa.Das.Sas.Web.UnitTests.Application.Handlers
{
    [TestFixture]
    public sealed class GetStandardHandlerTest
    {
        private GetStandardHandler _sut;
        private Mock<IGetStandards> _mockGetStandards;

        [SetUp]
        public void Init()
        {
            _mockGetStandards = new Mock<IGetStandards>();

            _sut = new GetStandardHandler(_mockGetStandards.Object);
        }

        [Test]
        public void ShouldReturnInvalidStandardIdStatusIfIdIsBelowZero()
        {
            var response = _sut.Handle(new GetStandardQuery { Id = -1, Keywords = "Test" });

            response.StatusCode.Should().Be(GetStandardResponse.ResponseCodes.InvalidStandardId);
        }

        [Test]
        public void ShouldReturnStandardNotFoundStatusIfStandardCannotBeFound()
        {
            var response = _sut.Handle(new GetStandardQuery() { Id = 1, Keywords = "Test" });

            response.StatusCode.Should().Be(GetStandardResponse.ResponseCodes.StandardNotFound);
        }

        [Test]
        public void ShouldGetStandardFromGetStandardRepository()
        {
            var query = new GetStandardQuery() { Id = 1, Keywords = "Test" };

            _mockGetStandards.Setup(x => x.GetStandardById(query.Id));

            var response = _sut.Handle(query);

            _mockGetStandards.Verify(x => x.GetStandardById(query.Id), Times.Once);
        }

        [Test]
        public void ShouldReturnFoundStandardInResponse()
        {
            var query = new GetStandardQuery() { Id = 1, Keywords = "Test" };
            var standard = new Standard { StandardId = query.Id };

            _mockGetStandards.Setup(x => x.GetStandardById(query.Id)).Returns(standard);

            var response = _sut.Handle(query);

            response.Standard.Should().Be(standard);
        }

        [Test]
        public void ShouldReturnSearchTerms()
        {
            var query = new GetStandardQuery() { Id = 1, Keywords = "Test" };
            var standard = new Standard { StandardId = query.Id };

            _mockGetStandards.Setup(x => x.GetStandardById(query.Id)).Returns(standard);

            var response = _sut.Handle(query);

            response.SearchTerms.Should().Be(query.Keywords);
        }
    }
}
