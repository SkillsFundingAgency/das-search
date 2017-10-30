using System.Linq;
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

        [TestCase(
            @"<div class='icon-alerts'><p class='icon-right'>" +
            "<span class='icon-content'>day release</span><span class='red-cross'></span>" +
            "<span class='icon-content'>block release</span><span class='red-cross'></span>" +
            "<span class='icon-content'>at your location</span><span class='red-cross'></span>" +
            "</p></div>")]
        [TestCase(
            @"<div class='icon-alerts'><p class='icon-right'>" +
            "<span class='icon-content'>day release</span><span class='green-tick'></span>" +
            "<span class='icon-content'>block release</span><span class='red-cross'></span>" +
            "<span class='icon-content'>at your location</span><span class='red-cross'></span>" +
            "</p></div>", "DayRelease")]
        [TestCase(
            @"<div class='icon-alerts'><p class='icon-right'>" +
            "<span class='icon-content'>day release</span><span class='red-cross'></span>" +
            "<span class='icon-content'>block release</span><span class='green-tick'></span>" +
            "<span class='icon-content'>at your location</span><span class='red-cross'></span>" +
            "</p></div>", "BlockRelease")]
        [TestCase(
            @"<div class='icon-alerts'><p class='icon-right'>" +
            "<span class='icon-content'>day release</span><span class='red-cross'></span>" +
            "<span class='icon-content'>block release</span><span class='red-cross'></span>" +
            "<span class='icon-content'>at your location</span><span class='green-tick'></span>" +
            "</p></div>", "100PercentEmployer")]
        [TestCase(
            @"<div class='icon-alerts'><p class='icon-right'>" +
            "<span class='icon-content'>day release</span><span class='green-tick'></span>" +
            "<span class='icon-content'>block release</span><span class='green-tick'></span>" +
            "<span class='icon-content'>at your location</span><span class='red-cross'></span>" +
            "</p></div>", "DayRelease", "BlockRelease")]
        [TestCase(
            @"<div class='icon-alerts'><p class='icon-right'>" +
            "<span class='icon-content'>day release</span><span class='green-tick'></span>" +
            "<span class='icon-content'>block release</span><span class='red-cross'></span>" +
            "<span class='icon-content'>at your location</span><span class='green-tick'></span>" +
            "</p></div>", "DayRelease", "100PercentEmployer")]
        [TestCase(
            @"<div class='icon-alerts'><p class='icon-right'>" +
            "<span class='icon-content'>day release</span><span class='red-cross'></span>" +
            "<span class='icon-content'>block release</span><span class='green-tick'></span>" +
            "<span class='icon-content'>at your location</span><span class='green-tick'></span>" +
            "</p></div>", "BlockRelease", "100PercentEmployer")]
        [TestCase(
            @"<div class='icon-alerts'><p class='icon-right'>" +
            "<span class='icon-content'>day release</span><span class='green-tick'></span>" +
            "<span class='icon-content'>block release</span><span class='green-tick'></span>" +
            "<span class='icon-content'>at your location</span><span class='red-cross'></span>" +
            "</p></div>", "BlockRelease", "DayRelease")]
        [TestCase(
            @"<div class='icon-alerts'><p class='icon-right'>" +
            "<span class='icon-content'>day release</span><span class='green-tick'></span>" +
            "<span class='icon-content'>block release</span><span class='red-cross'></span>" +
            "<span class='icon-content'>at your location</span><span class='green-tick'></span>" +
            "</p></div>", "100PercentEmployer", "DayRelease")]
        [TestCase(
            @"<div class='icon-alerts'><p class='icon-right'>" +
            "<span class='icon-content'>day release</span><span class='red-cross'></span>" +
            "<span class='icon-content'>block release</span><span class='green-tick'></span>" +
            "<span class='icon-content'>at your location</span><span class='green-tick'></span>" +
            "</p></div>", "100PercentEmployer", "BlockRelease")]
        [TestCase(
            @"<div class='icon-alerts'><p class='icon-right'>" +
            "<span class='icon-content'>day release</span><span class='green-tick'></span>" +
            "<span class='icon-content'>block release</span><span class='green-tick'></span>" +
            "<span class='icon-content'>at your location</span><span class='green-tick'></span>" +
            "</p></div>", "100PercentEmployer", "BlockRelease", "DayRelease")]
        public void WhenGetDeliveryOptionText(string expected, params string[] input)
        {
            var inputList = input?.ToList();
            ProviderMappingHelper.GetDeliveryOptionText(inputList).Should().BeEquivalentTo(expected);
        }
    }
}
