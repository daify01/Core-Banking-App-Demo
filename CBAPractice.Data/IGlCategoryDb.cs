using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;

namespace CBAPractice.Data
{
   public interface IGlCategoryDb
    {
        int InsertData(GlCategory item);
        GlCategory RetrieveById(int id);
        IList<GlCategory> RetrieveAll();
        void UpdateData(GlCategory item);

        void DeleteData(GlCategory item);

        IList<GlCategory> PagedSearchByNameAndGlAcctCategory(string name, string glcategory, int start, int length, out int querytotal, out int total);
    }
}
