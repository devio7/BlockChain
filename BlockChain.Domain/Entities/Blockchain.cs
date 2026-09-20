using BlockChain.Domain.Enums;

namespace BlockChain.Domain.Entities;

public sealed class Blockchain
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

    public static Blockchain Create(
        string name,
        BlockchainName blockchainName,
        int height,
        string hash,
        string time,
        string latestUrl,
        string previousHash,
        string previousUrl,
        int peerCount,
        int highFeePerKb,
        int mediumFeePerKb,
        int lowFeePerKb,
        int unconfirmedCount,
        int? lastForkHeight,
        string? lastForkHash,
        DateTime createdAt)
    {
        return new Blockchain
        {
            Name = name,
            BlockchainName = blockchainName,
            Height = height,
            Hash = hash,
            Time = time,
            LatestUrl = latestUrl,
            PreviousHash = previousHash,
            PreviousUrl = previousUrl,
            PeerCount = peerCount,
            HighFeePerKb = highFeePerKb,
            MediumFeePerKb = mediumFeePerKb,
            LowFeePerKb = lowFeePerKb,
            UnconfirmedCount = unconfirmedCount,
            LastForkHeight = lastForkHeight,
            LastForkHash = lastForkHash,
            CreatedAt = createdAt
        };
    }
}