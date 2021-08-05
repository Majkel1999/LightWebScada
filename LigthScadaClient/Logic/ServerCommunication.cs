using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using DatabaseClasses;
using Newtonsoft.Json;

namespace LigthScadaClient.Logic
{
    public class ServerCommunication
    {
        private const string ApiUrl = "https://localhost:5003";

        private static ServerCommunication m_instance;

        private readonly HttpClient m_client;

        public static ServerCommunication Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new ServerCommunication();
                return m_instance;
            }
        }

        private ServerCommunication()
        {
            m_client = new HttpClient();
            m_client.Timeout = new TimeSpan(0, 0, 10);
        }


        public async Task<List<ClientConfigEntity>> GetConfigurations(string apiKey)
        {
            List<ClientConfigEntity> configs = null;
            HttpResponseMessage response = null;
            try
            {
                response = await m_client.GetAsync(ApiUrl + "/api/configurations/" + apiKey);
            }
            catch (Exception e)
            {
                Trace.WriteLine("\nException Caught!");
                Trace.WriteLine("Message :{0} ", e.Message);
                return null;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                configs = JsonConvert.DeserializeObject<List<ClientConfigEntity>>(content);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                throw new KeyNotFoundException("The specified ApiKey is invalid!");

            return configs;
        }
    }
}
