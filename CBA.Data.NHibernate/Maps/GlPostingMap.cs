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
    public class GlPostingMap : EntityMap<GlPosting>
    {
        public GlPostingMap()
        {
            References(x => x.GlAccountToDebit).Not.Nullable().LazyLoad(Laziness.False);
            References(x => x.GlAccountToCredit).Not.Nullable().LazyLoad(Laziness.False);
            Map(x => x.CreditNarration).Not.Nullable();
            Map(x => x.DebitNarration).Not.Nullable();
            Map(x => x.Amount).Not.Nullable();
            Map(x => x.TransactionDate).Nullable();
            Map(x => x.IsReversed).Nullable();
        }
    }
}
