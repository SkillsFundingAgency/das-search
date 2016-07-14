using System.Linq;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.Core.Domain.Services;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public class GetStandardHandler : IRequestHandler<GetStandardQuery, GetStandardResponse>
    {
        private readonly IGetStandards _getStandards;
        private readonly IShortlistCollection<int> _shortlistCollection;

        public GetStandardHandler(IGetStandards getStandards, IShortlistCollection<int> _shortlistCollection)
        {
            _getStandards = getStandards;
            this._shortlistCollection = _shortlistCollection;
        }

        public GetStandardResponse Handle(GetStandardQuery message)
        {
            var response = new GetStandardResponse();

            if (message.Id < 0)
            {
                response.StatusCode = GetStandardResponse.ResponseCodes.InvalidStandardId;

                return response;
            }

            var standard = _getStandards.GetStandardById(message.Id);

            if (standard == null)
            {
                response.StatusCode = GetStandardResponse.ResponseCodes.StandardNotFound;

                return response;
            }

            response.Standard = standard;

            var shortlistedApprenticeships = _shortlistCollection.GetAllItems(Constants.StandardsShortListName);
            response.IsShortlisted = shortlistedApprenticeships.Any(x => x.ApprenticeshipId.Equals(response.Standard.StandardId));
            response.SearchTerms = message.Keywords;

            return response;
        }
    }
}
