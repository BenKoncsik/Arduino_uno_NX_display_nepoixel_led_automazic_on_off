namespace LedControleLinuxBlazor.Const
{
    public static class ProgramConstans
    {
        public static int LedCount { get; private set; }
        public static int LedControlPin { get; private set; }

        private static bool isInit = false;
        public static void Init(int ledCount, int ledControlPin)
        {
            if (!isInit)
            {
                LedCount = ledCount;
                LedControlPin = ledControlPin;
                isInit = true;
            }
        }
    }
}
