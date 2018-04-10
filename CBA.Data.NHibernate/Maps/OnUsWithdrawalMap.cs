using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBA.Data.Nhibernate.Maps;
using CBAPractice.Core;

namespace CBA.Data.NHibernate.Maps
{
    public class OnUsWithdrawalMap : EntityMap<OnUsWithdrawal>
    {
        public OnUsWithdrawalMap()
        {
            Map(x => x.Name);
            Map(x => x.TerminalID).Not.Nullable();
            Map(x => x.Location);
        }
    }
}
