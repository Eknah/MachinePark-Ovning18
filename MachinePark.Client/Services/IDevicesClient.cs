using MachinePark.Shared;

namespace MachinePark.Services
{
    public interface IDevicesClient
    {
        Task<IEnumerable<Device>> GetAsync();
        Task<Device?> PostAsync(CreateDevice createDevice);
    }
}
