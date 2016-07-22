namespace LARSMetaDataExplorer.Settings
{
    public class AppSettings : IAppSettings
    {
        public string ImServiceBaseUrl => "https://hub.imservices.org.uk";
        public string ImServiceUrl => "Learning%20Aims/Downloads/Pages/default.aspx";
        public string CsvFileNameStandards => "CSV/Standard.csv";
        public string CsvFileNameFrameworks => "CSV/Framework.csv";
        public string CsvFileNameFrameworkAims => "CSV/FrameworkAims.csv";
        public string CsvFileNameFrameworkComponentType => "CSV/FrameworkComponentType.csv";
        public string CsvFileNameLearningDelivery => "CSV/LearningDelivery.csv";
        public string CsvFileNameFunding => "CSV/Funding.csv";
        public string MetaDataBagFilePath => "MetaDataBag.dat";
        public string QualificationsFilePath => "Qualification.dat";
    }
}