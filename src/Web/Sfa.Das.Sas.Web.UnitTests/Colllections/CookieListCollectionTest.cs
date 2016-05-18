using System;
using System.Web;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Web.Collections;
using Sfa.Das.Sas.Web.Factories;

namespace Sfa.Das.Sas.Web.UnitTests.Colllections
{
    using Sfa.Das.Sas.ApplicationServices.Settings;

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
                Value = "1|2|3|4"
            });

            // Act
            var items = _sut.GetAllItems(ListName);

            // Assert
            Assert.IsNotNull(items);
            Assert.IsNotEmpty(items);
            Assert.AreEqual(new[] { 1, 2, 3, 4 }, items);
        }

        [Test]
        public void ShouldAddItemToList()
        {
            // Assign
            const int item = 10;

            // Act
            _sut.AddItem(ListName, item);

            // Assert
            Assert.IsNotNull(_responseCookieCollection[ListName]);
            Assert.AreEqual(item.ToString(), _responseCookieCollection[ListName].Value);
        }

        [Test]
        public void ShouldRemoveItemFromList()
        {
            // Assign
            _requestCookieCollection.Add(new HttpCookie(ListName)
            {
                Value = "1|2|3|4"
            });

            // Act
            _sut.RemoveItem(ListName, 3);

            // Assert
            Assert.IsNotNull(_responseCookieCollection[ListName]);
            Assert.AreEqual("1|2|4", _responseCookieCollection[ListName].Value);
        }

        [Test]
        public void ShouldRemoveListIfEmpty()
        {
            // Assign
            _requestCookieCollection.Add(new HttpCookie(ListName)
            {
                Value = "1"
            });

            // Act
            _sut.RemoveItem(ListName, 1);

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

        [Test]
        public void ShouldSortItems()
        {
            // Assign
            _requestCookieCollection.Add(new HttpCookie(ListName)
            {
                Value = "1|2|4"
            });

            // Act
            _sut.AddItem(ListName, 3);

            // Assert
            Assert.IsNotNull(_responseCookieCollection[ListName]);
            Assert.AreEqual("1|2|3|4", _responseCookieCollection[ListName].Value);
        }
    }
}
