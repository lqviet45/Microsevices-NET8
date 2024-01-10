using MongoDB.Driver;
using MongoDB.Entities;
using SearchService;
using SearchService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

try 
{
    await DbInitalizer.InitDb(app);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}
    
app.Run();