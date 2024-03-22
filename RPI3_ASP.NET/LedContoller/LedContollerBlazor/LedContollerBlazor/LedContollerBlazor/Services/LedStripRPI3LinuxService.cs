using System.Device.Gpio.Drivers;
using System.Device.Spi;
using System.Drawing;
using Iot.Device.Ws28xx;
using LedContoller.Const;
using rpi_ws281x;
using System;
using System.Device.Spi;
using System.Drawing;
using Iot.Device.Graphics;
using Iot.Device.Ws28xx;
using LedContoller.Model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using LedContollerBlazor.Extensions;

namespace LedController.Services
{
    public class LedStripRPI3LinuxService : ILedStripService
    {
        private SpiConnectionSettings settings;
        private int ledCount = ProgramConstans.LedCount;
        public LEDStateCollection LedStates = new LEDStateCollection();
        private SpiDevice spi;
        private Ws2812b device;
        public LedStripRPI3LinuxService()
        {
            for (int i = 0; i < ledCount; i++)
            {
                LedStates.Add(new LEDState(i));
            }
            LedStates.CollectionChanged += LedStates_CollectionChanged;
            // Bus 0, Chip Select 0
            settings = new SpiConnectionSettings(0, 0) 
            {
                // A WS2812B számára szükséges órajel frekvencia
                ClockFrequency = 2_400_000,
                // Az SPI mód
                Mode = SpiMode.Mode0,
                // Az adatbitek hossza
                DataBitLength = 8 
            };
            spi = SpiDevice.Create(settings);
            device = new Ws2812b(spi, ledCount);
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
            device.Image.SetPixel(led.LedNumber, 0, led.LedColor);
            device.Update();
        }

        public ref LEDStateCollection GetLedStates()
        {
            return ref LedStates;
        }
       
    }
}
