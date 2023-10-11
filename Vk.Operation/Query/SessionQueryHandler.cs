using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Vk.Base.Response;
using Vk.Data.Domain;
using Vk.Data.UnitOfWorks;
using Vk.Operation.Cqrs;
using Vk.Schema;

namespace Vk.Operation.Query;

public class SessionQueryHandler :
    IRequestHandler<GetCustomerBySessionIdQuery, ApiResponse<CustomerResponse>>,
    IRequestHandler<GetAddressBySessionIdQuery, ApiResponse<List<AddressResponse>>>,
    IRequestHandler<GetAccountsBySessionIdQuery, ApiResponse<List<AccountResponse>>>,
    IRequestHandler<GetAccountTransactionsBySessionIdQuery, ApiResponse<List<AccountTransactionResponse>>>,
    IRequestHandler<GetEftTransactionsBySessionIdQuery, ApiResponse<List<EftTransactionResponse>>>,
    IRequestHandler<GetCardsBySessionIdQuery, ApiResponse<List<CardResponse>>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor httpContextAccessor;

    public SessionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResponse<CustomerResponse>> Handle(GetCustomerBySessionIdQuery request, CancellationToken cancellationToken)
    {
        int sessionId = CheckSession().Response;

        Customer entity = await unitOfWork.CustomerRepository.GetByIdAsync(sessionId, cancellationToken, "Accounts" , "Addresses");

        CustomerResponse mapped = mapper.Map<CustomerResponse>(entity);

        return new ApiResponse<CustomerResponse>(mapped);
    }

    public async Task<ApiResponse<List<AddressResponse>>> Handle(GetAddressBySessionIdQuery request, CancellationToken cancellationToken)
    {
        int sessionId = CheckSession().Response;

        Customer entity = await unitOfWork.CustomerRepository.GetByIdAsync(sessionId, cancellationToken, "Addresses");

        List<Address> addresses = entity.Addresses.ToList();

        List<AddressResponse> mapped = mapper.Map<List<AddressResponse>>(addresses);

        return new ApiResponse<List<AddressResponse>>(mapped);
    }

    public async Task<ApiResponse<List<AccountResponse>>> Handle(GetAccountsBySessionIdQuery request, CancellationToken cancellationToken)
    {
        int sessionId = CheckSession().Response;

        Customer entity = await unitOfWork.CustomerRepository.GetByIdAsync(sessionId, cancellationToken, "Accounts");

        List<Account> accounts = entity.Accounts.ToList();

        List<AccountResponse> mapped = mapper.Map<List<AccountResponse>>(accounts);

        return new ApiResponse<List<AccountResponse>>(mapped);
    }

    public async Task<ApiResponse<List<AccountTransactionResponse>>> Handle(GetAccountTransactionsBySessionIdQuery request, CancellationToken cancellationToken)
    {
        // Checking session user id with CheckSession method
        int sessionId = CheckSession().Response;

        // Getting customer with user id
        Customer entity = await unitOfWork.CustomerRepository.GetByIdAsync(sessionId, cancellationToken, "Accounts.AccountTransactions");
        
        // Getting this customer accounts 
        List<Account> accounts = entity.Accounts.ToList();

        List<AccountTransaction> allAccountTransactions = new List<AccountTransaction>();

        // Getting all customer account transactions 
        foreach (var account in accounts)
        {
            List<AccountTransaction> accountTransactions = account.AccountTransactions.ToList();
            allAccountTransactions.AddRange(accountTransactions);
        }

        // Mapping to response class 
        List<AccountTransactionResponse> mapped = mapper.Map<List<AccountTransactionResponse>>(allAccountTransactions);

        return new ApiResponse<List<AccountTransactionResponse>>(mapped);
    }

    public async Task<ApiResponse<List<EftTransactionResponse>>> Handle(GetEftTransactionsBySessionIdQuery request, CancellationToken cancellationToken)
    {
        int sessionId = CheckSession().Response;

        Customer entity = await unitOfWork.CustomerRepository.GetByIdAsync(sessionId, cancellationToken, "Accounts.EftTransactions");

        List<Account> accounts = entity.Accounts.ToList();

        List<EftTransaction> allEftTransactions = new List<EftTransaction>();

        foreach (var account in accounts)
        {
            List<EftTransaction> eftTransactions = account.EftTransactions.ToList();
            allEftTransactions.AddRange(eftTransactions);
        }

        List<EftTransactionResponse> mapped = mapper.Map<List<EftTransactionResponse>>(allEftTransactions);

        return new ApiResponse<List<EftTransactionResponse>>(mapped);
    }

    public async Task<ApiResponse<List<CardResponse>>> Handle(GetCardsBySessionIdQuery request, CancellationToken cancellationToken)
    {
        int sessionId = CheckSession().Response;

        Customer entity = await unitOfWork.CustomerRepository.GetByIdAsync(sessionId, cancellationToken, "Accounts.Card");

        List<Account> accounts = entity.Accounts.ToList();

        List<Card> allCards = new List<Card>();

        foreach (var account in accounts)
        {
            Card card = account.Card;
            allCards.Add(card);
        }

        List<CardResponse> mapped = mapper.Map<List<CardResponse>>(allCards);

        return new ApiResponse<List<CardResponse>>(mapped);
    }

    private ApiResponse<int> CheckSession()
    {
        // Getting id information from session with httpcontext
        var sessionIdClaim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id");

        // It checks whether there is a user in the session and if it can convert the user's id to int type, it fills it into the sessionId variable.
        if (sessionIdClaim == null || !int.TryParse(sessionIdClaim.Value, out int sessionId))
        {
            return new ApiResponse<int>("Session id not found!");
        }

        return new ApiResponse<int>(sessionId);
    }
}
