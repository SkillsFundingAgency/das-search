using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Responses;

namespace Sfa.Das.Sas.ApplicationServices.Queries
{
    public sealed class ProviderNameSearchQuery : IRequest<ProviderSearchNameResponse>
    {
        public string SearchTerm { get; set; }

        public int Page { get; set; }
    }
}
