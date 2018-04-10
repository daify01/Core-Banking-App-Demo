using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Routing;
using CBA.Data.NHibernate;
using CBAPractice.Core;
using CBAPractice.Data;

namespace WebService.Controllers
{
    public class CurrentconfigController : ApiController
    {
        // GET api/currentconfig
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/currentconfig/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/currentconfig
        public void Post([FromBody]string value)
        {
        }

        // PUT api/currentconfig/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/currentconfig/5
        public void Delete(int id)
        {
        }

        static readonly ICurrentConfigDb currentConfig = new CurrentConfigDb();
        

        public IList<CurrentConfig> GetAllCurrentConfigs()
        {
            return currentConfig.RetrieveAll();
        }

        
        public CurrentConfig GetByBranch(int id)
        {
            return currentConfig.RetrieveAll().Where(x => x.Branch.Id == id).Single();
        }
    }
}
