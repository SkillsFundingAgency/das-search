namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Test
{
    using System.Linq;
    using NUnit.Framework;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services;

    [TestFixture]
    public class VstsTest
    {
        [Test]
        [Category("ExternalDependency")]
        public void GeStandards()
        {
            var vsts = new VstsService(new Settings());
            var standandards = vsts.GetStandards();
            Assert.IsTrue(50 < standandards.Count());
        }
    }
}
