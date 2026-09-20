using BlockChain.Domain.Enums;
using BlockChain.Domain.Interfaces;
using MediatR;

namespace BlockChain.Application.Features.Blockchain.GetBlockchainHistory;

public sealed class GetBlockchainHistoryHandler : IRequestHandler<GetBlockchainHistoryQuery, GetBlockchainHistoryResult>
{
    private readonly IBlockchainRepository _repository;

    public GetBlockchainHistoryHandler(IBlockchainRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetBlockchainHistoryResult> Handle(GetBlockchainHistoryQuery request, CancellationToken cancellationToken)
    {
        BlockchainName blockchainName = Enum.Parse<BlockchainName>(request.BlockchainName, true);

        var history = await _repository.GetHistoryByBlockchainNameAsync(blockchainName);

        IReadOnlyList<BlockchainRecord> items = history.Select(h => BlockchainMapper.ToRecord(h))
                                                       .ToList();

        var result = new GetBlockchainHistoryResult(items);

        return result;
    }
}