using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.UnitTests.Factories.MetaData
{
    [TestFixture]
    public class FrameworkMetaDataFactoryTest
    {
        private FrameworkMetaDataFactory _sut;

        [SetUp]
        public void Init()
        {
            _sut = new FrameworkMetaDataFactory();
        }

        [Test]
        public void ShouldReturnObjectOfCorrectType()
        {
            // Assign
            var data = new List<string> { "1", "2", "3", "PathName123", "2015-03-01", "2017-06-01", "1.2", "3.2", string.Empty, "NAS Title", string.Empty, string.Empty };

            // Act
            var metaData = _sut.Create(data);

            // Assert
            metaData.GetType().Should().Be(_sut.MetaDataType);
        }

        [Test]
        public void ShouldCreateFrameworkMetaData()
        {
            // Assign
            var data = new List<string> { "1", "2", "3", "PathName123", "2015-03-01", "2017-06-01", "1.2", "3.2", string.Empty, "NAS Title", string.Empty, string.Empty };

            // Act
            var metaData = _sut.Create(data) as FrameworkMetaData;

            // Assert
            metaData.Should().NotBeNull();
            metaData.FworkCode.Should().Be(1);
            metaData.ProgType.Should().Be(2);
            metaData.PwayCode.Should().Be(3);
            metaData.PathwayName.Should().Be("PathName123");
            metaData.EffectiveFrom.Should().Be(new DateTime(2015, 3, 1));
            metaData.EffectiveTo.Should().Be(new DateTime(2017, 6, 1));
            metaData.SectorSubjectAreaTier1.Should().Be(1.2);
            metaData.SectorSubjectAreaTier2.Should().Be(3.2);
            metaData.NasTitle.Should().Be("NAS Title");
        }
    }
}