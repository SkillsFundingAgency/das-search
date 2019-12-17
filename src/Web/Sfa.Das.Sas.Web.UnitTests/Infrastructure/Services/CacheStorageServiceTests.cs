using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Services
{
    public class CacheStorageServiceTests
    {
        private Mock<IMemoryCache> _mockMemoryCache;
        private Mock<IDistributedCache> _mockDistributedCache;
        private CacheStorageService _sut;
        private MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());

        [SetUp]
        public void Setup()
        {
            _mockMemoryCache = new Mock<IMemoryCache>();
            _mockDistributedCache = new Mock<IDistributedCache>();

            _sut = new CacheStorageService(_mockDistributedCache.Object, memoryCache);
        }

        [Test]
        public void GetsDataFromInMemoryCache_AndDoesntCheckDistributedCache()
        {
            memoryCache.Set("123456", "1234567889");

            var response = _sut.RetrieveFromCache<ApprenticeshipProviderDetailResponse>("123456");

            _mockDistributedCache.Verify(s => s.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public void GetsFromDistributedCacheIfKeyDoesntExistInMemoryCache()
        {
            _sut = new CacheStorageService(_mockDistributedCache.Object, memoryCache);

            var response = _sut.RetrieveFromCache<ApprenticeshipProviderDetailResponse>("123456");

            _mockDistributedCache.Verify(s => s.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
