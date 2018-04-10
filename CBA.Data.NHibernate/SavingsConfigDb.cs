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
    public class SavingsConfigDb : EntityDb<SavingsConfig>, ISavingsConfigDb
    {
        public SavingsConfig RetrieveByBranch(int id)
        {
            using (var session = GetSession())
            {
                return session.QueryOver<SavingsConfig>().Where(x => x.Branch.Id == id).SingleOrDefault();
            }
        }
    }
}
