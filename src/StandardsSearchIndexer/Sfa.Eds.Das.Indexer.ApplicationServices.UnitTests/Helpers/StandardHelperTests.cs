namespace Sfa.Eds.Das.Indexer.ApplicationServices.UnitTests.Helpers
{
    using System;

    using Nest;
    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;

    [TestFixture]
    public class StandardHelperTests
    {
        [Test]
        public void Testtest()
        {

            var indices = new FluentDictionary<string, Stats>
                              {
                                  { "provider-2016-01-04-00", null }, // Provider - Invalid
                                  { "standard-2016-01-04-00", null }, // Two days ago
                                  { ".kibana", null}, { "standard-20160104", null }, // Two invalid
                                  { "standard-2016-01-05-00", null }, // OneDayAgo
                                  { "standard-2016-01-06-00", null } // Current date
                              };

            var indexService = new IndexMaintenanceService();
            
            var oldIndices = indexService.GetOldIndices("standard", DateTime.Parse("2016-01-06"), indices);

            Assert.AreEqual(2, oldIndices.Count);
            Assert.AreEqual("standard-2016-01-04-00", oldIndices[0]);
            Assert.AreEqual("standard-2016-01-05-00", oldIndices[1]);
        }
    }
}