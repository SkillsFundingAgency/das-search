using System.Threading.Tasks;
using Sfa.Eds.Das.Core.Domain.Model;

namespace Sfa.Eds.Das.ApplicationServices
{
    public interface ILookupLocations
    {
        Task<Coordinate> GetLatLongFromPostCode(string postcode);
    }
}