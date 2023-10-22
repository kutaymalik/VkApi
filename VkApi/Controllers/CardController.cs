using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vk.Base.Response;
using Vk.Data.Domain;
using Vk.Operation;
using Vk.Schema;

namespace Vk.Api.Controllers;

[Route("vk/api/v1/[controller]")]
[ApiController]
public class CardController : ControllerBase
{
    private IMediator mediator;
    public CardController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<CardResponse>>> GetAll()
    {
        var operation = new GetAllCardQuery();

        var result = await mediator.Send(operation);

        return result;
    }


    [HttpGet("{id}")]
    public async Task<ApiResponse<CardResponse>> GetById(int id)
    {
        var operation = new GetCardByIdQuery(id);

        var result = await mediator.Send(operation);

        return result;
    }

    [HttpGet("ByCustomerId/{customerid}")]
    public async Task<ApiResponse<List<CardResponse>>> GetByCustomerId(int customerid)
    {
        var operation = new GetCardByCustomerIdQuery(customerid);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("ByAccountId/{accountid}")]
    public async Task<ApiResponse<CardResponse>> GetByAccountId(int accountid)
    {
        var operation = new GetCardByAccountIdQuery(accountid);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost]
    public async Task<ApiResponse<CardResponse>> Post([FromBody] CardRequest CardRequest)
    {
        var operation = new CreateCardCommand(CardRequest);

        var result = await mediator.Send(operation);

        return result;
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> Put(int id, [FromBody] CardRequest request)
    {
        var operation = new UpdateCardCommand(request, id);

        var result = await mediator.Send(operation);

        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new DeleteCardCommand(id);

        var result = await mediator.Send(operation);

        return result;
    }
}
