using CBAPractice.Core;
using FluentNHibernate.Mapping;

namespace CBA.Data.Nhibernate.Maps
{
    public class CustomerMap : EntityMap<Customer>
    {
        public CustomerMap()
        {
            
            Map(x => x.LastName).Not.Nullable();
            Map(x => x.FirstName).Not.Nullable();
            Map(x => x.OtherNames);
            Map(x => x.Address).Not.Nullable();
            Map(x => x.PhoneNumber).Unique().Not.Nullable();
            Map(x => x.Email).Unique().Not.Nullable();
            Map(x => x.Gender).Not.Nullable();
          
        }

    }
}
