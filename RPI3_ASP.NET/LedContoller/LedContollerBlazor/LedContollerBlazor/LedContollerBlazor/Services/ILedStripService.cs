using LedContoller.Model;
using LedContollerBlazor.Extensions;
using Microsoft.AspNetCore.Components.Routing;
using System.Collections.ObjectModel;

namespace LedController.Services
{
    public interface ILedStripService
    {
        
        bool OFF();
        bool ON();
        ref LEDStateCollection GetLedStates();
        void SetLed(LEDState led);
    }

}
