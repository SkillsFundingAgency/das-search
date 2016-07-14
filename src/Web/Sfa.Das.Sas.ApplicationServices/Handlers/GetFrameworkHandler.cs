using System.Linq;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.Core.Domain.Services;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public class GetFrameworkHandler : IRequestHandler<GetFrameworkQuery, GetFrameworkResponse>
    {
        private readonly IGetFrameworks _getFrameworks;
        private readonly IShortlistCollection<int> _shortlistCollection;

        public GetFrameworkHandler(IGetFrameworks getFrameworks, IShortlistCollection<int> _shortlistCollection)
        {
            _getFrameworks = getFrameworks;
            this._shortlistCollection = _shortlistCollection;
        }

        public GetFrameworkResponse Handle(GetFrameworkQuery message)
        {
            var response = new GetFrameworkResponse();

            if (message.Id < 0)
            {
                response.StatusCode = GetFrameworkResponse.ResponseCodes.InvalidFrameworkId;

                return response;
            }

            var framework = _getFrameworks.GetFrameworkById(message.Id);

            if (framework == null)
            {
                response.StatusCode = GetFrameworkResponse.ResponseCodes.FrameworkNotFound;

                return response;
            }

            response.Framework = framework;

            var shortlistedApprenticeships = _shortlistCollection.GetAllItems(Constants.StandardsShortListName);
            response.IsShortlisted = shortlistedApprenticeships.Any(x => x.ApprenticeshipId.Equals(response.Framework.FrameworkId));
            response.SearchTerms = message.Keywords;

            return response;
        }
    }
}
