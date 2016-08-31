namespace Sfa.Das.Sas.ApplicationServices.MetaData
{
    using Sfa.Das.Sas.Core.Models;

    public interface IUpdateMetaData
    {
        void GetFrameworkMetaData(FrameworkMetaData model);
        void GetStandardMetaData(StandardMetaData model);
    }
}