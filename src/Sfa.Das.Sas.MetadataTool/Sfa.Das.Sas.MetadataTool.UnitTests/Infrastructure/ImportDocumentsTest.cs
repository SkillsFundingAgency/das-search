namespace Sfa.Das.Sas.MetadataTool.UnitTests.Infrastructure
{
    using System;
    using System.IO;

    using FluentAssertions;

    using Moq;

    using NUnit.Framework;

    using ApplicationServices.Services.Interfaces;
    using Sas.Infrastructure.MongoDb;

    [TestFixture]
    public class ImportDocumentsTest
    {
        private IDocumentImporter _sut;

        private string _framework;

        private string _standard;

        [OneTimeSetUp]
        public void SetUp()
        {
            var projectPath = AppDomain.CurrentDomain.BaseDirectory;
            var pathFrameworks = Path.Combine(projectPath, "Infrastructure/TestData/Framework2.json");
            _framework = File.ReadAllText(pathFrameworks);

            var pathStandard = Path.Combine(projectPath, "Infrastructure/TestData/Standard2.json");
            _standard = File.ReadAllText(pathStandard);

            var mockDataClient = new Mock<IMongoDataClient>();
            var mockSettings = new Mock<IMongoSettings>();
            _sut = new MongoDocumentImporter(mockDataClient.Object, mockSettings.Object);
        }

        [Test]
        public void WhenImportingVstsStandardsButNotAList()
        {
            var response = _sut.Import(_standard, "standard");

            response.Message.Should().Be("Input not valid");
            response.InnerMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public void WhenImportingVstsStandardsButAskingForFramework()
        {
            var response = _sut.Import($"[{_standard}]", "framework");

            response.Message.Should().BeEmpty();
            response.InnerMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public void WhenImportingVstsStandards()
        {
            var response = _sut.Import($"[{_standard}]", "standard");

            response.Message.Should().Be("Imported 1 Standard");
            response.InnerMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public void WhenImportingVstsFrameworkButNotAList()
        {
            var response = _sut.Import(_framework, "framework");

            response.Message.Should().Be("Input not valid");
            response.InnerMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public void WhenImportingVstsStandardsButAskingForStandard()
        {
            var response = _sut.Import($"[{_framework}]", "standard");

            response.Message.Should().BeNullOrEmpty();
            response.InnerMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public void WhenImportingVstsFrameworks()
        {
            var response = _sut.Import($"[{_framework}]", "vstsframework");

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
