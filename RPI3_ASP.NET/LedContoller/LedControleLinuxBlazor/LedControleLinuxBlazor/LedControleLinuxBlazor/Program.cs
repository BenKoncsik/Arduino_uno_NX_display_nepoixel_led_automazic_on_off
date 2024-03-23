using LedControleLinuxBlazor.Components;
using LedControleLinuxBlazor.Const;
using LedControleLinuxBlazor.Services;
using LedControleLinuxBlazor.Socket;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddRazorPages(); 
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddSignalR();
ProgramConstans.Init(builder.Configuration.GetSection("LedNumber").Get<int>());

#if DEBUG
builder.Services.AddSingleton<ILedStripService, LedStripWindowsService>();
#else
    builder.Services.AddSingleton<ILedStripService, LedStripRPI3LinuxService>();
#endif


var app = builder.Build();



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
