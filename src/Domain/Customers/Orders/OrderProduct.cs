using SampleStore.Domain.Products;
using SampleStore.Domain.SharedKernel.Abstractions;
using SampleStore.Domain.SharedKernel.ValueObjects;
using System;

namespace SampleStore.Domain.Customers.Orders
{
    public class OrderProduct : Entity
    {
        public int Quantity { get; private set; }
        public Guid ProductId { get; private set; }
        internal MoneyValue Price { get; private set; }

        private OrderProduct() { }

        private OrderProduct(
            ProductPriceData productPrice,
            int quantity,
            string currenty)
        {
            ProductId = productPrice.ProductId;
            Quantity = quantity;

            CalculatePrice(productPrice);
        }

        private void CalculatePrice(ProductPriceData productPrice)
        {
            Price = Quantity * productPrice.Price;
        }

        internal static object CreateForProduct(ProductPriceData productPrice, int quantity, string currency)
        {
            throw new NotImplementedException();
        }
    }
}