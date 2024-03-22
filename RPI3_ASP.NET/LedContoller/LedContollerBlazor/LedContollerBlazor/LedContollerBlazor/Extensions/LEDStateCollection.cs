using LedContoller.Model;
using LedContollerBlazor.Model;
using System.Collections.ObjectModel;
using System.Drawing;

namespace LedContollerBlazor.Extensions
{
        public class LEDStateCollection : ObservableCollection<LEDState>
        {
            public ObservableCollection<LEDStateJsonModel> TransformLEDJsonModel()
            {
                ObservableCollection<LEDStateJsonModel> leds = new ObservableCollection<LEDStateJsonModel>();
                foreach(var ledState in this)
                {
                    LEDStateJsonModel ledJson = new LEDStateJsonModel
                    {
                        LedNumber = ledState.LedNumber,
                        LedColor = ColorTranslator.ToHtml(ledState.LedColor),
                        Brightness = ledState.Brightness,
                    };
                    leds.Add(ledJson);
                }
                return leds;
            }
        }
}

