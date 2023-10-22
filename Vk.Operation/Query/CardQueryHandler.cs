using Vk.Base.Response;
using Vk.Schema;
using MediatR;
using Vk.Data.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Vk.Data.Domain;
using Vk.Data.UnitOfWorks;

namespace Vk.Operation;

public class CardQueryHandler :
    IRequestHandler<GetAllCardQuery, ApiResponse<List<CardResponse>>>,
    IRequestHandler<GetCardByIdQuery, ApiResponse<CardResponse>>,
    IRequestHandler<GetCardByAccountIdQuery, ApiResponse<CardResponse>>,
    IRequestHandler<GetCardByCustomerIdQuery, ApiResponse<List<CardResponse>>>
{

    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly VkDbContext dbContext;

    public CardQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, VkDbContext dbContext)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.dbContext = dbContext;
    }


    public async Task<ApiResponse<List<CardResponse>>> Handle(GetAllCardQuery request, CancellationToken cancellationToken)
    {
        List<Card> list = unitOfWork.CardRepository.GetAll();

        List<CardResponse> mapped = mapper.Map<List<Card>, List<CardResponse>>(list);

        return new ApiResponse<List<CardResponse>>(mapped);
    }

    public async Task<ApiResponse<CardResponse>> Handle(GetCardByIdQuery request, CancellationToken cancellationToken)
    {
        Card entity = await unitOfWork.CardRepository.GetByIdAsync(
            request.Id, cancellationToken, "Accounts");

        if (entity == null)
        {
            return new ApiResponse<CardResponse>("Record not found!");
        }

        CardResponse mapped = mapper.Map<CardResponse>(entity);

        return new ApiResponse<CardResponse>(mapped);
    }

    public async Task<ApiResponse<CardResponse>> Handle(GetCardByAccountIdQuery request, CancellationToken cancellationToken)
    {
        Card? entity = await dbContext.Set<Card>()
            .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.AccountId == request.AccountId, cancellationToken);

        var mapped = mapper.Map<CardResponse>(entity);

        return new ApiResponse<CardResponse>(mapped);
    }

    public async Task<ApiResponse<List<CardResponse>>> Handle(GetCardByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        List<Card> list = await dbContext.Set<Card>()
            .Include(x => x.Account)
            .Where(x => x.Account.CustomerId == request.CustomerId)
            .ToListAsync(cancellationToken);


        var mapped = mapper.Map<List<CardResponse>>(list);

        return new ApiResponse<List<CardResponse>>(mapped);
    }
}
