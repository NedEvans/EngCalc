using EngineeringCalc.Components;
using EngineeringCalc.Data;
using EngineeringCalc.Services;
using Microsoft.EntityFrameworkCore;

// Load environment variables from .env file
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register calculation engine
builder.Services.AddScoped<CalculationEngine>();

// Configure database context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
connectionString = connectionString?
    .Replace("${DB_SERVER}", Environment.GetEnvironmentVariable("DB_SERVER"))
    .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME"))
    .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER"))
    .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD"));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Seed database with sample data and app constants
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    // Seed AppConstants first (so SeedData can reference them)
    await AppConstantSeeder.SeedAppConstants(context);

    // Seed sample card data with AppConstant bindings
    await SeedData.Initialize(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run("http://0.0.0.0:5227");
