using SFA.DAS.Apprenticeships.Api.Types.Pagination;

namespace Sfa.Das.Sas.Web.UnitTests.Application.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Core.Domain.Services;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Sas.ApplicationServices.Handlers;
    using Sas.ApplicationServices.Queries;
    using Sas.ApplicationServices.Responses;
    using SFA.DAS.Apprenticeships.Api.Types;
    using SFA.DAS.Apprenticeships.Api.Types.Exceptions;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;

    [TestFixture]
    public class ProviderDetailHandlerTests
    {
        private Mock<IGetProviderDetails> _mockProviderDetailRepository;
        private ProviderDetailHandler _handler;

        [Test]
        public void ShouldBeSuccessIfProviderReturned()
        {
            var provider = new Provider();
            var apprenticeshipTrainingSummary = new ApprenticeshipTrainingSummary
            {
                Count = 1,
                Ukprn = 42,
                ApprenticeshipTrainingItems = new List<ApprenticeshipTraining>
                {
                    new ApprenticeshipTraining
                    {
                        Identifier = "5",
                        TrainingType = ApprenticeshipTrainingType.Framework,
                        Type = "Framework",
                        Level = 3,
                        Name = "Software engineer"
                    }
                },
                PaginationDetails = new PaginationDetails
                {
                    LastPage = 2,
                    NumberOfRecordsToSkip = 0,
                    NumberPerPage = 20,
                    Page = 1,
                    TotalCount = 21
                }
            };
            _mockProviderDetailRepository = new Mock<IGetProviderDetails>();
            _mockProviderDetailRepository.Setup(x => x.GetProviderDetails(It.IsAny<long>())).Returns(Task.FromResult(provider));
            _mockProviderDetailRepository.Setup(x => x.GetApprenticeshipTrainingSummary(It.IsAny<long>(), It.IsAny<int>())).Returns(Task.FromResult(apprenticeshipTrainingSummary));
            _handler = new ProviderDetailHandler(_mockProviderDetailRepository.Object);
            var message = new ProviderDetailQuery();
            var response = _handler.Handle(message).Result;

            response.StatusCode.Should().Be(ProviderDetailResponse.ResponseCodes.Success);
            response.Provider.Should().Be(provider);
            response.ApprenticeshipTrainingSummary.Should().Be(apprenticeshipTrainingSummary);
        }

        [Test]
        public void ShouldBeProviderNotFoundStatusCodeIfNoProviderReturned()
        {
            var entityNotfoundException = new EntityNotFoundException(string.Empty, new Exception());
            _mockProviderDetailRepository = new Mock<IGetProviderDetails>();
            _mockProviderDetailRepository.Setup(x => x.GetProviderDetails(It.IsAny<long>())).Throws(entityNotfoundException);
            _handler = new ProviderDetailHandler(_mockProviderDetailRepository.Object);
            var message = new ProviderDetailQuery();
            var response = _handler.Handle(message).Result;

            response.StatusCode.Should().Be(ProviderDetailResponse.ResponseCodes.ProviderNotFound);
            response.Provider.Should().BeNull();
        }

        [Test]
        public void ShouldBeHttpRequestExceptionIfHttpRequestExceptionThrown()
        {
            _mockProviderDetailRepository = new Mock<IGetProviderDetails>();
            _mockProviderDetailRepository.Setup(x => x.GetProviderDetails(It.IsAny<long>())).Throws(new HttpRequestException());
            _handler = new ProviderDetailHandler(_mockProviderDetailRepository.Object);
            var message = new ProviderDetailQuery();
            var response = _handler.Handle(message).Result;

            response.StatusCode.Should().Be(ProviderDetailResponse.ResponseCodes.HttpRequestException);
            response.Provider.Should().BeNull();
        }
    }
}
