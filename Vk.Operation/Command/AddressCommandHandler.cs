using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vk.Base.Response;
using Vk.Data.Context;
using Vk.Data.Domain;
using Vk.Data.UnitOfWorks;
using Vk.Schema;

namespace Vk.Operation.Command;

public class AddressCommandHandler :
    IRequestHandler<CreateAddressCommand, ApiResponse<AddressResponse>>,
    IRequestHandler<UpdateAddressCommand, ApiResponse>,
    IRequestHandler<DeleteAddressCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public AddressCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public Task<ApiResponse<AddressResponse>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        Address mapped = mapper.Map<Address>(request.Model);

        unitOfWork.AddressRepository.InsertAsync(mapped, cancellationToken);

        unitOfWork.CompleteAsync(cancellationToken);

        var response = mapper.Map<AddressResponse>(mapped);

        return Task.FromResult(new ApiResponse<AddressResponse>(response));
    }

    public async Task<ApiResponse> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        // Finding required address to Address Table
        Address entity = await unitOfWork.AddressRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse("Record not found!");
        }

        // Changing required fields
        entity.AddressLine1 = request.Model.AddressLine1;
        entity.AddressLine2 = request.Model.AddressLine2;
        entity.City = request.Model.City;
        entity.County = request.Model.County;
        entity.PostalCode = request.Model.PostalCode;

        unitOfWork.AddressRepository.Update(entity);
        unitOfWork.CompleteAsync(cancellationToken);

        // Returning response as ApiResponse type
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        // Finding required address to Address Table
        Address entity = await unitOfWork.AddressRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse("Record not found!");
        }

        // Changing entity state
        entity.IsActive = false;

        // Saving changes to the database permanently
        unitOfWork.CompleteAsync(cancellationToken);

        // Returning response as ApiResponse type
        return new ApiResponse();
    }
}
