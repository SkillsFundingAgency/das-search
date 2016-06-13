using System.Collections.Generic;
using Moq;
using NUnit.Framework;

using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.Core.Logging;
using Sfa.Das.Sas.Indexer.Core.Services;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Models;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.UnitTests
{
    using Sfa.Das.Sas.Indexer.Core.Models;
    using Sfa.Das.Sas.Tools.MetaDataCreationTool.Models.Git;

    [TestFixture]
    public class MetaDataManagerTests
    {
        [Test]
        public void GenerateStandardMetadataFilesTestShouldPushOnlyMissingStandardsToGit()
        {
            // ILarsDataService larsDataService, IVstsService vstsService, IAppServiceSettings settings
            var mockLarsDataService = new Mock<ILarsDataService>();
            var mockVstsService = new Mock<IVstsService>();
            var mockSettings = new Mock<IAppServiceSettings>();
            var mockLogger = new Mock<ILog>(MockBehavior.Loose);
            List<FileContents> standardsToAdd = null;

            var currentStandards = new List<Standard>
                                       {
                                           new Standard { Id = 1, Title = "One" },
                                           new Standard { Id = 2, Title = "Two" },
                                           new Standard { Id = 3, Title = "Three" }
                                       };
            var existingMetaDataIds = new List<string> { "1", "2" };

            mockLarsDataService.Setup(x => x.GetListOfCurrentStandards()).Returns(currentStandards);
            mockVstsService.Setup(x => x.GetExistingStandardIds()).Returns(existingMetaDataIds);
            mockVstsService.Setup(x => x.PushCommit(It.IsAny<List<FileContents>>())).Callback<List<FileContents>>(x => { standardsToAdd = x; });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, mockVstsService.Object, mockSettings.Object, null, mockLogger.Object);

            metaDataManager.GenerateStandardMetadataFiles();

            Assert.That(standardsToAdd.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetAllAsJsonShouldReturnDictionary()
        {
            // ILarsDataService larsDataService, IVstsService vstsService, IAppServiceSettings settings
            var mockLarsDataService = new Mock<ILarsDataService>();
            var mockVstsService = new Mock<IVstsService>();
            var mockSettings = new Mock<IAppServiceSettings>();
            var mockJsonConverter = new Mock<IJsonMetaDataConvert>();
            var mockLogger = new Mock<ILog>(MockBehavior.Loose);

            mockJsonConverter.Setup(m => m.DeserializeObject<StandardMetaData>(It.IsAny<Dictionary<string, string>>())).Returns(new List<StandardMetaData>());

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, mockVstsService.Object, mockSettings.Object, mockJsonConverter.Object, mockLogger.Object);

            var standardJson = metaDataManager.GetStandardsMetaData();

            Assert.That(standardJson, Is.TypeOf<List<StandardMetaData>>());
        }
    }
}