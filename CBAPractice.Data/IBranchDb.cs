using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;

namespace CBAPractice.Data
{
    public interface IBranchDb
    {
        int InsertData(Branch item);
        Branch RetrieveById(int id);
        IList<Branch> RetrieveAll();
        void UpdateData(Branch item);
        void DeleteData(Branch item);

        IList<Branch> PagedSearchByNameAndRCNo(string searchName, string searchCode, int start, int length, out int querytotal, out int total); 

    }
}
