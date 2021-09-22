using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DatabaseClasses;
using Npgsql;

namespace LightScadaAPI.Contexts
{
    public class DatasetWriter
    {
        private readonly string m_connectionString;

        public DatasetWriter(string connectionString)
        {
            m_connectionString = connectionString;
        }

        public async Task<bool> WriteToDatabase(DataFrame dataFrame, string apiKey)
        {
            using IDbConnection db = new NpgsqlConnection(m_connectionString);
            try
            {
                Organization organization = db.Query<Organization>(@"SELECT * FROM common.organization WHERE ""ApiKey"" = @apiKey", new { apiKey }).First();
                string query = "Insert into " + GetTableName(organization) + @"(""ClientName"", ""Timestamp"", ""Dataset"") Values (@Name,@Date,@Dataset)";
                await db.ExecuteAsync(query, dataFrame);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string GetTableName(Organization organization)
        {
            return ("public." + organization.Name + "_" + organization.OrganizationId + "_data");
        }
    }
}
