using BlockChain.Application.DTOs;
using BlockChain.Application.ExternalServices.Abstract;
using BlockChain.Domain.Enums;
using BlockChain.Domain.Interfaces;
using MediatR;

namespace BlockChain.Application.Features.Blockchain.CreateBlockchainEntry;

public sealed class CreateBlockchainEntryHandler : IRequestHandler<CreateBlockchainEntryCommand, CreateBlockchainEntryResult>
{
    private readonly IBlockchainRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlockCypherClient _blockCypherClient;
    private readonly TimeProvider _timeProvider;

    public CreateBlockchainEntryHandler(IBlockchainRepository repository,
                                        IUnitOfWork unitOfWork,
                                        IBlockCypherClient blockCypherClient,
                                        TimeProvider timeProvider)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _blockCypherClient = blockCypherClient;
        _timeProvider = timeProvider;
    }

    public async Task<CreateBlockchainEntryResult> Handle(CreateBlockchainEntryCommand request, CancellationToken cancellationToken)
    {
        BlockchainName blockchainName = Enum.Parse<BlockchainName>(request.BlockchainName, true);

        BlockchainDTO result = await _blockCypherClient.GetBlockchainAsync(blockchainName, cancellationToken);

        Domain.Entities.Blockchain blockChain = Domain.Entities.Blockchain.Create(
            result.Name,
            blockchainName,
            result.Height,
            result.Hash,
            result.Time,
            result.LatestUrl,
            result.PreviousHash,
            result.PreviousUrl,
            result.PeerCount,
            result.HighFeePerKb,
            result.MediumFeePerKb,
            result.LowFeePerKb,
            result.UnconfirmedCount,
            result.LastForkHeight,
            result.LastForkHash,
            _timeProvider.GetUtcNow().DateTime
        );

        await _repository.AddEntryAsync(blockChain);
        await _unitOfWork.CommitAsync(cancellationToken);
        return new CreateBlockchainEntryResult(blockChain.Id);
    }
}