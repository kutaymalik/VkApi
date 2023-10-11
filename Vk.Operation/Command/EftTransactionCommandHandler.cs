using AutoMapper;
using MediatR;
using Vk.Base.Response;
using Vk.Data.Domain;
using Vk.Data.UnitOfWorks;
using Vk.Schema;

namespace Vk.Operation.Command;

public class EftTransactionCommandHandler :
    IRequestHandler<CreateEftTransactionCommand, ApiResponse<EftTransactionResponse>>,
    IRequestHandler<DeleteEftTransactionCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public EftTransactionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<EftTransactionResponse>> Handle(CreateEftTransactionCommand request, CancellationToken cancellationToken)
    {
        EftTransaction mapped = mapper.Map<EftTransaction>(request.Model);

        unitOfWork.EftTransactionRepository.InsertAsync(mapped, cancellationToken);

        unitOfWork.CompleteAsync(cancellationToken);

        var response = mapper.Map<EftTransactionResponse>(mapped);

        return new ApiResponse<EftTransactionResponse>(response);
    }

    public async Task<ApiResponse> Handle(DeleteEftTransactionCommand request, CancellationToken cancellationToken)
    {
        // Finding required address to Address Table
        EftTransaction entity = await unitOfWork.EftTransactionRepository.GetByIdAsync(request.Id, cancellationToken);

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
