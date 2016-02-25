namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Test.Services
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services;

    [TestFixture]
    public class GitTest
    {
        [Test]
        public void GeneratePushJson()
        {
            var git = new GitDynamicModelGenerator();
            var list = new List<StandardObject> { new StandardObject("file1.json", "{hej}") };
            var generated = git.GenerateCommitBody("branch/path/master", "1122-33-4-55-s-d-f-g", list);
            var s = generated;

            Assert.AreEqual(GitTestResults.GeneratePushJson, s);
        }

        [Test]
        public void GeneratePush2NewJson()
        {
            var git = new GitDynamicModelGenerator();
            var list = new List<StandardObject> { new StandardObject("file1.json", "{hej}"), new StandardObject("file2.json", "{varlden}")};
            var generated = git.GenerateCommitBody("branch/path/master", "1121q-33-asd2-55-s-d-f-g", list);
            var s = generated;

            Assert.AreEqual(GitTestResults.GeneratePush2NewJson, s);
        }
    }

    public static class GitTestResults
    {
        public static string GeneratePushJson {
            get
            {
                return
                    "{\"refUpdates\":[{\"name\":\"branch/path/master\",\"oldObjectId\":\"1122-33-4-55-s-d-f-g\"}],\"commits\":[{\"comment\":\"1 files created\",\"changes\":[{\"changeType\":\"add\",\"item\":{\"path\":\"file1.json\"},\"newContent\":{\"content\":\"{hej}\",\"contentType\":\"rawtext\"}}]}]}";
            }
        }
        public static string GeneratePush2NewJson
        {
            get
            {
                return
                    "{\"refUpdates\":[{\"name\":\"branch/path/master\",\"oldObjectId\":\"1121q-33-asd2-55-s-d-f-g\"}],\"commits\":[{\"comment\":\"2 files created\",\"changes\":[{\"changeType\":\"add\",\"item\":{\"path\":\"file1.json\"},\"newContent\":{\"content\":\"{hej}\",\"contentType\":\"rawtext\"}},{\"changeType\":\"add\",\"item\":{\"path\":\"file2.json\"},\"newContent\":{\"content\":\"{varlden}\",\"contentType\":\"rawtext\"}}]}]}";
            }
        }
    }
}
