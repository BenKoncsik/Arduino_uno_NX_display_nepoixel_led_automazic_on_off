using LedControleLinuxBlazor.Client.Components;
using LedControleLinuxBlazor.Client.Components.SplashScreen;
using LedControleLinuxBlazor.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;
using Microsoft.FluentUI.AspNetCore.Components;
using DialogService = Microsoft.FluentUI.AspNetCore.Components.DialogService;
using Microsoft.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<ITooltipService, TooltipService>();
builder.Services.AddScoped<IDialogService, DialogService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IToastService, ToastService>();
builder.Services.AddScoped<NavigationManager>();
builder.Services.AddScoped<ISocketService, SocketService>();
builder.Services.AddScoped<IComponentMsgService, ComponentMsgService>();
builder.Services.AddScoped<SplashScreen>();


await builder.Build().RunAsync();
