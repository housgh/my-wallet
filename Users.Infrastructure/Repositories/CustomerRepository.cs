using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyWallet.Common.Models;
using MyWallet.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Users.Repositories;
using Users.Infrastructure.DbContexts;
using Users.Infrastructure.Entities;

namespace Users.Infrastructure.Repositories
{
    internal class CustomerRepository : BaseRepository<UserDbContext, CustomerDto, Customer>, ICustomerRepository
    {
        private readonly UserDbContext _context;
        private readonly IMapper _mapper;

        public CustomerRepository(UserDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> ExistsAsync(string customerId)
        {
            return await _context.Customers
                .AnyAsync(x => x.CustomerId == customerId);
        }

        public async Task<CustomerDto> GetAsync(string customerId)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(x => x.CustomerId == customerId);
            return _mapper.Map<CustomerDto>(customer);
        }
    }
}
