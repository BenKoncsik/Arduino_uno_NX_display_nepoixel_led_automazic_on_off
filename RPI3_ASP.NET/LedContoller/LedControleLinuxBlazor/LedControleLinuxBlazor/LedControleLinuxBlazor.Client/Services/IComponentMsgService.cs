namespace LedControleLinuxBlazor.Client.Services
{
    public interface IComponentMsgService
    {
        event Func<Task> RefreshRequested;
        public event Action<string> ComponentLoaded;
        Task RefreshComponent();
        void OnLoaded(string componentName);
    }
}
