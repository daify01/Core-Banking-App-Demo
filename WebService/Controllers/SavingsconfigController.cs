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
    public class SavingsconfigController : ApiController
    {
        // GET api/savingsconfig
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/savingsconfig/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/savingsconfig
        public void Post([FromBody]string value)
        {
        }

        // PUT api/savingsconfig/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/savingsconfig/5
        public void Delete(int id)
        {
        }

        static readonly ISavingsConfigDb SavingsConfig = new SavingsConfigDb();

        public IList<SavingsConfig> GetAllSavingsConfigs()
        {
            return SavingsConfig.RetrieveAll();
        }

        public SavingsConfig GetByBranch(int id)
        {
            return SavingsConfig.RetrieveAll().Where(x => x.Branch.Id == id).Single();
        }
    }
}
