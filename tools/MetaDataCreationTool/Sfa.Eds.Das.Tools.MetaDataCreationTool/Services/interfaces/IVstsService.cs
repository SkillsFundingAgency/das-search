namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;

    using Newtonsoft.Json;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;

    internal interface IVstsService
    {
        void Start();
    }

    internal class VstsService : IVstsService
    {
        public void Start()
        {
            var gitrepository = "SFA-DAS-MetaDataStorage";
            var folderPath = "/standards/ci/json";
            var account = "sfa-gov-uk";
            var vstsProject = "Digital%20Apprenticeship%20Service";
            var jsonFolderUrl =
                $"https://{account}.visualstudio.com/DefaultCollection/{vstsProject}/_apis/git/repositories/{gitrepository}/items?scopePath={folderPath}&recursionLevel=Full&api-version=2.0";

            var folderTreeStr = Get(jsonFolderUrl);
            var tree = JsonConvert.DeserializeObject<GitTree>(folderTreeStr);

            var standards = new List<Standard>();
            foreach (var blob in tree.Value.Where(x => x.IsBlob))
            {
                var str = Get(blob.Url);
                var standard = JsonConvert.DeserializeObject<Standard>(str);
                standards.Add(standard);
            }
        }

        private string Get(string streamUrl)
        {
            var username = "USERNAME";
            var pwd = "TOKEN";
            using (WebClient client = new WebClient())
            {
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{pwd}"));
                client.Headers[HttpRequestHeader.Authorization] = $"Basic {credentials}";

                return client.DownloadString(streamUrl); // error check
            }
        }
    }

    public class GitTree
    {
        public int Count{ get; set; }

        public List<Entity> Value { get; set; }
    }

    public class Entity
    {
        public string ObjectId { get; set; }

        public string Url { get; set; }

        public string GitObjectType { get; set; }

        public bool IsBlob => GitObjectType.Equals("blob");
    }
}