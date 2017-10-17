using MvcRouteTester.Assertions;
using NUnit.Framework;

namespace Sfa.Das.Sas.Web.UnitTests.Routes
{
    public class NunitAssertEngine : IAssertEngine
    {
        [System.Diagnostics.DebuggerNonUserCode]
        public void Fail(string message)
        {
            Assert.Fail(message);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public void StringsEqualIgnoringCase(string s1, string s2, string message)
        {
            if (string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2))
            {
                return;
            }

            StringAssert.AreEqualIgnoringCase(s1, s2, message);
        }
    }
}