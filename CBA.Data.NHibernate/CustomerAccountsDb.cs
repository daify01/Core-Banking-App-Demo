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
    public class CustomerAccountsDb : EntityDb<CustomerAccounts>, ICustomerAccountsDb
    {
        public IList<CustomerAccounts> PagedSearchByNameAndAccountNumber(string name, string type, string branch, string searchAccountNumber, int start, int length, out int querytotal, out int total)
        {
            using (var session = GetSession())
            {
                var query = session.QueryOver<CustomerAccounts>(); //.Where(x => x.Name.IsInsensitiveLike(name));
                total = query.RowCount();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    query.Where(x => x.AccountName.IsInsensitiveLike(name, MatchMode.Anywhere));
                }
                if (!string.IsNullOrWhiteSpace(searchAccountNumber))
                {
                    query.Where(x => x.AccountNumber.IsInsensitiveLike(searchAccountNumber, MatchMode.Anywhere));
                }
                if (!string.IsNullOrWhiteSpace(type))
                {
                    query.Where(x => Enum.Parse(typeof(AccountType), type).ToString().IsInsensitiveLike(type, MatchMode.Anywhere));
                }
                if (!string.IsNullOrWhiteSpace(branch))
                {

                    query.JoinQueryOver(x => x.Branch)
                        .Where(x => x.BranchName.IsInsensitiveLike(branch, MatchMode.Anywhere));

                }
                querytotal = query.RowCount();
                query.Skip(start).Take(length);
                return query.List();
            }
        }

        public IList<Customer> PagedSearchByFirstNameAndLastName(string name, string Addr, int start, int length, out int querytotal, out int total)
        {
            using (var session = GetSession())
            {
                var query = session.QueryOver<Customer>(); //.Where(x => x.Name.IsInsensitiveLike(name));
                total = query.RowCount();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    query.Where(x => x.FirstName.IsInsensitiveLike(name, MatchMode.Anywhere));
                    //query.JoinQueryOver(x => x.Customer)
                    //    .Where(x => x.FirstName.IsInsensitiveLike(name, MatchMode.Anywhere));
                }
                if (!string.IsNullOrWhiteSpace(Addr))
                {
                    query.Where(x => x.LastName.IsInsensitiveLike(Addr, MatchMode.Anywhere));
                    //query.JoinQueryOver(x => x.Customer)
                    //    .Where(x => x.Address.IsInsensitiveLike(name, MatchMode.Anywhere));
                }
                querytotal = query.RowCount();
                query.Skip(start).Take(length);
                return query.List();
            }
        }

        public CustomerAccounts SearchbyAccountNumber(string Accountnumber)
        {
            using (var session = GetSession())
            {
                return session.QueryOver<CustomerAccounts>().Where(x => x.AccountNumber == Accountnumber).SingleOrDefault();
            }
        }

       

        //public CustomerAccounts RetrieveByAccountNumber(string AccountNumber)
        //{
        //    CustomerAccounts anItem = new CustomerAccounts();
        //    using (var session = GetSession())
        //    {
        //        anItem = session.Get<CustomerAccounts>(AccountNumber);
        //    }
        //    return anItem;
        //}
    }
}
