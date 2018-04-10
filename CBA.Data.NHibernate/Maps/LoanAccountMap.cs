using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBA.Data.Nhibernate.Maps;
using CBAPractice.Core;
using FluentNHibernate.Mapping;

namespace CBA.Data.NHibernate.Maps
{
    public class LoanAccountMap : EntityMap<LoanAccount>
    {
        public LoanAccountMap()
        {
            References(x => x.LinkedAccount).Not.Nullable().LazyLoad(Laziness.False);
            References(x => x.LoanConfig).Not.Nullable().LazyLoad(Laziness.False);
            Map(x => x.LoanAmount).Not.Nullable();
            Map(x => x.LoanDuration).Not.Nullable();
            Map(x => x.LoanInterest).Not.Nullable();
            Map(x => x.Balance).Not.Nullable();
            Map(x => x.AccountNumber).Not.Nullable();
            Map(x => x.AccountName).Not.Nullable();
            Map(x => x.LoanStartDate).Not.Nullable();
            Map(x => x.LoanDueDate).Not.Nullable();
            Map(x => x.PaymentSchedule).Nullable();
            Map(x => x.NumberOfDays).Nullable();
            Map(x => x.LoanStatus).Nullable();
            Map(x => x.TransactionDate).Nullable();
        }
    }
}
