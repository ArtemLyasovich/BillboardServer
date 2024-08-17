using BillboardServer.Data;
using BillboardServer.Services;
using BillboardServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

using static BillboardServer.Consts.Consts;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/BillboardServer-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<BillboardDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<BillboardRepository>();
builder.Services.AddSingleton<MessageQueueService>();
builder.Services.AddSingleton<MessageDisplayService>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();

app.MapControllers();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
app.Lifetime.ApplicationStarted.Register(() =>
{
    logger.LogInformation("{0} - Application started at {1}", APPLICATION_STARTUP_LOGID, DateTime.UtcNow);
});

app.Lifetime.ApplicationStopping.Register(() =>
{
    var messageDisplayService = app.Services.GetRequiredService<MessageDisplayService>();
    messageDisplayService.Stop().GetAwaiter().GetResult();

    logger.LogInformation("{0} - Application stopped at {1}", APPLICATION_SHUTDOWN_LOGID, DateTime.UtcNow);
});

app.Run();

Log.CloseAndFlush();