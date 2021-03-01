using SampleStore.Domain.Customers.Orders;
using SampleStore.Domain.SharedKernel.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace SampleStore.Domain.Customers
{
    internal class OrderShouldHaveAtLeastOneProductRule : IBusinessRule
    {
        private List<OrderProductData> _orderProducts;

        public OrderShouldHaveAtLeastOneProductRule(List<OrderProductData> orderProducts)
        {
            _orderProducts = orderProducts;
        }

        public string Message => "Order should have at least one product";

        public bool IsBroken() => !_orderProducts.Any();
    }
}