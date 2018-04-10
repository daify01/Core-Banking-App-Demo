using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CBA.Data.Nhibernate.Maps;
using CBAPractice.Core;
using FluentNHibernate.Mapping;
using NHibernate.Mapping;

namespace CBA.Data.NHibernate.Maps
{
    public class GlAccountMap : EntityMap<GlAccount>
    {
        public GlAccountMap()
        {
            References(x => x.GlCategory).Not.Nullable().LazyLoad(Laziness.False);
            Map(x => x.GlAccountName).Not.Nullable();
            References(x => x.Branch).Not.Nullable().LazyLoad(Laziness.False);
            Map(x => x.GlAccountCodes).Unique().Not.Nullable();
            Map(x => x.Balance).Nullable();
        }
    }
}
