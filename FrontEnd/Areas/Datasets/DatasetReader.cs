using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using FrontEnd.Areas.Organizations.Data;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using ScadaCommon;

namespace FrontEnd.Areas.Datasets
{
    public class DatasetReader
    {
        private static Dictionary<int, DatasetReader> m_instances = new Dictionary<int, DatasetReader>();
        private static string m_connectionString = Startup.Configuration.GetConnectionString("UserContextConnection");

        private CancellationTokenSource m_tokenSource = new CancellationTokenSource();
        private Organization m_organization;
        private List<string> m_queryStrings;
        private int m_viewId;
        private int m_clients;

        private DatasetReader(int viewId, Organization organization, View view)
        {
            m_viewId = viewId;
            m_organization = organization;
            m_instances.TryAdd(viewId, this);
            m_queryStrings = PrepareQueryStrings(view.GetRegisters());
            Task.Run(() => UpdateHubs(m_tokenSource.Token));
        }

        public static DatasetReader StartSession(int viewId, View view, Organization organization)
        {
            if (!m_instances.ContainsKey(viewId))
            {
                DatasetReader reader = new DatasetReader(viewId, organization, view);

            }
            m_instances[viewId].AddClient();
            return m_instances[viewId];
        }

        public static void EndSession(int viewId)
        {
            if (m_instances.ContainsKey(viewId))
                m_instances[viewId].RemoveClient();
        }

        public List<RegisterFrame> GetLastValues(RegisterType type, int registerAddress, int clientId)
        {
            using (NpgsqlConnection db = new NpgsqlConnection(m_connectionString))
            {
                db.Open();
                string query = @"Select * From " + DatasetContext.GetTableName(m_organization) +
                $@" WHERE ""RegisterAddress""=@address AND ""RegisterType""=@type AND ""ClientId""=@clientId Order By ""Timestamp"" DESC LIMIT 20 ";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("type", (int)type);
                parameters.Add("address", registerAddress);
                parameters.Add("clientId", clientId);
                List<RegisterFrame> dataFrames = db.Query<RegisterFrame>(query, parameters).ToList();
                db.Close();
                return dataFrames;
            }
        }

        private List<string> PrepareQueryStrings(List<(int, int, RegisterType)> registers)
        {
            List<string> queries = new List<string>();
            foreach (RegisterType type in Enum.GetValues(typeof(RegisterType)))
            {
                List<int> distinctClients = registers
                    .GroupBy(x => x.Item2)
                    .Select(x => x.FirstOrDefault())
                    .Select(x => x.Item2)
                    .ToList();

                foreach (int clientId in distinctClients)
                {
                    string query = @"SELECT DISTINCT ON (""RegisterAddress"") * FROM " + DatasetContext.GetTableName(m_organization) + GetWhereClause(type, clientId) +
                    @"AND ""RegisterAddress"" IN (";
                    int limit = 0;
                    foreach (var tuple in registers.Where(x => x.Item3 == type))
                    {
                        query += tuple.Item1 + ",";
                        limit++;
                    }
                    query = query.TrimEnd(',');
                    query += ")";
                    query += @" Order By ""RegisterAddress"",""Timestamp"" DESC";
                    if (limit > 0)
                        queries.Add(query + " LIMIT " + limit);
                }
            }
            // foreach (string s in queries)
            //     Console.WriteLine(s);
            return queries;
        }

        private string GetWhereClause(RegisterType type, int clientId)
        {
            return @" WHERE ""RegisterType""=" + (int)type + @" AND ""ClientId""=" + clientId + " ";
        }

        private void AddClient()
        {
            m_clients++;
            Console.WriteLine($"Added client on view {m_viewId} with current clients {m_clients}");
        }

        private void RemoveClient()
        {
            m_clients--;
            if (m_clients <= 0)
            {
                m_instances.Remove(m_viewId);
                m_tokenSource.Cancel();
            }
            Console.WriteLine($"Removed client on view {m_viewId} with current clients {m_clients}");
        }

        private async void UpdateHubs(CancellationToken token)
        {
            Console.WriteLine($"Update starting on {m_viewId}");
            HubConnection hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/viewhub")
                .Build();

            await hubConnection.StartAsync();
            do
            {
                using (NpgsqlConnection db = new NpgsqlConnection(m_connectionString))
                {
                    await db.OpenAsync();
                    List<RegisterFrame> frames = new List<RegisterFrame>();
                    foreach (string query in m_queryStrings)
                        frames.AddRange(db.Query<RegisterFrame>(query).ToList());

                    await hubConnection.SendAsync("SendMessage", JsonConvert.SerializeObject(frames), m_viewId.ToString());
                    await db.CloseAsync();
                }
                token.WaitHandle.WaitOne(5000);
            } while (!token.IsCancellationRequested);

            m_tokenSource.Dispose();
            Console.WriteLine($"Controller dying on {m_viewId}");
        }
    }
}