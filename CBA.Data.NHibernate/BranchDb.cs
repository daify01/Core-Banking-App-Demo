using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;
using CBAPractice.Data;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace CBA.Data.Nhibernate
{
   public class BranchDb : EntityDb<Branch>, IBranchDb
    {
        
        public IList<Branch> PagedSearchByNameAndRCNo(string name, string RCNo, int start, int length, out int querytotal, out int total)
        {
            using (var session = GetSession())
            {
                var query = session.QueryOver<Branch>(); //.Where(x => x.Name.IsInsensitiveLike(name));
                total = query.RowCount();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    query.Where(x => x.BranchName.IsInsensitiveLike(name, MatchMode.Anywhere));
                }
                if (!string.IsNullOrWhiteSpace(RCNo))
                {
                    query.Where(x => x.RcNumber.IsInsensitiveLike(RCNo, MatchMode.Anywhere));
                }
                querytotal = query.RowCount();
                query.Skip(start).Take(length);
                return query.List();
            }
        }
    }
}
