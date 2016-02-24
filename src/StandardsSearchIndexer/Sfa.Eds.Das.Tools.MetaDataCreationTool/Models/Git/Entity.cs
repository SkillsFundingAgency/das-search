namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Models.Git
{
    public class Entity
    {
        public string CommitId { get; set; }

        public string ObjectId { get; set; }

        public string Url { get; set; }

        public string GitObjectType { get; set; }

        public string Path { get; set; }

        public bool IsBlob => GitObjectType.Equals("blob");
    }
}