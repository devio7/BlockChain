using MediatR;

namespace BlockChain.Application.Features.Blockchain.CreateBlockchainEntry;

public sealed record CreateBlockchainEntryCommand(string BlockchainName) : IRequest<CreateBlockchainEntryResult>;