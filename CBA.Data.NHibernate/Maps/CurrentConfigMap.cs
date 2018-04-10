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
    public class CurrentConfigMap : EntityMap<CurrentConfig>
    {
        public CurrentConfigMap()
        {
            Map(x => x.creditInterestRate).Not.Nullable();
            Map(x => x.minimumBalance).Not.Nullable();
            Map(x => x.coT).Not.Nullable();
            References(x => x.coTIncomeGl).Not.Nullable().LazyLoad(Laziness.False);
            References(x => x.currentAccountGL).Not.Nullable().LazyLoad(Laziness.False);
            References(x => x.InterestExpenseGlAccount).Not.Nullable().LazyLoad(Laziness.False);
            References(x => x.Branch).Not.Nullable().LazyLoad(Laziness.False);

        }
    }
}
