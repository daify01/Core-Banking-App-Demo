using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;

namespace CBAPractice.Data
{
    public interface ITellerPostingDb
    {
        int InsertData(TellerPosting item);
        TellerPosting RetrieveById(int id);
        IList<TellerPosting> RetrieveAll();
        void UpdateData(TellerPosting item);

        void DeleteData(TellerPosting item);

        IList<TellerPosting> PagedSearchByAccountNameAndAccountNumber(string name, string tillacctname, string acctcode, DateTime searchdate, int start,
            int length, out int querytotal, out int total);



        //IList<User> PagedSearchByNameAndBranch(string searchName, int start, int length, out int total);
    }
}
