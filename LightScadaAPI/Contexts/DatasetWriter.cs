using System.Data;
using System.Linq;
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

        public async void WriteToDatabase(DataFrame dataFrame)
        {
            using IDbConnection db = new NpgsqlConnection(m_connectionString);
            Organization organization = db.Query<Organization>(@"SELECT * FROM common.organization WHERE ""ApiKey"" = @apiKey", new { dataFrame.ApiKey }).FirstOrDefault();
            string query = "Insert into " + GetTableName(organization) + @"(""ClientName"", ""Timestamp"", ""Dataset"") Values (@Name,@Date,@Dataset)";
            await db.ExecuteAsync(query, dataFrame);
        }

        private static string GetTableName(Organization organization)
        {
            return ("public." + organization.Name + "_" + organization.OrganizationId + "_data");
        }
    }
}
