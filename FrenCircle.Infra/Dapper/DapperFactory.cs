using FrenCircle.Shared.ConfigModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Infra.Dapper
{
    public class DapperFactory : IDapperFactory
    {
        private readonly SqlConfig _sql;

        public DapperFactory(IOptions<FcConfig> _config)
        {
            _sql = _config.Value.SqlConfig;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_sql.ConnectionString);
        }
    }
}
