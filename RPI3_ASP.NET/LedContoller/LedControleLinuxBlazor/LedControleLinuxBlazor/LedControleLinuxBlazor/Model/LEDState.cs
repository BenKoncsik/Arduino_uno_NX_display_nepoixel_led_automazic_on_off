using Iot.Device.BrickPi3.Sensors;
using System.Drawing;
using Iot.Device.Graphics;
using Iot.Device.Ws28xx;
using Color = System.Drawing.Color;
using System.ComponentModel;
using LedControleLinuxBlazor.Client.Model;
using System.Xml;


namespace LedControleLinuxBlazor.Model
{
    public class LEDState : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int LedNumber { get; set; }
        private Color _ledColor;
        public Color LedColor
        {
            get => _ledColor;
            set
            {
                if (_ledColor != value)
                {
                    _ledColor = value;
                    OnPropertyChanged(nameof(LedColor));
                }
            }
        }

        private float brightness = 1;

        public float Brightness
        {
            get => brightness;
            set
            {
                if (value != brightness)
                {
                    brightness = value;
                    UpdateLedBrightness();
                    OnPropertyChanged(nameof(Brightness));
                }
            }
        }

        public LEDState(int i)
        {
            LedNumber = i;
            Brightness = 0;
            LedColor = Color.Black;
        }
        public LEDState(LEDStateJsonModel led)
        {
            LedNumber = led.LedNumber;
            Brightness = led.Brightness;
            SetColorFromHtml(led.LedColor);

        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void SetColorRGB(int r, int g, int b)
        {
            LedColor = Color.FromArgb(r, g, b);
        }
        public void SetColorFromHtml(string color)
        {
            LedColor = ColorTranslator.FromHtml(color);
        }

        private void UpdateLedBrightness()
        {
            Color newColor = Color.FromArgb(
                _ledColor.A,
                (byte)(_ledColor.R * brightness),
                (byte)(_ledColor.G * brightness),
                (byte)(_ledColor.B * brightness)
            );
            LedColor = newColor;
        }

        public LEDStateJsonModel ConvertToLEDJsonModelState()
        {
            return new LEDStateJsonModel
            {
                LedNumber = this.LedNumber,
                LedColor = ColorTranslator.ToHtml(this.LedColor),
                Brightness = this.Brightness,
            };
        }

       
    }
}
