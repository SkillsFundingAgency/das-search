using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace IndexerPdfsTest.Model
{
    public class Attachment
    {
        [ElasticProperty(Name = "_content")]
        public string Content { get; set; }

        [ElasticProperty(Name = "_content_type")]
        public string ContentType { get; set; }

        [ElasticProperty(Name = "_name")]
        public string Name { get; set; }
    }
}
