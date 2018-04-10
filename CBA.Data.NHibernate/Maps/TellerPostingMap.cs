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
   public class TellerPostingMap : EntityMap<TellerPosting>
    {
       public TellerPostingMap()
       {
           References(x => x.CustomerAccounts).Not.Nullable().LazyLoad(Laziness.False);
           References(x => x.GlAccount).Not.Nullable().LazyLoad(Laziness.False);
           Map(x => x.PostingType).Not.Nullable();
           Map(x => x.Amount).Not.Nullable();
           Map(x => x.Narration).Not.Nullable();
           Map(x => x.TransactionDate).Not.Nullable();
       }
    }
}
