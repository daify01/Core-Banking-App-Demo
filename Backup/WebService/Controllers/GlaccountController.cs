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
    public class GlaccountController : ApiController
    {
        // GET api/glaccount
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/glaccount/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/glaccount
        public void Post([FromBody]string value)
        {
        }

        // PUT api/glaccount/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/glaccount/5
        public void Delete(int id)
        {
        }

        static readonly IGlAccountDb GlAccount = new GlAccountDb();
        public GlAccount GetByAccountCode(string Accountcode)
        {
                return GlAccount.RetrieveAll().Where(x => x.GlAccountCodes == Accountcode).Single();
        }

        public GlAccount GetByAccountname(string accountname)
        {
                return GlAccount.RetrieveAll().Where(x => x.GlAccountName == accountname).Single();
        }
        public void PutGlAccount(int glaccountsid, GlAccount glAccountstoUpdate)
        {
            //GlAccount.UpdateData(glAccountstoUpdate);
            IGlAccountDb glAccountDb = new GlAccountDb();
            glAccountDb.RetrieveById(glaccountsid);
            glAccountDb.UpdateData(glAccountstoUpdate);
        }
    }
}
