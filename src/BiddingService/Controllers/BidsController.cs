using AutoMapper;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace BiddingService;

[ApiController]
[Route("api/[controller]")]
public class BidsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly GrpcAuctionClient _auctionClient;
    public BidsController(IMapper mapper, IPublishEndpoint publishEndpoint, GrpcAuctionClient auctionClient)
    {
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _auctionClient = auctionClient;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PlaceBid(string auctionId, int amount)
    {
        var aution = await DB.Find<Auction>().OneAsync(auctionId);

        if (aution == null)
        {
            aution = await _auctionClient.GetAuction(auctionId);
            if (aution == null)
            {
                return BadRequest("Cannot accept bid for auction at this time. Please try again later.");
            }
        }

        if (aution.Seller == User.Identity.Name)
        {
            return BadRequest("You can't bid on your own auction");
        }

        var bid = new Bid
        {
            AuctionId = auctionId,
            Amount = amount,
            Bidder = User.Identity.Name
        };

        if (aution.AuctionEnd < DateTime.UtcNow)
        {
            bid.BidStatus = BidStatus.Finished;
        }
        else
        {
            var highBid = await DB.Find<Bid>()
                .Match(b => b.AuctionId == auctionId)
                .Sort(b => b.Descending(x => x.Amount))
                .ExecuteFirstAsync();

            if (highBid is not null && amount > highBid.Amount || highBid is null)
            {
                bid.BidStatus = amount > aution.ReservePrice
                ? BidStatus.Accepted
                : BidStatus.AcceptedBelowReserve;
            }

            if (highBid is not null && bid.Amount <= highBid.Amount)
            {
                bid.BidStatus = BidStatus.TooLow;
            }
        }

        await DB.SaveAsync(bid);
        await _publishEndpoint.Publish(_mapper.Map<BidPlaced>(bid));

        return Ok(_mapper.Map<BidDto>(bid));
    }

    [HttpGet("{auctionId}")]
    public async Task<IActionResult> GetBidsForAuction(string auctionId)
    {
        var bids = await DB.Find<Bid>()
            .Match(b => b.AuctionId == auctionId)
            .Sort(b => b.Descending(x => x.BidTime))
            .ExecuteAsync();

        return Ok(_mapper.Map<IReadOnlyCollection<BidDto>>(bids));
    }

}
