using LedControleLinuxBlazor.Model;

namespace LedControleLinuxBlazor.Const
{
    public static class ProgramConstants
    {
        public static int LedCount { get; private set; }
        public static int LedControlPin { get; private set; }
        public static List<LedGroup> LedGroups { get; private set; }


        private static bool isInit = false;
        public static void Init(int ledCount, int ledControlPin, List<LedGroup> ledGroups)
        {
            if (!isInit)
            {
                LedCount = ledCount;
                LedControlPin = ledControlPin;
                LedGroups = ledGroups;
                isInit = true;
            }
        }
    }
}
