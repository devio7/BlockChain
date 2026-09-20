namespace BlockChain.Application.DTOs;

public sealed class BlockchainDTO
{
    public string Name { get; init; } = string.Empty;

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
}