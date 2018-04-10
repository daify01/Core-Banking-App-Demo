using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;

namespace CBAPractice.Data
{
    public interface ILoanConfigDb
    {
        int InsertData(LoanConfig item);
        LoanConfig RetrieveById(int id);
        IList<LoanConfig> RetrieveAll();
        void UpdateData(LoanConfig item);

        void DeleteData(LoanConfig item);
        LoanConfig RetrieveByInterestIncomeAcctId(int id);
    }
}
