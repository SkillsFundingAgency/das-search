using System;
using System.Collections.Generic;
using SFA.DAS.Apprenticeships.Api.Types;

namespace Sfa.Das.Sas.Web.Services
{
    public interface IFundingBandService
    {
        FundingPeriod GetNextFundingPeriodWithinTimePeriod(List<FundingPeriod> fundingPeriods, DateTime? currentEffectiveFrom, int months);
    }
}
