
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vk.Base.Response;
using Vk.Data.Domain;
using Vk.Operation;
using Vk.Schema;

namespace Vk.Api.Controllers;
[Route("vk/api/v1/[controller]")]
[ApiController]
public class EftTransactionsController : ControllerBase
{
    private IMediator mediator;
    public EftTransactionsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<EftTransactionResponse>>> GetAll()
    {
        var operation = new GetAllEftTransactionQuery();

        var result = await mediator.Send(operation);

        return result;
    }


    [HttpGet("{id}")]
    public async Task<ApiResponse<EftTransactionResponse>> GetById(int id)
    {
        var operation = new GetEftTransactionByIdQuery(id);

        var result = await mediator.Send(operation);

        return result;
    }


    //[HttpGet("{id}")]
    //public async Task<ApiResponse<List<EftTransactionResponse>>> GetByAccountId(int id)
    //{
    //    var operation = new GetAllCustomerEftTransactionQuery(id);

    //    var result = await mediator.Send(operation);

    //    return result;
    //}

    [HttpPost]
    public async Task<ApiResponse<EftTransactionResponse>> Post([FromBody] EftTransactionRequest request)
    {
        var operation = new CreateEftTransactionCommand(request);

        var result = await mediator.Send(operation);

        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new DeleteEftTransactionCommand(id);

        var result = await mediator.Send(operation);

        return result;
    }
}
