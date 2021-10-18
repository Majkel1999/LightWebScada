using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private static ConcurrentDictionary<int, DatasetReader> m_instances = new ConcurrentDictionary<int, DatasetReader>();

        private Thread m_updateThread;
        private Mutex m_mutex = new Mutex();
        private Organization m_organization;
        private List<string> m_queryStrings;
        private string m_connectionString;
        private int m_viewId;
        private int m_clients;
        private bool m_stayAlive;

        public static DatasetReader StartSession(int viewId, View view, Organization organization)
        {
            if (!m_instances.ContainsKey(viewId))
            {
                DatasetReader reader = new DatasetReader(Startup.Configuration.GetConnectionString("UserContextConnection"), viewId, organization);
                m_instances.TryAdd(viewId, reader);
                reader.m_queryStrings = reader.PrepareQueryStrings(view.GetRegisters());
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
            foreach (string s in queries)
                Console.WriteLine(s);
            return queries;
        }

        private string GetWhereClause(RegisterType type, int clientId)
        {
            return @" WHERE ""RegisterType""=" + (int)type + @" AND ""ClientId""=" + clientId + " ";
        }

        private DatasetReader(string connectionString, int viewId, Organization organization)
        {
            m_connectionString = connectionString;
            m_viewId = viewId;
            m_organization = organization;
            m_stayAlive = true;
            StartUpdateThread();
        }

        private void StartUpdateThread()
        {
            if (m_updateThread == null)
            {
                m_mutex.WaitOne();
                m_updateThread = new Thread(new ThreadStart(UpdateHubs));
                m_updateThread.Start();
                m_mutex.ReleaseMutex();
            }
        }

        private void AddClient()
        {
            m_mutex.WaitOne();
            m_clients++;
            m_stayAlive = true;
            Console.WriteLine($"Added client on view {m_viewId} with current clients {m_clients}");
            m_mutex.ReleaseMutex();
        }

        private void RemoveClient()
        {
            m_mutex.WaitOne();
            m_clients--;
            if (m_clients <= 0)
                m_stayAlive = false;
            Console.WriteLine($"Removed client on view {m_viewId} with current clients {m_clients}");
            m_mutex.ReleaseMutex();
        }

        private async void UpdateHubs()
        {
            Console.WriteLine($"Update starting on {m_viewId}");
            HubConnection hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/viewhub")
                .Build();

            await hubConnection.StartAsync();

            while (m_stayAlive && m_clients > 0)
            {
                using (NpgsqlConnection db = new NpgsqlConnection(m_connectionString))
                {
                    await db.OpenAsync();
                    List<RegisterFrame> frames = new List<RegisterFrame>();
                    foreach (string query in m_queryStrings)
                        frames.AddRange((await db.QueryAsync<RegisterFrame>(query)).ToList());

                    await hubConnection.SendAsync("SendMessage", JsonConvert.SerializeObject(frames), m_viewId.ToString());
                    await db.CloseAsync();
                }
                Thread.Sleep(5000);
            }
            m_mutex.WaitOne();
            m_instances.TryRemove(m_viewId, out DatasetReader instance);
            m_updateThread = null;
            m_mutex.ReleaseMutex();
            Console.WriteLine($"Controller dying on {m_viewId}");
        }
    }
}