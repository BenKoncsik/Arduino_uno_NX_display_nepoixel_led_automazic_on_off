﻿using System.Collections.ObjectModel;
using LedControleLinuxBlazor.Model;
using LedControleLinuxBlazor.Extensions;
using LedControleLinuxBlazor.Model;
using LedControleLinuxBlazor.Services;
using Microsoft.AspNetCore.SignalR;

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
            await Clients.All.SendAsync("GetLeds", ledStates.TransformLEDJsonModel());
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

    }
}