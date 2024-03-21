using System.Collections.ObjectModel;
using LedContoller.Model;
using LedController.Services;
using Microsoft.AspNetCore.SignalR;

namespace LedContoller.Socket
{
    public class LedControlHub : Hub
    {
        private readonly ILedStripService _ledStrip;
        private ObservableCollection<LEDState> ledStates;
        public LedControlHub(ILedStripService ledStrip)
        {
            _ledStrip = ledStrip;
            ledStates = _ledStrip.GetLedStates();
        }
        public async Task SendLedState()
        {
            await Clients.All.SendAsync("GetLeds", ledStates);
        }

        public async Task SettLedState(LEDState newLed)
        {
            LEDState? ledState = ledStates.FirstOrDefault(led => led.LedNumber == newLed.LedNumber);
            if (ledState != null)
            {
                ledState.LedColor = newLed.LedColor;
                ledState.Brightness = newLed.Brightness;
            }
            
            await Clients.All.SendAsync("SettLedState", ledState);
        }
    }
}
