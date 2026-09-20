using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using BlockChain.API;
using BlockChain.Application.Features.Blockchain.CreateBlockchainEntry;
using BlockChain.Application.Features.Blockchain.GetBlockchainHistory;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BlockChain.FunctionalTests;

public sealed class BlockChainAPITests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BlockChainAPITests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostBlockchain_Should_Return200()
    {
        var response = await _client.PostAsJsonAsync("/api/blockchain", "BTC", CancellationToken.None);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PostBlockchain_Should_Return_Created_Id()
    {
        // Act
        var response = await _client.PostAsJsonAsync("/api/blockchain", "BTC", CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<CreateBlockchainEntryResult>(CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task PostBlockchain_WithInvalidName_Should_Return400()
    {
        CancellationToken cancellationToken = CancellationToken.None;
        
        var response = await _client.PostAsJsonAsync("/api/blockchain", "INVALID", cancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken);

        content!.Status.Should().Be(400);
    }

    [Theory]
    [InlineData("btc")]
    [InlineData("BtC")]
    [InlineData("BTC")]
    public async Task PostBlockchain_Should_Accept_Case_Insensitive_Name(
        string blockchainName)
    {
        // Act
        var response = await _client.PostAsJsonAsync("/api/blockchain", blockchainName, CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PostBlockchain_WithEmptyName_Should_Return400()
    {
        // Act
        CancellationToken cancellationToken = CancellationToken.None;

        var response = await _client.PostAsJsonAsync("/api/blockchain", "", cancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken);

        content.Should().NotBeNull();
        content!.Status.Should().Be(400);
    }

    [Fact]
    public async Task PostBlockchain_WithNullBody_Should_Return400()
    {
        var response = await _client.PostAsync("/api/blockchain", content: null, CancellationToken.None);

        response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
    }

    [Fact]
    public async Task GetBlockchainHistory_Should_Return200()
    {
        // Act
        var response = await _client.GetAsync("/api/blockchain/BTC/history", CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetBlockchainHistory_Should_Return_History_Result()
    {
        // Act
        CancellationToken cancellationToken = CancellationToken.None;

        var response = await _client.GetAsync("/api/blockchain/BTC/history", cancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        options.Converters.Add(new JsonStringEnumConverter());

        var result = await response.Content.ReadFromJsonAsync<GetBlockchainHistoryResult>(options, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Items.Should().NotBeNull();
    }

    [Fact]
    public async Task GetBlockchainHistory_WithInvalidName_Should_Return400()
    {
        // Act
        CancellationToken cancellationToken = CancellationToken.None;

        var response = await _client.GetAsync("/api/blockchain/INVALID/history", cancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken);

        content.Should().NotBeNull();
        content!.Status.Should().Be(400);
    }

    [Theory]
    [InlineData("btc")]
    [InlineData("BtC")]
    [InlineData("BTC")]
    public async Task GetBlockchainHistory_Should_Accept_Case_Insensitive_Name(
        string blockchainName)
    {
        // Act
        var response = await _client.GetAsync($"/api/blockchain/{blockchainName}/history", CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}