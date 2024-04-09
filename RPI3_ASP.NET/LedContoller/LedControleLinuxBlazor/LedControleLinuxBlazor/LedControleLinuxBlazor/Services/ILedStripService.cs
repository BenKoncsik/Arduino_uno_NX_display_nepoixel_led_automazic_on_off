using LedControleLinuxBlazor.Model;
using LedControleLinuxBlazor.Extensions;
using Microsoft.AspNetCore.Components.Routing;
using System.Collections.ObjectModel;

namespace LedControleLinuxBlazor.Services
{
    public interface ILedStripService
    {
        
        bool OFF();
        bool ON();
        ref LEDStateCollection GetLedStates();
        void SetLed(LEDState led);
        void SetLeds(List<LEDState> leds);
        void SetLedGroup(LedGroup group, LEDState state);
    }

}
