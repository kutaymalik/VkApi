﻿using Vk.Base.Response;
using Vk.Schema;
using MediatR;
using Vk.Data.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Vk.Data.Domain;

namespace Vk.Operation;

public class CustomerQueryHandler :
    IRequestHandler<GetAllCustomerQuery, ApiResponse<List<CustomerResponse>>>,
    IRequestHandler<GetCustomerByIdQuery, ApiResponse<CustomerResponse>>
{

    private readonly VkDbContext dbContext;
    private readonly IMapper mapper;

    public CustomerQueryHandler(VkDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }


    public async Task<ApiResponse<List<CustomerResponse>>> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken)
    {
        List<Customer> list = await dbContext.Set<Customer>().Include(x => x.Accounts).Include(x => x.Addresses).ToListAsync(cancellationToken);
       
        //var map = list.Select(x => new CustomerResponse
        //{
        //    FirstName = x.FirstName,
        //    LastName = x.LastName,
        //    ....
        //}).ToList();

        List<CustomerResponse> mapped = mapper.Map<List<Customer>, List<CustomerResponse>>(list);

        return new ApiResponse<List<CustomerResponse>>(mapped);
    }

    public async Task<ApiResponse<CustomerResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {

        Customer entity = await dbContext.Set<Customer>().Include(x => x.Accounts).Include(x => x.Addresses).FirstOrDefaultAsync(x => x.CustomerNumber == request.Id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse<CustomerResponse>("Record not found!");
        }

        CustomerResponse mapped = mapper.Map<CustomerResponse>(entity);

        return new ApiResponse<CustomerResponse>(mapped);
    }
}
