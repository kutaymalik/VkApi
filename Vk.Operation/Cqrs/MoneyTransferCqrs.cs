using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vk.Base.Response;
using Vk.Data.Domain;
using Vk.Schema;

namespace Vk.Operation;

public record CreateMoneyTransferCommand(MoneyTransferRequest Model) : IRequest<ApiResponse<MoneyTransferResponse>>;
public record GetMoneyTransferByReferenceQuery(string ReferenceNumber) : IRequest<ApiResponse<List<AccountTransactionResponse>>>;
public record GetMoneyTransferByAccountIdQuery(int AccountId) : IRequest<ApiResponse<List<AccountTransactionResponse>>>;
public record GetMoneyTransferByParametersQuery(int? accountId, int? customerId, decimal? minAmount, decimal? maxAmount, 
    DateTime? beginDate, DateTime? endDate, string description) : IRequest<ApiResponse<List<AccountTransactionResponse>>>;
