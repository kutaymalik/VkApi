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
public class CustomersController : ControllerBase
{
    private IMediator mediator;
    public CustomersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<List<CustomerResponse>>> GetAll()
    {
        var operation = new GetAllCustomerQuery();

        var result = await mediator.Send(operation);

        return result;
    }


    [HttpGet("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<CustomerResponse>> GetById(int id)
    {
        var operation = new GetCustomerByIdQuery(id);

        var result = await mediator.Send(operation);

        return result;
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<CustomerResponse>> Post(CustomerRequest customerRequest)
    {
        var operation = new CreateCustomerCommand(customerRequest);

        var result = await mediator.Send(operation);

        return result;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse> Put(int id, [FromBody] CustomerRequest request)
    {
        var operation = new UpdateCustomerCommand(request, id);

        var result = await mediator.Send(operation);

        return result;
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new DeleteCustomerCommand(id);

        var result = await mediator.Send(operation);

        return result;
    }
}
