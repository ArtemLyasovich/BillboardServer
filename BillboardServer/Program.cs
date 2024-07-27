using BillboardServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MessageQueueService>();
builder.Services.AddSingleton<MessageDisplayService>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();

app.MapControllers();

var messageDisplayService = app.Services.GetRequiredService<MessageDisplayService>();
app.Lifetime.ApplicationStopping.Register(messageDisplayService.Stop);

app.Run();