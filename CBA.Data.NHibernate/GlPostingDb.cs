using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CBA.Data.Nhibernate;
using CBAPractice.Core;
using CBAPractice.Data;
using NHibernate.Criterion;

namespace CBA.Data.NHibernate
{
    public class GlPostingDb : EntityDb<GlPosting>, IGlPostingDb
    {
        public IList<GlPosting> RetrievePostingToDate(DateTime SelectedTransactionDate)
        {
            using (var session = GetSession())
            {
                return session.QueryOver<GlPosting>().Where(x => x.TransactionDate <= SelectedTransactionDate).List();
            }
        }

        //public IList<GlPosting> RetrievePostingForDate(DateTime SelectedTransactionDate, int start, int length, out int querytotal, out int total)
        //{
        //    using (var session = GetSession())
        //    {
        //        var query = session.QueryOver<GlPosting>(); //.Where(x => x.Name.IsInsensitiveLike(name));
        //        total = query.RowCount();
        //        //return session.QueryOver<GlPosting>().Where(x => x.TransactionDate == SelectedTransactionDate).List();
        //        if ((SelectedTransactionDate != null))
        //        {
        //            query.Where(x => x.TransactionDate == SelectedTransactionDate);

        //        }
        //        querytotal = query.RowCount();
        //        query.Skip(start).Take(length);
        //        return query.List();
                
        //    }

        //}
        public IList<GlPosting> PagedSearchByGLNameAndCode(string name, string acctcode, int start,
            int length, out int querytotal, out int total)
        {
            using (var session = GetSession())
            {
                var query = session.QueryOver<GlPosting>(); //.Where(x => x.Name.IsInsensitiveLike(name));
                total = query.RowCount();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    //query.Where(x => x.GlAccountToDebit.GlAccountName.IsInsensitiveLike(name, MatchMode.Anywhere));
                    query.JoinQueryOver(x => x.GlAccountToDebit)
                        .Where(x => x.GlAccountName.IsInsensitiveLike(name, MatchMode.Anywhere));

                }

                if (!string.IsNullOrWhiteSpace(acctcode))
                {
                    //query.Where(x => x.GlAccountToDebit.GlAccountCodes.IsInsensitiveLike(name, MatchMode.Anywhere));
                    query.JoinQueryOver(x => x.GlAccountToDebit)
                        .Where(x => x.GlAccountCodes.IsInsensitiveLike(acctcode, MatchMode.Anywhere));

                }
                //if (!string.IsNullOrWhiteSpace(date))
                //{
                //    query.Where(x => x.TransactionDate.ToString().IsInsensitiveLike(date, MatchMode.Anywhere));

                //}
                querytotal = query.RowCount();
                query.Skip(start).Take(length);
                return query.List();
            }
        }
    }
}
