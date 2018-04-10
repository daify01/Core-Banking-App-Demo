using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;

namespace CBAPractice.Data
{
    public interface IGlAccountDb
    {
        int InsertData(GlAccount item);
        GlAccount RetrieveById(int id);
        IList<GlAccount> RetrieveAll();
        void UpdateData(GlAccount item);
        void DeleteData(GlAccount item);
        IList<GlAccount> PagedSearchByGLNameAndCategory(string searchGlName, string glcode, string glbranch, string searchCategory, int start,
            int length, out int querytotal, out int total);
        GlAccount RetrieveByAccountName(string AccountName);
        GlAccount RetrieveByAccountCode(string Accountcode);
        GlAccount RetrieveByGLCategoryId(int id);
        IList<GlAccount> RetrieveOtherAssettypes();
        IList<GlAccount> RetrieveExpenseGLtypes();
        IList<GlAccount> RetrieveLiablilityGltypes();
        IList<GlAccount> RetrieveIncomeGltypes();
        IList<GlAccount> RetrieveCashAssettypes();
        GlAccount RetrieveByBranch(int id);
    }
}
