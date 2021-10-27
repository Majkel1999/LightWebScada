using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Npgsql;
using ScadaCommon;

namespace LightScadaAPI.Contexts
{
    public class ReportDatasetReader
    {
        private readonly string m_connectionString;

        public ReportDatasetReader(string connectionString)
        {
            m_connectionString = connectionString;
        }

        public List<List<RegisterFrame>> GetDataList(int OrganizationId, ReportContent content)
        {
            using NpgsqlConnection db = new NpgsqlConnection(m_connectionString);
            try
            {
                string query = "SELECT * FROM public.org_" + OrganizationId + @"_data WHERE 
                ""RegisterAddress""=@address AND 
                ""RegisterType""=@type AND
                ""ClientId""=@clientId 
                Order By ""Timestamp"" DESC";

                List<List<RegisterFrame>> data = new List<List<RegisterFrame>>();
                foreach (ReportElement element in content.Content)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("address", element.Adress);
                    parameters.Add("type", element.Type);
                    parameters.Add("clientId", element.ClientID);
                    data.Add(db.Query<RegisterFrame>(query, parameters).AsList());
                }
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string GetOrganizationName(int OrganizationId)
        {
            using NpgsqlConnection db = new NpgsqlConnection(m_connectionString);
            try
            {
                string query = @"SELECT ""Name"" FROM common.organization WHERE 
                ""OrganizationId""=@id";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("id", OrganizationId);
                return db.Query<string>(query, parameters).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return String.Empty;
            }
        }
    }
}