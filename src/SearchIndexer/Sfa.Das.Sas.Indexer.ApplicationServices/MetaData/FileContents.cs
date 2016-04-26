namespace Sfa.Das.Sas.Indexer.ApplicationServices.MetaData
{
    public class FileContents
    {
        public FileContents(string fileName, string json)
        {
            FileName = fileName;
            Json = json;
        }

        public string FileName { get; }

        public string Json { get; }
    }
}