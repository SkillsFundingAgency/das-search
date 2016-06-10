using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.Web.Services.MappingActions;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Views.Services
{
    [TestFixture]
    public class ForProviderSearchMappingHelper
    {
        [Test]
        public void ShouldCreateDeliveryModesInTheCorrectOrder()
        {
            var trainingOptions = new Dictionary<string, long?>
                                      {
                                          { "blockrelease", 35 },
                                          { "100percentemployer", 1 },
                                          { "dayrelease", 0 }
                                      };

            var selectedTrainingOptions = new[] { "100percentemployer", "dayrelease" };
            var result = ProviderSearchMappingHelper.CreateDeliveryModes(trainingOptions, selectedTrainingOptions);

            result.Count().Should().Be(3);

            var first = result.First();
            var second = result.Skip(1).Take(1).First();
            var third = result.Skip(2).Take(1).First();

            first.Value.Should().Be("dayrelease");
            second.Value.Should().Be("blockrelease");
            third.Value.Should().Be("100percentemployer");
        }

        public void ShouldMapNamesSelectedAndCount()
        {
            var trainingOptions = new Dictionary<string, long?>
                                      {
                                          { "blockrelease", 35 },
                                          { "100percentemployer", 1 },
                                          { "dayrelease", 0 }
                                      };

            var selectedTrainingOptions = new[] { "100percentemployer", "dayrelease" };
            var result = ProviderSearchMappingHelper.CreateDeliveryModes(trainingOptions, selectedTrainingOptions);

            result.Count().Should().Be(3);

            var first = result.FirstOrDefault(m => m.Value.Equals("dayrelease", StringComparison.InvariantCulture));
            var second = result.FirstOrDefault(m => m.Value.Equals("blockrelease", StringComparison.InvariantCulture));
            var third = result.FirstOrDefault(m => m.Value.Equals("100percentemployer", StringComparison.InvariantCulture));

            first.Checked.Should().BeTrue();
            first.Title.Should().Be("day release");
            first.Count.Should().Be(0);

            second.Checked.Should().BeFalse();
            second.Title.Should().Be("block release");
            second.Count.Should().Be(35);

            third.Checked.Should().BeTrue();
            third.Title.Should().Be("at your location");
            third.Count.Should().Be(1);
        }

        [Test]
        public void ShouldIgnoreMissingTrainingOptions()
        {
            var trainingOptions = new Dictionary<string, long?>
                                      {
                                          { "blockrelease", 35 },
                                          { "100percentemployer", 1 }
                                      };

            var selectedTrainingOptions = new[] { "100percentemployer", "dayrelease" };
            var result = ProviderSearchMappingHelper.CreateDeliveryModes(trainingOptions, selectedTrainingOptions);

            result.Count().Should().Be(2);

            var first = result.FirstOrDefault(m => m.Value.Equals("dayrelease", StringComparison.InvariantCulture));
            var second = result.FirstOrDefault(m => m.Value.Equals("blockrelease", StringComparison.InvariantCulture));
            var third = result.FirstOrDefault(m => m.Value.Equals("100percentemployer", StringComparison.InvariantCulture));

            first.Should().BeNull();

            second.Checked.Should().BeFalse();
            second.Title.Should().Be("block release");
            second.Count.Should().Be(35);

            third.Checked.Should().BeTrue();
            third.Title.Should().Be("at your location");
            third.Count.Should().Be(1);
        }

        [TestCase(100, 10, 10)]
        [TestCase(101, 10, 11)]
        [TestCase(99, 10, 10)]
        [TestCase(89, 10, 9)]
        [TestCase(89, 0, 0, Description = "results to take must me more than 0")]
        [TestCase(-10, 10, -1)]
        public void ShouldCalculateLastPage(long totalResults, int resultsToTake, int expected)
        {
            ProviderSearchMappingHelper.CalculateLastPage(totalResults, resultsToTake).Should().Be(expected);
        }
    }
}
