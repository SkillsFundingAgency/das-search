namespace Sfa.Das.ApplicationServices
{
    using System.Threading.Tasks;

    using Sfa.Eds.Das.Core.Domain.Model;

    public interface ILookupLocations
    {
        Task<Coordinate> GetLatLongFromPostCode(string postcode);
    }
}