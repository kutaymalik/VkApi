using AutoMapper;
using MediatR;
using Vk.Base.Response;
using Vk.Data.Domain;
using Vk.Data.UnitOfWorks;
using Vk.Schema;

namespace Vk.Operation.Command;

public class AccountTransactionCommandHandler :
    IRequestHandler<CreateAccountTransactionCommand, ApiResponse<AccountTransactionResponse>>,
    IRequestHandler<DeleteAccountTransactionCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public AccountTransactionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<AccountTransactionResponse>> Handle(CreateAccountTransactionCommand request, CancellationToken cancellationToken)
    {
        AccountTransaction mapped = mapper.Map<AccountTransaction>(request.Model);

        unitOfWork.AccountTransactionRepository.InsertAsync(mapped, cancellationToken);

        unitOfWork.CompleteAsync(cancellationToken);

        var response =  mapper.Map<AccountTransactionResponse>(mapped);

        return new ApiResponse<AccountTransactionResponse>(response);
    }

    public async Task<ApiResponse> Handle(DeleteAccountTransactionCommand request, CancellationToken cancellationToken)
    {
        // Finding required address to Address Table
        AccountTransaction entity = await unitOfWork.AccountTransactionRepository.GetByIdAsync(request.Id, cancellationToken);

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
