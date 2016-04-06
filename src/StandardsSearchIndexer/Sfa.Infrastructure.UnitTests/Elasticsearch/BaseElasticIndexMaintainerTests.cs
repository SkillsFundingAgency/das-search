using System;
using Elasticsearch.Net;
using Moq;
using Nest;
using NUnit.Framework;
using Sfa.Infrastructure.Elasticsearch;

namespace Sfa.Infrastructure.UnitTests.Elasticsearch
{
    public class BaseElasticIndexMaintainerTests
    {
        protected Mock<IElasticsearchClientFactory> _mockElasticClientFactory;
        protected Mock<IElasticClient> _mockElasticClient;

        [SetUp]
        public virtual void Setup()
        {
            _mockElasticClient = new Mock<IElasticClient>();
            _mockElasticClientFactory = new Mock<IElasticsearchClientFactory>();
            _mockElasticClientFactory.Setup(x => x.GetElasticClient()).Returns(_mockElasticClient.Object);
        }

        internal class StubResponse : ICreateIndexResponse
        {
            private readonly ServerError _serverError;

            public StubResponse(int statusCode = 200)
            {
                _serverError = new ServerError();
                _serverError.Status = statusCode;
            }

            public bool Acknowledged
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public IApiCallDetails ApiCall
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public IApiCallDetails CallDetails
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }

            public string DebugInformation
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public bool IsValid
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public Exception OriginalException
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public ServerError ServerError
            {
                get
                {
                    return _serverError;
                }
            }
        }
    }
}
