using System.IO;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.Core.Extensions;

namespace Sfa.Das.Sas.Indexer.Core.UnitTests.Extensions
{
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