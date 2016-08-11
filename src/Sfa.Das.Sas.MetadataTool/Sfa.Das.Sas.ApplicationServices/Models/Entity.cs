using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public class Entity
    {
        public string CommitId { get; set; }

        public string ObjectId { get; set; }

        public string Url { get; set; }

        public string GitObjectType { get; set; }

        public string Path { get; set; }

        public bool IsBlob => GitObjectType.Equals("blob", StringComparison.InvariantCulture);
    }
}
