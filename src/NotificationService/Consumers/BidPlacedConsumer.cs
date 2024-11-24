using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

public abstract class BidPlacedConsumer(IHubContext<NotificationHub> hubContext) : IConsumer<BidPlaced>
{
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;

    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        Console.WriteLine("=========> Bid placed mess receive");
        
        await _hubContext.Clients.All.SendAsync("BidPlaced", context.Message);
    }
}
