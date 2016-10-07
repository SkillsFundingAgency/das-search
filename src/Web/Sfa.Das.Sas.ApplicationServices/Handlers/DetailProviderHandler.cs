using System;
using System.Linq;
using FluentValidation;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.ApplicationServices.Validators;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Core.Logging;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public sealed class DetailProviderHandler : IRequestHandler<ProviderDetailQuery, DetailProviderResponse>
    {
        private readonly AbstractValidator<ProviderDetailQuery> _validator;
        private readonly IApprenticeshipProviderRepository _apprenticeshipProviderRepository;
        private readonly ILog _logger;
        private readonly IGetStandards _getStandards;
        private readonly IGetFrameworks _getFrameworks;

        public DetailProviderHandler(
            AbstractValidator<ProviderDetailQuery> validator,
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

        public DetailProviderResponse Handle(ProviderDetailQuery message)
        {
            var result = _validator.Validate(message);

            if (result.Errors.Any(x => x.ErrorCode == ValidationCodes.InvalidInput))
            {
                return new DetailProviderResponse { StatusCode = DetailProviderResponse.ResponseCodes.InvalidInput };
            }

            if (result.IsValid && !string.IsNullOrEmpty(message.StandardCode))
            {
                return GetStandard(message);
            }

            if (result.IsValid && !string.IsNullOrEmpty(message.FrameworkId))
            {
                return GetFramework(message);
            }

            return new DetailProviderResponse { StatusCode = DetailProviderResponse.ResponseCodes.ApprenticeshipProviderNotFound };
        }

        private DetailProviderResponse GetStandard(ProviderDetailQuery message)
        {
            var model = _apprenticeshipProviderRepository.GetCourseByStandardCode(
                message.Ukprn,
                message.LocationId,
                message.StandardCode);

            var apprenticeshipData = _getStandards.GetStandardById(message.StandardCode);

            return CreateResponse(model, apprenticeshipData, ApprenticeshipTrainingType.Standard);
        }

        private DetailProviderResponse GetFramework(ProviderDetailQuery message)
        {
            var model = _apprenticeshipProviderRepository.GetCourseByFrameworkId(
                message.Ukprn,
                message.LocationId,
                message.FrameworkId);

            var apprenticeshipProduct = _getFrameworks.GetFrameworkById(message.FrameworkId);

            return CreateResponse(model, apprenticeshipProduct, ApprenticeshipTrainingType.Framework);
        }

        private DetailProviderResponse CreateResponse(ApprenticeshipDetails model, IApprenticeshipProduct apprenticeshipProduct, ApprenticeshipTrainingType apprenticeshipProductType)
        {
            if (model == null || apprenticeshipProduct == null)
            {
                return new DetailProviderResponse
                {
                    StatusCode = DetailProviderResponse.ResponseCodes.ApprenticeshipProviderNotFound
                };
            }

            var response = new DetailProviderResponse
            {
                StatusCode = DetailProviderResponse.ResponseCodes.Success,
                ApprenticeshipDetails = model,
                ApprenticeshipType = apprenticeshipProductType,
                ApprenticeshipName = apprenticeshipProduct.Title,
                ApprenticeshipLevel = apprenticeshipProduct.Level.ToString()
            };

            return response;
        }
    }
}