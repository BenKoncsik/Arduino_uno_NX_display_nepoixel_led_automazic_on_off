using System.Collections.ObjectModel;
using LedControleLinuxBlazor.Const;
using LedControleLinuxBlazor.Model;
using LedControleLinuxBlazor.Extensions;
using LedControleLinuxBlazor.Model;
using LedControleLinuxBlazor.Services;
using Microsoft.AspNetCore.SignalR;
using System.Xml;

namespace LedControleLinuxBlazor.Socket
{
    public class LedControlHub : Hub
    {
        private readonly ILedStripService _ledStrip;
        private LEDStateCollection ledStates;
        public LedControlHub(ILedStripService ledStrip)
        {
            _ledStrip = ledStrip;
            ledStates = _ledStrip.GetLedStates();
        }
        public async Task SendLedStates()
        {
            await Clients.All.SendAsync("GetLeds", _ledStrip.GetLedStates().TransformLEDJsonModel());
        }

        public async Task SendLedGroups()
        {
            await Clients.All.SendAsync("GetGroups", ProgramConstants.LedGroups);
        }

        //public async Task SettLedGroup(LedGroup goup)
        public async Task SettLedState(LEDStateJsonModel newLed)
        {
            LEDState? ledState = ledStates.FirstOrDefault(led => led.LedNumber == newLed.LedNumber);
            if (ledState != null)
            {
                ledState.SetColorFromHtml(newLed.LedColor);
                ledState.Brightness = newLed.Brightness;
                _ledStrip.SetLed(ledState);
            }
            
            await Clients.All.SendAsync("UpdateState", ledState.ConvertToLEDJsonModelState());
        }

        public async Task SettLedSates(LedGroup group, LEDStateJsonModel newState)
        {
            _ledStrip.SetLedGroup(group, new LEDState(newState));
            await SendLedStates();
        }

    }
}
