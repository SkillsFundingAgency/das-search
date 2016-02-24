using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Test
{
    [TestClass]
    public class MetaDataTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            MetaData metaData = new MetaData();
            metaData.Start();
            //var ids = metaData.GetExistingStandardIds();
            //var standards = metaData.GenerateStandardMetadataFiles(ids);
            //metaData.GetStandards();
        }
    }
}
