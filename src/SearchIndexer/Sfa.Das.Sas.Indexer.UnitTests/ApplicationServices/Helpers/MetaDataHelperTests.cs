using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.ApplicationServices.MetaData;
using Sfa.Das.Sas.Indexer.ApplicationServices.Standard;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Indexer.Core.Services;
using Sfa.Das.Sas.Tools.MetaDataCreationTool;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces;

namespace Sfa.Das.Sas.Indexer.UnitTests.ApplicationServices.Helpers
{
    using System.Linq;

    using FluentAssertions;

    using Sfa.Das.Sas.Indexer.Core.Models;
    using Sfa.Das.Sas.Tools.MetaDataCreationTool.Models.Git;

    [TestFixture]
    public class MetaDataHelperTests
    {
        private Mock<IVstsService> _mockVstsService;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _mockVstsService = new Mock<IVstsService>();
            _mockVstsService.Setup(m => m.GetFrameworks()).Returns(GetVstsMetaData());
        }

        [Test]
        public void GetAllFrameworks()
        {
            var mockLarsDataService = new Mock<ILarsDataService>();
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks())
                .Returns(
                    new List<FrameworkMetaData> { new FrameworkMetaData { EffectiveFrom = DateTime.Parse("2015-01-01"), EffectiveTo = DateTime.MinValue, FworkCode = 500, PwayCode = 1, ProgType = 2 } });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, _mockVstsService.Object, null, null, null);
            var frameworks = metaDataManager.GetAllFrameworks();

            Assert.AreEqual(1, frameworks.Count);
        }

        [Test]
        public void EffectiveToDateValid()
        {
            var mockLarsDataService = new Mock<ILarsDataService>();
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks())
                .Returns(
                    new List<FrameworkMetaData>
                        {
                            new FrameworkMetaData
                                {
                                    EffectiveFrom = DateTime.Parse("2015-01-01"),
                                    EffectiveTo = DateTime.Parse("2015-01-02"), // Date in the past
                                    FworkCode = 500,
                                    PwayCode = 1,
                                    ProgType = 22,
                                }
                        });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, _mockVstsService.Object, null, null, null);
            var frameworks = metaDataManager.GetAllFrameworks();

            Assert.AreEqual(0, frameworks.Count, "Effective to date can not be in the past");
        }

        [Test]
        public void GetFrameworkWithAllValidData()
        {
            var mockLarsDataService = new Mock<ILarsDataService>();
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks())
                .Returns(
                    new List<FrameworkMetaData> { new FrameworkMetaData { EffectiveFrom = DateTime.Parse("2015-01-01"), EffectiveTo = DateTime.MinValue, FworkCode = 500, PwayCode = 1, ProgType = 2 } });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, _mockVstsService.Object, null, null, null);
            var frameworks = metaDataManager.GetAllFrameworks();

            Assert.AreEqual(1, frameworks.Count, "Should find one framework");
        }

        [TestCase(399)]
        public void FrameworkCodeMustBeOverValue(int value)
        {
            var mockLarsDataService = new Mock<ILarsDataService>();
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks())
                .Returns(
                    new List<FrameworkMetaData> { new FrameworkMetaData { EffectiveFrom = DateTime.Parse("2015-01-01"), EffectiveTo = DateTime.MinValue, FworkCode = 399, PwayCode = 1, ProgType = 3 } });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, _mockVstsService.Object, null, null, null);
            var frameworks = metaDataManager.GetAllFrameworks();

            Assert.AreEqual(0, frameworks.Count, $"Framework code must be over {value}");
        }

        [Test]
        public void PathwayCodeMustBeOver0()
        {
            var mockLarsDataService = new Mock<ILarsDataService>();
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks())
                .Returns(
                    new List<FrameworkMetaData>
                        {
                            new FrameworkMetaData { EffectiveFrom = DateTime.Parse("2015-01-01"), EffectiveTo = DateTime.MinValue, FworkCode = 500, PwayCode = 0, ProgType = 20 }
                        });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, _mockVstsService.Object, null, null, null);
            var frameworks = metaDataManager.GetAllFrameworks();

            Assert.AreEqual(0, frameworks.Count, "Pathway codemust be over 0");
        }

        [Test]
        public void FrameworkanOnlyHaveCertainValues()
        {
            var mockLarsDataService = new Mock<ILarsDataService>();
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks())
                .Returns(
                    new List<FrameworkMetaData>
                        {
                            new FrameworkMetaData { EffectiveFrom = DateTime.Parse("2015-01-01"), EffectiveTo = DateTime.MinValue, FworkCode = 500, PwayCode = 1, ProgType = 16 }
                        });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, _mockVstsService.Object, null, null, null);
            var frameworks = metaDataManager.GetAllFrameworks();

            Assert.AreEqual(0, frameworks.Count);
        }

        [Test]
        public void FrameworkanEffectiveFromCantBeNull()
        {
            var mockLarsDataService = new Mock<ILarsDataService>();
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks()).Returns(
                new List<FrameworkMetaData>
                    {
                        new FrameworkMetaData
                            {
                                EffectiveFrom = DateTime.MinValue, // Not valid
                                EffectiveTo = DateTime.MinValue,
                                FworkCode = 500,
                                PwayCode = 1,
                                ProgType = 21
                            }
                    });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, _mockVstsService.Object, null, null, null);
            var frameworks = metaDataManager.GetAllFrameworks();

            Assert.AreEqual(0, frameworks.Count);
        }

        [Test]
        public void ShouldMapJobRoles()
        {
            var mockLarsDataService = new Mock<ILarsDataService>();
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks())
                .Returns(
                    new List<FrameworkMetaData>
                        {
                            new FrameworkMetaData { EffectiveFrom = DateTime.Parse("2015-01-01"), EffectiveTo = DateTime.MinValue, FworkCode = 500, PwayCode = 1, ProgType = 21 }
                        });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, _mockVstsService.Object, null, null, null);
            var framework = metaDataManager.GetAllFrameworks().FirstOrDefault();

            framework.Should().NotBeNull();
            framework.JobRoleItems.Count().Should().Be(2);
            framework.JobRoleItems.FirstOrDefault().Title.Should().Be("Job role 2");
            framework.JobRoleItems.FirstOrDefault().Description.Should().Be("Description 2");
        }

        [Test]
        public void ShouldMapTypicalLength()
        {
            var mockVstsService = new Mock<IVstsService>();
            mockVstsService.Setup(m => m.GetFrameworks()).Returns(GetVstsMetaData());

            var mockLarsDataService = new Mock<ILarsDataService>();
            mockLarsDataService.Setup(m => m.GetListOfCurrentFrameworks())
                .Returns(
                    new List<FrameworkMetaData>
                        {
                            new FrameworkMetaData { EffectiveFrom = DateTime.Parse("2015-01-01"), EffectiveTo = DateTime.MinValue, FworkCode = 500, PwayCode = 1, ProgType = 21 }
                        });

            var metaDataManager = new MetaDataManager(mockLarsDataService.Object, _mockVstsService.Object, null, null, null);
            var framework = metaDataManager.GetAllFrameworks().FirstOrDefault();

            framework.Should().NotBeNull();
            framework.TypicalLength.From.Should().Be(12);
            framework.TypicalLength.To.Should().Be(24);
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
                            TypicalLength = new TypicalLength { From = 18, To = 18, Unit = "m" }
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
                                TypicalLength = new TypicalLength { From = 12, To = 24, Unit = "m" }
                            }
                        };
        }

    }
}