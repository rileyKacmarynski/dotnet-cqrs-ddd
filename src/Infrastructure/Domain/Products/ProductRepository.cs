using Microsoft.EntityFrameworkCore;
using SampleStore.Application.Products;
using SampleStore.Domain.Products;
using SampleStore.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Infrastructure.Domain.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly OrdersContext _context;

        public ProductRepository(OrdersContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context
                .Products
                .IncludePaths("_prices")
                .ToListAsync();
        }

        public async Task<List<Product>> GetByIdsAsync(List<Guid> ids)
        {
            return await _context
                .Products
                .IncludePaths("_prices")
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();
        }
    }
}
