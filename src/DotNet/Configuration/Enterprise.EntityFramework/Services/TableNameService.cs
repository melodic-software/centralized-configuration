namespace Enterprise.EntityFramework.Services;

public static class TableNameService
{
    private const string EntitySuffix = "Entity";

    public static string GetTableName(string entityName)
    {
        if (!entityName.EndsWith(EntitySuffix, StringComparison.Ordinal))
            return entityName;

        string tableName = entityName.Substring(0, entityName.Length - EntitySuffix.Length);

        return tableName;
    }
}