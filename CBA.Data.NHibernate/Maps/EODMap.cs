using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBA.Data.Nhibernate.Maps;
using CBAPractice.Core;
using NHibernate.Mapping;

namespace CBA.Data.NHibernate.Maps
{
    public class EODMap : EntityMap<EOD>
    {
        public EODMap()
        {
            Map(x => x.IsClosed).Nullable();
            Map(x => x.FinancialDate).Nullable();
        }
    }
}
