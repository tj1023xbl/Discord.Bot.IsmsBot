using Discord.Bot.Database;
using Discord.Bot.Database.Repositories;
using Discord.Bot.WebUI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
SemaphoreSlim databaseSemaphore = new SemaphoreSlim(1, 1);

builder.Services.AddControllersWithViews();
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
