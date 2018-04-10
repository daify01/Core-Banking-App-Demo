using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;

namespace CBAPractice.Data
{
   public interface ICustomerAccountsDb
    {
        int InsertData(CustomerAccounts item);
        CustomerAccounts RetrieveById(int id);
        IList<CustomerAccounts> RetrieveAll();
        void UpdateData(CustomerAccounts item);

        void DeleteData(CustomerAccounts item);

        //CustomerAccounts RetrieveByAccountNumber(string AccountNumber);

        CustomerAccounts SearchbyAccountNumber(string Accountnumber);

        IList<CustomerAccounts> PagedSearchByNameAndAccountNumber(string name, string type, string branch, string searchAccountNumber, int start, int length, out int querytotal, out int total);
        IList<Customer> PagedSearchByFirstNameAndLastName(string name, string Addr, int start, int length, out int querytotal, out int total);

      
    }
}
