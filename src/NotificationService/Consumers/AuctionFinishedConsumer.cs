using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace NotificationService;

public class AuctionFinishedConsumer(IHubContext<NotificationHub> hubContext) : IConsumer<AuctionFinished>
{
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;

    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        Console.WriteLine("=========> Auction finished mess receive");
        
        await _hubContext.Clients.All.SendAsync("AuctionFinished", context.Message);
    }
}
