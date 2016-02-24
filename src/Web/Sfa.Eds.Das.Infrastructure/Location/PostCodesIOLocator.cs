using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.Core.Domain.Model;

namespace Sfa.Eds.Das.Infrastructure.Location
{
    public class PostCodesIOLocator : ILocator
    {
        public async Task<Coordinate> GetLatLongFromPostCode(string postcode)
        {
            var coordinates = new Coordinate();
            var sURL = "http://api.postcodes.io/postcodes/" + postcode.Replace(" ", string.Empty);

            using (var client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(
                    HttpMethod.Get,
                    string.Concat(sURL));

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
