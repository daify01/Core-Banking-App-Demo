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
    public class TellerpostinglogicController : ApiController
    {
        // GET api/tellerpostinglogic
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/tellerpostinglogic/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/tellerpostinglogic
        public void Post([FromBody]string value)
        {
        }

        // PUT api/tellerpostinglogic/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/tellerpostinglogic/5
        public void Delete(int id)
        {
        }

        public bool GetGLAccountBalanceConfirmed(int glAccountid, double amount)
        {
            IGlAccountDb glAccountDb = new GlAccountDb();
            GlAccount glAccount = glAccountDb.RetrieveById(glAccountid);
            if (glAccount.Balance < amount)
            {
                return false;
            }
            else return true;
        }

        public bool GetCustomerSavingsAccountBalanceConfirmed(int customerAccountsid, double amount)
        {
            ISavingsConfigDb savingsConfigDb = new SavingsConfigDb();
            IList<SavingsConfig> savingsConfig = savingsConfigDb.RetrieveAll();
            ICustomerAccountsDb customerAccountsDb = new CustomerAccountsDb();
            CustomerAccounts customerAccounts = customerAccountsDb.RetrieveById(customerAccountsid);
            if ((customerAccounts.Balance < amount + savingsConfig[0].minimumBalance))
            {
                return false;
            }
            else return true;
        }

        public bool GetCustomerCurrentAccountBalanceConfirmed(int customerAccountsid,  double amount)
        {
            ICurrentConfigDb currentConfigDb = new CurrentConfigDb();
            IList<CurrentConfig> currentConfig = currentConfigDb.RetrieveAll();
            ICustomerAccountsDb customerAccountsDb = new CustomerAccountsDb();
            CustomerAccounts customerAccounts = customerAccountsDb.RetrieveById(customerAccountsid);
            if ((customerAccounts.Balance < amount + currentConfig[0].minimumBalance + customerAccounts.CoTCharge))
            {
                return false;
            }
            else return true;
        }
        public double GetGlAccountWithdrawal(int glAccountid, double amount)
        {
            IGlAccountDb glAccountDb = new GlAccountDb();
            GlAccount glAccount = glAccountDb.RetrieveById(glAccountid);
            //Credit account if its any of these 2 glAccount Types below:
            if (glAccount.GlAccountCodes.StartsWith("1") || glAccount.GlAccountCodes.StartsWith("5"))
            {
                if (glAccount.Balance >= amount)
                {
                    glAccount.Balance = glAccount.Balance - amount;

                }
            }
                //Credit account if its any of these 3 glAccount Types below:
            else if (glAccount.GlAccountCodes.StartsWith("2") || glAccount.GlAccountCodes.StartsWith("3") ||
                     glAccount.GlAccountCodes.StartsWith("4"))
            {
                    glAccount.Balance = glAccount.Balance + amount;
            }

            return glAccount.Balance;
        }

        public double GetCustomerAccountWithdrawal(int customerAccountsid, double amount)
        {
            ICustomerAccountsDb customerAccountsDb = new CustomerAccountsDb();
            CustomerAccounts customerAccounts = customerAccountsDb.RetrieveById(customerAccountsid);
            if ( customerAccounts.Balance >= amount)
            {
                customerAccounts.Balance = customerAccounts.Balance - amount;

            }
            return customerAccounts.Balance;
        }
    }
}
