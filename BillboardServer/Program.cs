using BillboardServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MessageQueueService>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();

app.MapControllers();

var messageQueueService = app.Services.GetRequiredService<MessageQueueService>();
app.Lifetime.ApplicationStopping.Register(messageQueueService.Stop);

app.Run();