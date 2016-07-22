namespace Sfa.Das.Sas.Indexer.UnitTests.Infrastructure.DapperDB
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Indexer.Core.Logging;
    using Indexer.Infrastructure.DapperBD;
    using Indexer.Infrastructure.Settings;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class AchievementRateProviderTest
    {
        [Test]
        [Category("ExternalDependency")]
        [Category("Integration")]
        [Explicit]
        public void ProviderTest()
        {
            var databaseProvider = new DatabaseProvider(new InfrastructureSettings(new MachineSettings()), Mock.Of<ILog>());
            var sut = new AchievementRatesProvider(databaseProvider);

            var result = sut.GetAllByProvider().ToArray();

            result.Length.Should().BeGreaterThan(0);
            var totalCount = result.Length;

            result.Any(m => Math.Abs(m.Ssa2Code) > 0.0).Should().BeTrue();
            result.Count(m => m.Age.Equals("All Age")).Should().Be(totalCount);
            result.Count(m => !m.SectorSubjectAreaTier2.Equals("All SSA T2")).Should().Be(totalCount);
            result.Count(m => !m.ApprenticeshipLevel.Equals("All")).Should().Be(totalCount);
        }

        [Test]
        [Category("ExternalDependency")]
        [Category("Integration")]
        [Explicit]
        public void NationalTest()
        {
            var databaseProvider = new DatabaseProvider(new InfrastructureSettings(new MachineSettings()), Mock.Of<ILog>());
            var sut = new AchievementRatesProvider(databaseProvider);

            var result = sut.GetAllNational().ToArray();

            result.Length.Should().BeGreaterThan(0);
            var totalCount = result.Length;

            result.Any(m => Math.Abs(m.Ssa2Code) > 0.0).Should().BeTrue();
            result.Count(m => m.InstitutionType.Equals("All institution type", StringComparison.InvariantCultureIgnoreCase)).Should().Be(totalCount);
            result.Count(m => m.Age.Equals("All Age")).Should().Be(totalCount);
            result.Count(m => !m.SectorSubjectAreaTier2.Equals("All SSA T2", StringComparison.InvariantCultureIgnoreCase)).Should().Be(totalCount);
            result.Count(m => !m.ApprenticeshipLevel.Equals("All", StringComparison.InvariantCultureIgnoreCase)).Should().Be(totalCount);
        }
    }
}