using BlockChain.Application.Features.Blockchain.CreateBlockchainEntry;
using FluentValidation.TestHelper;
using Xunit;

namespace BlockChain.UnitTests.Validators;

public sealed class CreateBlockchainEntryValidatorTests
{
    private readonly CreateBlockchainEntryValidator _validator = new();

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
        var command = new CreateBlockchainEntryCommand(blockchainName);

        TestValidationResult<CreateBlockchainEntryCommand> result = _validator.TestValidate(command);

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
        var command = new CreateBlockchainEntryCommand(blockchainName);

        TestValidationResult<CreateBlockchainEntryCommand> result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.BlockchainName)
              .WithErrorMessage("Invalid blockchain name.");
    }
}