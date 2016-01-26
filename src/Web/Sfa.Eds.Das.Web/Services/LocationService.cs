using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sfa.Eds.Das.Web.Services
{
    public class LocationService : ILocationService
    {
        public bool IsLatLonIntoGb(double lat, double lon)
        {
            return true;
        }
    }
}