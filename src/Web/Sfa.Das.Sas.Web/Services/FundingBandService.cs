using System;
using System.Collections.Generic;
using System.Linq;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Web.Services
{
    public class FundingBandService: IFundingBandService
    {
        public FundingPeriod GetNextFundingPeriodWithinTimePeriod(List<FundingPeriod> fundingPeriods, DateTime? currentEffectiveFrom, int months)
        {
            return fundingPeriods?
               .Where(f => f.EffectiveFrom.HasValue && f.EffectiveFrom.Value >= DateTime.Today && f.EffectiveFrom.Value <= DateTime.Today.AddMonths(months))
               .Where(f => !currentEffectiveFrom.HasValue || f.EffectiveFrom > currentEffectiveFrom.Value)
               .OrderBy(f => f.EffectiveFrom).FirstOrDefault();
        }
    }
}