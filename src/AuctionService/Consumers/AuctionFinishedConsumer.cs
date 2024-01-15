using AuctionService.Data;
using AuctionService.Entities;
using Contracts;
using MassTransit;

namespace AuctionService;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    private readonly AuctionDbContext _dbContext;

    public AuctionFinishedConsumer(AuctionDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        Console.WriteLine($"------------> AuctionFinishedConsumer: {context.Message.AuctionId}");

        var aution = await _dbContext.Auctions.FindAsync(context.Message.AuctionId);
        
        if (context.Message.ItemSold)
        {
            aution.Winner = context.Message.Winner;
            aution.SoldAmount = context.Message.Amount;
        }

        aution.Status = aution.SoldAmount > aution.ReservePrice
        ? Status.Finished : Status.ReserveNotMet;

        await _dbContext.SaveChangesAsync();
    }
}
