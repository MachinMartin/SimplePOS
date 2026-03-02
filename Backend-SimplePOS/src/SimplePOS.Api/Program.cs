using Microsoft.EntityFrameworkCore;
using SimplePOS.Api.Middleware;
using SimplePOS.Application.ProductGroups;
using SimplePOS.Application.Products;
using SimplePOS.Infrastructure;
using SimplePOS.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ProductGroupService>();

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Middleware
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

// Apply migrations on startup (with retry for Docker)
await ApplyMigrationsWithRetryAsync(app);

app.Run();

static async Task ApplyMigrationsWithRetryAsync(WebApplication app)
{
    const int maxRetries = 10;
    var delay = TimeSpan.FromSeconds(2);

    for (var attempt = 1; attempt <= maxRetries; attempt++)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.MigrateAsync();
            return;
        }
        catch when (attempt < maxRetries)
        {
            await Task.Delay(delay);
        }
    }
}