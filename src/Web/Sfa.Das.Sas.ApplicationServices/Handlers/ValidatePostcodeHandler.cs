using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.ApplicationServices.Services;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public class ValidatePostcodeHandler : IRequestHandler<ValidatePostcodeQuery, bool>
    {
        private readonly IPostcodeIoService _postcodeIoService;

        public ValidatePostcodeHandler(IPostcodeIoService postcodeIoService)
        {
            _postcodeIoService = postcodeIoService;
        }

        public async Task<bool> Handle(ValidatePostcodeQuery request, CancellationToken cancellationToken)
        {
            if (String.IsNullOrWhiteSpace(request.Postcode))
            {
                return false;
            }

            var status = await _postcodeIoService.GetPostcodeStatus(request.Postcode);
            switch (status)
            {
                case "England":
                case "Wales":
                case "Scotland":
                case "Northern Ireland":
                    return true;
                default:
                    return false;
            }
        }
    }
}