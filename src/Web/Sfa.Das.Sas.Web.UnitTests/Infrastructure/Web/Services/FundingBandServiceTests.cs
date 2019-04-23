using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.Web.Services;
using SFA.DAS.Apprenticeships.Api.Types;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services
{
    [TestFixture]
    public class FundingBandServiceTests
    {
        private static readonly DateTime CurrentDatetime = DateTime.Today;
        private FundingBandService _fundingBandService;
 
        [SetUp]
        public void SetUp()
        {
            _fundingBandService = new FundingBandService();
        }

        [TestCaseSource(nameof(GetCurrentEffectiveFrom))]
        public void ShouldReturnNullIfNoFundingPeriodsEntered(DateTime? currentEffectiveFrom)
        {
            var res = _fundingBandService.GetNextFundingPeriodWithinTimePeriod(null, currentEffectiveFrom, 3);
            res.Should().BeNull();
        }

        [TestCaseSource(nameof(GetCurrentEffectiveFrom))]
        public void ShouldReturnTheSingleInstanceAt3Months(DateTime? currentEffectiveFrom)
        {
            var numberOfMonths = 3;
            var expectedFundingCap = 1500;
            var usedDateTime = DateTime.Today.AddMonths(numberOfMonths);
            var expectedFundingPeriod = new FundingPeriod { EffectiveFrom = usedDateTime, FundingCap = expectedFundingCap};

            var fundingBands = new List<FundingPeriod>
            {
                expectedFundingPeriod
            };

            var res = _fundingBandService.GetNextFundingPeriodWithinTimePeriod(fundingBands, currentEffectiveFrom, numberOfMonths);
           res.Should().Be(expectedFundingPeriod);
        }

        [TestCaseSource(nameof(GetCurrentEffectiveFrom))]
        public void ShouldReturnNoDetailsOfSingleInstanceAt3MonthsAnd1Day(DateTime? currentEffectiveFrom)
        {
            var numberOfMonths = 3;
            var expectedFundingCap = 1500;
            var usedDateTime = DateTime.Today.AddMonths(numberOfMonths).AddDays(1);
            var expectedFundingPeriod = new FundingPeriod { EffectiveFrom = usedDateTime, FundingCap = expectedFundingCap };

            var fundingBands = new List<FundingPeriod>
            {
                expectedFundingPeriod
            };

            var res = _fundingBandService.GetNextFundingPeriodWithinTimePeriod(fundingBands, currentEffectiveFrom, numberOfMonths);
            res.Should().BeNull();
        }

        [TestCaseSource(nameof(GetCurrentEffectiveFrom))]
        public void ShouldReturnNoDetailsOfSingleInstanceIfItIsCurrent(DateTime? currentEffectiveFrom)
        {
            var numberOfMonths = 3;
            var expectedFundingCap = 1500;
            var expectedFundingPeriod = new FundingPeriod { EffectiveFrom = currentEffectiveFrom, FundingCap = expectedFundingCap };

            var fundingBands = new List<FundingPeriod>
            {
                expectedFundingPeriod
            };

            var res = _fundingBandService.GetNextFundingPeriodWithinTimePeriod(fundingBands, currentEffectiveFrom, numberOfMonths);
            res.Should().BeNull();
        }

        [TestCaseSource(nameof(GetCurrentEffectiveFrom))]
        public void ShouldReturnTheCorrectInstanceAt3MonthsForListContainingCurrent(DateTime? currentEffectiveFrom)
        {
            var numberOfMonths = 3;
            var expectedFundingCap = 1500;
            var usedDateTime = DateTime.Today.AddMonths(numberOfMonths);
            var expectedFundingPeriod = new FundingPeriod { EffectiveFrom = usedDateTime, FundingCap = expectedFundingCap };
            var currentFundingPeriod = new FundingPeriod { EffectiveFrom = currentEffectiveFrom, FundingCap = 1200 };

            var fundingBands = new List<FundingPeriod>
            {
                currentFundingPeriod,
                expectedFundingPeriod
            };

            var res = _fundingBandService.GetNextFundingPeriodWithinTimePeriod(fundingBands, currentEffectiveFrom, numberOfMonths);
            res.Should().Be(expectedFundingPeriod);
        }


        [TestCaseSource(nameof(GetCurrentEffectiveFrom))]
        public void ShouldReturnTheCorrectInstanceAt3MonthsForMixedListOf5(DateTime? currentEffectiveFrom)
        {
            var numberOfMonths = 3;
            var earliestFundingCap = 1501;
            var earliestDateTime = DateTime.Today.AddDays(1);
            var secondDateTime = DateTime.Today.AddDays(7);
            var secondFundingCap = 1507;
            var thirdDateTime = DateTime.Today.AddDays(28);
            var thirdFundingCap = 1528;
            var fourthDateTime = DateTime.Today.AddDays(50);
            var fourthFundingCap = 1550;

            var firstFundingPeriod = new FundingPeriod { EffectiveFrom = earliestDateTime, FundingCap = earliestFundingCap };
            var secondFundingPeriod = new FundingPeriod { EffectiveFrom = secondDateTime, FundingCap = secondFundingCap };
            var thirdFundingPeriod = new FundingPeriod { EffectiveFrom = thirdDateTime, FundingCap = thirdFundingCap };
            var fourthFundingPeriod = new FundingPeriod { EffectiveFrom = fourthDateTime, FundingCap = fourthFundingCap };
            var currentFundingPeriod = new FundingPeriod { EffectiveFrom = currentEffectiveFrom, FundingCap = 1200 };

            var fundingBands = new List<FundingPeriod>
                    {
                        thirdFundingPeriod,
                        currentFundingPeriod,
                        secondFundingPeriod,
                        fourthFundingPeriod,
                        firstFundingPeriod
                    };

            var res = _fundingBandService.GetNextFundingPeriodWithinTimePeriod(fundingBands, currentEffectiveFrom, numberOfMonths);
            res.Should().Be(firstFundingPeriod);
        }

        private static IEnumerable<DateTime?> GetCurrentEffectiveFrom()
        {
            yield return null;
            yield return CurrentDatetime;
        }
    }
}
