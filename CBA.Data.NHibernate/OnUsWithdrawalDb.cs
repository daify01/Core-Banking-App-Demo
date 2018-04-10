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
    public class OnUsWithdrawalDb : EntityDb<OnUsWithdrawal>, IOnUsWithdrawalDb
    {
        public IList<OnUsWithdrawal> PagedSearchByName_IDAndLocation(string name, string ID, string location, int start, int length, out int querytotal, out int total)
        {
            using (var session = GetSession())
            {
                var query = session.QueryOver<OnUsWithdrawal>(); //.Where(x => x.Name.IsInsensitiveLike(name));
                total = query.RowCount();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    query.Where(x => x.Name.IsInsensitiveLike(name, MatchMode.Anywhere));
                }
                if (!string.IsNullOrWhiteSpace(ID))
                {
                    query.Where(x => x.TerminalID == ID);
                }
                if (!string.IsNullOrWhiteSpace(location))
                {
                    query.Where(x => x.Location == location);
                }
                querytotal = query.RowCount();
                query.Skip(start).Take(length);
                return query.List();
            }
        }
    }
}
