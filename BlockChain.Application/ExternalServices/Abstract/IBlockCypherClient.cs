using BlockChain.Application.DTOs;
using BlockChain.Domain.Enums;

namespace BlockChain.Application.ExternalServices.Abstract;

public interface IBlockCypherClient
{
    Task<BlockchainDTO> GetBlockchainAsync(BlockchainName blockchainName, CancellationToken cancellationToken);
}