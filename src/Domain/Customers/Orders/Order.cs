using SampleStore.Domain.Products;
using SampleStore.Domain.SharedKernel;
using SampleStore.Domain.SharedKernel.Abstractions;
using SampleStore.Domain.SharedKernel.Abstractions.TypedIds;
using SampleStore.Domain.SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleStore.Domain.Customers.Orders
{
    public record OrderId(Guid Value) : StronglyTypedId<Guid>(Value);

    public class Order : Entity
    {
        // internal because this isn't an aggregate root.
        public OrderId Id;
        private MoneyValue _total;
        private List<OrderProduct> _orderProducts;
        private bool _isRemoved;
        private OrderStatus _status;
        private DateTime _orderDate;
        private DateTime? _orderChangedDate;

        private Order()
        {
            _orderProducts = new List<OrderProduct>();
            _isRemoved = false;
        }

        private Order(List<OrderProductData> orderProductsData, List<ProductPriceData> productPrices, string currency)
        {
            _orderDate = SystemClock.Now;
            Id = new OrderId(Guid.NewGuid());
            _orderProducts = new List<OrderProduct>();

            foreach(var orderProductData in orderProductsData)
            {
                var productPrice = productPrices.Single(p => p.ProductId == orderProductData.ProductId && p.Price.Currency == currency);

                var orderProduct = OrderProduct.CreateForProduct(
                    productPrice,
                    orderProductData.Quantity,
                    currency);

                CalculateOrderTotal();
                _status = OrderStatus.Placed;
            }
        }

        internal static Order CreateNew(List<OrderProductData> orderProductsData, List<ProductPriceData> productPrices, string currency)
        {
            return new Order(orderProductsData, productPrices, currency);
        }

        internal MoneyValue GetTotal() => _total;

        private void CalculateOrderTotal()
        {
            _total = _orderProducts.Sum(p => p.Price);
        }
    }
}
