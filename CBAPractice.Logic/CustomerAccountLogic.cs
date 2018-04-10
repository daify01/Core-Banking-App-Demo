using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using CBAPractice.Core;
using CBAPractice.Data;

namespace CBAPractice.Logic
{
    public class CustomerAccountLogic
    {
        
        public bool DuplicateAccountCheck( string name, string accounttype,
            string branch)
        {
            IList<CustomerAccounts> customerAccounts = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>().RetrieveAll();
            foreach (var custAcct in customerAccounts)
            {
                if (name == custAcct.AccountName &&
                    accounttype == custAcct.AccountType.ToString() &&
                    branch == custAcct.Branch.BranchName)
                {
                    return true;
                }
                else return false;
            }
            return true;
        }
    }
}
