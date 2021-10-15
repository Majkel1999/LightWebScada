using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DatabaseClasses;
using DataRegisters;
using Newtonsoft.Json;
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
            using NpgsqlConnection db = new NpgsqlConnection(m_connectionString);
            try
            {
                await db.OpenAsync();
                Organization organization = db.Query<Organization>(@"SELECT * FROM common.organization WHERE ""ApiKey"" = @apiKey", new { apiKey }).First();
                DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(dataFrame.Dataset);
                string tableName = GetTableName(organization);

                NpgsqlCommand insertCommand = new NpgsqlCommand(@"Insert into " + tableName + @" (""ClientId"", ""Timestamp"", ""RegisterType"", ""RegisterAddress"", ""Value"") 
                values (@id,@timestamp,@registerType,@registerAddress,@value)", db);

                int i = 0;
                foreach (List<Register> registers in dataSet.GetRegisters())
                {
                    foreach (var reg in registers)
                        await SaveRegisterFrame(dataFrame, insertCommand, reg, i);
                    i++;
                }

                await db.CloseAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private static async Task SaveRegisterFrame(DataFrame dataFrame, NpgsqlCommand insertCommand, Register reg, int type)
        {
            insertCommand.Parameters.AddWithValue("id", dataFrame.ClientId);
            insertCommand.Parameters.AddWithValue("timestamp", dataFrame.Timestamp);
            insertCommand.Parameters.AddWithValue("registerType", type);
            insertCommand.Parameters.AddWithValue("registerAddress", reg.RegisterAddress);
            insertCommand.Parameters.AddWithValue("value", reg.CurrentValue);
            await insertCommand.PrepareAsync();
            await insertCommand.ExecuteNonQueryAsync();
            insertCommand.Parameters.Clear();
        }

        private static string GetTableName(Organization organization)
        {
            return ("public.org_" + organization.OrganizationId + "_data");
        }
    }
}
