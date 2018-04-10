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
    public class TellerDb : EntityDb<Teller>, ITellerDb
    {
        public IList<Teller> PagedSearchByName(string userName, string glAccountName, string searchlName, int start, int length, out int querytotal, out int total)
        {
            using (var session = GetSession())
            {
                var query = session.QueryOver<Teller>(); //.Where(x => x.Name.IsInsensitiveLike(name));
                total = query.RowCount();

                if (!string.IsNullOrWhiteSpace(userName))
                {

                    query.JoinQueryOver(x => x.User)
                        .Where(x => x.FirstName.IsInsensitiveLike(userName, MatchMode.Anywhere));

                }

                if (!string.IsNullOrWhiteSpace(searchlName))
                {

                    query.JoinQueryOver(x => x.User)
                        .Where(x => x.LastName.IsInsensitiveLike(searchlName, MatchMode.Anywhere));

                }

                if (!string.IsNullOrWhiteSpace(glAccountName))
                {

                    query.JoinQueryOver(x => x.GlAccount)
                        .Where(x => x.GlAccountName.IsInsensitiveLike(glAccountName, MatchMode.Anywhere));

                }

                querytotal = query.RowCount();
                query.Skip(start).Take(length);
                return query.List();
            }
        }
        public Teller RetrievebyUserName(User user)
        {
            using (var session = GetSession())
            {
                return session.QueryOver<Teller>().Where(x => x.User == user).SingleOrDefault();
            }
        }

        public Teller RetrievebyUserId(int userId)
        {
            using (var session = GetSession())
            {
                return session.QueryOver<Teller>().Where(x => x.User.Id == userId).SingleOrDefault();
            }
        }

        public Teller RetrievebyTillId(int tillId)
        {
            using (var session = GetSession())
            {
                return session.QueryOver<Teller>().Where(x => x.GlAccount.Id == tillId).SingleOrDefault();
            }
        }
    }
}
