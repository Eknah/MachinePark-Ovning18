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
            return await httpClient.GetFromJsonAsync<IEnumerable<Device>>("api/GetDevices");
        }

        public async Task<Device?> PostAsync(CreateDevice createDevice)
        {
            var response = await httpClient.PostAsJsonAsync<CreateDevice>("api/CreateDevice", createDevice);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<Device>();

            return null;
        }
    }
}
