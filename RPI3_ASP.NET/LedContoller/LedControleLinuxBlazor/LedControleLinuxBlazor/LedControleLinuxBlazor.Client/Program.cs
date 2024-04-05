using LedControleLinuxBlazor.Client.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<ITooltipService, TooltipService>();
builder.Services.AddScoped<IDialogService, DialogService>();
builder.Services.AddScoped<IMessageService, MessageService>();


await builder.Build().RunAsync();
