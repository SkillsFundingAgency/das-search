using System;
using System.Collections;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Infrastructure.Settings;

namespace Sfa.Infrastructure.UnitTests.Settings
{
    [TestFixture]
    public class LarsSettingsTests
    {
        [Test]
        public void ShouldGetDataSetNameSetting()
        {
            LarsSettings ls = new LarsSettings();

            var datasetName = ls.DatasetName;
            Assert.That(datasetName, Is.EqualTo("TestDataset"));
        }

        [Test]
        public void ShouldGetSearchEndpointNameSetting()
        {
            LarsSettings ls = new LarsSettings();

            var searchEndpointConfigurationName = ls.SearchEndpointConfigurationName;
            Assert.That(searchEndpointConfigurationName, Is.EqualTo("TestSearchEndpoint"));
        }

        [Test]
        public void ShouldGetStandardDescriptorNameSetting()
        {
            LarsSettings ls = new LarsSettings();

            var standardDescriptorName = ls.StandardDescriptorName;
            Assert.That(standardDescriptorName, Is.EqualTo("TestStandardCommonComponent"));
        }
    }
}
