namespace BlockChain.Application.Features.Blockchain.GetBlockchainHistory;

public static class BlockchainMapper
{
    public static BlockchainRecord ToRecord(Domain.Entities.Blockchain entity)
    {
        return new BlockchainRecord
        {
            Id = entity.Id,
            Name = entity.Name,
            BlockchainName = entity.BlockchainName,
            Height = entity.Height,
            Hash = entity.Hash,
            Time = entity.Time,
            LatestUrl = entity.LatestUrl,
            PreviousHash = entity.PreviousHash,
            PreviousUrl = entity.PreviousUrl,
            PeerCount = entity.PeerCount,
            HighFeePerKb = entity.HighFeePerKb,
            MediumFeePerKb = entity.MediumFeePerKb,
            LowFeePerKb = entity.LowFeePerKb,
            UnconfirmedCount = entity.UnconfirmedCount,
            LastForkHeight = entity.LastForkHeight,
            LastForkHash = entity.LastForkHash,
            CreatedAt = entity.CreatedAt
        };
    }
}