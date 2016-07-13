using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Models;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.UnitTests.Factories.MetaData
{
    [TestFixture]
    public class StandardMetaDataFactoryTest
    {
        private StandardMetaDataFactory _sut;

        [SetUp]
        public void Init()
        {
            _sut = new StandardMetaDataFactory();
        }

        [Test]
        public void ShouldReturnObjectOfCorrectType()
        {
            // Assign
            var data = new List<string>
            {
                "5", string.Empty, "standard title", string.Empty, "2", string.Empty,
                string.Empty, string.Empty, "standard url", "1.1", "2.2"
            };

            // Act
            var metaData = _sut.Create(data);

            // Assert
            metaData.GetType().Should().Be(_sut.MetaDataType);
        }

        [Test]
        public void ShouldCreateFrameworkAimMetaData()
        {
            // Assign
            var data = new List<string>
            {
                "5", string.Empty, "standard title", string.Empty, "2", string.Empty,
                string.Empty, string.Empty, "standard url", "1.1", "2.2"
            };

            // Act
            var metaData = _sut.Create(data) as LarsStandard;

            // Assert
            metaData.Should().NotBeNull();
            metaData.Id.Should().Be(5);
            metaData.Title.Should().Be("standard title");
            metaData.NotionalEndLevel.Should().Be(2);
            metaData.StandardUrl.Should().Be("standard url");
            metaData.SectorSubjectAreaTier1.Should().Be(1.1);
            metaData.SectorSubjectAreaTier2.Should().Be(2.2);
        }
    }
}