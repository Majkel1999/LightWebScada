using Npgsql;
using Dapper;
using DatabaseClasses;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace FrontEnd.Areas.Datasets
{
    public class DatasetContext
    {
        private const string TableArchetype = @"(""Id"" integer NOT NULL,
            ""ClientName"" text NOT NULL,
            ""Timestamp"" timestamp with time zone NOT NULL,
            ""Dataset"" text NOT NULL,
            PRIMARY KEY(""Id"")
            );";

        public void CreateNewTable(Organization organization)
        {
            using (IDbConnection db = new NpgsqlConnection(Startup.Configuration.GetConnectionString("UserContextConnection")))
                db.Execute("CREATE TABLE IF NOT EXISTS " + GetTableName(organization) + TableArchetype);
        }

        public void RemoveTable(Organization organization)
        {
            using (IDbConnection db = new NpgsqlConnection(Startup.Configuration.GetConnectionString("UserContextConnection")))
                db.Execute("Drop Table IF EXISTS " + GetTableName(organization));
        }

        private string GetTableName(Organization organization)
        {
            return ("public."+organization.Name + "_" + organization.OrganizationId + "_data");
        }
    }
}
