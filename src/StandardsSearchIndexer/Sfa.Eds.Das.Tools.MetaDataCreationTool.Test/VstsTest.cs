
namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Test
{
    using System.Linq;

    using NUnit.Framework;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services;

    /// <summary>
    /// Summary description for VstsTest
    /// </summary>
    [TestFixture]
    public class VstsTest
    {
        [Test]
        public void GeStandards()
        {
            var vsts = new VstsService(new Settings(null), null);
            var standandards = vsts.GetStandards();
            Assert.AreEqual(2, standandards.Count());
        }

        [Test]
        public void GDsg()
        {
            var vsts = new VstsService(new Settings(null), null);
            //var sts = vsts.GetStandardObjects();

            var x = 9;
        }
    }
}
