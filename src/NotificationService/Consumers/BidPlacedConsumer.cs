using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace NotificationService;

public class BidPlacedConsumer(IHubContext<NotificationHub> hubContext) : IConsumer<BidPlaced>
{
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;

    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        Console.WriteLine("=========> Bid placed mess receive");
        
        await _hubContext.Clients.All.SendAsync("BidPlaced", context.Message);
    }
}
