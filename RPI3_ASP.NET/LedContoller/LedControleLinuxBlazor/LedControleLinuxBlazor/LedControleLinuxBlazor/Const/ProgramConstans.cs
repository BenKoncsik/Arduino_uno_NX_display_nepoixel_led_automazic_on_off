namespace LedControleLinuxBlazor.Const
{
    public static class ProgramConstans
    {
        public static int LedCount { get; private set; }

        private static bool isInit = false;
        public static void Init(int ledCount)
        {
            if (!isInit)
            {
                LedCount = ledCount;
                isInit = true;
            }
        }
    }
}
