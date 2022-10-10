using MachinePark.Shared;
using System.Net.Http.Json;

namespace MachinePark.Services
{
    public class DevicesClient : IDevicesClient
    {
        private readonly HttpClient httpClient;

        public DevicesClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<Device>> GetAsync()
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<Device>>("api/devices");
        }
    }
}
