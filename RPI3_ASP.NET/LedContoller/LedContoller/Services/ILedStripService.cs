using LedContoller.Model;
using Microsoft.AspNetCore.Components.Routing;
using System.Collections.ObjectModel;

namespace LedController.Services
{
    public interface ILedStripService
    {
        
        bool OFF();
        bool ON();
        ref ObservableCollection<LEDState> GetLedStates();
        void SetLed(LEDState led);
    }

}
