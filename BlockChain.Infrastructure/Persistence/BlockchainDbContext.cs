using BlockChain.Domain.Entities;
using BlockChain.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlockChain.Infrastructure.Persistence;

public sealed class BlockchainDbContext : DbContext, IUnitOfWork
{
    public DbSet<Blockchain> Blockchains => Set<Blockchain>();

    public BlockchainDbContext(DbContextOptions<BlockchainDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlockchainDbContext).Assembly);
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(cancellationToken);
    }
}