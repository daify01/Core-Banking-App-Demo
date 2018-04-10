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
    public class SavingsConfigMap : EntityMap<SavingsConfig>
    {
        public SavingsConfigMap()
        {
            Map(x => x.creditInterestRate).Not.Nullable();
            Map(x => x.minimumBalance).Not.Nullable();
            References(x => x.InterestExpenseGlAccount).Not.Nullable().LazyLoad(Laziness.False);
            References(x => x.SavingsAccountGL).Not.Nullable().LazyLoad(Laziness.False);
            References(x => x.Branch).Not.Nullable().LazyLoad(Laziness.False);
        }
    }
}
