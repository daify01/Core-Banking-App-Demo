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
    public class CustomerDb : EntityDb<Customer>, ICustomerDb
    {
        public IList<Customer> PagedSearchByNameAndAddress(string fname, string lname, string gender, string Addr, int start, int length, out int querytotal, out int total)
        {
            using (var session = GetSession())
            {
                var query = session.QueryOver<Customer>(); //.Where(x => x.Name.IsInsensitiveLike(name));
                total = query.RowCount();
                if (!string.IsNullOrWhiteSpace(fname))
                {
                    query.Where(x => x.FirstName.IsInsensitiveLike(fname, MatchMode.Anywhere));
                }
                if (!string.IsNullOrWhiteSpace(lname))
                {
                    query.Where(x => x.LastName.IsInsensitiveLike(lname, MatchMode.Anywhere));
                }
                if (!string.IsNullOrWhiteSpace(gender))
                {
                    query.Where(x => Enum.Parse(typeof(Gender),gender).ToString().IsInsensitiveLike(gender, MatchMode.Anywhere));
                }
                //if (!string.IsNullOrWhiteSpace(gender))
                //{

                //    query.JoinQueryOver(x => x.Gender)
                //        .Where(x => x.ToString().IsInsensitiveLike(gender, MatchMode.Anywhere));

                //}
                if (!string.IsNullOrWhiteSpace(Addr))
                {
                    query.Where(x => x.Address.IsInsensitiveLike(Addr, MatchMode.Anywhere));
                }
                querytotal = query.RowCount();
                query.Skip(start).Take(length);
                return query.List();
            }
        }
    }
}
