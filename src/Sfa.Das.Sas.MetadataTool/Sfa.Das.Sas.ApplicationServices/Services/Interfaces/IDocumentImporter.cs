using System.Collections.Generic;

namespace Sfa.Das.Sas.ApplicationServices.Services.Interfaces
{
    public interface IDocumentImporter
    {
        void ImportDocuments<T>(IEnumerable<T> document, string collectionName);
    }
}