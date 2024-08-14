using WeatherDashboard.Components;
using WeatherDashboard.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

//var weatherPageApiUrl = builder.Configuration["WeatherPageApiUrl"] ?? throw new Exception("Url is not set");

builder.Services.AddSingleton<TransferService>();
//builder.Services.AddHttpClient("SMHI Forecasts API", sp => new HttpClient { BaseAddress = new Uri("https://opendata-download-metfcst.smhi.se") });
builder.Services.AddHttpClient<WeatherService>(client => 
{
    client.BaseAddress = new Uri("https://opendata-download-metfcst.smhi.se"); 
});
builder.Services.AddSingleton<CitiesService>();

var app = builder.Build();

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

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(WeatherDashboard.Client._Imports).Assembly);

app.Run();
