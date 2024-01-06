using Dapper;

namespace Enterprise.Dapper.TypeHandlers
{
    public static class TypeHandlerRegistrar
    {
        public static void RegisterTypeHandlers()
        {
            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        }
    }
}
