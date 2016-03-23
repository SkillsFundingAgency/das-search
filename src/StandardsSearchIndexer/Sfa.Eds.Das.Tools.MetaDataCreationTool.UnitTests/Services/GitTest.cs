namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.UnitTests.Services
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using Sfa.Eds.Das.Indexer.ApplicationServices.MetaData;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Test.Services;

    [TestFixture]
    public class GitTest
    {
        [Test]
        public void GeneratePushJson()
        {
            var git = new GitDynamicModelGenerator();
            var list = new List<FileContents> { new FileContents("file1.json", "{hej}") };
            var generated = git.GenerateCommitBody("branch/path/master", "1122-33-4-55-s-d-f-g", list);
            var s = generated;

            Assert.AreEqual(GitTestResults.GeneratePushJson, s);
        }

        [Test]
        public void GeneratePush2NewJson()
        {
            var git = new GitDynamicModelGenerator();
            var list = new List<FileContents> { new FileContents("file1.json", "{hej}"), new FileContents("file2.json", "{varlden}") };
            var generated = git.GenerateCommitBody("branch/path/master", "1121q-33-asd2-55-s-d-f-g", list);
            var s = generated;

            Assert.AreEqual(GitTestResults.GeneratePush2NewJson, s);
        }
    }
}
