using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.ApplicationServices.Http
{
    public interface IHttpGetFile
    {
        Stream GetFile(string url);
    }
}
