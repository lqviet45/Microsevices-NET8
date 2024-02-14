using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace NotificationService;

public class AuctionCreatedConsumer(IHubContext<NotificationHub> hubContext) : IConsumer<AuctionCreated>
{
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;

    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine($"=========> Auction created mess receive: {context.Message.Id}");
        
        await _hubContext.Clients.All.SendAsync("AuctionCreated", context.Message);

    }
}
