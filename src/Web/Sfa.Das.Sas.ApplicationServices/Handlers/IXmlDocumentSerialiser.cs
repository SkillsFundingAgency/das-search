using System.Collections.Generic;
using System.Xml.Linq;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public interface IXmlDocumentSerialiser
    {
        string Serialise(string xmlNamespace, string urlPlaceholder, IEnumerable<string> items);
    }
}