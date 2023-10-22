using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vk.Base.Response;
using Vk.Data.Domain;
using Vk.Operation;
using Vk.Schema;

namespace Vk.Api.Controllers;

[Route("vk/api/v1/[controller]")]
[ApiController]
public class MoneyTransferController : ControllerBase
{
    private IMediator mediator;

    public MoneyTransferController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    public async Task<ApiResponse<MoneyTransferResponse>> Post([FromBody] MoneyTransferRequest request)
    {
        var operation = new CreateMoneyTransferCommand(request);

        var result = await mediator.Send(operation);

        return result;
    }

    [HttpGet("ByReference/{referenceNumber}")]
    public async Task<ApiResponse<List<AccountTransactionResponse>>> GetByReferenceNumber(string referenceNumber)
    {
        var operation = new GetMoneyTransferByReferenceQuery(referenceNumber);

        var result = await mediator.Send(operation);

        return result;
    }

    [HttpGet("ByAccountId/{accountId}")]
    public async Task<ApiResponse<List<AccountTransactionResponse>>> GetByAccountId(int accountId)
    {
        var operation = new GetMoneyTransferByAccountIdQuery(accountId);

        var result = await mediator.Send(operation);

        return result;
    }

    [HttpGet("ByParameter")]
    public async Task<ApiResponse<List<AccountTransactionResponse>>> ByParameter(
        [FromQuery] int? accountId,
        [FromQuery] int? customerId,
        [FromQuery] decimal? minAmount,
        [FromQuery] decimal? maxAmount,
        [FromQuery] DateTime? beginDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] string? description
        )
    {
        var operation = new GetMoneyTransferByParametersQuery(accountId, customerId, minAmount, maxAmount, beginDate, endDate, description);

        var result = await mediator.Send(operation);

        return result;
    }
}
