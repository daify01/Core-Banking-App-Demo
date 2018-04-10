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

    public class LoanConfigMap : EntityMap<LoanConfig>
    {
        public LoanConfigMap()
        {
            References(x => x.InterestIncomeGlAccount).Not.Nullable().LazyLoad(Laziness.False);
            References(x => x.LoanPrincipalGlAccount).Nullable().LazyLoad(Laziness.False);
            Map(x => x.debitInterestRate).Not.Nullable();
        }
    }
}
