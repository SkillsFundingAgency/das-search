namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Models.Git
{
    public class GitPushFile
    {
        public RefUpdate RefUpdates { get; set; }

        public Commits Commits { get; set; }
    }

    public class Commits
    {
        public string Comment { get; set; }

        public Change Changes { get; set; }

    }

    public class Change
    {
        public string  ChangeType { get; set; }

        public ChangeItem Item { get; set; }

        public ContentItem NewContent { get; set; }

    }

    public class ContentItem
    {
        public string  Content { get; set; }

        public string ContentType { get; set; }
    }

    public class ChangeItem
    {
        public string Path { get; set; }
    }


    public class RefUpdate
    {
        public string Name { get; set; }

        public string OldObjectId { get; set; }
    }
}
