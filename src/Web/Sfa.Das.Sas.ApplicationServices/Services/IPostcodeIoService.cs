using System.Threading.Tasks;

namespace Sfa.Das.Sas.ApplicationServices.Services
{
    public interface IPostcodeIoService
    {
        Task<string> GetPostcodeStatus(string postcode);
    }
}