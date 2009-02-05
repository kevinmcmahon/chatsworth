namespace ChatsworthLib.DataAccess
{
    public class NHibernateSessionConfig
    {
        public NHibernateSessionConfig() {}

        public NHibernateSessionConfig(string mappingAssembly, string connectionString)
        {
            MappingAssembly = mappingAssembly;
            ConnectionString = connectionString;
        }

        public string MappingAssembly { get; set; }

        public string ConnectionString { get; set; }
    }
}