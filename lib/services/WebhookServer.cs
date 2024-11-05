using _02_PingProcessInformationToWebsiteService.lib.structs;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _02_PingProcessInformationToWebsiteService.lib
{
    internal class WebhookServer
    {
        private string ServerURI;
        private string ServerEndpoint;
        private HttpClient httpClient;
        
        public WebhookServer(HttpClient httpClient, string ServerURI, string ServerEndpoint)
        {
            this.ServerURI = ServerURI;
            this.ServerEndpoint = ServerEndpoint;
            this.httpClient = httpClient;
        }

        private string GetHMACHashString(string stringData, string secret)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(secret);

            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                byte[] hashBytes = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(stringData));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        public async Task<bool> SendProcessInformationToServer(ProcessTargetData[] processData, string authorizationSecret)
        {
            try
            {
                string payloadHash = GetHMACHashString(JsonSerializer.Serialize(processData), authorizationSecret);

                var jsonPayload = JsonSerializer.Serialize(new
                {
                    hash = payloadHash,
                    payload = processData,
                });

                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(this.ServerEndpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Returned Status code: {response.StatusCode} on endpoint URI {this.ServerEndpoint}");
                }

                return true;
            } catch (Exception ex)
            {
                ErrorLogger.GetInstance().LogEvent($"{ex}\n{ex.StackTrace}");
                return false;
            }
        }
    }
}
