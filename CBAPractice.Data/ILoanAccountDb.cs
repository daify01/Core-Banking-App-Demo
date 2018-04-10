using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;

namespace CBAPractice.Data
{
   public interface ILoanAccountDb
    {
        int InsertData(LoanAccount item);
        LoanAccount RetrieveById(int id);
        IList<LoanAccount> RetrieveAll();
        void UpdateData(LoanAccount item);

        void DeleteData(LoanAccount item);

        IList<LoanAccount> PagedSearchByNameAndAcctNumber(string amount, string duration, string ptype, string Duration, int start, int length,
           out int querytotal, out int total);

       IList<LoanAccount> RetrieveByOverdueStatus();
       IList<LoanAccount> SearchbyLinkedAccountId(string CustomerAccountId);
    }
}
