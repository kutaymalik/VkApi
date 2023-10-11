
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vk.Base.Response;
using Vk.Data.Domain;
using Vk.Operation;
using Vk.Schema;

namespace Vk.Api.Controllers;

[Route("vk/api/v1/[controller]")]
[ApiController]
public class AccountTransactionController : ControllerBase
{
    private IMediator mediator;
    public AccountTransactionController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<AccountTransactionResponse>>> GetAll()
    {
        var operation = new GetAllAccountTransactionQuery();

        var result = await mediator.Send(operation);

        return result;
    }


    [HttpGet("{id}")]
    public async Task<ApiResponse<AccountTransactionResponse>> GetById(int id)
    {
        var operation = new GetAccountTransactionByIdQuery(id);

        var result = await mediator.Send(operation);

        return result;
    }


    //[HttpGet("{id}")]
    //public async Task<ApiResponse<List<AccountTransactionResponse>>> GetByAccountId(int id)
    //{
    //    var operation = new GetAllCustomerAccountTransactionQuery(id);

    //    var result = await mediator.Send(operation);

    //    return result;
    //}

    [HttpPost]
    public async Task<ApiResponse<AccountTransactionResponse>> Post([FromBody] AccountTransactionRequest request)
    {
            var operation = new CreateAccountTransactionCommand(request);

            var result = await mediator.Send(operation);

            return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new DeleteAccountTransactionCommand(id);

        var result = await mediator.Send(operation);

        return result;
    }
}
