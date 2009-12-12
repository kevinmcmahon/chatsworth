using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace Chatsworth.Core.DataAccess
{
    public class NHibernateSessionManager
    {
        private readonly ISessionFactory _sessionFactory;

        public NHibernateSessionManager(NHibernateSessionConfig config)
        {
            _sessionFactory = GetSessionFactory(config.ConnectionString, config.MappingAssembly);
        }

        public ISession GetSession()
        {
            return _sessionFactory.OpenSession();
        }

        private ISessionFactory GetSessionFactory(string connectionString, string mappingAssembly)
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.ConnectionString(x=>x.Is(connectionString)))
                .Mappings(m =>
                          m.FluentMappings
                              .AddFromAssembly(Assembly.Load(mappingAssembly)))
                .BuildSessionFactory();
        }
    }
}