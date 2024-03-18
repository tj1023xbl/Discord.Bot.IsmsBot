using Discord.Bot.Database;
using Discord.Bot.Database.Models;
using Discord.Bot.Database.Repositories;
using Discord.Bot.WebUI.Data.JsonConverters;
using Discord.Bot.WebUI.Services;
using Serilog;
using Microsoft.AspNetCore.Identity;


SetupLogging();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
SemaphoreSlim databaseSemaphore = new SemaphoreSlim(1, 1);

builder.Services.AddControllersWithViews().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new ULongJsonConverter());
});

// SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DATABASE
builder.Services.AddDbContext<AppDBContext>();
builder.Services.AddAuthorization();

// AUTHENTICATION
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<AppDBContext>();

// SERVICES
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

SetUpSwagger(app);

// Authorization
// app.MapGroup("/api/account").MapIdentityApi<IdentityUser>();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UsePathBase(new PathString("/api"));
app.UseRouting();
app.UseAuthorization();

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

void SetUpSwagger(WebApplication app)
{
    string blah = string.Empty;
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger(c =>
        {
            c.RouteTemplate = "api/swagger/{documentname}/swagger.json";
        });

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "My Cool API V1");
            c.RoutePrefix = "api/swagger";

            blah = $"Swagger URLS: {string.Join(",", c.ConfigObject.Urls.Select(u => u.Url))}";
        });

    }
    Log.Information("Swagger URLS: {0}", blah);

}