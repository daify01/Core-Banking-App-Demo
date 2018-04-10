
using CBAPractice.Core;
using FluentNHibernate.Mapping;

namespace CBA.Data.Nhibernate.Maps
{
    public class BranchMap : EntityMap<Branch>
    {
        public BranchMap()
        {

            Map(x => x.BranchName).Unique().Not.Nullable();
            Map(x => x.Address);
            Map(x => x.RcNumber).Unique().Not.Nullable();
        }
    }
}
