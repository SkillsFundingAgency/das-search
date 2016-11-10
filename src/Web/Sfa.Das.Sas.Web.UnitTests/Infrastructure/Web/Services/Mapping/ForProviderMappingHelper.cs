using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services.Mapping
{
    [TestFixture]
    public class ForProviderMappingHelper
    {
        [TestCase(null, "no data available")]
        [TestCase(3, "3%")]
        public void WhenGetPercentageText(double? input, string expected)
        {
            ProviderMappingHelper.GetPercentageText(input).Should().BeEquivalentTo(expected);
        }

        [TestCase(null, false, "no data available")]
        [TestCase(null, true, "Not currently collected for this training organisation")]
        [TestCase(3, true, "3%")]
        [TestCase(3, false, "3%")]
        public void WhenGetPercentageText(double? input, bool hei, string expected)
        {
            ProviderMappingHelper.GetPercentageText(input, hei).Should().BeEquivalentTo(expected);
        }

        [Test]
        public void WhenGetCommaList()
        {
            string[] list = {
                "hello",
                null,
                "world",
                string.Empty,
                "sample",
                " ",
                "end"
            };

            var expected = "hello, world, sample, end";

            ProviderMappingHelper.GetCommaList(list).Should().BeEquivalentTo(expected);
        }
    }
}
