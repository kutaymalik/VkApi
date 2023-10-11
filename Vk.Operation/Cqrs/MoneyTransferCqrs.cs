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

public record CreateMoneyTransfer(MoneyTransferRequest Model) : IRequest<ApiResponse<MoneyTransferResponse>>;
public record GetMoneyTransferByReference(string ReferenceNumber) : IRequest<ApiResponse<List<AccountTransactionResponse>>>;
public record GetMoneyTransferByAccountId(int AccountId) : IRequest<ApiResponse<List<AccountTransactionResponse>>>;
