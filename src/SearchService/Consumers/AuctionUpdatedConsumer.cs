﻿using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService;

public abstract class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
{
    private readonly IMapper _mapper;

    protected AuctionUpdatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        Console.WriteLine($"Received AuctionUpdated event with id {context.Message.Id}");

        var item = _mapper.Map<Item>(context.Message);

        var result = await DB.Update<Item>()
            .Match(i => i.ID == context.Message.Id)
            .ModifyOnly(x => new 
            {
                x.Color,
                x.Make,
                x.Model,
                x.Year,
                x.Mileage
            }, item)
            .ExecuteAsync();

        if (!result.IsAcknowledged)
            throw new MessageException(typeof(AuctionUpdated), $"Could not update item with id {context.Message.Id}");
    }
}
