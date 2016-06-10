using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentAssertions;
using Moq;
using NUnit.Framework;

using Sfa.Das.Sas.Web.Collections;
using Sfa.Das.Sas.Web.Models;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Colllections
{
    using ApplicationServices.Settings;
    using Sas.Web.Factories.Interfaces;

    [TestFixture]
    public class CookieListCollectionTest
    {
        private const string ListName = "test_list";

        private Mock<ICookieSettings> _mockConfigurationSettings;
        private Mock<IHttpCookieFactory> _mockCookieFactory;
        private HttpCookieCollection _requestCookieCollection;
        private HttpCookieCollection _responseCookieCollection;

        private CookieListCollection _sut;

        [SetUp]
        public void Init()
        {
            _mockConfigurationSettings = new Mock<ICookieSettings>();
            _mockCookieFactory = new Mock<IHttpCookieFactory>();
            _requestCookieCollection = new HttpCookieCollection();
            _responseCookieCollection = new HttpCookieCollection();

            _mockCookieFactory.Setup(x => x.GetRequestCookies()).Returns(_requestCookieCollection);
            _mockCookieFactory.Setup(x => x.GetResponseCookies()).Returns(_responseCookieCollection);

            _sut = new CookieListCollection(_mockConfigurationSettings.Object, _mockCookieFactory.Object);
        }

        [Test]
        public void ShouldGetAllItemsFromList()
        {
            // Assign
            _requestCookieCollection.Add(new HttpCookie(ListName)
            {
                Value = "1=&2=&3=&4="
            });

            var expectedResult = new List<ShortlistedApprenticeship>
            {
                new ShortlistedApprenticeship { ApprenticeshipId = 1, ProvidersIdAndLocation = new List<ShortlistedProvider>() },
                new ShortlistedApprenticeship { ApprenticeshipId = 2, ProvidersIdAndLocation = new List<ShortlistedProvider>() },
                new ShortlistedApprenticeship { ApprenticeshipId = 3, ProvidersIdAndLocation = new List<ShortlistedProvider>() },
                new ShortlistedApprenticeship { ApprenticeshipId = 4, ProvidersIdAndLocation = new List<ShortlistedProvider>() }
            };

            // Act
            var items = _sut.GetAllItems(ListName);

            // Assert
            Assert.IsNotNull(items);
            Assert.IsNotEmpty(items);

            foreach (var result in items.Zip(expectedResult, Tuple.Create))
            {
                Assert.AreEqual(result.Item1.ApprenticeshipId, result.Item2.ApprenticeshipId);
            }
        }

        [Test]
        public void ShouldAddItemToList()
        {
            // Assign
            ShortlistedApprenticeship item = new ShortlistedApprenticeship { ApprenticeshipId = 10};

            // Act
            _sut.AddItem(ListName, item);

            // Assert
            _responseCookieCollection[ListName].Should().NotBeNull();
            _responseCookieCollection[ListName].Value.Should().Be(string.Concat(item.ApprenticeshipId, "="));
        }

        [Test]
        public void ShouldRemoveApprenticeshipFromList()
        {
            // Assign
            _requestCookieCollection.Add(new HttpCookie(ListName)
            {
                Value = "1=&2=&3=&4="
            });

            // Act
            _sut.RemoveApprenticeship(ListName, 3);

            // Assert
            Assert.IsNotNull(_responseCookieCollection[ListName]);
            Assert.AreEqual("1=&2=&4=", _responseCookieCollection[ListName].Value);
        }

        [Test]
        public void ShouldRemoveListIfEmpty()
        {
            // Assign
            _requestCookieCollection.Add(new HttpCookie(ListName)
            {
                Value = "1="
            });

            // Act
            _sut.RemoveApprenticeship(ListName, 1);

            // Assert
            Assert.IsNotNull(_responseCookieCollection[ListName]);
            Assert.IsTrue(_responseCookieCollection[ListName].Expires < DateTime.Now);
        }

        [Test]
        public void ShouldRemoveList()
        {
            // Assign
            _requestCookieCollection.Add(new HttpCookie(ListName)
            {
                Value = "1|2|3|4"
            });

            // Act
            _sut.RemoveList(ListName);

            // Assert
            Assert.IsNotNull(_responseCookieCollection[ListName]);
            Assert.IsTrue(_responseCookieCollection[ListName].Expires < DateTime.Now);
        }
    }
}
