﻿using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly IMapper _mapper;

    public AuctionCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine($"Received AuctionCreated event with id: {context.Message.Id}");
        var item = _mapper.Map<Item>(context.Message);

        if (item.Model == "Foo") throw new ArgumentException("Foo is not allowed");

        await item.SaveAsync();
    }
}