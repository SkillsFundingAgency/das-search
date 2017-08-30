namespace Sfa.Das.Sas.Web.UnitTests.Application.Handlers
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Core.Domain.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Sas.ApplicationServices.Handlers;
    using Sas.ApplicationServices.Queries;
    using Sas.ApplicationServices.Responses;
    using SFA.DAS.Apprenticeships.Api.Types.Exceptions;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;

    [TestFixture]
    public class ProviderDetailHandlerTests
    {
        private Mock<IProviderDetailRepository> _mockProviderDetailRepository;
        private ProviderDetailHandler _handler;

        [Test]
        public void ShouldBeSuccessIfProviderReturned()
        {
            var provider = new Provider();
            _mockProviderDetailRepository = new Mock<IProviderDetailRepository>();
            _mockProviderDetailRepository.Setup(x => x.GetProviderDetails(It.IsAny<long>())).Returns(Task.FromResult(provider));
            _handler = new ProviderDetailHandler(_mockProviderDetailRepository.Object);
            var message = new ProviderDetailQuery();
            var response = _handler.Handle(message).Result;

            response.StatusCode.Should().Be(ProviderDetailResponse.ResponseCodes.Success);
            response.Provider.Should().Be(provider);
        }

        [Test]
        public void ShouldBeProviderNotFoundStatusCodeIfNoProviderReturned()
        {
            var entityNotfoundException = new EntityNotFoundException(string.Empty, new Exception());
            _mockProviderDetailRepository = new Mock<IProviderDetailRepository>();
            _mockProviderDetailRepository.Setup(x => x.GetProviderDetails(It.IsAny<long>())).Throws(entityNotfoundException);
            _handler = new ProviderDetailHandler(_mockProviderDetailRepository.Object);
            var message = new ProviderDetailQuery();
            var response = _handler.Handle(message).Result;

            response.StatusCode.Should().Be(ProviderDetailResponse.ResponseCodes.ProviderNotFound);
            response.Provider.Should().BeNull();
        }

        [Test]
        public void ShouldBeUkPrnNotCorrectLengthStatusCodeIfOddStringLength()
        {
            _mockProviderDetailRepository = new Mock<IProviderDetailRepository>();
            _mockProviderDetailRepository.Setup(x => x.GetProviderDetails(It.IsAny<long>())).Throws(new HttpRequestException());
            _handler = new ProviderDetailHandler(_mockProviderDetailRepository.Object);
            var message = new ProviderDetailQuery();
            var response = _handler.Handle(message).Result;

            response.StatusCode.Should().Be(ProviderDetailResponse.ResponseCodes.UkPrnNotCorrectLength);
            response.Provider.Should().BeNull();
        }
    }
}
