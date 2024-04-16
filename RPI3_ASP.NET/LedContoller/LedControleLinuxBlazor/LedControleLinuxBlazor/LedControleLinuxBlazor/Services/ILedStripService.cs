using LedControleLinuxBlazor.Model;
using LedControleLinuxBlazor.Extensions;
using Microsoft.AspNetCore.Components.Routing;
using System.Collections.ObjectModel;
using LedControleLinuxBlazor.Client.Model;

namespace LedControleLinuxBlazor.Services
{
    public interface ILedStripService
    {
        
        bool OFF();
        bool ON();
        ref LEDStateCollection GetLedStates();
        ref LEDGroupStateCollection GetLedGroups();

        void SetLed(LEDState led);
        void SetLeds(List<LEDState> leds);
        void SetLedGroup(LedGroup group);
    }

}
