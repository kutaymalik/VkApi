using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vk.Base.Response;
using Vk.Operation;
using Vk.Operation.Cqrs;
using Vk.Schema;

namespace Vk.Api.Controllers;


[Route("vk/api/v1/[controller]")]
[ApiController]
public class SessionController : ControllerBase
{
    private readonly IMediator mediator;

    public SessionController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("GetCustomerInfo")]
    [Authorize(Roles = "customer")]
    public async Task<ApiResponse<CustomerResponse>> GetCustomerInfo()
    {
        var operation = new GetCustomerBySessionIdQuery();

        var result = await mediator.Send(operation);

        return result;
    }

    [HttpGet("GetCustomerAddresses")]
    [Authorize(Roles = "customer")]
    public async Task<ApiResponse<List<AddressResponse>>> GetCustomerAddresses()
    {
        var operation = new GetAddressBySessionIdQuery();

        var result = await mediator.Send(operation);

        return result;
    }

    [HttpGet("GetCustomerAccounts")]
    [Authorize(Roles = "customer")]
    public async Task<ApiResponse<List<AccountResponse>>> GetCustomerAccounts()
    {
        var operation = new GetAccountsBySessionIdQuery();

        var result = await mediator.Send(operation);

        return result;
    }

    [HttpGet("GetCustomerAccountTransactions")]
    [Authorize(Roles = "customer")]
    public async Task<ApiResponse<List<AccountTransactionResponse>>> GetCustomerAccountTransactions()
    {
        var operation = new GetAccountTransactionsBySessionIdQuery();

        var result = await mediator.Send(operation);

        return result;
    }

    [HttpGet("GetCustomerEftTransactions")]
    [Authorize(Roles = "customer")]
    public async Task<ApiResponse<List<EftTransactionResponse>>> GetCustomerEftTransactions()
    {
        var operation = new GetEftTransactionsBySessionIdQuery();

        var result = await mediator.Send(operation);

        return result;
    }

    [HttpGet("GetCustomerCards")]
    [Authorize(Roles = "customer")]
    public async Task<ApiResponse<List<CardResponse>>> GetCustomerCards()
    {
        var operation = new GetCardsBySessionIdQuery();

        var result = await mediator.Send(operation);

        return result;
    }
}
