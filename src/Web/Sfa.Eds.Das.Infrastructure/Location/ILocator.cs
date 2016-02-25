using System.Threading.Tasks;
using Sfa.Eds.Das.Core.Domain.Model;

namespace Sfa.Eds.Das.Infrastructure.Location
{
    public interface ILocator
    {
        Task<Coordinate> GetLatLongFromPostCode(string postcode);
    }
}