using Sfa.Das.Sas.ApplicationServices.Helpers;
using Sfa.Das.Sas.ApplicationServices.Http;
using Sfa.Das.Sas.ApplicationServices.MetaData;
using Sfa.Das.Sas.ApplicationServices.Services;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.Core.Logging;
using StructureMap;

namespace Sfa.Das.Sas.ApplicationServices.DependencyResolution {
    using Sfa.Das.Sas.ApplicationServices.Services.Interfaces;

    public class ApplicationServicesRegistry : Registry
    {
        public ApplicationServicesRegistry()
        {
            For<IJsonMetaDataConvert>().Use<JsonMetaDataConvert>();

            For<IAppServiceSettings>().Use<AppServiceSettings>();

            // Apprenticeships
            For<IMetaDataHelper>().Use<MetaDataHelper>();
            
            For<IHttpGetFile>().Use<HttpService>();
            For<IHttpGet>().Use<HttpService>();
            For<IHttpPost>().Use<HttpService>();

            For<IGetStandardMetaData>().Use<MetaDataManager>();
            For<IGetFrameworkMetaData>().Use<MetaDataManager>();
            For<IJsonMetaDataConvert>().Use<JsonMetaDataConvert>();

            For<IAppServiceSettings>().Use<AppServiceSettings>();

            For<IProvideSettings>().Use(c => new AppConfigSettingsProvider(new MachineSettings()));

            For<IMappingService>().Use<MappingService>();
        }
    }
}