using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public abstract class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        Console.WriteLine($"----------------> AuctionFinishedConsumer: {context.Message.AuctionId}");
        
        var aution = await DB.Find<Item>()
            .OneAsync(context.Message.AuctionId);

        if (context.Message.ItemSold) 
        {
            aution.Winner = context.Message.Winner;
            aution.SoldAmount = context.Message.Amount ?? 0;
        }

        aution.Status = "Finished";

        await aution.SaveAsync();
    }
}
