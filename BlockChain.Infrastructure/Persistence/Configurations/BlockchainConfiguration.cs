using BlockChain.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace BlockChain.Infrastructure.Persistence.Configurations;

public sealed class BlockchainConfiguration : IEntityTypeConfiguration<Blockchain>
{
    public void Configure(EntityTypeBuilder<Blockchain> builder)
    {
        builder.ToTable("BlockChainRecords");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Height)
            .IsRequired();

        builder.Property(x => x.Hash)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.Time)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.LatestUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.PreviousHash)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.PreviousUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.PeerCount)
            .IsRequired();

        builder.Property(x => x.HighFeePerKb)
            .IsRequired();

        builder.Property(x => x.MediumFeePerKb)
            .IsRequired();

        builder.Property(x => x.LowFeePerKb)
            .IsRequired();

        builder.Property(x => x.UnconfirmedCount)
            .IsRequired();

        builder.Property(x => x.LastForkHeight)
            .IsRequired(false);

        builder.Property(x => x.LastForkHash)
            .IsRequired(false)
            .HasMaxLength(128);

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}