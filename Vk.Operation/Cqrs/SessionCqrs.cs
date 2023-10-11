using MediatR;
using Vk.Base.Response;
using Vk.Schema;

namespace Vk.Operation.Cqrs;

public record GetCustomerBySessionIdQuery() : IRequest<ApiResponse<CustomerResponse>>;
public record GetAddressBySessionIdQuery() : IRequest<ApiResponse<List<AddressResponse>>>;
public record GetAccountsBySessionIdQuery() : IRequest<ApiResponse<List<AccountResponse>>>;
public record GetAccountTransactionsBySessionIdQuery() : IRequest<ApiResponse<List<AccountTransactionResponse>>>;
public record GetEftTransactionsBySessionIdQuery() : IRequest<ApiResponse<List<EftTransactionResponse>>>;
public record GetCardsBySessionIdQuery() : IRequest<ApiResponse<List<CardResponse>>>;