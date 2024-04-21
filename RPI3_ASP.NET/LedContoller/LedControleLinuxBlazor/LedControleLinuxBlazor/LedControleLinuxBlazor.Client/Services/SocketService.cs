using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace LedControleLinuxBlazor.Client.Services
{
    public class SocketService : ISocketService
    {
        public HubConnection HubCon { get; private set; }
        private readonly NavigationManager _navigationManager;
        public SocketService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public async Task<HubConnection> StartConnectionAsync()
        {
            if (HubCon == null || HubCon.State == HubConnectionState.Disconnected)
            {
                HubCon = new HubConnectionBuilder()
                    .WithUrl(_navigationManager.ToAbsoluteUri("/ledControlHub"))
                    .WithAutomaticReconnect()
                    .Build();
                await HubCon.StartAsync();
            }

            return HubCon;
        }

        public async Task<HubConnection> ReConnectAsync()
        {
            await StopConnectionAsync();
            return await StartConnectionAsync();
        }

        public async Task StopConnectionAsync()
        {
            if (HubCon != null)
            {
                await HubCon.StopAsync();
                await HubCon.DisposeAsync();
                HubCon = null;
            }
        }

    }
}
