using System.Net.Http.Json;
using System.Text.Json.Serialization;
using BlockChain.Domain.Enums;
using BlockChain.Application.DTOs;
using BlockChain.Application.ExternalServices.Abstract;

namespace BlockChain.Infrastructure.ExternalServices.BlockCypher;

public sealed class BlockCypherClient : IBlockCypherClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly Dictionary<BlockchainName, string> BlockchainNameToUrlMapping = new Dictionary<BlockchainName, string>()
    {
        { BlockchainName.BTC, BlockCypherConstants.BTCUrl },
        { BlockchainName.BTC_TEST3, BlockCypherConstants.BTCTest3Url },
        { BlockchainName.ETH, BlockCypherConstants.ETHUrl },
        { BlockchainName.LTC, BlockCypherConstants.LTCUrl },
        { BlockchainName.DASH, BlockCypherConstants.DASHUrl }
    };

    public BlockCypherClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<BlockchainDTO> GetBlockchainAsync(BlockchainName blockchainName, CancellationToken cancellationToken)
    {
        string url = BlockchainNameToUrlMapping[blockchainName];
        using var _httpClient = _httpClientFactory.CreateClient(BlockCypherConstants.HttpClientName);
        BlockchainResponse? result = await _httpClient.GetFromJsonAsync<BlockchainResponse>(url, cancellationToken);
        return MapResponseToDTO(result);
    }

    private BlockchainDTO MapResponseToDTO(BlockchainResponse? response)
    {
        var blochChainDTO = new BlockchainDTO
        {
            Name = response?.Name ?? string.Empty,
            Height = response?.Height ?? 0,
            Hash = response?.Hash ?? string.Empty,
            Time = response?.Time ?? string.Empty,
            LatestUrl = response?.LatestUrl ?? string.Empty,
            PreviousHash = response?.PreviousHash ?? string.Empty,
            PreviousUrl = response?.PreviousUrl ?? string.Empty,
            PeerCount = response?.PeerCount ?? 0,
            HighFeePerKb = response?.HighFeePerKb ?? 0,
            MediumFeePerKb = response?.MediumFeePerKb ?? 0,
            LowFeePerKb = response?.LowFeePerKb ?? 0,
            UnconfirmedCount = response?.UnconfirmedCount ?? 0,
            LastForkHeight = response?.LastForkHeight ?? 0,
            LastForkHash = response?.LastForkHash ?? string.Empty
        };

        return blochChainDTO;
    }
}

internal sealed class BlockchainResponse
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("height")]
    public int Height { get; init; }

    [JsonPropertyName("hash")]
    public string Hash { get; init; } = string.Empty;

    [JsonPropertyName("time")]
    public string Time { get; init; } = string.Empty;

    [JsonPropertyName("latest_url")]
    public string LatestUrl { get; init; } = string.Empty;

    [JsonPropertyName("previous_hash")]
    public string PreviousHash { get; init; } = string.Empty;

    [JsonPropertyName("previous_url")]
    public string PreviousUrl { get; init; } = string.Empty;

    [JsonPropertyName("peer_count")]
    public int PeerCount { get; init; }

    [JsonPropertyName("high_fee_per_kb")]
    public int HighFeePerKb { get; init; }

    [JsonPropertyName("medium_fee_per_kb")]
    public int MediumFeePerKb { get; init; }

    [JsonPropertyName("low_fee_per_kb")]
    public int LowFeePerKb { get; init; }

    [JsonPropertyName("unconfirmed_count")]
    public int UnconfirmedCount { get; init; }

    [JsonPropertyName("last_fork_height")]
    public int? LastForkHeight { get; init; }

    [JsonPropertyName("last_fork_hash")]
    public string? LastForkHash { get; init; }
}