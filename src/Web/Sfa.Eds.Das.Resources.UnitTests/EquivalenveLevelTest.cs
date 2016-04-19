using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Sfa.Eds.Das.Resources.UnitTests
{
    [TestFixture]
    public class EquivalenveLevelTest
    {
        [TestCase("1", "GCSEs at grades D to G")]
        [TestCase("2", "GCSEs at grades A* to C")]
        [TestCase("3", "A levels at grades A to E")]
        [TestCase("4", "certificate of higher education")]
        [TestCase("5", "foundation degree")]
        [TestCase("6", "bachelor's degree")]
        [TestCase("7", "master’s degree")]
        [TestCase("8", "doctorate")]
        public void ShouldReturnEquivalenceTextForLevel(string level, string expected)
        {
            var actual = EquivalenveLevelService.GetFrameworkLevel(level);

            actual.Should().Be(expected);
        }
    }
}
