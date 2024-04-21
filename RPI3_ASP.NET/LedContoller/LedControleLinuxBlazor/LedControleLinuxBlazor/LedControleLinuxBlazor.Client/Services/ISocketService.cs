using Microsoft.AspNetCore.SignalR.Client;

namespace LedControleLinuxBlazor.Client.Services
{
    public interface ISocketService
    {
        Task<HubConnection> StartConnectionAsync();
        Task<HubConnection> ReConnectAsync();
        Task StopConnectionAsync();
    }
}
