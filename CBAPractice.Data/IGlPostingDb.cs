using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;

namespace CBAPractice.Data
{
    public interface IGlPostingDb
    {
        int InsertData(GlPosting item);
        GlPosting RetrieveById(int id);
        IList<GlPosting> RetrieveAll();
        void UpdateData(GlPosting item);
        void DeleteData(GlPosting item);

        IList<GlPosting> RetrievePostingToDate(DateTime SelectedTransactionDate);

       
        IList<GlPosting> PagedSearchByGLNameAndCode(string name, string acctcode, int start,
            int length, out int querytotal, out int total);

        //IList<User> PagedSearchByNameAndBranch(string searchName, int start, int length, out int total);
    }
}
