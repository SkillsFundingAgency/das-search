using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Refit;
using Sfa.Das.FatApi.Client.Client;
using Sfa.Das.FatApi.Client.Model;

namespace Sfa.Das.FatApi.Client.Api
{
    public interface ISearchVApi
    {
        [Get("/v3/apprenticeship-programmes/search/")]
        Task<SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResults> SearchActiveApprenticeshipsAsync(string keywords, int? page = null, int? pageSize = null, int? order = null, string levels = null);

    }

}
