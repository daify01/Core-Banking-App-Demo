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
    public class TellerMap : EntityMap<Teller>
    {
        public TellerMap()
        {
            References(x => x.GlAccount).Unique().Not.Nullable().LazyLoad(Laziness.False);
            References(x => x.User).Not.Nullable().LazyLoad(Laziness.False);
        }
    }
}
