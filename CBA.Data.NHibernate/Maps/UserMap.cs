using CBAPractice.Core;
using FluentNHibernate.Mapping;

namespace CBA.Data.Nhibernate.Maps
{
    public class UserMap : EntityMap<User>
    {
        public UserMap()
        {
            References(x => x.Branch).Not.Nullable().LazyLoad(Laziness.False);
            Map(x => x.Email).Unique().Not.Nullable();
            Map(x => x.FirstName).Not.Nullable();
            Map(x => x.LastName).Not.Nullable();
            Map(x => x.OtherNames);
            Map(x => x.Password).Not.Nullable();
            Map(x => x.PhoneNumber).Unique().Not.Nullable();
            Map(x => x.UserRole).Not.Nullable();
            Map(x => x.UserName).Not.Nullable();
            Map(x => x.FullName).Nullable();

        }
    }
}
