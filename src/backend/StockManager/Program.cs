using Microsoft.EntityFrameworkCore;
using StockManager.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = 
    builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException (
        "Connection String 'DefaultConnection' not found."
    );

var serverVersion = new MySqlServerVersion(new Version(8, 0, 45));

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseMySql(connectionString, serverVersion));    


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.MapGet("/health", () =>
{
    return new {Status = "healthy"};
});

app.Run();


