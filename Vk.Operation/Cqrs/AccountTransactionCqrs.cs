using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vk.Base.Response;
using Vk.Data.Domain;
using Vk.Schema;

namespace Vk.Operation;

public record CreateAccountTransactionCommand(AccountTransactionRequest Model) : IRequest<ApiResponse<AccountTransactionResponse>>;


public record DeleteAccountTransactionCommand(int Id) : IRequest<ApiResponse>;


public record GetAllCustomerAccountTransactionQuery(int accountId) : IRequest<ApiResponse<List<AccountTransactionResponse>>>;
public record GetAllAccountTransactionQuery() : IRequest<ApiResponse<List<AccountTransactionResponse>>>;
public record GetAccountTransactionByIdQuery(int Id) : IRequest<ApiResponse<AccountTransactionResponse>>;