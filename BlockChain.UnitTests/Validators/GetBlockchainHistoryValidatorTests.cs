using BlockChain.Application.Features.Blockchain.GetBlockchainHistory;
using FluentValidation.TestHelper;
using Xunit;

namespace BlockChain.UnitTests.Validators;

public sealed class GetBlockchainHistoryValidatorTests
{
    private readonly GetBlockchainHistoryValidator _validator = new();

    [Theory]
    [InlineData("BTC")]
    [InlineData("btc")]
    [InlineData("Btc")]
    [InlineData("ETH")]
    [InlineData("DASH")]
    [InlineData("BTC_TEST3")]
    [InlineData("LTC")]
    public void BlockchainName_Should_Be_Valid(string blockchainName)
    {
        var query = new GetBlockchainHistoryQuery(blockchainName);

        TestValidationResult<GetBlockchainHistoryQuery> result = _validator.TestValidate(query);

        result.ShouldNotHaveValidationErrorFor(x => x.BlockchainName);
    }

    [Theory]
    [InlineData("INVALID")]
    [InlineData("ABC")]
    [InlineData("")]
    [InlineData("Unknown")]
    [InlineData("0")]
    [InlineData("999")]
    public void BlockchainName_Should_Be_Invalid(string blockchainName)
    {
        var query = new GetBlockchainHistoryQuery(blockchainName);

        TestValidationResult<GetBlockchainHistoryQuery> result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.BlockchainName)
            .WithErrorMessage("Invalid blockchain name.");
    }
}