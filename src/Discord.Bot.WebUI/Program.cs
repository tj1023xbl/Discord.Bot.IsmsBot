using Discord.Bot.Database;
using Discord.Bot.Database.Repositories;
using Discord.Bot.WebUI.Data.JsonConverters;
using Discord.Bot.WebUI.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Serilog;
using Serilog.Extensions.Logging;

SetupLogging();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
SemaphoreSlim databaseSemaphore = new SemaphoreSlim(1, 1);

builder.Services.AddControllersWithViews().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new ULongJsonConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDBContext>();
builder.Services.AddScoped((services) => new SayingRepository(services.GetService<AppDBContext>(), databaseSemaphore));
builder.Services.AddScoped<IsmsService>();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(opt => opt.WithHeaders(new string[] { "GET", "PUT", "POST", "DELETE", "OPTIONS" }));

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();


/// <summary>
/// Sets up logging options.
/// </summary>
 void SetupLogging()
{
    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "IsmsBotUI", "IsmsBotUILog.log");
    Log.Logger = Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(path, rollingInterval: RollingInterval.Month, rollOnFileSizeLimit: true, fileSizeLimitBytes: 1024 * 1024 * 10)
    .CreateLogger();

    Log.Information("Logs will be stored at {0}", path);

}
