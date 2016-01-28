namespace FindProviders.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NUnit.Framework;
    using NUnit.Framework.Compatibility;

    using Assert = NUnit.Framework.Assert;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void FindCitiesWithRadius()
        {
            var providerCoventry = new Provider() { Name = "Coventry", Location = new Location() { Lat = 52.406822, Long = -1.519692999999961 }, Radius = 30 };
            var rugby = new Location()
                                 {
                                     Lat = 52.370878,
                                     Long = -1.2650320000000193
            };

            var warwik = new Location()
            {
                Lat = 52.28231599999999,
                Long = -1.5849269999999933
            };
            var leicester = new Location(){Lat = 52.6368778,Long = -1.1397591999999577};

            var london = new Location() { Lat = 51.5073509, Long = -0.12775829999998223 };

            var calculator = new Calculator();

            // Rugby and Warwik is closer than 30 km to Coventry
            var rugby_yes = calculator.IsWithin(rugby, providerCoventry.Location, providerCoventry.Radius);
            var warwik_yes = calculator.IsWithin(warwik, providerCoventry.Location, providerCoventry.Radius);

            // Leicester is further away than 30 km
            var leicester_no = calculator.IsWithin(leicester, providerCoventry.Location, providerCoventry.Radius);

            Assert.IsTrue(rugby_yes);
            Assert.IsTrue(warwik_yes);
            Assert.IsFalse(leicester_no);


        }

        [TestCase]
        public void TestWith1millionCalculations()
        {
            var providerCoventry = new Provider() { Name = "Coventry", Location = new Location() { Lat = 52.406822, Long = -1.519692999999961 }, Radius = 30 };
            var rugby = new Location() { Lat = 52.370878,Long = -1.2650320000000193 };

            var calculator = new Calculator();
            var timer = Stopwatch.StartNew();
            for (int i = 0; i < 1000000; i++)
            {
                calculator.IsWithin(rugby, providerCoventry.Location, providerCoventry.Radius);
            }
            timer.Stop();
            var x = timer.Elapsed;
            var b = x.Milliseconds < 1000;
            System.Console.WriteLine(x.Milliseconds);
            Assert.IsTrue(b);

            // Takes les than 400 illiseconds
        }
    }
}
