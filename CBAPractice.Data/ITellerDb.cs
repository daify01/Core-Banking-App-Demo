using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;

namespace CBAPractice.Data
{
    public interface ITellerDb 
    {
        int InsertData(Teller item);
        Teller RetrieveById(int id);
        IList<Teller> RetrieveAll();
        void UpdateData(Teller item);

        void DeleteData(Teller item);

        Teller RetrievebyUserName(User user);

        Teller RetrievebyUserId(int userId);

        Teller RetrievebyTillId(int tillId);

        //Teller SearchbyUserName(string UserName);
        IList<Teller> PagedSearchByName(string searchUser, string searchGLAccountName, string searchLName, int start, int length,
           out int querytotal, out int total);

        //IList<User> PagedSearchByNameAndBranch(string searchName, int start, int length, out int total);
    }
}
