using LedControleLinuxBlazor.Model;

namespace LedControleLinuxBlazor.Client.Model
{
    public class LedGroupBundle
    {
        
        public LEDStateJsonModel LedState { get; set; }
        public LedGroup LedGroup { get; set; }
        public LedGroupBundle(LedGroup ledGroup)
        {
            LedState = new LEDStateJsonModel();
            LedGroup = ledGroup;
        }
    }
}
