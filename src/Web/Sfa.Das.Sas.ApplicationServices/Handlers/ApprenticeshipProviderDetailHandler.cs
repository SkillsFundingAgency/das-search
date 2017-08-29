using System;
using System.Linq;
using FluentValidation;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.ApplicationServices.Validators;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Repositories;
using Sfa.Das.Sas.Core.Domain.Services;

using SFA.DAS.NLog.Logger;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public sealed class ApprenticeshipProviderDetailHandler : IRequestHandler<ApprenticeshipProviderDetailQuery, ApprenticeshipProviderDetailResponse>
    {
        private readonly AbstractValidator<ApprenticeshipProviderDetailQuery> _validator;
        private readonly IApprenticeshipProviderRepository _apprenticeshipProviderRepository;
        private readonly ILog _logger;
        private readonly IGetStandards _getStandards;
        private readonly IGetFrameworks _getFrameworks;

        public ApprenticeshipProviderDetailHandler(
            AbstractValidator<ApprenticeshipProviderDetailQuery> validator,
            IApprenticeshipProviderRepository apprenticeshipProviderRepository,
            IGetStandards getStandards,
            IGetFrameworks getFrameworks,
            ILog logger)
        {
            _validator = validator;
            _apprenticeshipProviderRepository = apprenticeshipProviderRepository;
            _getStandards = getStandards;
            _getFrameworks = getFrameworks;
            _logger = logger;
        }

        public ApprenticeshipProviderDetailResponse Handle(ApprenticeshipProviderDetailQuery message)
        {
            var result = _validator.Validate(message);

            if (result.Errors.Any(x => x.ErrorCode == ValidationCodes.InvalidInput))
            {
                return new ApprenticeshipProviderDetailResponse { StatusCode = ApprenticeshipProviderDetailResponse.ResponseCodes.InvalidInput };
            }

            if (result.IsValid && !string.IsNullOrEmpty(message.StandardCode))
            {
                return GetStandard(message);
            }

            if (result.IsValid && !string.IsNullOrEmpty(message.FrameworkId))
            {
                return GetFramework(message);
            }

            return new ApprenticeshipProviderDetailResponse { StatusCode = ApprenticeshipProviderDetailResponse.ResponseCodes.ApprenticeshipProviderNotFound };
        }

        private ApprenticeshipProviderDetailResponse GetStandard(ApprenticeshipProviderDetailQuery message)
        {
            var model = _apprenticeshipProviderRepository.GetCourseByStandardCode(
                message.UkPrn,
                message.LocationId,
                message.StandardCode);

            var apprenticeshipData = _getStandards.GetStandardById(message.StandardCode);

            return CreateResponse(model, apprenticeshipData, ApprenticeshipTrainingType.Standard);
        }

        private ApprenticeshipProviderDetailResponse GetFramework(ApprenticeshipProviderDetailQuery message)
        {
            var model = _apprenticeshipProviderRepository.GetCourseByFrameworkId(
                message.UkPrn,
                message.LocationId,
                message.FrameworkId);

            var apprenticeshipProduct = _getFrameworks.GetFrameworkById(message.FrameworkId);

            return CreateResponse(model, apprenticeshipProduct, ApprenticeshipTrainingType.Framework);
        }

        private ApprenticeshipProviderDetailResponse CreateResponse(ApprenticeshipDetails model, IApprenticeshipProduct apprenticeshipProduct, ApprenticeshipTrainingType apprenticeshipProductType)
        {
            if (model == null || apprenticeshipProduct == null)
            {
                return new ApprenticeshipProviderDetailResponse
                {
                    StatusCode = ApprenticeshipProviderDetailResponse.ResponseCodes.ApprenticeshipProviderNotFound
                };
            }

            var response = new ApprenticeshipProviderDetailResponse
            {
                StatusCode = ApprenticeshipProviderDetailResponse.ResponseCodes.Success,
                ApprenticeshipDetails = model,
                ApprenticeshipType = apprenticeshipProductType,
                ApprenticeshipName = apprenticeshipProduct.Title,
                ApprenticeshipLevel = apprenticeshipProduct.Level.ToString()
            };

            return response;
        }
    }
}