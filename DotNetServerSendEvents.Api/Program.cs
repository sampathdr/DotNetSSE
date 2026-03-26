var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});


var app = builder.Build();
app.UseCors();
app.MapGet("/", () => "Hello World!");

app.MapGet("/stream", async (HttpContext context) =>
{
    context.Response.Headers.Add("Content-Type", "text/event-stream");

    while (!context.RequestAborted.IsCancellationRequested)
    {
        var data = $"data: {DateTime.Now}\n\n";
        await context.Response.WriteAsync(data);
        await context.Response.Body.FlushAsync();
        await Task.Delay(2000);
    }
});

app.Run();
