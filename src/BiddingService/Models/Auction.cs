﻿using MongoDB.Entities;

namespace BiddingService;

public class Auction : Entity
{
    public DateTime AuctionEnd { get; set; }
    public string Seller { get; set; }
    
}
