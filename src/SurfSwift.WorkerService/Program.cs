using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SurfSwift.Engine;
using SurfSwift.WorkerService;
using SurfSwift.WorkerService.Context;
using SurfSwift.WorkerService.ListDTO;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<ConfigurationListDTO>(
    builder.Configuration.GetSection("AppSetting"));

// Register it as a singleton if you want to inject the value directly later
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<ConfigurationListDTO>>().Value);

var configuration = builder.Configuration.GetSection("AppSetting").Get<ConfigurationListDTO>();

builder.Services.AddDbContext<SurfSwiftDbContext>(options =>
    options.UseSqlServer(configuration.ConnectionString));

builder.Services.AddWindowsService();

builder.Services.AddHostedService<Worker>();

// Registering Automation Engine
builder.Services.AddScoped<IAutomationEngine, AutomationEngine>();

//Registering Automaiton Engine
builder.Services.AddScoped<IAutomationEngine, AutomationEngine>();

var host = builder.Build();
host.Run();
