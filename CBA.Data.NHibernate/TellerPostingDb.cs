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
    public class TellerPostingDb : EntityDb<TellerPosting>, ITellerPostingDb
    {
        public IList<TellerPosting> PagedSearchByAccountNameAndAccountNumber(string name, string tillacctname, string acctnumber, DateTime searchdate, int start,
            int length, out int querytotal, out int total)
        {
            using (var session = GetSession())
            {
                var query = session.QueryOver<TellerPosting>(); //.Where(x => x.Name.IsInsensitiveLike(name));
                total = query.RowCount();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    //query.Where(x => x.CustomerAccounts.AccountName.IsInsensitiveLike(name, MatchMode.Anywhere));
                    query.JoinQueryOver(x => x.CustomerAccounts)
                        .Where(x => x.AccountName.IsInsensitiveLike(name, MatchMode.Anywhere));
                }

                if (!string.IsNullOrWhiteSpace(acctnumber))
                {
                    //query.Where(x => x.CustomerAccounts.AccountNumber.IsInsensitiveLike(name, MatchMode.Anywhere));
                    query.JoinQueryOver(x => x.CustomerAccounts)
                        .Where(x => x.AccountNumber.IsInsensitiveLike(acctnumber, MatchMode.Anywhere));
                }

                if (!string.IsNullOrWhiteSpace(tillacctname))
                {
                    //query.Where(x => x.CustomerAccounts.AccountNumber.IsInsensitiveLike(name, MatchMode.Anywhere));
                    query.JoinQueryOver(x => x.GlAccount)
                        .Where(x => x.GlAccountName.IsInsensitiveLike(tillacctname, MatchMode.Anywhere));
                }

                if (searchdate != DateTime.Now.Date)
                {
                    query.Where(x => x.TransactionDate >= searchdate && x.TransactionDate < searchdate.AddDays(1));
                }
                //if (searchdate == DateTime.Parse("yyyy-mm-dd").Date)
                //{
                //    query.Where(x => x.TransactionDate.ToShortDateString().IsInsensitiveLike(searchdate.ToShortDateString(), MatchMode.Anywhere));
                //}
                querytotal = query.RowCount();
                query.Skip(start).Take(length);
                return query.List();
            }
        }
    }

 }

