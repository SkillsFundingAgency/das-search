using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.UnitTests.Factories
{
    [TestFixture]
    public class LarsMetaDataFactoryTest
    {
        private LarsMetaDataFactory _sut;
        private Mock<IMetaDataFactory> _mockFactory;
        private FrameworkMetaData _metaData;

        [SetUp]
        public void Init()
        {
            _metaData = new FrameworkMetaData();
            _mockFactory = new Mock<IMetaDataFactory>();
            _mockFactory.Setup(x => x.Create(It.IsAny<IReadOnlyList<string>>())).Returns(_metaData);
            _mockFactory.Setup(x => x.MetaDataType).Returns(typeof(FrameworkMetaData));

            _sut = new LarsMetaDataFactory(new List<IMetaDataFactory> {_mockFactory.Object});
        }

        [Test]
        public void ShouldReturnMetaData()
        {
            // Act
            var metaData = _sut.Create<FrameworkMetaData>(new List<string> { "some data" });

            // Assert
            metaData.Should().Be(_metaData);
        }

        [Test]
        public void ShouldReturnNullIfEmptyParameterIsPassed()
        {
            // Act
            var metaData = _sut.Create<FrameworkMetaData>(new List<string>());

            // Assert
            metaData.Should().BeNull();
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
