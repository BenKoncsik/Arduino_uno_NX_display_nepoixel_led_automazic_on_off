using LedControleLinuxBlazor.Const;
using LedControleLinuxBlazor.Model;
using LedControleLinuxBlazor.Extensions;
using LedControleLinuxBlazor.Services;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;

namespace LedControleLinuxBlazor.Services
{
    public class LedStripWindowsService : ILedStripService
    {
        private int ledCount = ProgramConstans.LedCount;
        public LEDStateCollection LedStates = new LEDStateCollection();
        public LedStripWindowsService()
        {
            for (int i = 0; i < ledCount; i++)
            {
                LedStates.Add(new LEDState(i));
            }
            LedStates.CollectionChanged += LedStates_CollectionChanged;
        }
        public ref LEDStateCollection GetLedStates()
        {
            return ref LedStates;
        }

        public bool OFF()
        {
            try
            {
                foreach (var led in LedStates)
                {
                    led.LedColor = Color.Black;
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool ON()
        {
            try
            {
                foreach (var led in LedStates)
                {
                    if (led.LedColor.Equals(Color.Black))
                    {
                        led.LedColor = Color.White;
                    }
                }
                return true;
            }

            catch (Exception e)
            {
                return false;
            }
        }

        public void SetLed(LEDState led)
        {
            Console.WriteLine($"Bunny methode: {led.LedNumber} Color: {led.LedColor.ToString()} Brightness: {led.Brightness}");
        }


        private void LedStates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (LEDState newState in e.NewItems)
                {
                    newState.PropertyChanged += LEDState_PropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (LEDState oldState in e.OldItems)
                {
                    oldState.PropertyChanged -= LEDState_PropertyChanged;
                }
            }
        }
        private void LEDState_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LEDState.LedColor)
                || e.PropertyName == nameof(LEDState.Brightness))
            {
                SetLed((LEDState)sender);
            }
        }
    }
}
