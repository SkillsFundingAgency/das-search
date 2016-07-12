using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.UnitTests.Factories.MetaData
{
    [TestFixture]
    public class FrameworkComponentTypeMetaDataFactoryTest
    {
        private FrameworkComponentTypeMetaDataFactory _sut;

        [SetUp]
        public void Init()
        {
            _sut = new FrameworkComponentTypeMetaDataFactory();
        }

        [Test]
        public void ShouldReturnObjectOfCorrectType()
        {
            // Assign
            var data = new List<string> { "1", "Description 1", "Description 2", "2015-03-01", "2017-06-01" };

            // Act
            var metaData = _sut.Create(data);

            // Assert
            metaData.GetType().Should().Be(_sut.MetaDataType);
        }

        [Test]
        public void ShouldCreateFrameworkContentTypeMetaData()
        {
            // Assign
            var data = new List<string> { "1", "Description 1", "Description 2", "2015-03-01", "2017-06-01" };

            // Act
            var metaData = _sut.Create(data) as FrameworkComponentTypeMetaData;

            // Assert
            metaData.Should().NotBeNull();
            metaData.FrameworkComponentType.Should().Be(1);
            metaData.FrameworkComponentTypeDesc.Should().Be("Description 1");
            metaData.EffectiveFrom.Should().Be(new DateTime(2015, 3, 1));
            metaData.EffectiveTo.Should().Be(new DateTime(2017, 6, 1));
        }
    }
}
