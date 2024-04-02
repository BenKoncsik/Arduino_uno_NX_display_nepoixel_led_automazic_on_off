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
        private int ledCount = ProgramConstans.LedCount;
        public LEDStateCollection LedStates = new LEDStateCollection();
        private SpiDevice spi;
        //  private Ws2812b device;
        private WS281x device;
        public LedStripRPI3LinuxService()
        {
            for (int i = 0; i < ledCount; i++)
            {
                LedStates.Add(new LEDState(i));
            }
            LedStates.CollectionChanged += LedStates_CollectionChanged;
            // BusId 0, ChipSelectLine 0 (CE0 - GPIO 8)
            /*settings = new SpiConnectionSettings(0, 0) 
            {
                // A WS2812B számára szükséges órajel frekvencia
                ClockFrequency = 2_400_000,
                // Az SPI mód
                Mode = SpiMode.Mode0,
                // Az adatbitek hossza
                DataBitLength = 8 
            };*/
            settings = Settings.CreateDefaultSettings();
            //spi = SpiDevice.Create(settings);
            //device = new Ws2812b(spi, ledCount);
            settings.Channels[0] = new Channel(ledCount, 10, 255, false, StripType.WS2812_STRIP);
            // device = new WS281x(settings);
            Start();
        }

        private void Start()
        {
            using (var controller = new WS281x(settings))
            {
                for (int i = 0; i < ledCount; i++)
                {
                    controller.SetLEDColor(0, i, Color.Green);
                    controller.Render();
                    Thread.Sleep(10);
                    controller.SetLEDColor(0, i, Color.Black);
                    controller.Render();
                }
                Thread.Sleep(100);
                for (int i = 0; i < ledCount; i++)
                {
                    controller.SetLEDColor(0, i, Color.Green);
                }
                controller.Render();
                Thread.Sleep(100);
                for (int i = 0; i < ledCount; i++)
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
        public void SetLed(LEDState led)
        {
            /*var image = device.Image;
            image.SetPixel(0, led.LedNumber, led.LedColor);
            device.Image.SetPixel(0, led.LedNumber, led.LedColor);
            device.Update();*/
            using (var rpi = new WS281x(settings))
            {
                Console.WriteLine($"Sett led: {led.LedNumber} color: {led.LedColor}");
                rpi.SetLEDColor(0, led.LedNumber, led.LedColor);
                rpi.Render();
            }
        }

        public ref LEDStateCollection GetLedStates()
        {
            return ref LedStates;
        }
       
    }
}
