namespace LedControleLinuxBlazor.Client.Services
{
    public class ComponentMsgService : IComponentMsgService
    {
        public event Func<Task> RefreshRequested;
        public event Action<string> ComponentLoaded;

        public async Task RefreshComponent()
        {
            if (RefreshRequested != null)
            {
                await RefreshRequested.Invoke();
            }
        }

        public void OnLoaded(string componentName)
        {
            ComponentLoaded?.Invoke(componentName);
        }


    }
}
