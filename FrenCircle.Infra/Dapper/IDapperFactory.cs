using System.Data;

namespace FrenCircle.Infra.Dapper
{
    public interface IDapperFactory
    {
        IDbConnection CreateConnection();
    }
}
