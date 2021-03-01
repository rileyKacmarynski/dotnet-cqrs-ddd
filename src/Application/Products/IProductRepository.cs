using SampleStore.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Application.Products
{
    public interface IProductRepository
    {
        Task<List<Product>> GetByIdsAsync(List<Guid> ids);
        Task<List<Product>> GetAllAsync();
    }
}
