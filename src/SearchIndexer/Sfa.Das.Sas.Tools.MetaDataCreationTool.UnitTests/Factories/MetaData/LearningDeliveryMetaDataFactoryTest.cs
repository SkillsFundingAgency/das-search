using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.UnitTests.Factories.MetaData
{
    [TestFixture]
    public class LearningDeliveryMetaDataFactoryTest
    {
        private LearningDeliveryMetaDataFactory _sut;

        [SetUp]
        public void Init()
        {
            _sut = new LearningDeliveryMetaDataFactory();
        }

        [Test]
        public void ShouldReturnObjectOfCorrectType()
        {
            // Assign
            var data = new List<string> { "LearnRef1", "2015-03-01", "2017-06-01", "Test Title", "3" };

            // Act
            var metaData = _sut.Create(data);

            // Assert
            metaData.GetType().Should().Be(_sut.MetaDataType);
        }

        [Test]
        public void ShouldCreateLearningDeliveryMetaData()
        {
            // Assign
            var data = new List<string> { "LearnRef1", "2015-03-01", "2017-06-01", "Test Title", "3" };

            // Act
            var metaData = _sut.Create(data) as LearningDeliveryMetaData;

            // Assert
            metaData.Should().NotBeNull();
            metaData.LearnAimRef.Should().Be("LearnRef1");
            metaData.LearnAimRefType.Should().Be(3);
            metaData.LearnAimRefTitle.Should().Be("Test Title");
            metaData.EffectiveFrom.Should().Be(new DateTime(2015, 3, 1));
            metaData.EffectiveTo.Should().Be(new DateTime(2017, 6, 1));
        }
    }
}
