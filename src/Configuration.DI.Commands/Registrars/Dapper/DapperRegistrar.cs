using Enterprise.Dapper.TypeHandlers;

namespace Configuration.DI.Commands.Registrars.Dapper
{
    public static class DapperRegistrar
    {
        public static void RegisterDapperServices()
        {
            TypeHandlerRegistrar.RegisterTypeHandlers();
        }
    }
}
