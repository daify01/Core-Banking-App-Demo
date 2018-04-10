using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;

namespace CBAPractice.Data
{
    public interface ICustomerDb
    {
        int InsertData(Customer item);
        Customer RetrieveById(int id);
        IList<Customer> RetrieveAll();
        void UpdateData(Customer item);

        void DeleteData(Customer item);

        IList<Customer> PagedSearchByNameAndAddress(string fname, string lname, string gender, string searchAddress, int start, int length, out int querytotal, out int total);
    }
}
