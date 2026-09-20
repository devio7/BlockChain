using BlockChain.Domain.Enums;
using FluentValidation;

namespace BlockChain.Application.Features.Blockchain.CreateBlockchainEntry;

public sealed class CreateBlockchainEntryValidator : AbstractValidator<CreateBlockchainEntryCommand>
{
    public CreateBlockchainEntryValidator()
    {
        RuleFor(x => x.BlockchainName)
            .Must(value => Enum.TryParse(value, true, out BlockchainName blockchainName) &&
                           Enum.IsDefined(blockchainName) &&
                           blockchainName != BlockchainName.Unknown)
            .WithMessage("Invalid blockchain name.");
    }
}