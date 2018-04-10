using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using CBA.Data.Nhibernate.Maps;
using CBAPractice.Core;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;
using Configuration = NHibernate.Cfg.Configuration;

//using Configuration = NHibernate.Cfg.Configuration;

namespace CBA.Data.Nhibernate
{
    public class EntityDb<T> where T : class, IEntity, new()
    {
        private ISessionFactory factory;

        protected ISession GetSession()
        {
            if (factory == null)
            {
                factory = CreateSessionFactory();
            }
            return factory.OpenSession();
        }

        public SqlConnection DefineSqlConnection()
        {

            string connString = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
            return new SqlConnection(connString);
        }

        ISessionFactory CreateSessionFactory()
        {
            string connString = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
            return Fluently.Configure()
                            .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connString))
                            .Mappings(m => m.FluentMappings
                                                .AddFromAssemblyOf<UserMap>())
                                .ExposeConfiguration(CreateSchema)
                                .BuildConfiguration()
                                .BuildSessionFactory();
        }

        public int InsertData(T item)
        {
            int id;
            using (var session = GetSession())
            {
                using (var trans = session.BeginTransaction())
                {
                    id = (int)session.Save(item);
                    trans.Commit();
                }

            }
            return id;
        }

        public T RetrieveById(int id)
        {
            T anItem = new T();
            using (var session = GetSession())
            {
                anItem = session.Get<T>(id);
            }
            return anItem;
        }

        public IList<T> RetrieveAll()
        {
            IList<T> theList = new List<T>();
            using (var session = GetSession())
            {
                using (var trans = session.BeginTransaction())
                {
                    theList = session.Query<T>().ToList();
                    trans.Commit();
                }

            }

            return theList;
        }

        public void UpdateData(T item)
        {
            using (var session = GetSession())
            {
                using (var trans = session.BeginTransaction())
                {
                    session.Update(item);
                    trans.Commit();
                }
            }
        }

        public void DeleteData(T item)
        {
            using (var session = GetSession())
            {
                using (var trans = session.BeginTransaction())
                {
                    session.Delete(item);
                    trans.Commit();
                }

            }
        }

        private static void CreateSchema(Configuration cfg)
        {
            new SchemaUpdate(cfg).Execute(false, true);
        }
    }
}
