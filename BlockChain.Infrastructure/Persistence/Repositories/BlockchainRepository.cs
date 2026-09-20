using BlockChain.Domain.Entities;
using BlockChain.Domain.Enums;
using BlockChain.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlockChain.Infrastructure.Persistence.Repositories;

public sealed class BlockchainRepository : IBlockchainRepository
{
    private readonly BlockchainDbContext _dbContext;

    public BlockchainRepository(BlockchainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddEntryAsync(Blockchain blockchain)
    {
        await _dbContext.Blockchains.AddAsync(blockchain);
    }

    public async Task<IReadOnlyList<Blockchain>> GetHistoryByBlockchainNameAsync(BlockchainName blockchainName)
    {
        IReadOnlyList<Blockchain> history = await _dbContext.Blockchains.Where(b => b.BlockchainName == blockchainName)
                                                                        .OrderByDescending(b => b.CreatedAt)
                                                                        .ToListAsync();

        return history;
    }
}