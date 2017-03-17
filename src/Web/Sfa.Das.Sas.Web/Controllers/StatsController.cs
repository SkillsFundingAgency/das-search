using System.Web.Mvc;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;

using Sfa.Das.Sas.Web.Attribute;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;
using SFA.DAS.NLog.Logger;

namespace Sfa.Das.Sas.Web.Controllers
{
    [NoCache]
    public sealed class StatsController : Controller
    {
        private readonly ILog _logger;
        private readonly IMappingService _mappingService;
        private readonly IMediator _mediator;

        public StatsController(
            ILog logger,
            IMappingService mappingService,
            IMediator mediator)
        {
            _logger = logger;
            _mappingService = mappingService;
            _mediator = mediator;
        }

        public ActionResult Stats()
        {
            var response = _mediator.Send(new StatsQuery());

            var viewModel = _mappingService.Map<StatsResponse, StatsViewModel>(response);

            return View(viewModel);
        }
    }
}