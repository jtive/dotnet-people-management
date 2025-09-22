using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using PersonalInfoBlazor.Data;
using PersonalInfoBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Configure SignalR for AWS App Runner
builder.Services.AddSignalR(options =>
{
    // Disable WebSockets for App Runner compatibility
    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;
});

// Add HTTP client for API communication
builder.Services.AddHttpClient<IPersonalInfoApiService, PersonalInfoApiService>();

// Remove the weather service as we don't need it
// builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub(options =>
{
    // Configure Blazor Hub for App Runner
    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;
});
app.MapFallbackToPage("/_Host");

app.Run();
