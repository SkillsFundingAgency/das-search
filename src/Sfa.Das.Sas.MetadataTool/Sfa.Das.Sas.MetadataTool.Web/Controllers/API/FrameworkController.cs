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
        public FrameworkMetaData Get(int id)
        {
            return _metaDataHelper.GetFrameworkMetaData(id);
        }

        // POST: api/FrameworkApi
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/FrameworkApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/FrameworkApi/5
        public void Delete(int id)
        {
        }
    }
}
