using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vk.Base.Response;
using Vk.Data.Domain;
using Vk.Operation;
using Vk.Schema;

namespace Vk.Api.Controllers;


public class CustomerServiceController : Controller
{
    private readonly IMediator mediator;

    public CustomerServiceController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("GetCustomerAccounts")]
    [Authorize(Roles = "customer")]
    public async Task<ApiResponse<List<AccountResponse>>> GetCustomerAccounts()
    {
        var id = (User.Identity as ClaimsIdentity).FindFirst("Id").Value;

        var operation = new GetAccountByCustomerIdQuery(int.Parse(id));

        var result = await mediator.Send(operation);

        return result;
    }

    [HttpGet("GetCustomerInfo")]
    [Authorize]
    public async Task<ApiResponse<CustomerResponse>> GetCustomerInfo()
    {
        // Catching customer.Id on TokanCommandHandler GetClaims method.
        var id = (User.Identity as ClaimsIdentity).FindFirst("Id").Value;

        var operation = new GetCustomerByIdQuery(int.Parse(id));

        var result = await mediator.Send(operation);

        return result;
    }
}
