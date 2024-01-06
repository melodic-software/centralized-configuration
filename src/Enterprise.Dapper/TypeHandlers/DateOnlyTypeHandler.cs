using Dapper;
using System.Data;

namespace Enterprise.Dapper.TypeHandlers
{
    /// <summary>
    /// Dapper doesn't support DateOnly types out of the box.
    /// This type handler handles materialization into memory and setting values for persistence.
    /// </summary>
    public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
    {
        public override void SetValue(IDbDataParameter parameter, DateOnly value)
        {
            parameter.DbType = DbType.Date;
            parameter.Value = value;
        }

        public override DateOnly Parse(object value)
        {
            DateOnly result = DateOnly.FromDateTime((DateTime)value);

            return result;
        }
    }
}
