namespace Sfa.Eds.Das.Indexer.Core.UnitTests.Extensions
{
    using System.IO;

    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.Core.Extensions;

    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void ShouldConvertAStringToAStream()
        {
            var input = "test";
            var stream = input.GenerateStreamFromString();
            var result = new StreamReader(stream).ReadToEnd();

            Assert.AreEqual(input, result);
        }
    }
}