using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBA.Data.Nhibernate;
using CBAPractice.Core;
using CBAPractice.Data;

namespace CBA.Data.NHibernate
{
    public class LoanConfigDb : EntityDb<LoanConfig>, ILoanConfigDb
    {
        public LoanConfig RetrieveByInterestIncomeAcctId(int id)
        {
            using (var session = GetSession())
            {
                return session.QueryOver<LoanConfig>().Where(x => x.InterestIncomeGlAccount.Id == id).SingleOrDefault();
            }
        }
    }
}
