using LedContoller.Const;
using LedContoller.Socket;
using LedContollerBlazor.Components;
using LedContollerBlazor.Services;
using LedController.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddRazorPages(); // Add this line to register Razor Pages services
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

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

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
    .AddAdditionalAssemblies(typeof(LedContollerBlazor.Client._Imports).Assembly);

app.Run();
