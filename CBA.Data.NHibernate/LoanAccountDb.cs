using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBA.Data.Nhibernate;
using CBAPractice.Core;
using CBAPractice.Data;
using NHibernate.Criterion;

namespace CBA.Data.NHibernate
{
   public class LoanAccountDb :  EntityDb<LoanAccount>, ILoanAccountDb
    {
       public IList<LoanAccount> PagedSearchByNameAndAcctNumber(string name, string lduration, string ptype, string AcctNumber, int start, int length, out int querytotal, out int total)
       {
           using (var session = GetSession())
           {
               var query = session.QueryOver<LoanAccount>(); //.Where(x => x.Name.IsInsensitiveLike(name));
               total = query.RowCount();
               if (!string.IsNullOrWhiteSpace(name))
               {
                   query.Where(x => x.AccountName.IsInsensitiveLike(name, MatchMode.Anywhere));
               }
               if (!string.IsNullOrWhiteSpace(AcctNumber))
               {
                   query.Where(x => x.AccountNumber.IsInsensitiveLike(AcctNumber, MatchMode.Anywhere));
               }
               if (!string.IsNullOrWhiteSpace(lduration))
               {
                   query.Where(x => x.LoanDuration.ToString().IsInsensitiveLike(lduration, MatchMode.Anywhere));
               }
               if (!string.IsNullOrWhiteSpace(ptype))
               {
                   query.Where(x => Enum.Parse(typeof(PaymentSchedule), ptype).ToString().IsInsensitiveLike(ptype, MatchMode.Anywhere));
               }
               querytotal = query.RowCount();
               query.Skip(start).Take(length);
               return query.List();
           }
       }

       public IList<LoanAccount> RetrieveByOverdueStatus()
       {
           return RetrieveAll().Where(x => x.LoanStatus == LoanStatus.Overdue).ToList();
       }

       public IList<LoanAccount> SearchbyLinkedAccountId(string CustomerAccountId)
       {
           return RetrieveAll().Where(x => x.LinkedAccount.Id.ToString() == CustomerAccountId).ToList();
       }
    }
}
