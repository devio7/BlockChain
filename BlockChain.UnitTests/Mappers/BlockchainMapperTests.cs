using BlockChain.Application.Features.Blockchain.GetBlockchainHistory;
using BlockChain.Domain.Entities;
using BlockChain.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace BlockChain.UnitTests.Mappers;

public sealed class BlockchainMapperTests
{
    [Fact]
    public void ToRecord_Should_Map_All_Properties()
    {
        // Arrange
        var createdAt = new DateTime(2026, 9, 19, 12, 30, 0);

        var entity = Blockchain.Create(
            "Bitcoin",
            BlockchainName.BTC,
            100,
            "hash",
            new DateTime(2026, 9, 18, 10, 0, 0).ToString(),
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
            createdAt);

        // Act
        var result = BlockchainMapper.ToRecord(entity);

        // Assert
        result.Should().NotBeNull();

        result.Id.Should().Be(entity.Id);
        result.Name.Should().Be(entity.Name);
        result.BlockchainName.Should().Be(entity.BlockchainName);
        result.Height.Should().Be(entity.Height);
        result.Hash.Should().Be(entity.Hash);
        result.Time.Should().Be(entity.Time);
        result.LatestUrl.Should().Be(entity.LatestUrl);
        result.PreviousHash.Should().Be(entity.PreviousHash);
        result.PreviousUrl.Should().Be(entity.PreviousUrl);
        result.PeerCount.Should().Be(entity.PeerCount);
        result.HighFeePerKb.Should().Be(entity.HighFeePerKb);
        result.MediumFeePerKb.Should().Be(entity.MediumFeePerKb);
        result.LowFeePerKb.Should().Be(entity.LowFeePerKb);
        result.UnconfirmedCount.Should().Be(entity.UnconfirmedCount);
        result.LastForkHeight.Should().Be(entity.LastForkHeight);
        result.LastForkHash.Should().Be(entity.LastForkHash);
        result.CreatedAt.Should().Be(entity.CreatedAt);
    }
}