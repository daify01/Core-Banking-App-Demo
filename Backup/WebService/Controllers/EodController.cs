using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CBA.Data.NHibernate;
using CBAPractice.Core;
using CBAPractice.Data;

namespace WebService.Controllers
{
    public class EodController : ApiController
    {
        // GET api/eod
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/eod/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/eod
        public void Post([FromBody]string value)
        {
        }

        // PUT api/eod/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/eod/5
        public void Delete(int id)
        {
        }

        static readonly IEODDb Eod = new EODDb();
        public IList<EOD> GetAllEods()
        {
            return Eod.RetrieveAll();
        }
    }
}
