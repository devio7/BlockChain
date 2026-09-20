using BlockChain.Application.Features.Blockchain.GetBlockchainHistory;
using BlockChain.Domain.Entities;
using BlockChain.Domain.Enums;
using BlockChain.Domain.Interfaces;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace BlockChain.UnitTests.Handlers;

public sealed class GetBlockchainHistoryHandlerTests
{
    private readonly IBlockchainRepository _repository;
    private readonly GetBlockchainHistoryHandler _handler;

    public GetBlockchainHistoryHandlerTests()
    {
        _repository = Substitute.For<IBlockchainRepository>();
        _handler = new GetBlockchainHistoryHandler(_repository);
    }

    [Fact]
    public async Task Handle_Should_Call_Repository()
    {
        // Arrange
        _repository.GetHistoryByBlockchainNameAsync(BlockchainName.BTC)
                   .Returns([]);

        var query = new GetBlockchainHistoryQuery("BTC");

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _repository.Received(1)
                         .GetHistoryByBlockchainNameAsync(BlockchainName.BTC);
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_Result()
    {
        // Arrange
        _repository
            .GetHistoryByBlockchainNameAsync(BlockchainName.BTC)
            .Returns([]);

        var query = new GetBlockchainHistoryQuery("BTC");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Return_History_Items()
    {
        // Arrange
        var blockchain = Blockchain.Create(
            "Bitcoin",
            BlockchainName.BTC,
            100,
            "hash",
            DateTime.UtcNow.ToString(),
            "latest-url",
            "previous-hash",
            "previous-url",
            10,
            100,
            80,
            50,
            5,
            90,
            "fork-hash",
            DateTime.UtcNow);

        _repository.GetHistoryByBlockchainNameAsync(BlockchainName.BTC)
                   .Returns([blockchain]);

        var query = new GetBlockchainHistoryQuery("BTC");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(1);

        result.Items[0].Should().BeEquivalentTo(BlockchainMapper.ToRecord(blockchain));

        await _repository.Received(1).GetHistoryByBlockchainNameAsync(BlockchainName.BTC);
    }
}