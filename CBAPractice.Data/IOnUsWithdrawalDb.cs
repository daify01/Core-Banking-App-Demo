using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;

namespace CBAPractice.Data
{
    public interface IOnUsWithdrawalDb
    {
        int InsertData(OnUsWithdrawal item);
        OnUsWithdrawal RetrieveById(int id);
        IList<OnUsWithdrawal> RetrieveAll();
        void UpdateData(OnUsWithdrawal item);
        void DeleteData(OnUsWithdrawal item);

        IList<OnUsWithdrawal> PagedSearchByName_IDAndLocation(string searchName, string id, string location, int start, int length, out int querytotal, out int total); 
    }
}
