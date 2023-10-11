using AutoMapper;
using MediatR;
using Vk.Base.Response;
using Vk.Data.Domain;
using Vk.Data.UnitOfWorks;
using Vk.Schema;

namespace Vk.Operation.Query;

public class EftTransactionQueryHandler :
    IRequestHandler<GetAllCustomerEftTransactionQuery, ApiResponse<List<EftTransactionResponse>>>,
    IRequestHandler<GetAllEftTransactionQuery, ApiResponse<List<EftTransactionResponse>>>,
    IRequestHandler<GetEftTransactionByIdQuery, ApiResponse<EftTransactionResponse>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public EftTransactionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<EftTransactionResponse>>> Handle(GetAllCustomerEftTransactionQuery request, CancellationToken cancellationToken)
    {
        Account account = await unitOfWork.AccountRepository.GetByIdAsync(
            request.accountId, cancellationToken, "EftTransactions");

        List<EftTransaction> list = account.EftTransactions;

        List<EftTransactionResponse> mapped = mapper.Map<List<EftTransactionResponse>>(list);

        return new ApiResponse<List<EftTransactionResponse>>(mapped);
    }

    public async Task<ApiResponse<List<EftTransactionResponse>>> Handle(GetAllEftTransactionQuery request, CancellationToken cancellationToken)
    {
        List<EftTransaction> list = unitOfWork.EftTransactionRepository.GetAll();

        List<EftTransactionResponse> mapped = mapper.Map<List<EftTransactionResponse>>(list);

        return new ApiResponse<List<EftTransactionResponse>>(mapped);
    }

    public async Task<ApiResponse<EftTransactionResponse>> Handle(GetEftTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        EftTransaction entity = await unitOfWork.EftTransactionRepository.GetByIdAsync(
            request.Id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse<EftTransactionResponse>("Transaction not found!");
        }

        EftTransactionResponse mapped = mapper.Map<EftTransactionResponse>(entity);

        return new ApiResponse<EftTransactionResponse>(mapped);
    }
}
