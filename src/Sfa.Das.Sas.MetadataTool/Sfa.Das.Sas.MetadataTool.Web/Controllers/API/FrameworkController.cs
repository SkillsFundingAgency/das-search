using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sfa.Das.Sas.ApplicationServices.Helpers;
using Sfa.Das.Sas.Core.Models;

namespace Sfa.Das.Sas.MetadataTool.Web.Controllers.API
{
    public class FrameworkController : ApiController
    {
        private readonly IMetaDataHelper _metaDataHelper;

        public FrameworkController(IMetaDataHelper metaDataHelper)
        {
            _metaDataHelper = metaDataHelper;
        }

        // GET: api/FrameworkApi
        public IEnumerable<FrameworkMetaData> Get()
        {
            return _metaDataHelper.GetAllFrameworksMetaData();
        }

        // GET: api/FrameworkApi/5
        public FrameworkMetaData Get(string id)
        {
            return _metaDataHelper.GetFrameworkMetaData(id);
        }
    }
}
