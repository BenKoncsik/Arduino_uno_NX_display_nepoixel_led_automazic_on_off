using LedControleLinuxBlazor.Client.Components;
using LedControleLinuxBlazor.Client.Components.SplashScreen;
using LedControleLinuxBlazor.Client.Model;
using LedControleLinuxBlazor.Client.Services;
using LedControleLinuxBlazor.Components;
using LedControleLinuxBlazor.Const;
using LedControleLinuxBlazor.Model;
using LedControleLinuxBlazor.Services;
using LedControleLinuxBlazor.Socket;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;
using DialogService = Microsoft.FluentUI.AspNetCore.Components.DialogService;

var options = new WebApplicationOptions
{
#if RELEASE
    ContentRootPath = "/home/rpi_3/server_portable/",
    WebRootPath = "/home/rpi_3/server_portable/wwwroot",
#endif
    Args = args
};

var builder = WebApplication.CreateBuilder(options);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    int port = builder.Configuration.GetSection("port").Get<int>();
    serverOptions.Listen(System.Net.IPAddress.Any, port == null ? 80 : port);
});

//fluent
builder.Services.AddHttpClient();
builder.Services.AddFluentUIComponents();
builder.Services.AddScoped<ITooltipService, TooltipService>();
builder.Services.AddScoped<IDialogService, DialogService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IToastService, ToastService>();



// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddRazorPages(); 
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddSignalR();
ProgramConstants.Init(
   ledCount: builder.Configuration.GetSection("LedNumber").Get<int>(),
   ledControlPin: builder.Configuration.GetSection("LedControlPin").Get<int>(),
    ledGroups: builder.Configuration.GetSection("LedGroups").Get<List<LedGroup>>() ?? new List<LedGroup>()
    );

builder.Services.AddScoped<ISocketService, SocketService>();
builder.Services.AddScoped<IComponentMsgService, ComponentMsgService>();
builder.Services.AddScoped<SplashScreen>();


#if DEBUG
builder.Services.AddSingleton<ILedStripService, LedStripBunnyService>();
#else
 builder.Services.AddSingleton<ILedStripService, LedStripRPI3LinuxService>();
#endif

var app = builder.Build();
app.Urls.Add("http://localhost");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}


/*app.UseStaticFiles();
app.UseAntiforgery();*/

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
    endpoints.MapHub<LedControlHub>("/ledControlHub");
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(LedControleLinuxBlazor.Client._Imports).Assembly);

app.Run();
