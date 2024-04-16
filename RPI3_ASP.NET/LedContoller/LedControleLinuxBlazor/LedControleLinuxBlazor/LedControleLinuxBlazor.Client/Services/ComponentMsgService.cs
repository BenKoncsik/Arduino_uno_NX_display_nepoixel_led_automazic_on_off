namespace LedControleLinuxBlazor.Client.Services
{
    public class ComponentMsgService : IComponentMsgService
    {
        public event Func<Task> RefreshRequested;

        public async Task RefreshComponent()
        {
            if (RefreshRequested != null)
            {
                await RefreshRequested.Invoke();
            }
        }


    }
}
