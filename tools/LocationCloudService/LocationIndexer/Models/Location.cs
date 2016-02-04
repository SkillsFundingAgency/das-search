using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationIndexer.Models
{
    public class Location
    {
        public string PostCode { get; set; }
        public string Region { get; set; }
        public Coordinate Coordinate { get; set; }
    }
}
