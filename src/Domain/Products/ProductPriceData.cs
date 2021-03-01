using SampleStore.Domain.SharedKernel.Abstractions;
using SampleStore.Domain.SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Domain.Products
{
    public class ProductPriceData
    {
        public ProductPriceData(Guid productId, MoneyValue price)
        {
            ProductId = productId;
            Price = price;
        }

        public Guid ProductId { get; }
        public MoneyValue Price { get; }
    }
}
