using BlockChain.Domain.Enums;
using FluentValidation;

namespace BlockChain.Application.Features.Blockchain.GetBlockchainHistory;

public sealed class GetBlockchainHistoryValidator : AbstractValidator<GetBlockchainHistoryQuery>
{
    public GetBlockchainHistoryValidator()
    {
        RuleFor(x => x.BlockchainName)
            .Must(value => Enum.TryParse(value, true, out BlockchainName blockchainName) &&
                           Enum.IsDefined(blockchainName) &&
                           blockchainName != BlockchainName.Unknown)
            .WithMessage("Invalid blockchain name.");
    }
}