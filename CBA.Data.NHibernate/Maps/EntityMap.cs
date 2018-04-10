using CBAPractice.Core;
using FluentNHibernate.Mapping;

namespace CBA.Data.Nhibernate.Maps
{
    public class EntityMap<T> : ClassMap<T> where T : Entity
    {
        public EntityMap()
        {
            Id(x => x.Id).Unique().Not.Nullable();
            Map(x => x.DateAdded).Not.Nullable();
            Map(x => x.DateUpdated).Not.Nullable();
        }
    }
}
