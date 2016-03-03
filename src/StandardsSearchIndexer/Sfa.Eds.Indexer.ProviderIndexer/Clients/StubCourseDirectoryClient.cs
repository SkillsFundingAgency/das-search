namespace Sfa.Eds.Das.ProviderIndexer.Clients
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Sfa.Eds.Das.Indexer.Common.Models;
    using Sfa.Eds.Das.ProviderIndexer.Models;

    public class StubCourseDirectoryClient : ICourseDirectoryClient
    {
        public async Task<IEnumerable<Provider>> GetProviders()
        {
            return ListOfProviders();
        }

        private IEnumerable<Provider> ListOfProviders()
        {
            yield return
                new Provider
                    {
                        UkPrn = "10003347",
                        PostCode = "CV21 2BB",
                        ProviderName = "Intec Business Colleges",
                        VenueName = "INTEC BUSINESS COLLEGES",
                        Radius = 35,
                        Coordinate = new Coordinate { Lat = 52.3714464, Lon = -1.2669471 },
                        StandardsId = new List<int> { 25 }
                    };
            yield return
                new Provider
                    {
                        UkPrn = "10001309",
                        PostCode = "CV32 4JE",
                        ProviderName = "Coventry & Warwickshire Chamber Training (CWT)",
                        VenueName = "COVENTRY & WARWICKSHIRE CHAMBER TRAINING (CWT)",
                        Radius = 40,
                        Coordinate = new Coordinate { Lat = 52.290897, Lon = -1.528915 },
                        StandardsId = new List<int> { 25 }
                    };
            yield return
                new Provider
                    {
                        UkPrn = "10031241",
                        PostCode = "B4 7LR",
                        ProviderName = "Aspire Achieve Advance Limited",
                        VenueName = "3AAA BIRMINGHAM",
                        Radius = 30,
                        Coordinate = new Coordinate { Lat = 52.4819902, Lon = -1.8923181 },
                        StandardsId = new List<int> { 25 }
                    };
            yield return
                new Provider
                    {
                        UkPrn = "10005967",
                        PostCode = "B5 5SU",
                        ProviderName = "South & City College Birmingham",
                        VenueName = "Digbeth Campus",
                        Radius = 30,
                        Coordinate = new Coordinate { Lat = 52.4754573, Lon = -1.8857531 },
                        StandardsId = new List<int> { 12, 25 }
                    };
            yield return
                new Provider
                    {
                        UkPrn = "10007015",
                        PostCode = "DE24 8AJ",
                        ProviderName = "Training Services 2000 Ltd",
                        VenueName = "TRAINING SERVICES 2000 LTD",
                        Radius = 30,
                        Coordinate = new Coordinate { Lat = 52.9106629, Lon = -1.4467433 },
                        StandardsId = new List<int> { 25 }
                    };
            yield return
                new Provider
                    {
                        UkPrn = "10031241",
                        PostCode = "DE1 2JT",
                        ProviderName = "Aspire Archive Advance Limited",
                        VenueName = "3AAA DERBY",
                        Radius = 60,
                        Coordinate = new Coordinate { Lat = 52.918635, Lon = -1.4761639 },
                        StandardsId = new List<int> { 25 }
                    };
            yield return
                new Provider
                    {
                        UkPrn = "10031241",
                        PostCode = "WC1X 8QB",
                        ProviderName = "Aspire Achieve Advance Limited",
                        VenueName = "3AAA KINGS CROSS",
                        Radius = 30,
                        Coordinate = new Coordinate { Lat = 51.5292025, Lon = -0.1202702 },
                        StandardsId = new List<int> { 25 }
                    };
            yield return
                new Provider
                    {
                        UkPrn = "10012834",
                        PostCode = "W6 7AN",
                        ProviderName = "Skills Team Ltd",
                        VenueName = "EMPLOYERS WORK PLACE",
                        Radius = 30,
                        Coordinate = new Coordinate { Lat = 51.4938191, Lon = -0.2236763 },
                        StandardsId = new List<int> { 25 }
                    };
            yield return
                new Provider
                    {
                        UkPrn = "10005264",
                        PostCode = "NG10 1LL",
                        ProviderName = "Millbrook Management Services Limited",
                        VenueName = "PROSTART TRAINING",
                        Radius = 60,
                        Coordinate = new Coordinate { Lat = 52.8967801, Lon = -1.2682401 },
                        StandardsId = new List<int> { 25 }
                    };
            yield return
                new Provider
                    {
                        UkPrn = "10004355",
                        PostCode = "CV1 2JG",
                        ProviderName = "Midland Group Training Services Limited",
                        VenueName = "Midland Group Training Services Ltd",
                        Radius = 10,
                        Coordinate = new Coordinate { Lat = 52.4050479, Lon = -1.4966412 },
                        StandardsId = new List<int> { 12 }
                    };
            yield return
                new Provider
                    {
                        UkPrn = "10001919",
                        PostCode = "DE248JE",
                        ProviderName = "Derby College",
                        VenueName = "DERBY COLLEGE @ THE ROUNDHOUSE",
                        Radius = 30,
                        Coordinate = new Coordinate { Lat = 52.9159961, Lon = -1.4589891 },
                        StandardsId = new List<int> { 12 }
                    };
            yield return
                new Provider
                    {
                        UkPrn = "10005991",
                        PostCode = "NG7 2RU",
                        ProviderName = "Central College Nottingham",
                        VenueName = "Highfields",
                        Radius = 30,
                        Coordinate = new Coordinate { Lat = 52.9367136, Lon = -1.1869524 },
                        StandardsId = new List<int> { 12 }
                    };
            yield return
                new Provider
                    {
                        UkPrn = "10007924",
                        PostCode = "DY1 3AH",
                        ProviderName = "Dudley College of Technology",
                        VenueName = "Wolverhampton Street",
                        Radius = 40,
                        Coordinate = new Coordinate { Lat = 52.5113022, Lon = -2.090677 },
                        StandardsId = new List<int> { 12 }
                    };
            yield return
                new Provider
                    {
                        UkPrn = "10004355",
                        PostCode = "B98 8LY",
                        ProviderName = "Midland Group Training Services Limited",
                        VenueName = "MIDLAND GROUP TRAINING SERVICES LIMITED",
                        Radius = 30,
                        Coordinate = new Coordinate { Lat = 52.3063609, Lon = -1.9297031 },
                        StandardsId = new List<int> { 12 }
                    };
            yield return
                new Provider
                    {
                        UkPrn = "10005673",
                        PostCode = "B70 0AE",
                        ProviderName = "Sandwell Training Association Limited",
                        VenueName = "PHOENIX STREET",
                        Radius = 40,
                        Coordinate = new Coordinate { Lat = 52.5257464, Lon = -2.0192208 },
                        StandardsId = new List<int> { 12 }
                    };
        }
    }
}