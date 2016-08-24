using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.ApplicationServices.Http;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.Core.Models;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Tools.MetaDataCreationTool;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Models.Git;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces;

namespace Sfa.Das.Sas.Indexer.UnitTests.ApplicationServices.Helpers
{
    [TestFixture]
    public class MetaDataHelperTests
    {
        private Mock<IVstsService> _mockVstsService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _mockVstsService = new Mock<IVstsService>();
            _mockVstsService.Setup(m => m.GetFrameworks()).Returns(GetVstsMetaData());
        }

        [Test]
        public void GetAllFrameworks()
        {
            var mockSettings = new Mock<IAppServiceSettings>();
            var mockLarsDataService = new Mock<ILarsDataService>();
            var mockMetadataApiService = new Mock<IMetadataApiService>();

            mockMetadataApiService.Setup(x => x.GetFrameworks()).Returns(new List<VstsFrameworkMetaData>());
            mockSettings.Setup(x => x.MetadataApiUri).Returns("www.abba.co.uk");
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks())
                .Returns(
                    new List<FrameworkMetaData> { new FrameworkMetaData { EffectiveFrom = DateTime.Parse("2015-01-01"), EffectiveTo = null, FworkCode = 500, PwayCode = 1, ProgType = 2 } });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, _mockVstsService.Object, mockSettings.Object, null, mockMetadataApiService.Object, null);
            var frameworks = metaDataManager.GetAllFrameworks();

            Assert.AreEqual(1, frameworks.Count);
        }

        [Test]
        public void GetFrameworkWithAllValidData()
        {
            var mockSettings = new Mock<IAppServiceSettings>();
            var mockLarsDataService = new Mock<ILarsDataService>();
            var mockMetadataApiService = new Mock<IMetadataApiService>();

            mockSettings.Setup(x => x.MetadataApiUri).Returns("www.abba.co.uk");
            mockMetadataApiService.Setup(x => x.GetFrameworks()).Returns(new List<VstsFrameworkMetaData>());

            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks())
                .Returns(
                    new List<FrameworkMetaData> { new FrameworkMetaData { EffectiveFrom = DateTime.Parse("2015-01-01"), EffectiveTo = null, FworkCode = 500, PwayCode = 1, ProgType = 2 } });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, _mockVstsService.Object, mockSettings.Object, null, mockMetadataApiService.Object, null);
            var frameworks = metaDataManager.GetAllFrameworks();

            Assert.AreEqual(1, frameworks.Count, "Should find one framework");
        }

        [Test]
        public void ShouldMapJobRoles()
        {
            var mockSettings = new Mock<IAppServiceSettings>();
            var mockHttp = new Mock<IHttpGet>();
            var mockLarsDataService = new Mock<ILarsDataService>();
            var mockMetadataApiService = new Mock<IMetadataApiService>();

            mockSettings.Setup(x => x.MetadataApiUri).Returns("www.abba.co.uk");
            var frameworkFromRepo = new List<VstsFrameworkMetaData>
            {
                new VstsFrameworkMetaData
                {
                    FrameworkCode = 500,
                    PathwayCode = 1,
                    ProgType = 21,
                    JobRoleItems = new List<JobRoleItem>
                    {
                        new JobRoleItem
                        {
                            Title = "Job role 2",
                            Description = "Description 2"
                        },
                        new JobRoleItem()
                    }
                }
            };

            mockMetadataApiService.Setup(x => x.GetFrameworks()).Returns(frameworkFromRepo);
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks())
                .Returns(
                    new List<FrameworkMetaData>
                    {
                        new FrameworkMetaData { EffectiveFrom = DateTime.Parse("2015-01-01"), EffectiveTo = null, FworkCode = 500, PwayCode = 1, ProgType = 21 }
                    });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, _mockVstsService.Object, mockSettings.Object, null, mockMetadataApiService.Object, null);
            var framework = metaDataManager.GetAllFrameworks().FirstOrDefault();

            framework.Should().NotBeNull();
            framework.JobRoleItems.Count().Should().Be(2);
            framework.JobRoleItems.FirstOrDefault().Title.Should().Be("Job role 2");
            framework.JobRoleItems.FirstOrDefault().Description.Should().Be("Description 2");
        }

        [Test]
        public void ShouldMapTypicalLength()
        {
            var mockSettings = new Mock<IAppServiceSettings>();
            var mockMetadataApiService = new Mock<IMetadataApiService>();

            var mockLarsDataService = new Mock<ILarsDataService>();

            mockSettings.Setup(x => x.MetadataApiUri).Returns("www.abba.co.uk");
            var frameworkFromRepo = GetVstsMetaData();

            mockMetadataApiService.Setup(x => x.GetFrameworks()).Returns(frameworkFromRepo.ToList());
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks())
                .Returns(
                    new List<FrameworkMetaData>
                    {
                        new FrameworkMetaData { EffectiveFrom = DateTime.Parse("2015-01-01"), EffectiveTo = null, FworkCode = 500, PwayCode = 1, ProgType = 21 }
                    });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, _mockVstsService.Object, mockSettings.Object, null, mockMetadataApiService.Object, null);
            var framework = metaDataManager.GetAllFrameworks().FirstOrDefault();

            framework.Should().NotBeNull();
            framework.TypicalLength.From.Should().Be(12);
            framework.TypicalLength.To.Should().Be(24);
        }

        [Test]
        public void ShouldMapKeywords()
        {
            var mockSettings = new Mock<IAppServiceSettings>();
            var mockHttp = new Mock<IHttpGet>();
            var mockMetadataApiService = new Mock<IMetadataApiService>();
            var mockLarsDataService = new Mock<ILarsDataService>();

            mockSettings.Setup(x => x.MetadataApiUri).Returns("www.abba.co.uk");
            var frameworkFromRepo = GetVstsMetaData();

            mockMetadataApiService.Setup(x => x.GetFrameworks()).Returns(frameworkFromRepo.ToList());
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks())
                .Returns(
                    new List<FrameworkMetaData>
                    {
                        new FrameworkMetaData { EffectiveFrom = DateTime.Parse("2015-01-01"), EffectiveTo = null, FworkCode = 500, PwayCode = 1, ProgType = 21 }
                    });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, _mockVstsService.Object, mockSettings.Object, null, mockMetadataApiService.Object, null);
            var framework = metaDataManager.GetAllFrameworks().FirstOrDefault();

            framework.Should().NotBeNull();
            framework.Keywords.Should().Contain(new string[] { "keyword1", "keyword2" });
        }

        private IEnumerable<VstsFrameworkMetaData> GetVstsMetaData()
        {
            return new List<VstsFrameworkMetaData>
            {
                new VstsFrameworkMetaData
                {
                    FrameworkCode = 500,
                    PathwayCode = 1,
                    ProgType = 20,
                    JobRoleItems = new List<JobRoleItem> { new JobRoleItem { Title = "Job role 1", Description = "Description 1" } },
                    TypicalLength = new TypicalLength { From = 18, To = 18, Unit = "m" },
                    Keywords = new string[] { "keyword1", "keyword2" }
                },
                new VstsFrameworkMetaData
                {
                    FrameworkCode = 500,
                    PathwayCode = 1,
                    ProgType = 21,
                    JobRoleItems =
                        new List<JobRoleItem>
                        {
                            new JobRoleItem { Title = "Job role 2", Description = "Description 2" },
                            new JobRoleItem { Title = "Job role 3", Description = "Description 3" }
                        },
                    TypicalLength = new TypicalLength { From = 12, To = 24, Unit = "m" },
                    Keywords = new string[] { "keyword1", "keyword2" }
                }
            };
        }
    }
}