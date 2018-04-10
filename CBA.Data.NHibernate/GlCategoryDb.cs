using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBA.Data.Nhibernate;
using CBAPractice.Core;
using CBAPractice.Data;
using NHibernate.Criterion;

namespace CBA.Data.NHibernate
{
    public class GlCategoryDb : EntityDb<GlCategory>, IGlCategoryDb
    {
        public IList<GlCategory> PagedSearchByNameAndGlAcctCategory(string name, string glcategory, int start,
            int length, out int querytotal, out int total)
        {
            using (var session = GetSession())
            {
                var query = session.QueryOver<GlCategory>(); //.Where(x => x.Name.IsInsensitiveLike(name));
                total = query.RowCount();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    query.Where(x => x.GlCategoryName.IsInsensitiveLike(name, MatchMode.Anywhere));

                }

                if (!string.IsNullOrWhiteSpace(glcategory))
                {
                    //query.Where(x => x.MainAccountCategory.ToString().IsInsensitiveLike(name, MatchMode.Anywhere));
                     session.QueryOver<GlCategory>().Where(x => x.MainAccountCategory.ToString() == glcategory).SingleOrDefault();
                }

                querytotal = query.RowCount();
                query.Skip(start).Take(length);
                return query.List();
            }
        }
    }
}
