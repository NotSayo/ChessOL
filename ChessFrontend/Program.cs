using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ChessFrontend;
using ChessFrontend.ServerAccess;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices(options =>
    options.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft);

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());

builder.Services.AddScoped(sp => new HttpClient { BaseAddress =new Uri("http://localhost:7000")});
builder.Services.AddScoped<ApiAccessor>();
builder.Services.AddBlazoredLocalStorage();



await builder.Build().RunAsync();