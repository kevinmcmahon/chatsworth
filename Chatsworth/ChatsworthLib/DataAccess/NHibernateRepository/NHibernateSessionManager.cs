using System.Reflection;
using FluentNHibernate;
using NHibernate;
using Configuration = NHibernate.Cfg.Configuration;

namespace ChatsworthLib.DataAccess
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
            Configuration cfg = CreateNHibernateConfig(connectionString, mappingAssembly);
            return cfg.BuildSessionFactory();
        }

        private static Configuration CreateNHibernateConfig(string connectionString, string mappingAssembly)
        {
            var config = new Configuration();
            config.Properties.Clear();

            config.SetProperty("connection.driver_class","NHibernate.Driver.SQLite20Driver");
            config.SetProperty("dialect", "NHibernate.Dialect.SQLiteDialect");
            config.SetProperty("connection.connection_string", connectionString);
            config.SetProperty("cache.use_second_level_cache", "false");
            config.SetProperty("query.substitutions", "true=1;false=0");
            config.SetProperty("show_sql","false");
            config.SetProperty("connection.release_mode","auto");
            config.SetProperty("adonet.batch_size", "500");

            var persistenceModel = new PersistenceModel();
            persistenceModel.addMappingsFromAssembly(Assembly.Load(mappingAssembly));
            persistenceModel.Configure(config);
            
            return config;
        }
    }
}
