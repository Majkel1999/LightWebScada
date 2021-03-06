using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using ScadaCommon;

namespace LightScadaAPI.Contexts
{
    /// <summary>
    /// Represents a class which converts JSON data from local clients into single register frames
    /// and saves them to database
    /// </summary>
    public class DatasetWriter
    {
        private readonly string m_connectionString;

        public DatasetWriter(string connectionString)
        {
            m_connectionString = connectionString;
        }

        /// <summary>
        /// Creates the name of the table which belongs to the specified organization
        /// </summary>
        /// <param name="organization">Organization which table should be returned</param>
        /// <returns>Table name with data of the specified organization</returns>
        public static string GetTableName(Organization organization)
        {
            return ("public.org_" + organization.OrganizationId + "_data");
        }

        /// <summary>
        /// Saves each RegisterFrame to database with proper register type, saved as (int)RegisterType
        /// </summary>
        /// <param name="dataFrame">DataFrame received from client</param>
        /// <param name="apiKey">ApiKey assosiacted with the given request</param>
        /// <returns>Tuple <bool,string> where:
        /// Item1 is result true if succeded, false otherwise
        /// Item2 is an error message, if Item1 is false</returns>
        public async Task<(bool, string)> WriteToDatabase(DataFrame dataFrame, string apiKey)
        {
            using NpgsqlConnection db = new NpgsqlConnection(m_connectionString);
            try
            {
                await db.OpenAsync();
                Organization organization = db.Query<Organization>(@"SELECT * FROM common.organization WHERE ""ApiKey"" = @apiKey", new { apiKey }).FirstOrDefault();
                if (organization == null)
                    throw new System.Exception("API Key invalid!");

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
                return (true, "OK");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return (false, e.Message);
            }
        }

        /// <summary>
        /// Saves signle register frame to the correct table in database
        /// </summary>
        /// <param name="dataFrame">DataFrame from which the register is taken</param>
        /// <param name="insertCommand">Prepared command</param>
        /// <param name="reg">Register data</param>
        /// <param name="type">Register type</param>
        /// <returns></returns>
        private async Task SaveRegisterFrame(DataFrame dataFrame, NpgsqlCommand insertCommand, Register reg, int type)
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
    }
}
