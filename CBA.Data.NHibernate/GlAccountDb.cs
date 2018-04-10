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
    public class GlAccountDb : EntityDb<GlAccount>, IGlAccountDb
    {
        public IList<GlAccount> PagedSearchByGLNameAndCategory(string name, string glcode, string glbranch, string glcategory, int start,
            int length, out int querytotal, out int total)
        {
            using (var session = GetSession())
            {
                var query = session.QueryOver<GlAccount>(); //.Where(x => x.Name.IsInsensitiveLike(name));
                total = query.RowCount();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    query.Where(x => x.GlAccountName.IsInsensitiveLike(name, MatchMode.Anywhere));

                }
                if (!string.IsNullOrWhiteSpace(glcode))
                {
                    query.Where(x => x.GlAccountCodes.IsInsensitiveLike(glcode, MatchMode.Anywhere));

                }
                if (!string.IsNullOrWhiteSpace(glbranch))
                {

                    query.JoinQueryOver(x => x.Branch)
                        .Where(x => x.BranchName.IsInsensitiveLike(glbranch, MatchMode.Anywhere));

                }
                if (!string.IsNullOrWhiteSpace(glcategory))
                {
                    //query.Where(x => x.GlCategory.GlCategoryName.IsInsensitiveLike(name, MatchMode.Anywhere));
                    query.JoinQueryOver(x => x.GlCategory)
                        .Where(x => x.GlCategoryName.IsInsensitiveLike(glcategory, MatchMode.Anywhere));

                }

                querytotal = query.RowCount();
                query.Skip(start).Take(length);
                return query.List();
            }
        }
        public GlAccount RetrieveByAccountName(string Accountname)
        {
            using (var session = GetSession())
            {
                return session.QueryOver<GlAccount>().Where(x => x.GlAccountName == Accountname).SingleOrDefault();
            }
        }

        public GlAccount RetrieveByAccountCode(string Accountcode)
        {
            using (var session = GetSession())
            {
                return session.QueryOver<GlAccount>().Where(x => x.GlAccountCodes == Accountcode).SingleOrDefault();
            }
        }

        public GlAccount RetrieveByGLCategoryId(int id)
        {
            using (var session = GetSession())
            {
                return session.QueryOver<GlAccount>().Where(x => x.GlCategory.Id == id).SingleOrDefault();
            }
        }

        public GlAccount RetrieveByBranch(int id)
        {
            using (var session = GetSession())
            {
                return session.QueryOver<GlAccount>().Where(x => x.Branch.Id == id).SingleOrDefault();
            }
        }
        public IList<GlAccount> RetrieveOtherAssettypes()
        {
            return RetrieveAll().Where(x => x.GlCategory.GlCategoryName == "Assets").ToList();
        }

        public IList<GlAccount> RetrieveCashAssettypes()
        {
            return RetrieveAll().Where(x => x.GlCategory.GlCategoryName == "Cash Assets").ToList();
        }

        public IList<GlAccount> RetrieveExpenseGLtypes()
        {
            return RetrieveAll().Where(x => x.GlCategory.GlCategoryName == "Expense").ToList();
        }

        public IList<GlAccount> RetrieveLiablilityGltypes()
        {
            return RetrieveAll().Where(x => x.GlCategory.GlCategoryName == "Liabilities").ToList();
        }

        public IList<GlAccount> RetrieveIncomeGltypes()
        {
            return RetrieveAll().Where(x => x.GlCategory.GlCategoryName == "Income").ToList();
        }
    }
}
