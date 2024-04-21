using LedControleLinuxBlazor.Const;
using LedControleLinuxBlazor.Model;
using LedControleLinuxBlazor.Extensions;
using LedControleLinuxBlazor.Services;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Xml;
using LedControleLinuxBlazor.Client.Model;

namespace LedControleLinuxBlazor.Services
{
    public class LedStripBunnyService : ILedStripService
    {
        private int ledCount = ProgramConstants.LedCount;
        public LEDStateCollection LedStates = new LEDStateCollection();
        public LEDGroupStateCollection LedGroupStates = new LEDGroupStateCollection();
        public LedStripBunnyService()
        {
            LedGroupStates.UpdateFromList(ProgramConstants.LedGroups);
            for (int i = 0; i < ledCount; i++)
            {
                LedStates.Add(new LEDState(i));
            }
        }
        public ref LEDStateCollection GetLedStates()
        {
            return ref LedStates;
        }

        public ref LEDGroupStateCollection GetLedGroups()
        {
            return ref LedGroupStates;
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

        public void SetLedGroup(LedGroup group)
        {
            Console.WriteLine($"State Color: {group.GroupState.LedColor}");
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
                    Console.WriteLine($"State color before: {group.GroupState.LedColor}");
                    ledState.LedColor = ColorTranslator.FromHtml(group.GroupState.LedColor);
                    ledState.Brightness = group.GroupState.Brightness;
                    Console.WriteLine($"State Color after: {ledState.LedColor}");
                    SetLed(ledState);        
                }
            }
        }


    }
}
