namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Test.Services
{
    using System.Linq;
    using NUnit.Framework;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services;

    [TestFixture]
    public class CsvServiceTest
    {
        [Test]
        public void CreateFrameworksFromCsv()
        {
            CsvService csvService = new CsvService(new AngleSharpService());
            var frameworks = csvService.ReadFrameworksFromStream(testCsvData);

            Assert.AreEqual(10, frameworks.Count);

            var firstDate = frameworks.FirstOrDefault()?.EffectiveFrom.ToString("yyyy-MM-dd");

            Assert.AreEqual("2013-10-07", firstDate);

            var tradeBusinessServices = frameworks.Single(m => m.PathwayName.Equals("Trade Business Services"));

            Assert.AreEqual(3, tradeBusinessServices.ProgType);
            Assert.AreEqual("2050-01-18", tradeBusinessServices.EffectiveTo.ToString("yyyy-MM-dd"));
        }

        private string testCsvData =
            "\"FworkCode\",\"ProgType\",\"PwayCode\",\"PathwayName\",\"EffectiveFrom\",\"EffectiveTo\",\"SectorSubjectAreaTier1\",\"SectorSubjectAreaTier2\",\"DataSource\",\"NASTitle\",\"ImplementDate\",\"IssuingAuthorityTitle\",\"IssuingAuthority\",\"DataReceivedDate\",\"MI_FullLevel2\",\"MI_FullLevel2Percent\",\"MI_FullLevel3\",\"MI_FullLevel3Percent\",\"CurrentVersion\",\"Created_On\",\"Created_By\",\"Modified_On\",\"Modified_By\"\n\"612\",\"21\",\"2\",\"Construction Management (Sustainability)\",\"07 Oct 2013\",\"\",\"5.00\",\"5.20\",\"\",\"Construction Management\",\"05 Mar 2015\",\"Construction Management - Higher Apprenticeship (Level 5)\",\"124\",\"05 Mar 2015\",\"0\",\"0.00\",\"0\",\"0.00\",\"3\",\"03 Feb 2014\",\"suttonf\",\"22 Apr 2015\",\"System\"\n\"612\",\"21\",\"3\",\"Foundation Degree in Architecture\",\"01 Aug 2015\",\"\",\"5.00\",\"5.20\",\"\",\"Construction Management\",\"05 Mar 2015\",\"Construction Management - Higher Apprenticeship (Level 5)\",\"124\",\"05 Mar 2015\",\"\",\"\",\"\",\"\",\"3\",\"20 Jan 2016\",\"guinivas\",\"20 Jan 2016\",\"guinivas\"\"612\",\"21\",\"4\",\"Foundation Degree in Built Environment\",\"01 Aug 2015\",\"\",\"5.00\",\"5.20\",\"\",\"Construction Management\",\"05 Mar 2015\",\"Construction Management - Higher Apprenticeship (Level 5)\",\"124\",\"05 Mar 2015\",\"\",\"\",\"\",\"\",\"3\",\"20 Jan 2016\",\"guinivas\",\"20 Jan 2016\",\"guinivas\"\"612\",\"21\",\"5\",\"Foundation Degree in Civil Engineering\",\"01 Aug 2015\",\"\",\"5.00\",\"5.20\",\"\",\"Construction Management\",\"05 Mar 2015\",\"Construction Management - Higher Apprenticeship (Level 5)\",\"124\",\"05 Mar 2015\",\"\",\"\",\"\",\"\",\"3\",\"20 Jan 2016\",\"guinivas\",\"20 Jan 2016\",\"guinivas\"\"612\",\"22\",\"1\",\"Construction Management \",\"07 Oct 2013\",\"\",\"5.00\",\"5.20\",\"\",\"Construction Management\",\"05 Mar 2015\",\"Construction Management - Higher Apprenticeship (Level 6)\",\"124\",\"05 Mar 2015\",\"0\",\"0.00\",\"0\",\"0.00\",\"3\",\"03 Feb 2014\",\"suttonf\",\"22 Apr 2015\",\"System\"\n\"612\",\"22\",\"2\",\"Construction Management \",\"07 Oct 2013\",\"\",\"5.00\",\"5.20\",\"\",\"Construction Management\",\"05 Mar 2015\",\"Construction Management - Higher Apprenticeship (Level 6)\",\"124\",\"05 Mar 2015\",\"0\",\"0.00\",\"0\",\"0.00\",\"3\",\"03 Feb 2014\",\"suttonf\",\"22 Apr 2015\",\"System\"\n\"613\",\"2\",\"1\",\"Supporting Teaching and Learning in Physical Education and School Sport\",\"25 Nov 2013\",\"\",\"8.00\",\"8.10\",\"\",\"Supporting Teaching and Learning in Physical Education and School Sport\",\"03 Feb 2014\",\"Supporting Teaching and Learning in Physical Education and School Sport - Advanced Level Apprenticeship\",\"120\",\"20 Nov 2013\",\"0\",\"0.00\",\"1\",\"100.00\",\"2\",\"03 Feb 2014\",\"suttonf\",\"22 Apr 2015\",\"System\"\n\"614\",\"21\",\"1\",\"Criminal Investigation in Serious and Complex Crime\",\"16 Dec 2013\",\"\",\"1.00\",\"1.40\",\"\",\"Criminal Investigation\",\"03 Feb 2014\",\"Criminal Investigation - Higher Apprenticeship (Level 5)\",\"117\",\"06 Dec 2013\",\"0\",\"0.00\",\"0\",\"0.00\",\"1\",\"03 Feb 2014\",\"suttonf\",\"22 Apr 2015\",\"System\"\n\"615\",\"22\",\"1\",\"Professional Aviation Pilot Practice\",\"02 Dec 2013\",\"\",\"4.00\",\"4.30\",\"\",\"Professional Aviation Pilot Practice\",\"03 Feb 2014\",\"Professional Aviation Pilot Practice - Higher Apprenticeship (Level 6)\",\"113\",\"20 Dec 2013\",\"0\",\"0.00\",\"0\",\"0.00\",\"2\",\"03 Feb 2014\",\"suttonf\",\"22 Apr 2015\",\"System\"\n\"616\",\"3\",\"1\",\"Trade Business Services\",\"16 Dec 2013\",\"18 Jan 2050\",\"7.00\",\"7.10\",\"\",\"Trade Business Services\",\"03 Feb 2014\",\"Trade Business Services - Intermediate Level Apprenticeship\",\"129\",\"06 Dec 2013\",\"1\",\"100.00\",\"0\",\"0.00\",\"3\",\"03 Feb 2014\",\"suttonf\",\"22 Apr 2015\",\"System\"\n\"617\",\"3\",\"1\",\"Community Fire Safety\",\"13 Jan 2014\",\"\",\"1.00\",\"1.40\",\"\",\"Community Safety\",\"03 Feb 2014\",\"Community Safety - Intermediate Level Apprenticeship\",\"117\",\"20 Dec 2013\",\"1\",\"100.00\",\"0\",\"0.00\",\"2\",\"03 Feb 2014\",\"suttonf\",\"22 Apr 2015\",\"System\"\n\"618\",\"20\",\"1\",\"Higher Apprenticeship in Sustainable Resource Operations and Management\",\"10 Feb 2014\",\"\",\"1.00\",\"1.40\",\"\",\"Sustainable Resource Operations and Management\",\"01 Oct 2013\",\"Sustainable Resource Operations and Management - Higher Apprenticeship (Level 4)\",\"106\",\"07 Feb 2014\",\"0\",\"0.00\",\"0\",\"0.00\",\"1\",\"04 Mar 2014\",\"Guinivas\",\"22 Apr 2015\",\"System\"\n\"619\",\"20\",\"1\",\"Contact Centre Operations Management\",\"24 Feb 2014\",\"24 Feb 2015\",\"15.00\",\"15.20\",\"\",\"Contact Centre Operations Management\",\"25 Mar 2014\",\"Contact Centre Operations Management - Higher Apprenticeship (Level 4)\",\"102\",\"20 Mar 2014\",\"0\",\"0.00\",\"0\",\"0.00\",\"8\",\"25 Mar 2014\",\"Guinivas\",\"22 Apr 2015\",\"System\"\n";
    }
}