using Microsoft.Data.SqlClient;
using System.Data;

namespace test
{
    public class DbCommand
    {
        private readonly string _conn;
        public DbCommand(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection")!;
        }

        public SqlCommand CreateStoredProcedureCommand(string storedProcedureName)
        {
            var conn = new SqlConnection(_conn);
            var cmd = new SqlCommand(storedProcedureName, conn);
            cmd.CommandType = CommandType.StoredProcedure;


            return cmd;
        }
    }
}
