using BlockChain.Domain.Enums;

namespace BlockChain.Application.Features.Blockchain.GetBlockchainHistory;

public sealed record GetBlockchainHistoryResult(IReadOnlyList<BlockchainRecord> Items);

public sealed record BlockchainRecord
{
    public long Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public BlockchainName BlockchainName { get; init; }

    public int Height { get; init; }

    public string Hash { get; init; } = string.Empty;

    public string Time { get; init; } = string.Empty;

    public string LatestUrl { get; init; } = string.Empty;

    public string PreviousHash { get; init; } = string.Empty;

    public string PreviousUrl { get; init; } = string.Empty;

    public int PeerCount { get; init; }

    public int HighFeePerKb { get; init; }

    public int MediumFeePerKb { get; init; }

    public int LowFeePerKb { get; init; }

    public int UnconfirmedCount { get; init; }

    public int? LastForkHeight { get; init; }

    public string? LastForkHash { get; init; }

    public DateTime CreatedAt { get; init; }
}