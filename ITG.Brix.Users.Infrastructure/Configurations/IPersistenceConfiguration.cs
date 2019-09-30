namespace ITG.Brix.Users.Infrastructure.Configurations
{
    public interface IPersistenceConfiguration
    {
        string ConnectionString { get; }
        string Database { get; }
        string Collection { get; }
    }
}
