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
    IRequestHandler<GetCardByIdQuery, ApiResponse<CardResponse>>
{

    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public CardQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
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
            request.Id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse<CardResponse>("Record not found!");
        }

        CardResponse mapped = mapper.Map<CardResponse>(entity);

        return new ApiResponse<CardResponse>(mapped);
    }
}
