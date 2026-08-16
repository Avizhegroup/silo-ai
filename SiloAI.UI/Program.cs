using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SiloAI.Identity.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; });

builder.Services.AddRazorPages();

builder.Services.AddAuthenticationCore();

builder.Services.AddOptions();

builder.Services.AddScoped<ProtectedLocalStorage>();

builder.Services.AddScoped<AuthenticationStateProvider, SiloAuthenticationStateProvider>();

builder.Services.AddScoped(sp =>
    (SiloAuthenticationStateProvider)sp.GetRequiredService<AuthenticationStateProvider>());

builder.Services.AddScoped<IClaimManager, AiClaimManager>();

builder.Services.AddScoped<IAiAuthenticationService, AiAuthenticationService>();

builder.Services.AddScoped<AiApiClient>();

builder.Services.AddHttpClient("AiApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AiApi:BaseUrl"] ?? "http://localhost:5100/");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();

app.MapFallbackToPage("/_Host");

app.Run();
