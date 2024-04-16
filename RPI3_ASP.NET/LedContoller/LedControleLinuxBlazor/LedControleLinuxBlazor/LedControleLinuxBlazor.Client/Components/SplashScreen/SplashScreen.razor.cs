using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.AspNetCore.Components;

namespace LedControleLinuxBlazor.Client.Components.SplashScreen
{
    public partial class SplashScreen
    {

        private IDialogReference? _dialog;
        private IDialogService dialogService;

        public SplashScreen(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }
        public async Task OpenSplashDefaultAsync(string title, string subtitle = "", string message = "")
        {
            DialogParameters<SplashScreenContent> parameters = new()
            {
                Content = new()
                {
                    DisplayTime = 0,
                    Title = title,
                    SubTitle = subtitle,
                    LoadingText = "Loading...",
                    Message = (MarkupString)message,
                    Logo = FluentSplashScreen.LOGO,
                },
                Width = "640px",
                Height = "480px",
            };
            _dialog = await dialogService.ShowSplashScreenAsync(parameters);
        }

        public async void Close()
        {
            if (_dialog != null)
            {
                await _dialog.CloseAsync();

                DialogResult result = await _dialog.Result;
                await HandleDefaultSplashAsync(result);
            }
        }

        public async void UpdateScreen(string msg)
        {
            if (_dialog != null)
            {
                var splashScreen = (SplashScreenContent)_dialog.Instance.Content;
                splashScreen.UpdateLabels(loadingText: msg);
            }
        }
        private void OpenSplashDefault()
        {
            DialogParameters<SplashScreenContent> parameters = new()
            {
                Content = new()
                {
                    Title = "Core components",
                    SubTitle = "Microsoft Fluent UI Blazor library",
                    LoadingText = "Loading...",
                    Message = (MarkupString)"some <i>extra</i> text <strong>here</strong>",
                    Logo = FluentSplashScreen.LOGO,
                },
                Width = "640px",
                Height = "480px",
            };
            DialogService.ShowSplashScreen(this, HandleDefaultSplashAsync, parameters);
        }

        private async Task HandleDefaultSplashAsync(DialogResult result)
        {

        }
    }
}
