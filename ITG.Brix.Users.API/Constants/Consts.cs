namespace ITG.Brix.Users.API.Constants
{
    internal static class Consts
    {
        internal static class Config
        {
            internal const string ConfigurationPackageObject = "Config";

            internal static class Environment
            {
                internal const string Section = "Environment";
                internal const string Param = "ASPNETCORE_ENVIRONMENT";
            }

            internal static class Database
            {
                internal const string Section = "Database";
                internal const string Param = "DatabaseConnectionString";
            }

            internal static class AzureServiceBusTransport
            {
                internal const string Section = "AzureServiceBus";
                internal const string Status = "Status";
                internal const string ConnectionString = "ConnectionString";
            }

            internal static class RabbitMQTransport
            {
                internal const string Section = "RabbitMQ";
                internal const string Status = "Status";
                internal const string ConnectionString = "ConnectionString";
            }
        }

        internal static class Configuration
        {
            internal const string Id = "ITG.Brix.Users";
            internal const string ConnectionString = "ConnectionString";
            internal const string AzureServiceBusEnabled = "AzureServiceBusEnabled";
            internal const string AzureServiceBusConnectionString = "AzureServiceBusConnectionString";
            internal const string RabbitMQEnabled = "RabbitMQEnabled";
            internal const string RabbitMQConnectionString = "RabbitMQConnectionString";
        }
    }
}
