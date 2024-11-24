using Contracts;
using MassTransit;

namespace AuctionService;

public abstract class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
{
    public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
        Console.WriteLine($"Received AuctionCreated fault with message: {context.Message.Message}");
        
        var exception = context.Message.Exceptions.First();

        if (exception.ExceptionType == typeof(ArgumentException).FullName)
        {
            context.Message.Message.Model = "FooBar";
            await context.Publish(context.Message.Message);
        }
        else 
        {
            Console.WriteLine($"Unhandled exception: {exception.ExceptionType}");
        }
    }
}
