using System.Text.Json;
using System.Text.Json.Serialization;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService;

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

        if (count == 0) 
        {
            Console.WriteLine("No data - will attempt to seed");
            var itemData = await File.ReadAllTextAsync("Data/auctions.json");

#pragma warning disable CA1869 // Cache and reuse 'JsonSerializerOptions' instances
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
#pragma warning restore CA1869 // Cache and reuse 'JsonSerializerOptions' instances

            var items = JsonSerializer.Deserialize<List<Item>>(itemData, options);

            await DB.SaveAsync(items);
        }
    }
}
