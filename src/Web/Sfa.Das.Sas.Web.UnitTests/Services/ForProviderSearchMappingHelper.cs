namespace Sfa.Das.Sas.Web.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using NUnit.Framework;

    using Sfa.Das.Sas.Web.Services.MappingActions;

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

            var first = result.FirstOrDefault(m => m.Value.Equals("dayrelease"));
            var second = result.FirstOrDefault(m => m.Value.Equals("blockrelease"));
            var third = result.FirstOrDefault(m => m.Value.Equals("100percentemployer"));

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

            var first = result.FirstOrDefault(m => m.Value.Equals("dayrelease"));
            var second = result.FirstOrDefault(m => m.Value.Equals("blockrelease"));
            var third = result.FirstOrDefault(m => m.Value.Equals("100percentemployer"));

            first.Should().BeNull();

            second.Checked.Should().BeFalse();
            second.Title.Should().Be("block release");
            second.Count.Should().Be(35);

            third.Checked.Should().BeTrue();
            third.Title.Should().Be("at your location");
            third.Count.Should().Be(1);
        }
    }
}
