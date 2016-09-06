namespace Sfa.Das.Sas.MetadataTool.UnitTests.Infrastructure
{
    using System;
    using System.IO;

    using FluentAssertions;

    using Moq;

    using NUnit.Framework;

    using Sfa.Das.Sas.Infrastructure.MongoDb;

    [TestFixture]
    public class ImportVstsDocumentsTest
    {
        private MongoDocumentImporter _sut;

        private string _projectPath;

        [OneTimeSetUp]
        public void SetUp()
        {
            var mockDataClient = new Mock<IMongoDataClient>();
            var mockSettings = new Mock<IMongoSettings>();
            _sut = new MongoDocumentImporter(mockDataClient.Object, mockSettings.Object);
            _projectPath = AppDomain.CurrentDomain.BaseDirectory;
        }

        [Test]
        public void WhenImportingVstsStandardsButNotAList()
        {
            var path = Path.Combine(_projectPath, "Infrastructure/TestData/Standard1.json");
            var standard1 = File.ReadAllText(path);

            var response = _sut.Import(standard1, "vstsstandard");

            response.Message.Should().Be("Input not valid");
            response.InnerMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public void WhenImportingVstsStandardsButAskingForFramework()
        {
            var path = Path.Combine(_projectPath, "Infrastructure/TestData/Standard1.json");
            var standard1 = File.ReadAllText(path);

            var response = _sut.Import($"[{standard1}]", "vstsframework");

            response.Message.Should().Be("Not possible to parse json to Framework");
            response.InnerMessage.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void WhenImportingVstsStandards()
        {
            var path = Path.Combine(_projectPath, "Infrastructure/TestData/Standard1.json");
            var standard1 = File.ReadAllText(path);

            var response = _sut.Import($"[{standard1}]", "vstsstandard");

            response.Message.Should().Be("Imported 1 Standard");
            response.InnerMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public void WhenImportingVstsFrameworkButNotAList()
        {
            var path = Path.Combine(_projectPath, "Infrastructure/TestData/Framework1.json");
            var framework1 = File.ReadAllText(path);

            var response = _sut.Import(framework1, "vstsframework");

            response.Message.Should().Be("Input not valid");
            response.InnerMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public void WhenImportingVstsStandardsButAskingForStandard()
        {
            var path = Path.Combine(_projectPath, "Infrastructure/TestData/Framework1.json");
            var framework1 = File.ReadAllText(path);

            var response = _sut.Import($"[{framework1}]", "vstsstandard");

            response.Message.Should().BeNullOrEmpty();
            response.InnerMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public void WhenImportingVstsFrameworks()
        {
            var path = Path.Combine(_projectPath, "Infrastructure/TestData/Framework1.json");
            var framework1 = File.ReadAllText(path);

            var response = _sut.Import($"[{framework1}]", "vstsframework");

            response.Message.Should().Be("Imported 1 Framework");
            response.InnerMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public void WhenImportingWithInvalidType()
        {
            var response = _sut.Import("[{}]", "invalifType");

            response.Message.Should().Be("Input not valid");
            response.InnerMessage.Should().BeNullOrEmpty();
        }
    }
}
