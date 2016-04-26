using NUnit.Framework;
using Sfa.Das.Sas.Indexer.Core.Extensions;

namespace Sfa.Das.Sas.Indexer.Core.UnitTests.Extensions
{
    [TestFixture]
    public class EnumExtensionsTests
    {
        [Test]
        public void ShouldFindTheEnumValueDescriptionFromTheAttrbute()
        {
            var mode = EnumWithDescription.Test;
            var result = mode.GetDescription();
            Assert.AreEqual("Test Description", result);
        }
    }
}