using AuctionService;
using Grpc.Net.Client;

namespace BiddingService;

public class GrpcAuctionClient(ILogger<GrpcAuctionClient> logger,
    IConfiguration configuration)
{
    public async Task<Auction> GetAuction(string id) 
    {
        logger.LogInformation("==========> GetAuctionGrpc called with id: {id}", id);
        var channel = GrpcChannel.ForAddress(configuration["GrpcAuction"] ?? string.Empty);
        var client = new GrpcAuction.GrpcAuctionClient(channel);
        var request = new GetAuctionRequest { Id = id };

        try
        {
            var rely = await client.GetAuctionAsync(request);
            var auction = new Auction
            {
                ID = rely.Auction.Id,
                AuctionEnd = DateTime.Parse(rely.Auction.AuctionEnd),
                Seller = rely.Auction.Seller,
                ReservePrice = rely.Auction.ReservePrice
            };

            return auction;
        }
        catch (Exception ex)
        {
            logger.LogError("Error in GetAuctionGrpc: {ex}", ex.Message);
            return null;
        }
    }
}
