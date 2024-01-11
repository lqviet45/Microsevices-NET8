using System.Text.Json;
using System.Text.Json.Serialization;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Data;

public class DbInitalizer
{
    public static async Task InitDb(WebApplication app)
    {

        await DB.InitAsync("SerachDb", MongoClientSettings
            .FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Item>()
            .Key(i => i.Make, KeyType.Text)
            .Key(i => i.Model, KeyType.Text)
            .Key(i => i.Color, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Item>();

        using var scope = app.Services.CreateScope();

        var httpClient = scope.ServiceProvider.GetRequiredService<AuctionServicesHttpClient>();

        var item = await httpClient.GetItemForSearchDb();

        Console.WriteLine("Items from AuctionService: " + item.Count);

        if (item.Count > 0) 
        {
            await DB.SaveAsync(item);
        }
    }
}
