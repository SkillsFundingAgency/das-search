using NUnit.Framework;
using Sfa.Das.Sas.Web.Helpers;
using Assert = NUnit.Framework.Assert;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Helpers
{
    [TestFixture]
    public class StringUrlHelperTests
    {
        [TestCase("help", "help")]
        [TestCase("help me", "help-me")]

        public void ShouldReturnStringModifiedForUrlUsage(string words, string modifiedwords)
        {
            var actual = new StringUrlHelper().ModifyStringForUrlUsage(words);
            Assert.AreEqual(actual, modifiedwords);

        }
    }
}
