using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vk.Base.Response;
using Vk.Data.Domain;
using Vk.Operation;
using Vk.Schema;

namespace Vk.Api.Controllers;


[Route("vk/api/v1/[controller]")]
[ApiController]
public class AddressController : ControllerBase
{
    private IMediator mediator;
    public AddressController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<AddressResponse>>> GetAll()
    {
        var operation = new GetAllAddressQuery();

        var result = await mediator.Send(operation);

        return result;
    }


    [HttpGet("{id}")]
    public async Task<ApiResponse<AddressResponse>> GetById(int id)
    {
        var operation = new GetAddressByIdQuery(id);

        var result = await mediator.Send(operation);

        return result;
    }

    [HttpPost]
    public async Task<ApiResponse<AddressResponse>> Post([FromBody] AddressRequest AddressRequest)
    {
        var operation = new CreateAddressCommand(AddressRequest);

        var result = await mediator.Send(operation);

        return result;
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> Put(int id, [FromBody] AddressRequest request)
    {
        var operation = new UpdateAddressCommand(request, id);

        var result = await mediator.Send(operation);

        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new DeleteAddressCommand(id);

        var result = await mediator.Send(operation);

        return result;
    }
}
