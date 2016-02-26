namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Helper
{
    public interface IHttpHelper
    {
        string DownloadFile(string downloadFileUrl, string workingfolder);
        string DownloadString(string streamUrl, string username, string pwd);
        void Post(string url, string body, string user, string password);
    }
}