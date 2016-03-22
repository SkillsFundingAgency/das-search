namespace Sfa.Infrastructure.Settings
{
    using System.Configuration;

    public class LarsSettings : ILarsSettings
    {
        public string SearchEndpointConfigurationName => ConfigurationManager.AppSettings["SearchEndpointConfigurationName"];

        public string DatasetName => ConfigurationManager.AppSettings["DatasetName"];

        public string StandardDescriptorName => ConfigurationManager.AppSettings["StandardDescriptorName"];
    }
}