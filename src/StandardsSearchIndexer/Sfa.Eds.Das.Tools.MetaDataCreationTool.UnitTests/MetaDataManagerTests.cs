namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.UnitTests
{
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.ApplicationServices.MetaData;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core.Services;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

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

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, mockVstsService.Object, mockSettings.Object, mockLogger.Object);

            metaDataManager.GenerateStandardMetadataFiles();

            Assert.That(standardsToAdd.Count, Is.EqualTo(1));
        }
    }
}