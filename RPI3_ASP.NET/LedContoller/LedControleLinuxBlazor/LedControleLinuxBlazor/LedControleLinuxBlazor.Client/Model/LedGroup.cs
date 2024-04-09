namespace LedControleLinuxBlazor.Model
{
    public class LedGroup
    {
        public string Name { get; set; }
        public List<int> LedIndexs { get; set; }
        public List<string> LedActions { get; set; }
    }
}
