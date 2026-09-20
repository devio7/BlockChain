using System.Net;
using System.Text;
using BlockChain.Domain.Enums;
using BlockChain.Infrastructure.ExternalServices.BlockCypher;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace BlockChain.IntegrationTests.ExternalServices;

public sealed class BlockCypherClientTests
{
    [Fact]
    public async Task GetBlockchainAsync_Should_Return_Blockchain()
    {
        // Arrange
        var responseJson = """
        {
            "name": "Bitcoin",
            "height": 900000,
            "hash": "abc123",
            "time": "2026-09-19T12:00:00Z",
            "latest_url": "https://example.com/latest",
            "previous_hash": "previous-hash",
            "previous_url": "https://example.com/previous",
            "peer_count": 10,
            "high_fee_per_kb": 100,
            "medium_fee_per_kb": 80,
            "low_fee_per_kb": 50,
            "unconfirmed_count": 20,
            "last_fork_height": 899999,
            "last_fork_hash": "fork-hash"
        }
        """;

        var handler = new FakeHttpMessageHandler(
            new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            }
        );

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.blockcypher.com/v1/")
        };

        var httpClientFactory = Substitute.For<IHttpClientFactory>();

        httpClientFactory.CreateClient("BlockCypher")
                         .Returns(httpClient);

        var client = new BlockCypherClient(httpClientFactory);

        // Act
        var result = await client.GetBlockchainAsync(BlockchainName.BTC, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Bitcoin");
        result.Height.Should().Be(900000);
        result.Hash.Should().Be("abc123");

        httpClientFactory.Received(1).CreateClient("BlockCypher");

        handler.Request.Should().NotBeNull();
        handler.Request!.Method.Should().Be(HttpMethod.Get);

        handler.Request.RequestUri.Should().Be(new Uri("https://api.blockcypher.com/v1/btc/main"));
    }

    [Fact]
    public async Task GetBlockchainAsync_Should_Throw_When_Request_Fails()
    {
        // Arrange
        var handler = new FakeHttpMessageHandler(
            new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            }
        );

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.blockcypher.com/v1/")
        };

        var httpClientFactory = Substitute.For<IHttpClientFactory>();

        httpClientFactory.CreateClient("BlockCypher").Returns(httpClient);

        var client = new BlockCypherClient(httpClientFactory);

        // Act
        var action = () => client.GetBlockchainAsync(BlockchainName.BTC, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task GetBlockchainAsync_Should_Call_Real_BlockCypher_Api()
    {
        // Arrange
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.blockcypher.com/v1/")
        };

        var httpClientFactory = Substitute.For<IHttpClientFactory>();

        httpClientFactory.CreateClient("BlockCypher").Returns(httpClient);

        var client = new BlockCypherClient(httpClientFactory);

        // Act
        var result = await client.GetBlockchainAsync(BlockchainName.BTC, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().NotBeNullOrEmpty();
        result.Height.Should().BeGreaterThan(0);
        result.Hash.Should().NotBeNullOrEmpty();
    }
}

internal sealed class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpResponseMessage _response;

    public HttpRequestMessage? Request { get; private set; }

    public FakeHttpMessageHandler(HttpResponseMessage response)
    {
        _response = response;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Request = request;

        return Task.FromResult(_response);
    }
}