using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public abstract class BidPlacedConsumer : IConsumer<BidPlaced>
{
    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        Console.WriteLine($"----------------> BidPlacedConsumer: {context.Message.AuctionId}");
        var aution = await DB.Find<Item>()
            .OneAsync(context.Message.AuctionId);

        if (context.Message.BidStatus.Contains("Accepted")
        && context.Message.Amount > aution.CurrentHighBid)
        {
            aution.CurrentHighBid = context.Message.Amount;
            await aution.SaveAsync();
        }
    }
}
