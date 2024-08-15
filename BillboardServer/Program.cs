using BillboardServer.Data;
using BillboardServer.Services;
using BillboardServer.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BillboardDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<BillboardRepository>();
builder.Services.AddSingleton<MessageQueueService>();
builder.Services.AddSingleton<MessageDisplayService>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();

app.MapControllers();

var messageDisplayService = app.Services.GetRequiredService<MessageDisplayService>();
app.Lifetime.ApplicationStopping.Register(() => messageDisplayService.Stop().GetAwaiter().GetResult());

app.Run();