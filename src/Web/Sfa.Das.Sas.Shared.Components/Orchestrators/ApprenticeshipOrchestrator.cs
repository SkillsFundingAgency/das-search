using System;
using System.Threading.Tasks;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Shared.Components.Mapping;
using Sfa.Das.Sas.Shared.Components.ViewComponents.ApprenticeshipDetails;
using SFA.DAS.NLog.Logger;

namespace Sfa.Das.Sas.Shared.Components.Orchestrators
{
    public class ApprenticeshipOrchestrator : IApprenticeshipOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ILog _logger;
        private readonly IFrameworkDetailsViewModelMapper _frameworkDetailsViewModelMapper;

        public ApprenticeshipOrchestrator(IMediator mediator, ILog logger, IFrameworkDetailsViewModelMapper frameworkDetailsViewModelMapper)
        {
            _mediator = mediator;
            _logger = logger;
            _frameworkDetailsViewModelMapper = frameworkDetailsViewModelMapper;
        }

        public async Task<FrameworkDetailsViewModel> GetFramework(string id)
        {
            var response = await _mediator.Send(new GetFrameworkQuery { Id = id });

            string message;

            switch (response.StatusCode)
            {
                case GetFrameworkResponse.ResponseCodes.InvalidFrameworkId:
                    _logger.Info("404 - Framework id has wrong format");


                    throw new ArgumentException($"Framework id: {id} has wrong format");
                
                case GetFrameworkResponse.ResponseCodes.FrameworkNotFound:
                    message = $"Cannot find framework: {id}";

                    _logger.Warn($"404 - {message}");

                    throw new ArgumentException(message);
              
                case GetFrameworkResponse.ResponseCodes.Gone:
                    message = $"Expired framework request: {id}";

                    _logger.Warn($"410 - {message}");

                    throw new ArgumentException(message);
                 
                case GetFrameworkResponse.ResponseCodes.Success:
                    _logger.Info($"Mapping Framework {id}");

                     var viewModel = _frameworkDetailsViewModelMapper.Map(response.Framework);

                    return viewModel;

                default:
                    _logger.Info($"Cannot handle GetFrameworkQuery response: {response.StatusCode.ToString()}");
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ApprenticeshipType GetApprenticeshipType(string id)
        {
            return id.Contains("-") ? ApprenticeshipType.Framework : ApprenticeshipType.Standard;
        }
    }
}
