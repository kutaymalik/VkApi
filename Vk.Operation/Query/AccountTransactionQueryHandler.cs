using AutoMapper;
using MediatR;
using Vk.Base.Response;
using Vk.Data.Domain;
using Vk.Data.UnitOfWorks;
using Vk.Schema;

namespace Vk.Operation.Query;

public class AccountTransactionQueryHandler :
    IRequestHandler<GetAllCustomerAccountTransactionQuery, ApiResponse<List<AccountTransactionResponse>>>,
    IRequestHandler<GetAllAccountTransactionQuery, ApiResponse<List<AccountTransactionResponse>>>,
    IRequestHandler<GetAccountTransactionByIdQuery, ApiResponse<AccountTransactionResponse>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public AccountTransactionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<AccountTransactionResponse>>> Handle(GetAllCustomerAccountTransactionQuery request, CancellationToken cancellationToken)
    {
        Account account = await unitOfWork.AccountRepository.GetByIdAsync(
            request.accountId, cancellationToken, "Customer", "EftTransactions","AccountTransactions");

        List<AccountTransaction> list = account.AccountTransactions;

        List<AccountTransactionResponse> mapped =  mapper.Map<List<AccountTransactionResponse>>(list);

        return new ApiResponse<List<AccountTransactionResponse>>(mapped);
    }

    public async Task<ApiResponse<List<AccountTransactionResponse>>> Handle(GetAllAccountTransactionQuery request, CancellationToken cancellationToken)
    {
        List<AccountTransaction> list = unitOfWork.AccountTransactionRepository.GetAll("Account");

        List<AccountTransactionResponse> mapped = mapper.Map<List<AccountTransactionResponse>>(list);

        return new ApiResponse<List<AccountTransactionResponse>>(mapped);
    }

    public async Task<ApiResponse<AccountTransactionResponse>> Handle(GetAccountTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        AccountTransaction entity = await unitOfWork.AccountTransactionRepository.GetByIdAsync(
            request.Id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse<AccountTransactionResponse>("Transaction not found!");
        }

        AccountTransactionResponse mapped = mapper.Map<AccountTransactionResponse>(entity);

        return new ApiResponse<AccountTransactionResponse>(mapped);
    }
}
