using Microsoft.EntityFrameworkCore;
using SurfSwift.Engine;
using SurfSwift.Infra;
using SurfSwift.WorkerService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService();

builder.Services.AddHostedService<Worker>();

var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "SurfSwift.db");

builder.Services.AddDbContext<SurfSwiftDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

//Registering Automaiton Engine
builder.Services.AddScoped<IAutomationEngine, AutomationEngine>();

var host = builder.Build();
host.Run();
