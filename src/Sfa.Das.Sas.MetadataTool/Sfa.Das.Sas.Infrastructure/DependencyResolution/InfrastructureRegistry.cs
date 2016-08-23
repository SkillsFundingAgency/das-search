using StructureMap;

namespace Sfa.Das.Sas.Infrastructure.DependencyResolution
{
    using Core.Configuration;
    using Core.Logging;
    using Logging;

    using ApplicationServices.Services.Interfaces;
    using MongoDb;

    public sealed class InfrastructureRegistry : Registry
    {
        public InfrastructureRegistry()
        {
            For<ILog>().Use(x => new NLogLogger(x.ParentType, x.GetInstance<IConfigurationSettings>(), x.GetInstance<IRequestContext>())).AlwaysUnique();
            For<IConfigurationSettings>().Use<InfrastructureSettings>();
            For<IMongoDataClient>().Use<MongoDataClient>();

            For<IDocumentImporter>().Use<MongoDocumentImporter>();

            For<IMetaDataService>().Use<MongoDataService>();
            For<IMongoSettings>().Use<MongoSettings>();
        }
    }
}