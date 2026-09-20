using BlockChain.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Xunit;

namespace BlockChain.IntegrationTests.Persistence;

public sealed class DatabaseFixture : IAsyncLifetime
{
    private SqliteConnection _connection;

    public BlockchainDbContext DbContext { get; private set; }

    public Respawner Respawner { get; private set; }

    public async ValueTask InitializeAsync()
    {
        _connection = new SqliteConnection("Data Source=:memory:");

        await _connection.OpenAsync();

        var options = new DbContextOptionsBuilder<BlockchainDbContext>()
            .UseSqlite(_connection)
            .Options;

        DbContext = new BlockchainDbContext(options);

        await DbContext.Database.EnsureCreatedAsync();

        Respawner = await Respawner.CreateAsync(_connection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Sqlite
            }
        );
    }

    public async Task ResetAsync()
    {
        await Respawner.ResetAsync(_connection);
    }

    public async ValueTask DisposeAsync()
    {
        await DbContext.DisposeAsync();
        await _connection.DisposeAsync();
    }
}