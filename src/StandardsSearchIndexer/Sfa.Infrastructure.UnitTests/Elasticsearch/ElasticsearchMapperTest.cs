namespace Sfa.Infrastructure.UnitTests.Elasticsearch
{
    using System;

    using NUnit.Framework;

    using Eds.Das.Indexer.Core.Models.Framework;
    using Infrastructure.Elasticsearch;

    [TestFixture]
    public class ElasticsearchMapperTest
    {
        [Test]
        public void WhenCreatingFrameworkDocument()
        {
            var frameworkMetaData = new FrameworkMetaData
            {
                EffectiveFrom = DateTime.Parse("10-Feb-14"),
                EffectiveTo = DateTime.MinValue,
                FworkCode = 123,
                PwayCode = 1,
                IssuingAuthorityTitle = "Sustainable Resource Operations and Management - Higher Apprenticeship (Level 4)",
                NASTitle = "Sustainable Resource Operations and Management",
                PathwayName = "Higher Apprenticeship in Sustainable Resource Operations and Management",
                ProgType = 20
            };

            var mapper = new ElasticsearchMapper(null);

            var framework = mapper.CreateFrameworkDocument(frameworkMetaData);

            Assert.AreEqual("Sustainable Resource Operations and Management : Higher Apprenticeship in Sustainable Resource Operations and Management", framework.Title);
            Assert.AreEqual(4, framework.Level,"Should have level");
            Assert.AreEqual("123201", framework.FrameworkId, "Should have id from fwcode, progtype and pwcode");
        }

        [Test]
        public void WhenCreatingFrameworkDocumentAndTitleIsPathway()
        {
            var frameworkMetaData = new FrameworkMetaData
            {
                EffectiveFrom = DateTime.Parse("10-Feb-14"),
                EffectiveTo = DateTime.MinValue,
                FworkCode = 616,
                PwayCode = 1,
                IssuingAuthorityTitle = "Trade Business Services - Intermediate Level Apprenticeship",
                NASTitle = "Trade Business Services",
                PathwayName = "Trade Business Services",
                ProgType = 3
            };

            var mapper = new ElasticsearchMapper(null);

            var framework = mapper.CreateFrameworkDocument(frameworkMetaData);

            Assert.AreEqual("Trade Business Services", framework.Title);
            Assert.AreEqual(2, framework.Level, "Should have level");
            Assert.AreEqual("61631", framework.FrameworkId, "Should have id from fwcode, progtype and pwcode");
        }

        [Test]
        public void WhenCreatingFrameworkDocumentAndPathwayIsMissing()
        {
            var frameworkMetaData = new FrameworkMetaData
            {
                EffectiveFrom = DateTime.Parse("10-Feb-14"),
                EffectiveTo = DateTime.MinValue,
                FworkCode = 616,
                PwayCode = 1,
                IssuingAuthorityTitle = "Trade Business Services - Intermediate Level Apprenticeship",
                NASTitle = "Trade Business Services",
                PathwayName = " ",
                ProgType = 23
            };

            var mapper = new ElasticsearchMapper(null);

            var framework = mapper.CreateFrameworkDocument(frameworkMetaData);

            Assert.AreEqual("Trade Business Services", framework.Title);
            Assert.AreEqual(7, framework.Level, "Should have level");
            Assert.AreEqual("616231", framework.FrameworkId, "Should have id from fwcode, progtype and pwcode");
        }
    }
}
