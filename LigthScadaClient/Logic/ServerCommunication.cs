using System;
using System.Data;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace LigthScadaClient.Logic
{
    static class ServerCommunication
    {
        private static readonly HttpClient m_client = new();

        public static bool SendDataToServer(ref DataSet dataSet,string url)
        {
            using (var content = new StringContent(JsonConvert.SerializeObject(dataSet), Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = m_client.PostAsync(url, content).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return true;
                }

                string returnValue = result.Content.ReadAsStringAsync().Result;
                throw new Exception($"Failed to POST data: ({result.StatusCode}): {returnValue}");
            }
        }
    }
}
