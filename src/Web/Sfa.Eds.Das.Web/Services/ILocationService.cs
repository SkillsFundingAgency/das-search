namespace Sfa.Eds.Das.Web.Services
{
    public interface ILocationService
    {
        bool IsLatLonIntoGb(double lat, double lon);
    }
}