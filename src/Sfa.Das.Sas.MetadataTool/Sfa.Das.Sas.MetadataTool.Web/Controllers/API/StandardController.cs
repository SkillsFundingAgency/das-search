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
    public class StandardController : ApiController
    {
        private readonly IMetaDataHelper _metaDataHelper;

        public StandardController(IMetaDataHelper metaDataHelper)
        {
            _metaDataHelper = metaDataHelper;
        }

        // GET: api/Standard
        public IEnumerable<StandardMetaData> Get()
        {
            return _metaDataHelper.GetAllStandardsMetaData();
        }

        // GET: api/Standard/5
        public StandardMetaData Get(string id)
        {
            return _metaDataHelper.GetStandardMetaData(id);
        }
    }
}
