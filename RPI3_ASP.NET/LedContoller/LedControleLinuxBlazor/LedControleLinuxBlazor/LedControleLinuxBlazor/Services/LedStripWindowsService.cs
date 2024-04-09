using LedControleLinuxBlazor.Const;
using LedControleLinuxBlazor.Model;
using LedControleLinuxBlazor.Extensions;
using LedControleLinuxBlazor.Services;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Xml;

namespace LedControleLinuxBlazor.Services
{
    public class LedStripWindowsService : ILedStripService
    {
        private int ledCount = ProgramConstants.LedCount;
        public LEDStateCollection LedStates = new LEDStateCollection();
        public LedStripWindowsService()
        {
            for (int i = 0; i < ledCount; i++)
            {
                LedStates.Add(new LEDState(i));
            }
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

        public void SetLeds(List<LEDState> leds)
        {
            foreach(LEDState led in leds)
            {
                SetLed(led);
            }
        }

        public void SetLedGroup(LedGroup group, LEDState state)
        {
            foreach (LEDState led in LedStates)
            {
                led.LedColor = Color.Black;
                SetLed(led);
            }
            
            foreach (var ledIndex in group.LedIndexs)
            {
                LEDState? ledState = LedStates.FirstOrDefault(led => led.LedNumber == ledIndex);
                if (ledState != null)
                {
                    ledState.LedColor = state.LedColor;
                    ledState.Brightness = state.Brightness;
                    SetLed(ledState);        
                }
            }
        }


    }
}
