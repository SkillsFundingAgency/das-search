namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Logging;
    using FluentValidation;
    using MediatR;
    using Models;
    using Queries;
    using Sfa.Das.Sas.ApplicationServices.Settings;
    using Sfa.Das.Sas.ApplicationServices.Validators;
    using Sfa.Das.Sas.Core.Domain.Model;
    using Sfa.Das.Sas.Core.Domain.Services;

    public sealed class DetailProviderHandler : IRequestHandler<ProviderDetailQuery, DetailProviderResponse>
    {
        private readonly AbstractValidator<ProviderDetailQuery> _validator;

        private readonly IApprenticeshipProviderRepository _apprenticeshipProviderRepository;

        private readonly IShortlistCollection<int> _shortlistCollection;

        private readonly ILog _logger;

        private readonly IGetStandards _getStandards;

        private readonly IGetFrameworks _getFrameworks;

        public DetailProviderHandler(
            AbstractValidator<ProviderDetailQuery> validator,
            IApprenticeshipProviderRepository apprenticeshipProviderRepository,
            IShortlistCollection<int> shortlistCollection,
            IGetStandards getStandards,
            IGetFrameworks getFrameworks,
            ILog logger)
        {
            _validator = validator;
            _apprenticeshipProviderRepository = apprenticeshipProviderRepository;
            _shortlistCollection = shortlistCollection;
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
                   message.ProviderId,
                   message.LocationId,
                   message.StandardCode);

            var apprenticeshipData = _getStandards.GetStandardById(Convert.ToInt32(message.StandardCode));

            return CreateResponse(model, apprenticeshipData, ApprenticeshipTrainingType.Standard);
        }

        private DetailProviderResponse GetFramework(ProviderDetailQuery message)
        {
            var model = _apprenticeshipProviderRepository.GetCourseByFrameworkId(
                    message.ProviderId,
                    message.LocationId,
                    message.FrameworkId);

            var apprenticeshipProduct = _getFrameworks.GetFrameworkById(Convert.ToInt32(message.FrameworkId));

            return CreateResponse(model, apprenticeshipProduct, ApprenticeshipTrainingType.Framework);
        }

        private DetailProviderResponse CreateResponse(ApprenticeshipDetails model, IApprenticeshipProduct apprenticeshipProduct, ApprenticeshipTrainingType apprenticeshipProductType)
        {
            if (model != null && apprenticeshipProduct != null)
            {
                var response = new DetailProviderResponse
                                   {
                                       StatusCode = DetailProviderResponse.ResponseCodes.Success,
                                       ApprenticeshipDetails = model,
                                       ApprenticeshipType = apprenticeshipProductType,
                                       ApprenticeshipNameWithLevel = apprenticeshipProduct.Title,
                                       ApprenticeshipLevel = apprenticeshipProduct.Level.ToString()
                                   };

                response.IsShortlisted = IsShortlisted(response, model.Location.LocationId, model.Provider.Id);
                return response;
            }

            return new DetailProviderResponse() { StatusCode = DetailProviderResponse.ResponseCodes.ApprenticeshipProviderNotFound };
        }

        private bool IsShortlisted(DetailProviderResponse response, int locationId, int providerId)
        {
            var shortlistListName = response.ApprenticeshipType == ApprenticeshipTrainingType.Framework
               ? Constants.StandardsShortListName
               : Constants.FrameworksShortListName;

            var shortlistedApprenticeships = _shortlistCollection.GetAllItems(shortlistListName);

            var apprenticeship = shortlistedApprenticeships?.SingleOrDefault(x =>
                        x.ApprenticeshipId.Equals(response.ApprenticeshipDetails.Product.Apprenticeship.Code));

            var isShortlisted = apprenticeship?.ProvidersIdAndLocation.Any(x =>
                x.LocationId.Equals(locationId) &&
                x.ProviderId.Equals(providerId.ToString(), StringComparison.CurrentCulture));

            return isShortlisted.HasValue && isShortlisted.Value;
        }
    }
}
