using System;
using System.Collections.Concurrent;
using System.Data;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Dapper;
using DatabaseClasses;
using Npgsql;

namespace FrontEnd.Areas.Datasets
{
    public class DatasetReader
    {
        private static ConcurrentDictionary<string, DatasetReader> m_instances = new ConcurrentDictionary<string, DatasetReader>();

        private Thread m_updateThread;
        private Organization m_organization;
        private string m_connectionString;
        private int m_clients;
        private int m_lastId = -1;
        private bool m_stayAlive;

        public static void StartSession(Organization organization)
        {
            string name = GetInstanceName(organization);
            if (!m_instances.ContainsKey(name))
                m_instances.TryAdd(name, new DatasetReader(Startup.Configuration.GetConnectionString("UserContextConnection"), organization));
            m_instances[name].AddClient();
        }

        public static void EndSession(Organization organization)
        {
            string name = GetInstanceName(organization);
            if (m_instances.ContainsKey(name))
                m_instances[name].RemoveClient();
        }

        private DatasetReader(string connectionString, Organization organization)
        {
            m_organization = organization;
            m_connectionString = connectionString;
            m_stayAlive = true;
            StartUpdateThread();
        }

        private static string GetInstanceName(Organization organization)
        {
            return organization.Name + "_" + organization.OrganizationId;
        }

        private void StartUpdateThread()
        {
            if (m_updateThread == null)
            {
                m_updateThread = new Thread(new ThreadStart(UpdateHubs));
                m_updateThread.Start();
            }
        }

        private void AddClient()
        {
            m_clients++;
            m_stayAlive = true;
            Console.WriteLine("Added client on " + m_organization.Name + ". Client count: " + m_clients);
        }

        private void RemoveClient()
        {
            m_clients--;
            if (m_clients <= 0)
                m_stayAlive = false;
            Console.WriteLine("Removed client on " + m_organization.Name + ". Client count: " + m_clients);
        }

        private async void UpdateHubs()
        {
            Console.WriteLine("Update starting " + m_organization.Name);
            HubConnection hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/viewhub")
                .Build();

            await hubConnection.StartAsync();

            while (m_stayAlive && m_clients > 0)
            {
                using (IDbConnection db = new NpgsqlConnection(m_connectionString))
                {
                    var frame = db.Query<DataFrame>
                        (@"Select * From " + DatasetContext.GetTableName(m_organization) + @" Order By ""Timestamp"" DESC")
                        .FirstOrDefault();
                    if (m_lastId != frame.Id)
                    {
                        m_lastId = frame.Id;
                        await hubConnection.SendAsync("SendMessage", frame.Dataset, m_organization.Name);
                    }
                }
                Thread.Sleep(2000);
            }
            m_instances.TryRemove(GetInstanceName(m_organization), out DatasetReader instance);
            m_updateThread = null;
            Console.WriteLine("Controller dying " + m_organization.Name);
        }
    }
}