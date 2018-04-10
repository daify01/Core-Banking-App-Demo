using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBA.Data.Nhibernate;
using CBAPractice.Core;
using CBAPractice.Data;

namespace CBA.Data.NHibernate
{
    public class CurrentConfigDb : EntityDb<CurrentConfig>, ICurrentConfigDb
    {
        public CurrentConfig RetrieveByBranch(int id)
        {
            using (var session = GetSession())
            {
                return session.QueryOver<CurrentConfig>().Where(x => x.Branch.Id == id).SingleOrDefault();
            }
        }
    }
}
