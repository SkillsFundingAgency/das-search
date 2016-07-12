using NUnit.Framework;

using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.Core.Logging;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.DependencyResolution;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces;
using StructureMap;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.UnitTests
{
    using System.Linq;

    using FluentAssertions;

    using Moq;

    using Sfa.Das.Sas.Indexer.ApplicationServices.Http;
    using Sfa.Das.Sas.Indexer.ApplicationServices.Infrastructure;
    using Sfa.Das.Sas.Indexer.Core.Services;
    using Sfa.Das.Sas.Indexer.Infrastructure.Services;
    using Sfa.Das.Sas.Indexer.Infrastructure.Settings;

    [TestFixture]
    public class MetaDataTest
    {
        [Test]
        [Category("ExternalDependency")]
        [Category("Integration")]
        [Explicit]
        public void TestGenerationOfFiles()
        {
            var httpGetMock = new Mock<IHttpGet>();
            httpGetMock.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns("hello");
            var container = new Container(c =>
            {
                c.AddRegistry<MetaDataCreationRegistry>();
                c.For<IHttpGetFile>().Use<HttpService>();
                c.For<IHttpGet>().Use<HttpService>();
                c.For<IProvideSettings>().Use(() => new AppConfigSettingsProvider(new MachineSettings()));
                c.For<ILog>().Use(() => Mock.Of<ILog>());
                c.For<IUnzipStream>().Use<ZipFileExtractor>();
            });

            var larsDataService = container.GetInstance<ILarsDataService>();
            var vstsDataService = container.GetInstance<IVstsService>();
            var settings = container.GetInstance<IAppServiceSettings>();
            var logger = container.GetInstance<ILog>();
            var angleSharpService = container.GetInstance<IAngleSharpService>();

            Assert.IsNotNullOrEmpty(settings.GitUsername);
            Assert.IsNotNullOrEmpty(settings.GitPassword);

            MetaDataManager metaData = new MetaDataManager(larsDataService, vstsDataService, settings, angleSharpService, logger);

            var standardsFromLars = larsDataService.GetListOfCurrentStandards();

            metaData.GenerateStandardMetadataFiles();
            var result = metaData.GetStandardsMetaData();

            result.Count.Should().Be(standardsFromLars.Count());
        }
    }
}
