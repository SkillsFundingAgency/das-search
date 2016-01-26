using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Nito.AsyncEx;

namespace LatLongLocator
{
    public class LocationService
    {
        static void Main(string[] args)
        {
            LocationService ls = new LocationService();
            AsyncContext.Run(() => ls.MainAsync(args));
        }

        async void MainAsync(string[] args)
        {
            await IsLatLonIntoGb(51.889908, -2.117493);
        }

        public async Task<bool> IsLatLonIntoGb(double lat, double lon)
        {
            string countryShortName = string.Empty;

            string baseUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false", lat, lon);
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(baseUri);
            var responseElement = XElement.Parse(response);
            IEnumerable<XElement> statusElement = from st in responseElement.Elements("status") select st;
            if (statusElement.FirstOrDefault() != null)
            {
                string status = statusElement.FirstOrDefault().Value;
                if (status.ToLower() == "ok")
                {
                    IEnumerable<XElement> resultElement = from rs in responseElement.Elements("result") select rs;
                    if (resultElement.FirstOrDefault() != null)
                    {
                        IEnumerable<XElement> addressElement = from ad in resultElement.FirstOrDefault().Elements("address_component") select ad;
                        foreach (XElement element in addressElement)
                        {
                            IEnumerable<XElement> typeElement = from te in element.Elements("type") select te;
                            string type = typeElement.FirstOrDefault().Value;
                            if (type == "country")
                            {
                                IEnumerable<XElement> countryElement = from ln in element.Elements("short_name") select ln;
                                countryShortName = countryElement.FirstOrDefault().Value;
                                break;
                            }
                        }
                    }
                }
            }

            return countryShortName.ToLower().Equals("gb");
        }
    }
}
