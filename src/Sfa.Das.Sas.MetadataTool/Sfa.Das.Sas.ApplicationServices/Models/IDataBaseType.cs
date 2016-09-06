using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public interface IDataBaseType
    {
        Guid Id { get; set; }

        string DocumentVersion { get; set; }
    }
}
