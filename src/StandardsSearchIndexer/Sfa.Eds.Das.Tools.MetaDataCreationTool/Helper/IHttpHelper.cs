namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Helper
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IHttpHelper
    {
        string DownloadFile(string downloadFileUrl, string workingfolder);
        string DownloadString(string streamUrl, string username, string pwd);
        Task<Stream> DownloadStream(string streamUrl, string username, string pwd);
        void Post(string url, string body, string user, string password);
    }
}