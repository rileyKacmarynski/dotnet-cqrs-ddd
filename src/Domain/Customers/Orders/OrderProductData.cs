using System;

namespace SampleStore.Domain.Customers.Orders
{
    public class OrderProductData
    {
        public OrderProductData(Guid productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public Guid ProductId { get; }
        public int Quantity { get; }
    }
}