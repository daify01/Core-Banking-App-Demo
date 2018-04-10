using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;

namespace CBAPractice.Data
{
    public interface IUserDb
    {
        int InsertData( User item);
        User RetrieveById(int id);
        IList<User> RetrieveAll();
        void UpdateData(User item);

        void DeleteData(User item);

        IList<User> PagedSearchByNameAndBranch(string searchfName, string searchlName, string searchbranch, int start, int length, out int querytotal, out int total);
        User RetrievebyUserName(string UserName);
        User RetrievebyEmail(string Email);
        User RetrievebyPassword(string Password);
        IList<User> RetrievebyAdminRole();
    }
}
