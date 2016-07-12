using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.UnitTests.Factories.MetaData
{
    [TestFixture]
    public class FrameworkAimMetaDataFactoryTest
    {
        private FrameworkAimMetaDataFactory _sut;

        [SetUp]
        public void Init()
        {
            _sut = new FrameworkAimMetaDataFactory();
        }

        [Test]
        public void ShouldReturnObjectOfCorrectType()
        {
            // Assign
            var data = new List<string> { "1", "2", "3", "Test123", "2015-03-01", "2017-06-01", "4" };

            // Act
            var metaData = _sut.Create(data);

            // Assert
            metaData.GetType().Should().Be(_sut.MetaDataType);
        }

        [Test]
        public void ShouldCreateFrameworkAimMetaData()
        {
            // Assign
            var data = new List<string> { "1", "2", "3", "Test123", "2015-03-01", "2017-06-01", "4" };

            // Act
            var metaData = _sut.Create(data) as FrameworkAimMetaData;

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
    }
}