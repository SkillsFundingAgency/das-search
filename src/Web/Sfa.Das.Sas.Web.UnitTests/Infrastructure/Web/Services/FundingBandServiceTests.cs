using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.Web.Services;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services
{
    [TestFixture]
    public class FundingBandServiceTests
    {
        private FundingBandService _fundingBandService;

        [SetUp]
        public void SetUp()
        {

            _fundingBandService = new FundingBandService();
        }
        [Test]
        public void ShouldReturnNullIfNoMatches()
        {
            var res = _fundingBandService.GetNextFundingPeriodWithinTimePeriod(null, 0);
            res.Should().BeNull();
        }

    }
}
