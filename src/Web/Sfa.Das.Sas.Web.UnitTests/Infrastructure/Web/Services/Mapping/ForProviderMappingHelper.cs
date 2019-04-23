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

        [TestCaseSource(nameof(_deliveryOptionCases))]
        public void WhenGettingDeliveryOptionText(string expected, List<string> input)
        {
            ProviderMappingHelper.GetDeliveryOptionText(input).Should().BeEquivalentTo(expected);
        }

        private static object[] _deliveryOptionCases =
        {
            new object[]
            {
                GetTextForDeliveryModesDayAndBlockAnd100Percent(true, true, true),
                new List<string> { "100PercentEmployer", "BlockRelease", "DayRelease" }
            },
            new object[]
            {
                GetTextForDeliveryModesDayAndBlockAnd100Percent(false, true, true),
                new List<string> { "100PercentEmployer", "BlockRelease" }
            },
            new object[]
            {
                GetTextForDeliveryModesDayAndBlockAnd100Percent(true, false, true),
                new List<string> { "100PercentEmployer", "DayRelease" }
            },
            new object[]
            {
                GetTextForDeliveryModesDayAndBlockAnd100Percent(true, true, false),
                new List<string>{ "BlockRelease", "DayRelease" }
            },
            new object[]
            {
                GetTextForDeliveryModesDayAndBlockAnd100Percent(false, true, true),
                 new List<string> { "BlockRelease", "100PercentEmployer" }
            },
            new object[]
            {
                GetTextForDeliveryModesDayAndBlockAnd100Percent(false, false, false),
                new List<string>()
                },
            new object[]
                {
                    GetTextForDeliveryModesDayAndBlockAnd100Percent(true, false, false),
                new List<string> { "DayRelease" }
                },
            new object[]
                {
                    GetTextForDeliveryModesDayAndBlockAnd100Percent(false, true, false),
                new List<string> { "BlockRelease" }
                },
            new object[]
                {
                GetTextForDeliveryModesDayAndBlockAnd100Percent(false, false, true),
                new List<string> { "100PercentEmployer" }
                },
            new object[]
                {
                GetTextForDeliveryModesDayAndBlockAnd100Percent(true, true, false),
                new List<string> { "DayRelease", "BlockRelease" }
                },
            new object[]
                {
                GetTextForDeliveryModesDayAndBlockAnd100Percent(true, false, true),
                new List<string> { "DayRelease", "100PercentEmployer" }
                }
            };

        private static string GetTextForDeliveryModesDayAndBlockAnd100Percent(bool isDayRelease, bool isBlockRelease, bool is100PercentEmployer)
        {
            var dayReleaseDesc = isDayRelease ? "green-tick" : "red-cross";
            var blockReleaseDesc = isBlockRelease ? "green-tick" : "red-cross";
            var hundredPercentEmployer = is100PercentEmployer ? "green-tick" : "red-cross";

            return $@"<div class='icon-alerts'><p class='icon-right inline-alert'>" +
                   $"<span class='icon-content'>day release</span><span class='{dayReleaseDesc}'></span>" +
                   $"<span class='icon-content'>block release</span><span class='{blockReleaseDesc}'></span>" +
                   $"<span class='icon-content'>at your location</span><span class='{hundredPercentEmployer}'></span>" +
                   "</p></div>";
        }
    }
}
