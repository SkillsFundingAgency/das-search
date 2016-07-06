using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.UnitTests.Factories
{
    [TestFixture]
    public class LarsMetaDataFactoryTest
    {
        private LarsMetaDataFactory _sut;

        [SetUp]
        public void Init()
        {
            _sut = new LarsMetaDataFactory();
        }

        [Test]
        public void ShouldCreateFrameworkMetaData()
        {
            // Assign
            var data = new List<string> { "1", "2", "3", "PathName123", "2015-03-01", "2017-06-01", "1.2", "3.2", string.Empty, "NAS Title", string.Empty, string.Empty };

            // Act
            var metaData = _sut.Create<FrameworkMetaData>(data);

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
            metaData.NASTitle.Should().Be("NAS Title");
        }

        [Test]
        public void ShouldCreateFrameworkAimMetaData()
        {
            // Assign
            var data = new List<string> { "1", "2", "3", "Test123", "2015-03-01", "2017-06-01", "4" };

            // Act
            var metaData = _sut.Create<FrameworkAimMetaData>(data);

            // Assert
            metaData.Should().NotBeNull();
            metaData.FworkCode.Should().Be(1);
            metaData.ProgType.Should().Be(2);
            metaData.PwayCode.Should().Be(3);
            metaData.LearnAimRef.Should().Be("Test123");
            metaData.EffectiveFrom.Should().Be(new DateTime(2015, 3, 1));
            metaData.EffectiveTo.Should().Be(new DateTime(2017, 6, 1));
            metaData.FrameworkComponentType.Should().Be(4);
        }

        [Test]
        public void ShouldCreateFrameworkContentTypeMetaData()
        {
            // Assign
            var data = new List<string> { "1", "Description 1", "Description 2", "2015-03-01", "2017-06-01" };

            // Act
            var metaData = _sut.Create<FrameworkComponentTypeMetaData>(data);

            // Assert
            metaData.Should().NotBeNull();
            metaData.FrameworkComponentType.Should().Be(1);
            metaData.FrameworkComponentTypeDesc.Should().Be("Description 1");
            metaData.EffectiveFrom.Should().Be(new DateTime(2015, 3, 1));
            metaData.EffectiveTo.Should().Be(new DateTime(2017, 6, 1));
        }

        [Test]
        public void ShouldCreateLearningDeliveryMetaData()
        {
            // Assign
            var data = new List<string> { "LearnRef1", "2015-03-01", "2017-06-01", "Test Title", "3" };

            // Act
            var metaData = _sut.Create<LearningDeliveryMetaData>(data);

            // Assert
            metaData.Should().NotBeNull();
            metaData.LearnAimRef.Should().Be("LearnRef1");
            metaData.LearnAimRefType.Should().Be(3);
            metaData.LearnAimRefTitle.Should().Be("Test Title");
            metaData.EffectiveFrom.Should().Be(new DateTime(2015, 3, 1));
            metaData.EffectiveTo.Should().Be(new DateTime(2017, 6, 1));
        }

        [Test]
        public void ShouldReturnNullIfNullParameterIsPassed()
        {
            // Act
            var metaData = _sut.Create<FrameworkMetaData>(null);

            // Assert
            metaData.Should().BeNull();
        }
    }
}
