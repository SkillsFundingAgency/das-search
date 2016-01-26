using System.Threading.Tasks;

namespace Sfa.Eds.Das.Web.Services
{
    public interface ILocationService
    {
        Task<bool> IsLatLonIntoGb(double lat, double lon);
    }
}