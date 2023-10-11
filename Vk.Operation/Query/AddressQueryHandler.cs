using AutoMapper;
using MediatR;
using Vk.Base.Response;
using Vk.Data.Domain;
using Vk.Data.UnitOfWorks;
using Vk.Schema;

namespace Vk.Operation.Query;

public class AddressQueryHandler :
    IRequestHandler<GetAllAddressQuery, ApiResponse<List<AddressResponse>>>,
    IRequestHandler<GetAddressByIdQuery, ApiResponse<AddressResponse>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public AddressQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<AddressResponse>>> Handle(GetAllAddressQuery request, CancellationToken cancellationToken)
    {
        List<Address> list = unitOfWork.AddressRepository.GetAll("Customer");

        List<AddressResponse> mapped = mapper.Map<List<AddressResponse>>(list);

        return new ApiResponse<List<AddressResponse>>(mapped);
    }

    public async Task<ApiResponse<AddressResponse>> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
    {
        Address entity = await unitOfWork.AddressRepository.GetByIdAsync(request.Id, cancellationToken, "Customer");

        if (entity == null)
        {
            return new ApiResponse<AddressResponse>("Record not found!");
        }

        AddressResponse mapped = mapper.Map<AddressResponse>(entity);

        return new ApiResponse<AddressResponse>(mapped);
    }
}
