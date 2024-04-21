using System.Collections.ObjectModel;
using LedControleLinuxBlazor.Const;
using LedControleLinuxBlazor.Model;
using LedControleLinuxBlazor.Extensions;
using LedControleLinuxBlazor.Model;
using LedControleLinuxBlazor.Services;
using Microsoft.AspNetCore.SignalR;
using System.Xml;
using LedControleLinuxBlazor.Client.Model;

namespace LedControleLinuxBlazor.Socket
{
    public class LedControlHub : Hub
    {
        private readonly ILedStripService _ledStrip;
        private LEDStateCollection ledStates;
        private LEDGroupStateCollection ledGroupStates;
        public LedControlHub(ILedStripService ledStrip)
        {
            _ledStrip = ledStrip;
            ledStates = _ledStrip.GetLedStates();
            ledGroupStates = _ledStrip.GetLedGroups();
        }
        public async Task SendLedStates()
        {
            await Clients.All.SendAsync("GetLeds", _ledStrip.GetLedStates().TransformLEDJsonModel());
        }

        public async Task SendLedGroups()
        {
            await Clients.All.SendAsync("GetGroups", ledGroupStates);
        }

        
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

        public async Task SettLedSates(LedGroup group)
        {
            LedGroup ledgroup = ledGroupStates.FirstOrDefault(grup => group.GroupState.LedNumber == grup.GroupState.LedNumber);
            if (ledgroup != null)
            {
                ledgroup.GroupState = group.GroupState;
                _ledStrip.SetLedGroup(group);
            }
            await SendLedStates();
            await Clients.All.SendAsync("UpdateGroup", group);
        }

    }
}
