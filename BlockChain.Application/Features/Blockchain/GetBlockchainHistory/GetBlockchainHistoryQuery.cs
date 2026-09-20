using MediatR;

namespace BlockChain.Application.Features.Blockchain.GetBlockchainHistory;

public sealed record GetBlockchainHistoryQuery(string BlockchainName) : IRequest<GetBlockchainHistoryResult>;