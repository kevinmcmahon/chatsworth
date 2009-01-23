namespace ChatsworthLib.DataAccess
{
    public interface IPersistenceProvider
    {
        void Save<T>(T obj);
        T Get<T>();
    }
}