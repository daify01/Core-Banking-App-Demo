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
    public class CustomerAccountsMap : EntityMap<CustomerAccounts>
    {
        public CustomerAccountsMap()
        {
            References(x => x.Customer).Not.Nullable().LazyLoad(Laziness.False);
            References(x => x.Branch).Not.Nullable().LazyLoad(Laziness.False);
            Map(x => x.AccountName).Not.Nullable();
            Map(x => x.AccountNumber).Unique().Not.Nullable();
            Map(x => x.AccountType).Not.Nullable();
            Map(x => x.Balance).Nullable();
            Map(x => x.CoTCharge).Nullable();
            Map(x => x.IsClosed).Nullable();
        }
    }
}
