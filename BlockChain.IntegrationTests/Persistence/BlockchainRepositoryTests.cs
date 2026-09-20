using BlockChain.Domain.Entities;
using BlockChain.Domain.Enums;
using BlockChain.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BlockChain.IntegrationTests.Persistence;

public sealed class BlockchainRepositoryTests : IClassFixture<DatabaseFixture>, IAsyncLifetime
{
    private readonly DatabaseFixture _fixture;

    public BlockchainRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task AddEntryAsync_Should_Save_Blockchain()
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

        var repository = new BlockchainRepository(_fixture.DbContext);

        // Act
        await repository.AddEntryAsync(blockchain);
        await _fixture.DbContext.SaveChangesAsync(CancellationToken.None);

        // Assert
        var result = await _fixture.DbContext.Blockchains.SingleOrDefaultAsync(CancellationToken.None);

        result!.Name.Should().Be("Bitcoin");
        result.BlockchainName.Should().Be(BlockchainName.BTC);
        result.Height.Should().Be(100);
    }

    [Fact]
    public async Task GetHistoryByBlockchainNameAsync_Should_Return_Filtered_And_Ordered_History()
    {
        // Arrange
        var older = Blockchain.Create(
            "Bitcoin",
            BlockchainName.BTC,
            100,
            "hash-1",
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
            DateTime.UtcNow.AddMinutes(-10));

        var newer = Blockchain.Create(
            "Bitcoin",
            BlockchainName.BTC,
            200,
            "hash-2",
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

        var ethereum = Blockchain.Create(
            "Ethereum",
            BlockchainName.ETH,
            300,
            "hash-3",
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
            DateTime.UtcNow.AddMinutes(10));

        await _fixture.DbContext.Blockchains.AddRangeAsync(older, newer, ethereum);

        await _fixture.DbContext.SaveChangesAsync(CancellationToken.None);

        var repository = new BlockchainRepository(_fixture.DbContext);

        // Act
        var result = await repository.GetHistoryByBlockchainNameAsync(BlockchainName.BTC);

        // Assert
        result.Should().HaveCount(2);

        result.Should().OnlyContain(x => x.BlockchainName == BlockchainName.BTC);

        result[0].CreatedAt.Should().BeAfter(result[1].CreatedAt);
    }

    [Fact]
    public async Task GetHistoryByBlockchainNameAsync_Should_Return_Empty_When_No_History_Exists()
    {
        // Arrange
        var repository = new BlockchainRepository(_fixture.DbContext);

        // Act
        var result = await repository.GetHistoryByBlockchainNameAsync(
            BlockchainName.BTC);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public async ValueTask InitializeAsync()
    {
        await _fixture.ResetAsync();
    }
}