using System;
using System.Collections;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.Infrastructure.Settings;

namespace Sfa.Das.Sas.Indexer.UnitTests.Infrastructure.Settings
{
    [TestFixture]
    public class MachineSettingsTests
    {
        [SetUp]
        public void Setup()
        {
            RemoveTestEnvironmentVariables();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            RemoveTestEnvironmentVariables();
        }

        [Test]
        [Category("Problematic")]
        public void ShouldGetSettingFromEnvironmentVariable()
        {
            CreateEnvironmentVariable("TestSetting");

            var settingProvider = new MachineSettings();

            var setting = settingProvider.GetSetting("TestSetting");

            Assert.That(setting, Is.EqualTo("TestSettingValue"));
        }

        private void CreateEnvironmentVariable(string settingKey)
        {
            Environment.SetEnvironmentVariable("DAS_" + settingKey, "TestSettingValue", EnvironmentVariableTarget.User);
        }

        private void RemoveTestEnvironmentVariables()
        {
            foreach (DictionaryEntry de in Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User))
            {
                var key = de.Key as string;

                if (key != null && key.StartsWith("DAS_Test"))
                {
                    Environment.SetEnvironmentVariable(key, null, EnvironmentVariableTarget.User);
                }
            }
        }
    }
}
