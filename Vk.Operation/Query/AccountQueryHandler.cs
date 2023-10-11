using Vk.Base.Response;
using Vk.Schema;
using MediatR;
using AutoMapper;
using Vk.Data.Domain;
using Vk.Data.UnitOfWorks;

namespace Vk.Operation;

public class AccountQueryHandler :
    IRequestHandler<GetAllAccountQuery, ApiResponse<List<AccountResponse>>>,
    IRequestHandler<GetAccountByIdQuery, ApiResponse<AccountResponse>>,
    IRequestHandler<GetAccountByCustomerIdQuery, ApiResponse<List<AccountResponse>>>
{

    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public AccountQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }


    public async Task<ApiResponse<List<AccountResponse>>> Handle(GetAllAccountQuery request, CancellationToken cancellationToken)
    {
        List<Account> list = unitOfWork.AccountRepository.GetAll("Customer", "EftTransactions", "AccountTransactions"); 
       
        // Manual Mapping
        //var map = list.Select(x => new AccountResponse
        //{
        //    FirstName = x.FirstName,
        //    LastName = x.LastName,
        //    ....
        //}).ToList();

        List<AccountResponse> mapped = mapper.Map<List<Account>, List<AccountResponse>>(list);

        return new ApiResponse<List<AccountResponse>>(mapped);
    }

    public async Task<ApiResponse<AccountResponse>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        Account entity = await unitOfWork.AccountRepository.GetByIdAsync(
            request.Id, cancellationToken, "Customer", "EftTransactions", "AccountTransactions");

        // Short form was written above
        // Account entity = await dbContext.Set<Account>().Include(x => x.Customer).Include(x => x.EftTransactions).Include(x => x.AccountTransactions).FirstOrDefaultAsync(x => x.AccountNumber == request.Id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse<AccountResponse>("Record not found!");
        }

        AccountResponse mapped = mapper.Map<AccountResponse>(entity);

        return new ApiResponse<AccountResponse>(mapped);
    }

    public async Task<ApiResponse<List<AccountResponse>>> Handle(GetAccountByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        List<Account> list = unitOfWork.AccountRepository.Where(x => x.CustomerId == request.CustomerId, "Customer").ToList();

        List<AccountResponse> mapped = mapper.Map<List<Account>, List<AccountResponse>>(list);

        return new ApiResponse<List<AccountResponse>>(mapped);
    }
}

