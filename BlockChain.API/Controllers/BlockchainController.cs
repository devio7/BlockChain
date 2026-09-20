using BlockChain.Application.Features.Blockchain.CreateBlockchainEntry;
using BlockChain.Application.Features.Blockchain.GetBlockchainHistory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlockChain.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlockchainController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<BlockchainController> _logger;

    public BlockchainController(IMediator mediator,
                                ILogger<BlockchainController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(CreateBlockchainEntryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddBlochchainEntry([FromBody] string blockchainName)
    {
        _logger.LogInformation("Sending CreateBlockchainEntryCommand for {BlockchainName}", blockchainName);

        var result = await _mediator.Send(new CreateBlockchainEntryCommand(blockchainName));

        _logger.LogInformation("CreateBlockchainEntryCommand completed for {BlockchainName}", blockchainName);

        return Ok(result);
    }

    [HttpGet("{blockChainName}/history")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetBlockchainHistoryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBlochchainHistory([FromRoute] string blockChainName)
    {
        _logger.LogInformation("Sending GetBlockchainHistoryQuery for {BlockchainName}", blockChainName);

        var result = await _mediator.Send(new GetBlockchainHistoryQuery(blockChainName));

        _logger.LogInformation("GetBlockchainHistoryQuery completed for {BlockchainName}", blockChainName);

        return Ok(result);
    }
}