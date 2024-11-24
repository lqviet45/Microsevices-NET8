using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

public abstract class AuctionCreatedConsumer(IHubContext<NotificationHub> hubContext) : IConsumer<AuctionCreated>
{
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;

    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine($"=========> Auction created mess receive: {context.Message.Id}");
        
        await _hubContext.Clients.All.SendAsync("AuctionCreated", context.Message);

    }
}
