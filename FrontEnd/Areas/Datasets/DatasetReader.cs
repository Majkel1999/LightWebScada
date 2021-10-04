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
        private static ConcurrentDictionary<int, DatasetReader> m_instances = new ConcurrentDictionary<int, DatasetReader>();

        private Thread m_updateThread;
        private Mutex m_mutex = new Mutex();
        private Organization m_organization;
        private DataFrame m_lastFrame;
        private string m_connectionString;
        private int m_viewId;
        private int m_clients;
        private int m_lastId = -1;
        private bool m_stayAlive;

        public DataFrame LastFrame => m_lastFrame;

        public static DatasetReader StartSession(int viewId, Organization organization)
        {
            if (!m_instances.ContainsKey(viewId))
                m_instances.TryAdd(viewId, new DatasetReader(Startup.Configuration.GetConnectionString("UserContextConnection"), viewId, organization));
            m_instances[viewId].AddClient();
            return m_instances[viewId];
        }

        public static void EndSession(int viewId)
        {
            if (m_instances.ContainsKey(viewId))
                m_instances[viewId].RemoveClient();
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
                .WithUrl("https://localhost:5001/viewhub")
                .Build();

            await hubConnection.StartAsync();

            while (m_stayAlive && m_clients > 0)
            {
                using (IDbConnection db = new NpgsqlConnection(m_connectionString))
                {
                    m_lastFrame = db.Query<DataFrame>
                         (@"Select * From " + DatasetContext.GetTableName(m_organization) + @" Order By ""Timestamp"" DESC")
                         .FirstOrDefault();
                    if (m_lastId != m_lastFrame.Id)
                    {
                        m_lastId = m_lastFrame.Id;
                        await hubConnection.SendAsync("SendMessage", m_lastFrame.Dataset, m_viewId.ToString());
                    }
                }
                Thread.Sleep(2000);
            }
            m_mutex.WaitOne();
            m_instances.TryRemove(m_viewId, out DatasetReader instance);
            m_updateThread = null;
            m_mutex.ReleaseMutex();
            Console.WriteLine($"Controller dying on {m_viewId}");
        }
    }
}