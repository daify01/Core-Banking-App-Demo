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
    public class GlCategoryMap : EntityMap<GlCategory>
    {
        public GlCategoryMap()
        {
            Map(x => x.GlCategoryName).Not.Nullable();
            Map(x => x.MainAccountCategory).Not.Nullable();
            Map(x => x.Description).Not.Nullable();
        }
    }
}
