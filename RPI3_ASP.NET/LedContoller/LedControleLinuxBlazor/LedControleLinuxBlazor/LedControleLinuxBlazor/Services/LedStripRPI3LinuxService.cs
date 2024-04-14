using System.Device.Gpio.Drivers;
using System.Device.Spi;
using System.Drawing;
using Iot.Device.Ws28xx;
using LedControleLinuxBlazor.Const;
using rpi_ws281x;
using System;
using System.Device.Spi;
using System.Drawing;
using Iot.Device.Graphics;
using Iot.Device.Ws28xx;
using LedControleLinuxBlazor.Model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using LedControleLinuxBlazor.Extensions;

namespace LedControleLinuxBlazor.Services
{
    public class LedStripRPI3LinuxService : ILedStripService
    {
        //private SpiConnectionSettings settings;
        private rpi_ws281x.Settings settings;
        public LEDStateCollection LedStates = new LEDStateCollection();
        public LEDGroupStateCollection LedGroupStates = new LEDGroupStateCollection();
        private int LedCount = ProgramConstants.LedCount;
        private int LedControlPin = ProgramConstants.LedControlPin;
        private SpiDevice spi;
        //  private Ws2812b device;
        private WS281x device;
        public LedStripRPI3LinuxService()
        {
            LedGroupStates.UpdateFromList(ProgramConstants.LedGroups);
            for (int i = 0; i < LedCount; i++)
            {
                LedStates.Add(new LEDState(i));
            }
            settings = Settings.CreateDefaultSettings();
            settings.Channels[0] = new Channel(LedCount, ProgramConstants.LedControlPin, 255, false, StripType.WS2812_STRIP);
            Start();
        }

        private void Start()
        {
            using (var controller = new WS281x(settings))
            {
                for (int i = 0; i < LedCount; i++)
                {
                    controller.SetLEDColor(0, i, Color.Green);
                    controller.Render();
                    Thread.Sleep(10);
                    controller.SetLEDColor(0, i, Color.Black);
                    controller.Render();
                }
                Thread.Sleep(100);
                for (int i = 0; i < LedCount; i++)
                {
                    controller.SetLEDColor(0, i, Color.Green);
                }
                controller.Render();
                Thread.Sleep(100);
                for (int i = 0; i < LedCount; i++)
                {
                    controller.SetLEDColor(0, i, Color.Black);
                }
                controller.Render();

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
             
        public void SetLed(LEDState led)
        {
            using (var rpi = new WS281x(settings))
            {
                rpi.SetLEDColor(0, led.LedNumber, led.LedColor);
                rpi.Render();
            }
        }
        public void SetLeds(List<LEDState> leds)
        {
            using (var rpi = new WS281x(settings))
            {
                foreach(LEDState led in leds)
                {
                    rpi.SetLEDColor(0, led.LedNumber, led.LedColor);
                }
                rpi.Render();
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
        public void SetLedGroup(LedGroup group)
        {
            using (var rpi = new WS281x(settings))
            {
                foreach (LEDState led in LedStates)
                {
                    rpi.SetLEDColor(0, led.LedNumber, Color.Black);
                }
                foreach (var ledIndex in group.LedIndexs)
                {
                    LEDState? ledState = LedStates.FirstOrDefault(led => led.LedNumber == ledIndex);
                    if (ledState != null)
                    {
                        ledState.LedColor = ColorTranslator.FromHtml(group.GroupState.LedColor);
                        ledState.Brightness = group.GroupState.Brightness;
                        rpi.SetLEDColor(0, ledState.LedNumber, ledState.LedColor);
                    }
                }
                rpi.Render();
            }
            

            
        }

    }
}
