using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;
using CBAPractice.Data;
using NHibernate.Criterion;

namespace CBA.Data.Nhibernate
{
    public class UserDb : EntityDb<User>, IUserDb
    {
        public IList<User> PagedSearchByNameAndBranch(string fname, string lname, string branch, int start, int length, out int querytotal, out int total)
        {
            using (var session = GetSession())
            {
                var query = session.QueryOver<User>(); //.Where(x => x.Name.IsInsensitiveLike(name));
                total = query.RowCount();
                
                if (!string.IsNullOrWhiteSpace(fname))
                {
                    query.Where(x => x.FirstName.IsInsensitiveLike(fname, MatchMode.Anywhere));
                   
                }

                if (!string.IsNullOrWhiteSpace(lname))
                {
                    query.Where(x => x.LastName.IsInsensitiveLike(lname, MatchMode.Anywhere));
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

        public User RetrievebyUserName(string UserName)
        {
            using (var session = GetSession())
            {
                return session.QueryOver<User>().Where(x => x.UserName == UserName).SingleOrDefault();
            }
        }

        public User RetrievebyEmail(string Email)
        {
            using (var session = GetSession())
            {
                return session.QueryOver<User>().Where(x => x.Email == Email).SingleOrDefault();
            }
        }

        public User RetrievebyPassword(string Password)
        {
            using (var session = GetSession())
            {
                return session.QueryOver<User>().Where(x => x.Password == Password).SingleOrDefault();
            }
        }

        public IList<User> RetrievebyAdminRole()
        {
            return RetrieveAll().Where(x => x.UserRole == Role.Admin).ToList();
        }
      
    }

        
    
}
