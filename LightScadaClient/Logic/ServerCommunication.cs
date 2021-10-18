using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LigthScadaClient.Logic.Utility;
using Newtonsoft.Json;
using ScadaCommon;

namespace LigthScadaClient.Logic
{
    public class ServerCommunication : Singleton<ServerCommunication>
    {
        private const string ApiUrl = "http://maluch.mikr.us:30104";//"http://localhost:5002";

        private HttpClient m_client;

        protected override void OnCreate()
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

        public async Task SendData(DataSet data, string apiKey)
        {
            DataFrame frame = new DataFrame();
            frame.Timestamp = DateTime.UtcNow;
            frame.ClientId = LocalConfiguration.Instance.ClientId;
            frame.Dataset = JsonConvert.SerializeObject(data);
            HttpRequestMessage request = new HttpRequestMessage();
            var uriBuilder = new UriBuilder(ApiUrl + "/api/send");
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["apiKey"] = apiKey;
            uriBuilder.Query = query.ToString();
            request.RequestUri = uriBuilder.Uri;
            request.Method = HttpMethod.Post;
            request.Content = new StringContent(JsonConvert.SerializeObject(frame), Encoding.UTF8, "application/json");
            var response = await m_client.SendAsync(request);
            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
                StatusLogger.Instance.Log("Data sent but not accepted : " + response.ReasonPhrase
                + " " + await response.Content.ReadAsStringAsync());
        }
    }
}
