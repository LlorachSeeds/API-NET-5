using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Services.DTOs.Sessions;
using Services.UTLs;

namespace Services.REPs
{
    public class ServiceRemoteRepository<TEntity> : IServiceRemoteRepository<TEntity>
    {
        private string ChirpStackApiRoute { get; set; }

        protected ServiceRemoteRepository(IConfiguration configuration)
        {
            ChirpStackApiRoute = configuration["ChirpStackApiRoute"];
        }

        public async Task<string> RemoteRequest(string endpoint, TEntity remoteObject)
        {
            string responseContent = string.Empty;
            if (!string.IsNullOrWhiteSpace(endpoint))
            {
                HttpClient client = new HttpClient();
                Uri url = new Uri(ChirpStackApiRoute + endpoint);
                Task<HttpResponseMessage> response = client.PostAsJsonAsync(url, remoteObject);
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    responseContent = await response.Result.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new ErrorException((int)response.Result.StatusCode, "RemoteRequest", response.Result.ToString());
                }
            }

            return responseContent;
        }
    }
}