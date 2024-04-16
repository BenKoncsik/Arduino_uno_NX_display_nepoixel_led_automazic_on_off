namespace LedControleLinuxBlazor.Client.Services
{
    public interface IComponentMsgService
    {
        event Func<Task> RefreshRequested;
        Task RefreshComponent();
    }
}
