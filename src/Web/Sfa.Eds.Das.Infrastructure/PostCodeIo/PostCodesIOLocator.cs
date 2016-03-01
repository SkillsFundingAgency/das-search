using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.ApplicationServices;
using Sfa.Eds.Das.Core.Domain.Model;

namespace Sfa.Eds.Das.Infrastructure.PostCodeIo
{
    public class PostCodesIOLocator : ILookupLocations
    {
        public async Task<Coordinate> GetLatLongFromPostCode(string postcode)
        {
            var coordinates = new Coordinate();
            var sURL = "http://api.postcodes.io/postcodes/" + postcode.Replace(" ", string.Empty);

            using (var client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(
                    HttpMethod.Get,
                    sURL);

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var value = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<PostCodeResponse>(value);
                    coordinates.Lat = result.Result.Latitude;
                    coordinates.Lon = result.Result.Longitude;
                }
            }

            return coordinates;
        }
    }
}
