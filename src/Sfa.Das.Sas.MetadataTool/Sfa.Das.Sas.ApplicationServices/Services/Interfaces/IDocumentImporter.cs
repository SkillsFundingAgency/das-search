namespace Sfa.Das.Sas.ApplicationServices.Services.Interfaces
{
    using Models;

    public interface IDocumentImporter
    {
        MapperResponse Import(string text, string type);
    }
}