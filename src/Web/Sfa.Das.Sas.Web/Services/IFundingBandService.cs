using System.Collections.Generic;
using System.Data.SqlTypes;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Web.Services
{
    public interface IFundingBandService
    {
        FundingPeriod GetNextFundingPeriodWithinTimePeriod(List<FundingPeriod> fundingPeriods, int days);
    }
}
