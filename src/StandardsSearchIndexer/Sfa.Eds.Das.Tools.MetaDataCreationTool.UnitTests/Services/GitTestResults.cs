namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.UnitTests.Services
{
    public static class GitTestResults
    {
        public static string GeneratePushJson
        {
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
