using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public class FileContents
    {
        public FileContents(string fileName, string json)
        {
            FileName = fileName;
            Json = json;
        }

        public string FileName { get; }

        public string Json { get; }
    }
}
