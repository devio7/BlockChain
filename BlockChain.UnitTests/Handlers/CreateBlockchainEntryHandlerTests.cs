using BlockChain.Application.DTOs;
using BlockChain.Application.ExternalServices.Abstract;
using BlockChain.Application.Features.Blockchain.CreateBlockchainEntry;
using BlockChain.Domain.Enums;
using BlockChain.Domain.Interfaces;
using NSubstitute;
using Xunit;

namespace BlockChain.UnitTests.Handlers;

public sealed class CreateBlockchainEntryHandlerTests
{
    private readonly IBlockchainRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlockCypherClient _blockCypherClient;
    private readonly TimeProvider _timeProvider;

    private readonly CreateBlockchainEntryHandler _handler;

    public CreateBlockchainEntryHandlerTests()
    {
        _repository = Substitute.For<IBlockchainRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _blockCypherClient = Substitute.For<IBlockCypherClient>();
        _timeProvider = Substitute.For<TimeProvider>();

        _handler = new CreateBlockchainEntryHandler(
            _repository,
            _unitOfWork,
            _blockCypherClient,
            _timeProvider);
    }

    [Theory]
    [InlineData("BTC", BlockchainName.BTC)]
    [InlineData("btc", BlockchainName.BTC)]
    [InlineData("Eth", BlockchainName.ETH)]
    [InlineData("LTC", BlockchainName.LTC)]
    public async Task Handle_Should_Request_Correct_Blockchain(string blockchainName, BlockchainName expectedBlockchain)
    {
        // Arrange
        CancellationToken cancellationToken = CancellationToken.None;

        _blockCypherClient.GetBlockchainAsync(expectedBlockchain, cancellationToken)
                          .Returns(new BlockchainDTO());

        var command = new CreateBlockchainEntryCommand(blockchainName);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        await _blockCypherClient.Received(1).GetBlockchainAsync(expectedBlockchain, cancellationToken);
    }

    [Fact]
    public async Task Handle_Should_Add_Blockchain_Entity_To_Repository()
    {
        // Arrange
        CancellationToken cancellationToken = CancellationToken.None;

        var createdAt = new DateTimeOffset(
            2026, 9, 19, 15, 0, 0, TimeSpan.Zero);

        _timeProvider.GetUtcNow()
            .Returns(createdAt);

        var dto = new BlockchainDTO
        {
            Name = "Bitcoin",
            Height = 900000,
            Hash = "hash",
            Time = new DateTime(2026, 9, 19, 14, 0, 0).ToString(),
            LatestUrl = "latest",
            PreviousHash = "previous",
            PreviousUrl = "previous-url",
            PeerCount = 10,
            HighFeePerKb = 100,
            MediumFeePerKb = 80,
            LowFeePerKb = 50,
            UnconfirmedCount = 5,
            LastForkHeight = 899999,
            LastForkHash = "fork-hash"
        };

        _blockCypherClient.GetBlockchainAsync(BlockchainName.BTC, cancellationToken)
                          .Returns(dto);

        var command = new CreateBlockchainEntryCommand("BTC");

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        await _repository.Received(1).AddEntryAsync(
                Arg.Is<Domain.Entities.Blockchain>(entity =>
                    entity.Name == dto.Name &&
                    entity.Height == dto.Height &&
                    entity.Hash == dto.Hash &&
                    entity.Time == dto.Time &&
                    entity.LatestUrl == dto.LatestUrl &&
                    entity.PreviousHash == dto.PreviousHash &&
                    entity.PreviousUrl == dto.PreviousUrl &&
                    entity.PeerCount == dto.PeerCount &&
                    entity.HighFeePerKb == dto.HighFeePerKb &&
                    entity.MediumFeePerKb == dto.MediumFeePerKb &&
                    entity.LowFeePerKb == dto.LowFeePerKb &&
                    entity.UnconfirmedCount == dto.UnconfirmedCount &&
                    entity.LastForkHeight == dto.LastForkHeight &&
                    entity.LastForkHash == dto.LastForkHash &&
                    entity.CreatedAt == createdAt.DateTime));
    }

    [Fact]
    public async Task Handle_Should_Commit_UnitOfWork()
    {
        // Arrange
        CancellationToken cancellationToken = CancellationToken.None;

        _blockCypherClient.GetBlockchainAsync(BlockchainName.BTC, cancellationToken)
                          .Returns(new BlockchainDTO());

        var command = new CreateBlockchainEntryCommand("BTC");

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        await _unitOfWork.Received(1).CommitAsync(cancellationToken);
    }
}