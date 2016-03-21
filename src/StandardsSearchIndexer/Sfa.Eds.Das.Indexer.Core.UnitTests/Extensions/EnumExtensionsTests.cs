namespace Sfa.Eds.Das.Indexer.Core.UnitTests.Extensions
{
    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.Core.Extensions;

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