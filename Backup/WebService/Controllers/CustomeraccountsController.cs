using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CBAPractice.Core;
using CBAPractice.Data;
using CBA.Data.NHibernate;

namespace WebService.Controllers
{
    public class CustomeraccountsController : ApiController
    {

        // GET api/customeraccounts
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/customeraccounts/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/customeraccounts
        public void Post([FromBody]string value)
        {
        }

        // PUT api/customeraccounts/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/customeraccounts/5
        public void Delete(int id)
        {
        }

     
        static readonly ICustomerAccountsDb customerAccounts = new CustomerAccountsDb();

        public IEnumerable<CustomerAccounts> GetAllCustomerAccounts()
        {
            return customerAccounts.RetrieveAll();
        }

        public CustomerAccounts GetByAccountNumber(string accountnumber)
        {
            
            return customerAccounts.RetrieveAll().Where(x => x.AccountNumber == accountnumber).Single();
        }

        public CustomerAccounts GetId(int id)
        {
            CustomerAccounts item = customerAccounts.RetrieveById(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }

        public void PutCustomerAccounts(int customeraccountsid, CustomerAccounts customerAccountstoUpdate)
        {
                //customerAccounts.UpdateData(customerAccountstoUpdate);
            ICustomerAccountsDb customerAccountsDb = new CustomerAccountsDb();
            customerAccountsDb.RetrieveById(customeraccountsid);
            customerAccountsDb.UpdateData(customerAccountstoUpdate);
        }
    }
}
