﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Apprenticeships.Api.Types.Providers;

namespace Sfa.Das.Sas.Core.Domain.Repositories
{
    public interface IProviderDetailRepository
    {
        Task<IEnumerable<ProviderSummary>> GetProviderList();
        Task<Provider> GetProviderDetails(long ukPrn);
    }
}