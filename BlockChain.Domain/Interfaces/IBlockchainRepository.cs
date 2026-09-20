using BlockChain.Domain.Entities;
using BlockChain.Domain.Enums;

namespace BlockChain.Domain.Interfaces;

public interface IBlockchainRepository
{
    Task AddEntryAsync(Blockchain blockchain);

    Task<IReadOnlyList<Blockchain>> GetHistoryByBlockchainNameAsync(BlockchainName blockchainName);
}