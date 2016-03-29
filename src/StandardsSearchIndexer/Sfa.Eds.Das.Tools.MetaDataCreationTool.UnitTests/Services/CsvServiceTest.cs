namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using NUnit.Framework;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

    using CsvService = Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.CsvService;

    [TestFixture]
    public class CsvServiceTest
    {
        private readonly string _testFrameworkCsvData =
            "\"FworkCode\",\"ProgType\",\"PwayCode\",\"PathwayName\",\"EffectiveFrom\",\"EffectiveTo\",\"SectorSubjectAreaTier1\",\"SectorSubjectAreaTier2\",\"DataSource\",\"NASTitle\",\"ImplementDate\",\"IssuingAuthorityTitle\",\"IssuingAuthority\",\"DataReceivedDate\",\"MI_FullLevel2\",\"MI_FullLevel2Percent\",\"MI_FullLevel3\",\"MI_FullLevel3Percent\",\"CurrentVersion\",\"Created_On\",\"Created_By\",\"Modified_On\",\"Modified_By\"\n\"612\",\"21\",\"2\",\"Construction Management (Sustainability)\",\"07 Oct 2013\",\"\",\"5.00\",\"5.20\",\"\",\"Construction Management\",\"05 Mar 2015\",\"Construction Management - Higher Apprenticeship (Level 5)\",\"124\",\"05 Mar 2015\",\"0\",\"0.00\",\"0\",\"0.00\",\"3\",\"03 Feb 2014\",\"suttonf\",\"22 Apr 2015\",\"System\"\n\"612\",\"21\",\"3\",\"Foundation Degree in Architecture\",\"01 Aug 2015\",\"\",\"5.00\",\"5.20\",\"\",\"Construction Management\",\"05 Mar 2015\",\"Construction Management - Higher Apprenticeship (Level 5)\",\"124\",\"05 Mar 2015\",\"\",\"\",\"\",\"\",\"3\",\"20 Jan 2016\",\"guinivas\",\"20 Jan 2016\",\"guinivas\"\"612\",\"21\",\"4\",\"Foundation Degree in Built Environment\",\"01 Aug 2015\",\"\",\"5.00\",\"5.20\",\"\",\"Construction Management\",\"05 Mar 2015\",\"Construction Management - Higher Apprenticeship (Level 5)\",\"124\",\"05 Mar 2015\",\"\",\"\",\"\",\"\",\"3\",\"20 Jan 2016\",\"guinivas\",\"20 Jan 2016\",\"guinivas\"\"612\",\"21\",\"5\",\"Foundation Degree in Civil Engineering\",\"01 Aug 2015\",\"\",\"5.00\",\"5.20\",\"\",\"Construction Management\",\"05 Mar 2015\",\"Construction Management - Higher Apprenticeship (Level 5)\",\"124\",\"05 Mar 2015\",\"\",\"\",\"\",\"\",\"3\",\"20 Jan 2016\",\"guinivas\",\"20 Jan 2016\",\"guinivas\"\"612\",\"22\",\"1\",\"Construction Management \",\"07 Oct 2013\",\"\",\"5.00\",\"5.20\",\"\",\"Construction Management\",\"05 Mar 2015\",\"Construction Management - Higher Apprenticeship (Level 6)\",\"124\",\"05 Mar 2015\",\"0\",\"0.00\",\"0\",\"0.00\",\"3\",\"03 Feb 2014\",\"suttonf\",\"22 Apr 2015\",\"System\"\n\"612\",\"22\",\"2\",\"Construction Management \",\"07 Oct 2013\",\"\",\"5.00\",\"5.20\",\"\",\"Construction Management\",\"05 Mar 2015\",\"Construction Management - Higher Apprenticeship (Level 6)\",\"124\",\"05 Mar 2015\",\"0\",\"0.00\",\"0\",\"0.00\",\"3\",\"03 Feb 2014\",\"suttonf\",\"22 Apr 2015\",\"System\"\n\"613\",\"2\",\"1\",\"Supporting Teaching and Learning in Physical Education and School Sport\",\"25 Nov 2013\",\"\",\"8.00\",\"8.10\",\"\",\"Supporting Teaching and Learning in Physical Education and School Sport\",\"03 Feb 2014\",\"Supporting Teaching and Learning in Physical Education and School Sport - Advanced Level Apprenticeship\",\"120\",\"20 Nov 2013\",\"0\",\"0.00\",\"1\",\"100.00\",\"2\",\"03 Feb 2014\",\"suttonf\",\"22 Apr 2015\",\"System\"\n\"614\",\"21\",\"1\",\"Criminal Investigation in Serious and Complex Crime\",\"16 Dec 2013\",\"\",\"1.00\",\"1.40\",\"\",\"Criminal Investigation\",\"03 Feb 2014\",\"Criminal Investigation - Higher Apprenticeship (Level 5)\",\"117\",\"06 Dec 2013\",\"0\",\"0.00\",\"0\",\"0.00\",\"1\",\"03 Feb 2014\",\"suttonf\",\"22 Apr 2015\",\"System\"\n\"615\",\"22\",\"1\",\"Professional Aviation Pilot Practice\",\"02 Dec 2013\",\"\",\"4.00\",\"4.30\",\"\",\"Professional Aviation Pilot Practice\",\"03 Feb 2014\",\"Professional Aviation Pilot Practice - Higher Apprenticeship (Level 6)\",\"113\",\"20 Dec 2013\",\"0\",\"0.00\",\"0\",\"0.00\",\"2\",\"03 Feb 2014\",\"suttonf\",\"22 Apr 2015\",\"System\"\n\"616\",\"3\",\"1\",\"Trade Business Services\",\"16 Dec 2013\",\"18 Jan 2050\",\"7.00\",\"7.10\",\"\",\"Trade Business Services\",\"03 Feb 2014\",\"Trade Business Services - Intermediate Level Apprenticeship\",\"129\",\"06 Dec 2013\",\"1\",\"100.00\",\"0\",\"0.00\",\"3\",\"03 Feb 2014\",\"suttonf\",\"22 Apr 2015\",\"System\"\n\"617\",\"3\",\"1\",\"Community Fire Safety\",\"13 Jan 2014\",\"\",\"1.00\",\"1.40\",\"\",\"Community Safety\",\"03 Feb 2014\",\"Community Safety - Intermediate Level Apprenticeship\",\"117\",\"20 Dec 2013\",\"1\",\"100.00\",\"0\",\"0.00\",\"2\",\"03 Feb 2014\",\"suttonf\",\"22 Apr 2015\",\"System\"\n\"618\",\"20\",\"1\",\"Higher Apprenticeship in Sustainable Resource Operations and Management\",\"10 Feb 2014\",\"\",\"1.00\",\"1.40\",\"\",\"Sustainable Resource Operations and Management\",\"01 Oct 2013\",\"Sustainable Resource Operations and Management - Higher Apprenticeship (Level 4)\",\"106\",\"07 Feb 2014\",\"0\",\"0.00\",\"0\",\"0.00\",\"1\",\"04 Mar 2014\",\"Guinivas\",\"22 Apr 2015\",\"System\"\n\"619\",\"20\",\"1\",\"Contact Centre Operations Management\",\"24 Feb 2014\",\"24 Feb 2015\",\"15.00\",\"15.20\",\"\",\"Contact Centre Operations Management\",\"25 Mar 2014\",\"Contact Centre Operations Management - Higher Apprenticeship (Level 4)\",\"102\",\"20 Mar 2014\",\"0\",\"0.00\",\"0\",\"0.00\",\"8\",\"25 Mar 2014\",\"Guinivas\",\"22 Apr 2015\",\"System\"\n";

        private readonly string _testStandardCsvData =
            "\"StandardCode\",\"Version\",\"StandardName\",\"StandardSectorCode\",\"NotionalEndLevel\",\"EffectiveFrom\",\"LastDateStarts\",\"EffectiveTo\",\"UrlLink\",\"SectorSubjectAreaTier1\",\"SectorSubjectAreaTier2\",\"Created_On\",\"Created_By\",\"Modified_On\",\"Modified_By\"\n\"1\",\"1\",\"Network Engineer\",\"7\",\"4\",\"01 Aug 2014\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-network-engineer\",\"6.00\",\"6.10\",\"09 Jul 2015\",\"goodmand\",\"09 Jul 2015\",\"goodmand\"\n\"2\",\"1\",\"Software Developer\",\"7\",\"4\",\"01 Aug 2014\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-software-developer\",\"6.00\",\"6.20\",\"09 Jul 2015\",\"goodmand\",\"09 Jul 2015\",\"goodmand\"\n\"3\",\"1\",\"Aerospace Manufacturing Fitter\",\"2\",\"3\",\"01 Aug 2014\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-aerospace-manufacturing-fitter\",\"4.00\",\"4.10\",\"09 Jul 2015\",\"goodmand\",\"09 Jul 2015\",\"goodmand\"\n\"4\",\"1\",\"Mechatronics Maintenance Technician\",\"3\",\"3\",\"01 Aug 2014\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-mechatronics-maintenance-technician\",\"4.00\",\"4.20\",\"09 Jul 2015\",\"goodmand\",\"09 Jul 2015\",\"goodmand\"\n\"5\",\"1\",\"Installation Electrician/Maintenance Electrician\",\"19\",\"3\",\"01 Aug 2015\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-installation-electricianmaintenance-electrician\",\"4.00\",\"4.10\",\"16 Sep 2015\",\"fisherk\",\"16 Sep 2015\",\"fisherk\"\n\"6\",\"1\",\"Power Network Craftsperson\",\"8\",\"3\",\"01 Aug 2014\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-power-network-craftsperson\",\"4.00\",\"4.10\",\"09 Jul 2015\",\"goodmand\",\"09 Jul 2015\",\"goodmand\"\n\"7\",\"1\",\"Relationship Manager (Banking)\",\"9\",\"6\",\"01 Aug 2014\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-relationship-manager-banking\",\"15.00\",\"15.10\",\"09 Jul 2015\",\"goodmand\",\"09 Jul 2015\",\"goodmand\"\n\"8\",\"1\",\"Financial Services Administrator\",\"9\",\"3\",\"01 Aug 2014\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-financial-services-administrator\",\"15.00\",\"15.10\",\"09 Jul 2015\",\"goodmand\",\"09 Jul 2015\",\"goodmand\"\n\"9\",\"1\",\"Control /Technical Support Engineer\",\"3\",\"6\",\"01 Aug 2014\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-controltechnical-support-engineer\",\"4.00\",\"4.20\",\"09 Jul 2015\",\"goodmand\",\"06 Aug 2015\",\"vijaydek\"\n\"10\",\"1\",\"Electrical /Electronic Technical Support Engineer\",\"3\",\"6\",\"01 Aug 2014\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-electricalelectronic-technical-support-engineer\",\"4.00\",\"4.20\",\"09 Jul 2015\",\"goodmand\",\"06 Aug 2015\",\"vijaydek\"\n\"11\",\"1\",\"Manufacturing Engineer\",\"3\",\"6\",\"18 Mar 2015\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-manufacturing-engineer\",\"4.00\",\"4.20\",\"09 Jul 2015\",\"goodmand\",\"06 Aug 2015\",\"vijaydek\"\n\"12\",\"1\",\"Product Design and Development Engineer\",\"3\",\"6\",\"18 Mar 2015\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-product-design-and-development-engineer\",\"4.00\",\"4.20\",\"09 Jul 2015\",\"goodmand\",\"06 Aug 2015\",\"vijaydek\"\n\"13\",\"1\",\"Product Design and Development Technician\",\"3\",\"3\",\"18 Mar 2015\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-product-design-and-development-technician\",\"4.00\",\"4.20\",\"09 Jul 2015\",\"goodmand\",\"06 Aug 2015\",\"vijaydek\"\n\"14\",\"1\",\"Laboratory Technician\",\"4\",\"3\",\"01 Aug 2014\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-laboratory-technician\",\"4.00\",\"4.20\",\"09 Jul 2015\",\"goodmand\",\"06 Aug 2015\",\"vijaydek\"\n\"15\",\"1\",\"Science Manufacturing Technician\",\"4\",\"3\",\"01 Aug 2014\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-science-manufacturing-technician\",\"4.00\",\"4.20\",\"09 Jul 2015\",\"goodmand\",\"06 Aug 2015\",\"vijaydek\"\n\"16\",\"1\",\"Food and Drink Maintenance Engineer\",\"5\",\"3\",\"01 Aug 2014\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-food-and-drink-maintenance-engineer\",\"4.00\",\"4.10\",\"09 Jul 2015\",\"goodmand\",\"06 Aug 2015\",\"vijaydek\"\n\"17\",\"1\",\"Actuarial Technician\",\"1\",\"4\",\"26 Mar 2015\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-actuarial-technician\",\"15.00\",\"15.10\",\"09 Jul 2015\",\"goodmand\",\"09 Jul 2015\",\"goodmand\"\n\"18\",\"1\",\"Dental Technician\",\"6\",\"5\",\"26 Mar 2015\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-dental-technician\",\"1.00\",\"1.10\",\"09 Jul 2015\",\"goodmand\",\"09 Jul 2015\",\"goodmand\"\n\"19\",\"1\",\"Dental Laboratory Assistant\",\"6\",\"3\",\"26 Mar 2015\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-dental-laboratory-assistant\",\"1.00\",\"1.10\",\"09 Jul 2015\",\"goodmand\",\"09 Jul 2015\",\"goodmand\"\n\"20\",\"1\",\"Dental Practice Manager\",\"6\",\"4\",\"26 Mar 2015\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-dental-practice-manager\",\"1.00\",\"1.10\",\"09 Jul 2015\",\"goodmand\",\"09 Jul 2015\",\"goodmand\"\n\"21\",\"1\",\"Golf Greenkeeper\",\"10\",\"2\",\"26 Mar 2015\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-golf-greenkeeper\",\"3.00\",\"3.10\",\"09 Jul 2015\",\"goodmand\",\"09 Jul 2015\",\"goodmand\"\n\"22\",\"1\",\"Junior Journalist\",\"11\",\"3\",\"26 Mar 2015\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-junior-journalist\",\"9.00\",\"9.30\",\"09 Jul 2015\",\"goodmand\",\"09 Jul 2015\",\"goodmand\"\n\"23\",\"1\",\"Property Maintenance Operative\",\"12\",\"2\",\"26 Mar 2015\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-property-maintenance-operative\",\"5.00\",\"5.20\",\"09 Jul 2015\",\"goodmand\",\"09 Jul 2015\",\"goodmand\"\n\"24\",\"1\",\"Railway Engineering Design Technician\",\"13\",\"3\",\"26 Mar 2015\",\"\",\"\",\"https://www.gov.uk/government/publications/apprenticeship-standard-railway-engineering-design-technician\",\"4.00\",\"4.10\",\"09 Jul 2015\",\"goodmand\",\"09 Jul 2015\",\"goodmand\"";

        [Test]
        public void CreateFrameworksFromCsv()
        {
            CsvService csvService = new CsvService(null);
            var frameworks = csvService.ReadFrameworksFromStream(_testFrameworkCsvData);

            Assert.AreEqual(10, frameworks.Count);

            var firstDate = frameworks.FirstOrDefault()?.EffectiveFrom.ToString("yyyy-MM-dd");

            Assert.AreEqual("2013-10-07", firstDate);

            var tradeBusinessServices = frameworks.Single(m => m.PathwayName.Equals("Trade Business Services"));

            Assert.AreEqual(3, tradeBusinessServices.ProgType);
            Assert.AreEqual("2050-01-18", tradeBusinessServices.EffectiveTo.ToString("yyyy-MM-dd"));
        }

        [Test]
        public void CreateStandardsFromCsv()
        {
            var mockAngleService = new Mock<IAngleSharpService>();

            mockAngleService.Setup(m => m.GetLinks(It.IsAny<string>(), ".attachment-details h2 a", "Apprenticeship")).Returns(new List<string> { "/path/to/assessmentDocument.pdf" });
            mockAngleService.Setup(m => m.GetLinks(It.IsAny<string>(), ".attachment-details h2 a", "Assessment")).Returns(new List<string> { "/path/to/document.pdf" });

            CsvService csvService = new CsvService(mockAngleService.Object);
            var standards = csvService.ReadStandardsFromStream(_testStandardCsvData);

            Assert.AreEqual(24, standards.Count);
            Assert.AreEqual(4, standards.FirstOrDefault()?.NotionalEndLevel);

            var standard = standards.Single(m => m.Id == 15);

            Assert.AreEqual("Science Manufacturing Technician", standard.Title);
            Assert.AreEqual("m", standard.TypicalLength.Unit);
            Assert.AreEqual("https://www.gov.uk//path/to/document.pdf", standard.AssessmentPlanPdfUrl);
        }

        [Test]
        public void CreateStandardsFromCsvButUrlIsEmpty()
        {
            var mockAngleService = new Mock<IAngleSharpService>();

            mockAngleService.Setup(m => m.GetLinks(It.IsAny<string>(), ".attachment-details h2 a", "Apprenticeship")).Returns(new List<string>());
            mockAngleService.Setup(m => m.GetLinks(It.IsAny<string>(), ".attachment-details h2 a", "Assessment")).Returns(new List<string>());

            CsvService csvService = new CsvService(mockAngleService.Object);
            var standards = csvService.ReadStandardsFromStream(_testStandardCsvData);

            Assert.AreEqual(24, standards.Count);

            var standard = standards.Single(m => m.Id == 15);
            Assert.AreEqual(string.Empty, standard.AssessmentPlanPdfUrl);
        }
    }
}