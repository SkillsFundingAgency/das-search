using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Services
{
    public class CacheStorageServiceTests
    {
        private Mock<IMemoryCache> _mockMemoryCache;
        private Mock<IDistributedCache> _mockDistributedCache;

        [SetUp]
        public void Setup()
        {
            _sut = new CacheStorageService(_mockDistributedCache.Object, _mockMemoryCache.Object);

        }
    }
}
