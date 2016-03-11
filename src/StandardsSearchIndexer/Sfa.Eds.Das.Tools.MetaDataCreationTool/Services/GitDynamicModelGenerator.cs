namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using System.Collections.Generic;

    using Sfa.Eds.Das.Indexer.ApplicationServices.MetaData;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

    public class GitDynamicModelGenerator : IGitDynamicModelGenerator
    {
        public string GenerateCommitBody(string branchPath, string oldObjectId, List<FileContents> items)
        {
            var str = new
            {
                refUpdates = new dynamic[] { new { name = branchPath, oldObjectId } },
                commits = new dynamic[]
                {
                    new
                    {
                        comment = $"{items.Count} files created",
                        changes = Changes(items)
                    }
                }
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(str);
        }

        private dynamic[] Changes(IEnumerable<FileContents> items)
        {
            var returnList = new List<dynamic>();
            foreach (var item in items)
            {
                var obj = new
                {
                    changeType = "add",
                    item = new
                    {
                        path = item.FileName
                    },
                    newContent = new
                    {
                        content = item.Json,
                        contentType = "rawtext"
                    }
                };
                returnList.Add(obj);
            }

            return returnList.ToArray();
        }
    }
}